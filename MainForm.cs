using Logic.Tools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DateTime
{
    public partial class MainForm : Form
    {
        private readonly string[] offlineFortunes = { "大吉", "中吉", "小吉", "平", "凶", "大凶" };
        private readonly Random random = new Random();
        private readonly Timer colorTimer = new Timer();

        private List<string> currentYiItems = new List<string>();
        private List<string> currentJiItems = new List<string>();

        private bool colorRefreshPending = true;
        private Color lastBackColor = Color.Empty;
        private System.DateTime lastApplyTime = System.DateTime.MinValue;


        public MainForm()
        {
            InitializeComponent();
            ConfigureWrapLabels();
            InitDailyInfo();
            uiPanel1.MouseMove += WYZ_Tools.MoveMainFormWindow;

            //colorTimer.Interval = 300;
            //colorTimer.Tick += ColorTimer_Tick;
            //colorTimer.Start();

            this.FormClosed += MainForm_FormClosed;
            this.Move += MainForm_MoveResize;
            this.Resize += MainForm_MoveResize;

            //ApplyAdaptiveBackColor();
            this.TopMost = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            colorTimer.Stop();
        }

        private void MainForm_MoveResize(object sender, EventArgs e)
        {
            colorRefreshPending = true;
        }

        private void ConfigureWrapLabels()
        {
            // 保持 AutoSize=true，配合 MaximumSize 实现自动换行
            lblYi.AutoSize = true;
            lblYi.MaximumSize = new System.Drawing.Size(560, 0);

            lblJi.AutoSize = true;
            lblJi.MaximumSize = new System.Drawing.Size(560, 0);
        }
        private void InitDailyInfo()
        {
            try
            {
                // 先尝试使用已安装的 Lunar-CSharp 运行时类型（反射调用，避免 编译时依赖不匹配）
                List<string> yiFromLib, jiFromLib;
                string gongliText, nongliText;
                if (TryGetFromLunarCSharp(out yiFromLib, out jiFromLib, out gongliText, out nongliText))
                {
                    currentYiItems = yiFromLib ?? new List<string>();
                    currentJiItems = jiFromLib ?? new List<string>();

                    lblDate.Text = gongliText + "\r\n农历：" + nongliText;
                    lblYi.Text = "宜：" + (currentYiItems.Count > 0 ? string.Join(" ", currentYiItems) : "无");
                    lblJi.Text = "忌：" + (currentJiItems.Count > 0 ? string.Join(" ", currentJiItems) : "无");
                }
                else
                {
                    MessageBox.Show("算命失败");
                }

                lblFortune.Text = "今日运势：待抽取";
            }
            catch (Exception ex)
            {
                MessageBox.Show("黄历算法计算失败：" + ex.Message);
            }
        }

        private bool TryGetFromLunarCSharp(out List<string> yi, out List<string> ji, out string gongliText, out string nongliText)
        {
            yi = new List<string>();
            ji = new List<string>();
            gongliText = System.DateTime.Now.ToString("yyyy年M月d日");
            nongliText = string.Empty;

            try
            {
                var now = System.DateTime.Now;
                var solar = Lunar.Solar.FromDate(now);
                if (solar == null)
                {
                    return false;
                }

                gongliText = string.Format("{0}年 {1}月 {2}日", solar.Year, solar.Month, solar.Day);

                var lunar = solar.Lunar;
                if (lunar == null)
                {
                    return true;
                }

                nongliText = string.Format("{0}{1}{2}",
                    lunar.YearInChinese ?? "",
                    lunar.MonthInChinese ?? "",
                    lunar.DayInChinese ?? "");

                var yiItems = lunar.GetDayYi();
                var jiItems = lunar.GetDayJi();

                if (yiItems != null)
                {
                    yi = new List<string>(yiItems);
                }

                if (jiItems != null)
                {
                    ji = new List<string>(jiItems);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        private void btnDrawLot_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> pool = new List<string>();

                for (int i = 0; i < currentYiItems.Count; i++)
                {
                    pool.Add("宜|" + currentYiItems[i]);
                }

                for (int i = 0; i < currentJiItems.Count; i++)
                {
                    pool.Add("忌|" + currentJiItems[i]);
                }

                if (pool.Count == 0)
                {
                    int fallbackIndex = random.Next(offlineFortunes.Length);
                    lblFortune.Text = "今日运势抽签结果：" + offlineFortunes[fallbackIndex] + " (离线)";
                    return;
                }

                int index = random.Next(pool.Count);
                string[] result = pool[index].Split('|');
                lblFortune.Text = "今日运势抽签结果：" + result[0] + " " + result[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message);
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private void lblDate_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}