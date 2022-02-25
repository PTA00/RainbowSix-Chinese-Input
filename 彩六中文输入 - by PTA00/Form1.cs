using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Threading.Thread;
using System.Runtime.InteropServices;


namespace 彩六中文输入___by_PTA00
{

    public partial class Form1 : Form
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }
        public struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [DllImport("user32")]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            int i = 127;
            //冷知识：彩六最多能打127个英文字符或42个中文字符，一个中文字符等于3个英文字符
            string keys = textBox1.Text;
            Sleep(3000);
            
            foreach (char c in keys)
            {
                if (i >= 3)
                {
                    if (c > 127)
                    {
                        //中文3
                        i -= 3;
                    }
                    else
                    {
                        //英文1
                        i--;
                    }
                }
                else
                {
                    SimulateInputKey(0x0D);//ENTER键
                    Sleep(2000);
                    
                    i = 127;
                }
                SimulateInputString(c);//发送字符
                int num = r.Next(50, 151);//随机延时
                Sleep(num);
            }
            SimulateInputKey(0x0D);//ENTER键

        }


        public void SimulateInputString(char c)
        {
            INPUT[] input = new INPUT[2];
                
            input[0].type = 1;
            input[0].ki.wVk = 0;//dwFlags 为KEYEVENTF_UNICODE 即4时，wVk必须为0
            input[0].ki.wScan = (short)c;
            input[0].ki.dwFlags = 4;//输入UNICODE字符
            input[0].ki.time = 0;
            input[0].ki.dwExtraInfo = IntPtr.Zero;
            input[1].type = 1;
            input[1].ki.wVk = 0;
            input[1].ki.wScan = (short)c;
            input[1].ki.dwFlags = 6;
            input[1].ki.time = 0;
            input[1].ki.dwExtraInfo = IntPtr.Zero;
            SendInput(2u, input, Marshal.SizeOf((object)default(INPUT)));
            
        }

        public void SimulateInputKey(int key)
        {
            INPUT[] input = new INPUT[1];

            input[0].type = 1;//模拟键盘
            input[0].ki.wVk = (short)key;
            input[0].ki.dwFlags = 0;//按下
            SendInput(1u, input, Marshal.SizeOf((object)default(INPUT)));
            Sleep(50);

            input[0].type = 1;//模拟键盘
            input[0].ki.wVk = (short)key;
            input[0].ki.dwFlags = 2;//抬起
            SendInput(1u, input, Marshal.SizeOf((object)default(INPUT)));
            Sleep(50);
        }

        
    }
}
