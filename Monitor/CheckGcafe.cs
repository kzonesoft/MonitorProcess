using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smss
{
    public class CheckGcafe
    {
        private readonly string _processNameDefault = "prohook,cpm,wnhst,wnhst64";
        private string[] _processToKill = null;
        public CheckGcafe() => Initialize();
       
        public Task StartAsync()
        {
            return Task.Run(LoopCheck);
        }
        public void Start()
        {
            LoopCheck().Wait();
        }

        private void Initialize()
        {
            var textPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "smsetup.log");
            if (File.Exists(textPath))
            {
                var p = File.ReadAllText(textPath);
                _processToKill = p.Split(',').ToArray();
            }
            else
            {
                File.WriteAllText(textPath,_processNameDefault);
                _processToKill = _processNameDefault.Split(',').ToArray();
            }
        }

        private async Task LoopCheck()
        {
            while (true) {
                var pList = Process.GetProcesses().Where(x => _processToKill.Contains(x.ProcessName));
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
