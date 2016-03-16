using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;              //包含了用于导入C类型和结构的实用函数

namespace FastLoopExample
{
    static class Program
    { 
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            Application.Run(form);

        }
    }
    [StructLayout(LayoutKind.Sequential)]  //告诉C#按照编写结构的方式在内存中布局这个结构
    public struct Message
    {
        public IntPtr hWnd;
        public Int32 msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }
    public class FastLoop
    {
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin,
            uint messageFilterMax, uint flags);

        public delegate void LoopCallback(double dt);

        LoopCallback _callback;

        PreciseTimer _timer = new PreciseTimer();       //获取间隔的计时器

        const int WM_MOVE = 0x0003;  //移动一个窗口
        const int WM_SIZE = 0x0005;

        public FastLoop(LoopCallback callback)
        {
            _callback = callback;
          
            Application.Idle += new EventHandler(OnApplicationEnterIdle);  //应用程序有空闲时会执行
        }
        void OnApplicationEnterIdle(object sender, EventArgs e)
        {
            while (IsAppStillIdle())      //是否有消息在等待，无则执行回调循环
            {
                _callback(_timer.GetElapsedTime());    
            }
        }
        bool result = false;
        private bool IsAppStillIdle()    //确定应用程序的事件队列中是否有等待处理的事件
        {
            Message msg;
            result = PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            //if (msg.msg > 0x0007 && msg.msg<0x03E0)
            //{
            //    return true;
            //}

            return !result;
        }
    }
    public class PreciseTimer           //可获取帧间时间间隔
    {
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Kernel32")]
        private static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Kernel32")]
        private static extern bool QueryPerformanceCounter(ref long PerformanceCount);
        long _ticksPerSecond = 0;
        long _previousElapsedTime = 0;
        public PreciseTimer()
        {
            QueryPerformanceFrequency(ref _ticksPerSecond); //_ticksPerSecond获取每秒的ticks值（换算时间）
            GetElapsedTime();                           //获取第一次的垃圾结果（丢弃）   
        }
        public double GetElapsedTime()                  //获取帧间间隔
        {
            long time = 0;
            QueryPerformanceCounter(ref time);
            double elapsedTime = (double)(time - _previousElapsedTime) / (double)_ticksPerSecond;
            _previousElapsedTime = time;
            return elapsedTime;
        }
    }


    public class LoopTread
    {
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin,
            uint messageFilterMax, uint flags);

        public delegate void LoopCallback(double dt);

        PreciseTimer _timer = new PreciseTimer();       //获取间隔的计时器

        Form1 form;

        Thread thread;

        public LoopTread(Form1 f)
        {
            form =f;
            thread = new Thread(new ParameterizedThreadStart(Run));
            thread.Priority = ThreadPriority.Highest;
            thread.Start(f);
           // Application.Idle += new EventHandler(OnApplicationEnterIdle);  //应用程序有空闲时会执行
        } 

        void Run(object form)
        {
            Form1 f = form as Form1;
            while(true)
              f.GameLoop(_timer.GetElapsedTime());
        }
        private bool IsAppStillIdle()    //确定应用程序的事件队列中是否有等待处理的事件
        {
            Message msg;
            return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
        }
    }

}
