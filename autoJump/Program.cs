using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace autoJump
{
    class Program
    {
        [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")] private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")] private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")] private static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")] private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll")] private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr? hWnd, IntPtr hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        //ShowWindow参数
        private const int SW_SHOWNORMAL = 1;
        private const int SW_RESTORE = 9;
        private const int SW_SHOWNOACTIVATE = 4;
        private const int KEY_JUMP = 0X20;

        //SendMessage参数
        private const int WM_KEYDOWN = 0X100;
        private const int WM_KEYUP = 0X101;
        private const int WM_SYSCHAR = 0X106;
        private const int WM_SYSKEYUP = 0X105;
        private const int WM_SYSKEYDOWN = 0X104;
        private const int WM_CHAR = 0X102;


        private const int JUMP_TIMEOUT = 1000 * 60 * 3;



        public delegate bool CallBack(int hwnd, int lParam);
        [DllImport("user32.dll")] public static extern int EnumWindows(CallBack x, int y);
        [DllImport("user32.dll")] private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")] private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")] static extern IntPtr SetActiveWindow(IntPtr hWnd);

        public static void InputStr(IntPtr myIntPtr, string Input)
        {
            byte[] ch = (ASCIIEncoding.ASCII.GetBytes(Input));
            Console.WriteLine("尝试输入字符串" + Input);
            for (int i = 0; i < ch.Length; i++)
            {
                Console.WriteLine("尝试字符串" + ch[i]);
                SendMessage(myIntPtr, WM_CHAR, ch[i], 0);
            }
        }

        public static void jump(IntPtr ptr)
        {
            Console.WriteLine("尝试跳跃，窗口ID为:" + ptr);
            SendMessage(ptr, WM_SYSKEYDOWN, KEY_JUMP, 0); //空格键按下
            SendMessage(ptr, WM_SYSKEYUP, KEY_JUMP, 0); //空格键弹起
        }

        public static void doJump(object intPtrObject)
        {
            string ptrString = intPtrObject as string;
            int intPtr = Convert.ToInt32(ptrString);
            IntPtr ptr = new IntPtr(intPtr);
            while (true)
            {
                Random rd = new Random();
                int JUMP_TIME = rd.Next(1000, 50000) + JUMP_TIMEOUT;
                
                jump(ptr);
                Console.WriteLine(JUMP_TIME + "ms后进行跳跃");

                Thread.Sleep(JUMP_TIME);

            }
        }

/*        public IntPtr[] getFormIds(string name)
        {
            ArrayList WOW_ID_LIST = new ArrayList<IntPtr>;
            IntPtr PREV_WOW_ID = new IntPtr(0);
            IntPtr NEXT_WOW_ID = new IntPtr(-1);
            IntPtr PARENT_ID = new IntPtr(0);


            while (NEXT_WOW_ID.ToInt32() != 0)
            {
                NEXT_WOW_ID = FindWindowEx(PARENT_ID, PREV_WOW_ID, null, name);
                WOW_ID_LIST.push
            }

            return WOW_ID_LIST;
        }*/


        static void Main(string[] args)
        {
            Console.WriteLine("开启自动跳跃模式");
            Console.WriteLine("获取魔兽世界窗口中");

            IntPtr WOW_ID = FindWindow(null, "魔兽世界"); //null为类名，可以用Spy++得到，也可以为空
            
            Console.WriteLine("获取魔兽世界窗口成功，ID为:" + WOW_ID);

            Thread JumpThread = new Thread(doJump);
            JumpThread.Start(WOW_ID.ToString());
        }
    }
}
