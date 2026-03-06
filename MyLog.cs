using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLog
{
    public static class Logger
    {
        // 定义一个内部结构来存日志，不仅存文本，还存时间和目标文件名
        private class LogItem
        {
            public string Text;
            public string FileName;
            public System.DateTime Time;
        }

        // 线程安全队列
        private static readonly BlockingCollection<LogItem> _logQueue = new BlockingCollection<LogItem>();

        public static string filepath = AppDomain.CurrentDomain.BaseDirectory + @"MyLogs";
        public static string thisfilepath = AppDomain.CurrentDomain.BaseDirectory + @"MyLogs" + @"\" + @"OtherLogs";

        // 读写锁：确保写文件时不能读，读文件时不能写
        static ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        static Logger()
        {
            // 在类第一次被使用时，自动启动后台写文件线程（最低优先级，避免影响更高优先级线程）
            Thread writeThread = new Thread(ProcessQueue);
            writeThread.IsBackground = true;
            writeThread.Name = "MyLog 写入线程";
            writeThread.Priority = ThreadPriority.Lowest;
            writeThread.Start();
        }

        public static void WritePathMyLog(string errorText, string thisfilename)
        {
            // 直接扔进队列
            if (!_logQueue.IsAddingCompleted)
            {
                _logQueue.Add(new LogItem
                {
                    Text = errorText,
                    FileName = thisfilename,
                    Time = System.DateTime.Now
                });
            }
        }

        // 唯一的干活线程
        private static void ProcessQueue()
        {
            foreach (var item in _logQueue.GetConsumingEnumerable())
            {
                try
                {
                    // 确保目录存在
                    if (!Directory.Exists(thisfilepath))
                    {
                        Directory.CreateDirectory(thisfilepath);
                    }

                    string fullPath = Path.Combine(thisfilepath, item.FileName + ".txt");

                    // 拼接格式
                    string finalLog = "[" + item.Time.Year.ToString()
                                      + " / " + item.Time.Month.ToString()
                                      + " / " + item.Time.Day.ToString()
                                      + " / " + item.Time.ToString("HH:mm:ss:fff") + "]  ------>  " + item.Text + Environment.NewLine;

                    byte[] data = Encoding.UTF8.GetBytes(finalLog);

                    // 加上写锁，防止和 ReadMyLog 冲突
                    readerWriterLockSlim.EnterWriteLock();
                    try
                    {
                        // Append 模式写入
                        using (FileStream log = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.Read))
                        {
                            log.Write(data, 0, data.Length);
                        }

                        // 写入完成后检查大小（在锁内，防止删除/读取冲突）
                        CheckLogSize(item.FileName);
                    }
                    finally
                    {
                        readerWriterLockSlim.ExitWriteLock();
                    }

                    // 限速放锁外：让出 CPU，但不占着锁阻塞读/删
                    Thread.Sleep(100);
                }
                catch (Exception)
                {

                }
            }
        }

        private static void CheckLogSize(string filename)
        {
            long OneMb = 1024 * 1024;
            string fullPath = Path.Combine(thisfilepath, filename + ".txt");

            FileInfo fileInfo = new FileInfo(fullPath);
            if (fileInfo.Exists && fileInfo.Length > 1 * OneMb)
            {
                // 如果超过1MB，直接删除（保留了你原本的逻辑）
                fileInfo.Delete();
            }
        }

        public static void DeleteLogFile(string filename)
        {
            string fullPath = Path.Combine(thisfilepath, filename);

            // 加锁删除，防止删的时候正在写
            readerWriterLockSlim.EnterWriteLock();
            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch { }
            finally
            {
                readerWriterLockSlim.ExitWriteLock();
            }
        }

        public static string[] ReadMyLog(string dateTime, string pathname)
        {
            List<string> readline = new List<string>();

            readerWriterLockSlim.EnterReadLock();
            try
            {
                if (!Directory.Exists(thisfilepath)) return new string[0];

                DirectoryInfo directoryInfo = new DirectoryInfo(thisfilepath);

                List<FileInfo> fileInfos = directoryInfo.GetFiles().Where(e => e.Name.Contains(pathname)).ToList();
                fileInfos.Sort((x, y) => (int)(y.CreationTime - x.CreationTime).TotalMilliseconds);

                for (int i = 0; i < fileInfos.Count; i++)
                {
                    using (FileStream fs = new FileStream(fileInfos[i].FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.Length != 0)
                            {
                                readline.Add(line);
                            }
                        }
                    }
                }
                return readline.ToArray();
            }
            catch
            {
                return new string[0];
            }
            finally
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }
    }
}