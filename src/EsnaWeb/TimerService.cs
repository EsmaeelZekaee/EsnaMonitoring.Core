using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace EsnaWeb
{
    public class TimerService
    {
        public Timer Timer { get; set; }
        public TimerService()
        {
            Timer = new Timer();
            Timer.Interval = 1000;
            Timer.Enabled = true;
            Timer.Start();
        }
        
    }
}
