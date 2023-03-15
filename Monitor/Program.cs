using smss;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            RunAction().Wait();
        }


        public static async Task RunAction()
        {
            var checkTime = new CheckDateTime().StartAsync();
            var checkGcafe = new CheckGcafe().StartAsync();
            await Task.WhenAll( checkTime, checkGcafe );
        }

      
    }  
}
