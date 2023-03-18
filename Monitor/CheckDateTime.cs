using System;

using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace smss
{
    public class CheckDateTime
    {
        private static readonly string[] title = { "Change date and time", "Date and Time" };

        public Task StartAsync()
        {
            return Task.Run(LoopCheck)
;        }
            
        public void Start()
        {
            LoopCheck().Wait();
        }
        private async Task LoopCheck()
        {

            while (true)
            {
                foreach (var i in title)
                {
                    Kill(i);
                }
                await Task.Delay(30);
            }

        }

        private void Kill(string title)
        {
            try
            {
                var handle = GetHandleByTitle(title);
                if (handle == IntPtr.Zero)
                {
                    return;
                }
                SendMessage((int)handle, WM_SYSCOMMAND, SC_CLOSE, 0);
            }
            catch
            {
            }


        }

        [DllImport("USER32.DLL")]
        static extern IntPtr GetShellWindow();

        [DllImport("USER32.DLL")]
        static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        static IntPtr GetHandleByTitle(string windowTitle)
        {
            const int nChars = 256;

            IntPtr shellWindow = GetShellWindow();
            IntPtr found = IntPtr.Zero;

            EnumWindows(
                delegate (IntPtr hWnd, int lParam)
                {
                    //ignore shell window
                    if (hWnd == shellWindow) return true;

                    //get Window Title
                    StringBuilder Buff = new StringBuilder(nChars);

                    if (GetWindowText(hWnd, Buff, nChars) > 0)
                    {
                        //Case insensitive match
                        if (Buff.ToString().Contains(windowTitle))
                        {
                            found = hWnd;
                            return true;
                        }
                    }
                    return true;

                }, 0
            );

            return found;
        }

        delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;
    }
}
