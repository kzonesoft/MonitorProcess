using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smss
{
    public class CheckGcafe
    {
        private string[] _listProcessName = { "prohook", "cpm", "wnhst", "wnhst64"};
        public Task StartAsync()
        {
            return Task.Run(LoopCheck);
        }
        public void Start()
        {
            LoopCheck().Wait();
        }

        private async Task LoopCheck()
        {
            while (true) {
                var pList = Process.GetProcesses().Where(x => _listProcessName.Contains(x.ProcessName));
                if (pList != null && pList.Count() > 0)
                {
                    foreach (var p in pList)
                    {
                        try
                        {
                            p.Kill();
                            p.WaitForExit(10000);
                        }
                        catch { }
                    }
                }
                await Task.Delay(1000);
            }
            
        }
    }
}
