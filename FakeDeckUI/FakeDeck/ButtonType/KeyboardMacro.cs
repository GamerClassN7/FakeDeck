using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FakeeDeck.ButtonType
{
    internal class KeyboardMacro
    {
        //https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void keybd_event(uint bVk, uint bScan, uint dwFlags, uint dwExtraInfo);

        public static void SendKey(uint Key)
        {
            System.Threading.Thread.Sleep(60);
            keybd_event(Key, 0, 0, 0);
            System.Threading.Thread.Sleep(60);
            keybd_event(Key, 0, 2, 0);
          
        }
    }
}
