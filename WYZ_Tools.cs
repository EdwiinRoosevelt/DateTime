using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace Logic.Tools
{
    public class WYZ_Tools
    {
        public static TForm ShowSingleInstance<TForm>() where TForm : Form, new()
        {
            var existing = Application.OpenForms.OfType<TForm>().FirstOrDefault();
            if (existing == null)
            {
                var frm = new TForm();
                frm.Show();
                return frm;
            }
            EnsureToFront(existing);
            return existing;
        }
        /// <summary>
        /// 将控件移动到新的父容器，同时保持其在屏幕上的视觉位置不变
        /// </summary>
        /// <param name="child">要移动的子控件</param>
        /// <param name="newParent">新的父容器</param>
        public void ReparentControl(Control child, Control newParent)
        {
            //如果控件为空或者父容器没变，直接返回
            if (child == null || newParent == null || child.Parent == newParent) return;
            // 获取子控件在屏幕上的绝对坐标
            Point screenPoint = child.PointToScreen(Point.Empty);
            child.Parent = newParent;
            // 将屏幕绝对坐标转换为新父容器内部的相对坐标
            child.Location = newParent.PointToClient(screenPoint);
            child.BringToFront();
        }
        // 新增：使用工厂委托创建实例（可传入构造参数）
        public static TForm ShowSingleInstance<TForm>(Func<TForm> factory) where TForm : Form
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            var existing = Application.OpenForms.OfType<TForm>().FirstOrDefault();
            if (existing == null)
            {
                var frm = factory();
                frm.Show();
                return frm;
            }
            EnsureToFront(existing);
            return existing;
        }

        // 可选：根据匹配谓词选择现有实例（用于区分不同数据的同类窗体）
        public static TForm ShowSingleInstance<TForm>(Func<TForm> factory, Func<TForm, bool> match)
            where TForm : Form
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            var existing = Application.OpenForms
                .OfType<TForm>()
                .FirstOrDefault(f => match == null || match(f));

            if (existing == null)
            {
                var frm = factory();
                frm.Show();
                return frm;
            }
            EnsureToFront(existing);
            return existing;
        }

        private static void EnsureToFront(Form existing)
        {
            if (existing.WindowState == FormWindowState.Minimized)
                existing.WindowState = FormWindowState.Normal;

            existing.BringToFront();
            existing.Activate();

            bool originalTopMost = existing.TopMost;
            existing.TopMost = true;
            existing.TopMost = originalTopMost;

            existing.Show();
        }
        /// <summary>
        /// 将窗口显示到最前面
        /// </summary>
        public static void ShowTopMost(object frm)
        {
            ((Form)frm).BringToFront();
            ((Form)frm).Activate();
            bool originalTopMost = ((Form)frm).TopMost;
            ((Form)frm).TopMost = true;
            ((Form)frm).TopMost = originalTopMost;
        }
        public static class NativeWindowsApi
        {
            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wparam, int lparam);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            public static extern bool ReleaseCapture();
        }
        /// <summary>
        /// 鼠标拖动窗口
        /// </summary>
        /// <param name="e"></param>
        public static void  MoveMainFormWindow(object sender, MouseEventArgs e)
        {
            const int WM_NCLBUTTONDOWN = 0x00A1;
            const int HTCAPTION = 2;
            if (e.Button != MouseButtons.Left) return;

            var control = sender as Control;
            if (control == null) return;

            var targetForm = control.FindForm();
            if (targetForm == null) return;

            NativeWindowsApi.ReleaseCapture();
            control.Capture = false;//释放鼠标使能够手动操作
            NativeWindowsApi.SendMessage(targetForm.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);//拖动窗体
        }
    }
    //public static class AxisExtensions
    //{
    //    public static object ob = new object();
    //    /// <summary>
    //    /// 调用时没有指定速度参数则使用整机速度百分比，否则使用指定速度参数进行移动
    //    /// </summary>
    //    /// <param name="axis"></param>
    //    /// <param name="speed"></param>
    //    /// <param name="pos"></param>
    //    public static void MoveBySpd_Abs(this AxisClass axis, float pos, float spd = 0)
    //    {
    //        lock (ob)
    //        {
    //            if (spd == 0)
    //            {

    //                float spdTotal = axis.AxisPara.RunSpeed * GSD.Ins.TotalSpeed / 100f;

    //                axis.MC_MoveAbs(spdTotal, pos);
    //            }
    //            else
    //            {
    //                axis.MC_MoveAbs(spd, pos);
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// 调用时没有指定速度参数则使用整机速度百分比，否则使用指定速度参数进行移动
    //    /// </summary>
    //    /// <param name="axis"></param>
    //    /// <param name="speed"></param>
    //    /// <param name="pos"></param>
    //    public static void MoveBySpd_Rel(this AxisClass axis, float pos, float spd = 0)
    //    {
    //        lock (ob)
    //        {
    //            if (spd == 0)
    //            {

    //                float spdTotal = axis.AxisPara.RunSpeed * GSD.Ins.TotalSpeed / 100f;

    //                axis.MC_MoveRel(spdTotal, pos);
    //            }
    //            else
    //            {
    //                axis.MC_MoveRel(spd, pos);
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 调用时没有指定速度参数则使用整机速度百分比，否则使用指定速度参数进行移动
    //    /// </summary>
    //    /// <param name="customTask"></param>
    //    /// <param name="AxisIdx"></param>
    //    /// <param name="pos"></param>
    //    /// <param name="spd"></param>
    //    public static void Cmd_MoveBySpd_Abs(this CustomTask customTask, int AxisIdx, float pos, float spd = 0)
    //    {
    //        lock (ob)
    //        {
    //            if (spd == 0)
    //            {

    //                float spdTotal = customTask.MotionCard.Axis[AxisIdx].AxisPara.RunSpeed * GSD.Ins.TotalSpeed / 100f;
    //                customTask.Cmd_Mc_MoveAbs(AxisIdx, pos, spdTotal);
    //            }
    //            else
    //            {
    //                customTask.Cmd_Mc_MoveAbs(AxisIdx, pos, spd);
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// 调用时没有指定速度参数则使用整机速度百分比，否则使用指定速度参数进行移动
    //    /// </summary>
    //    /// <param name="customTask"></param>
    //    /// <param name="AxisIdx"></param>
    //    /// <param name="pos"></param>
    //    /// <param name="spd"></param>
    //    public static void Cmd_MoveBySpd_Rel(this CustomTask customTask, int AxisIdx, float pos, float spd = 0)
    //    {
    //        lock (ob)
    //        {
    //            if (spd == 0)
    //            {
    //                float spdTotal = customTask.MotionCard.Axis[AxisIdx].AxisPara.RunSpeed * GSD.Ins.TotalSpeed / 100f;
    //                customTask.Cmd_Mc_MoveRel(AxisIdx, pos, spdTotal);
    //            }
    //            else
    //            {
    //                customTask.Cmd_Mc_MoveRel(AxisIdx, pos, spd);
    //            }
    //        }
    //    }
    //}


}
