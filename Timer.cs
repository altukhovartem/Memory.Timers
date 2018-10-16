using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.Timers
{ 
    public class Timer : IDisposable
    {
        private string _id;
        private DateTime _startTime;
        public long Elapsed;
        public static string report;
        public static int _level = 0;

        public Timer()
        {
            _id = string.Empty;
            _startTime = DateTime.Now;
            _level++;
        }

        public Timer(string timerID)
        {
            _id = timerID;
            _startTime = DateTime.Now;
            _level++;
        }

        public static IDisposable Start() => new Timer();
        public static IDisposable Start(string timerID) => new Timer(timerID);

        public static string Report => report;

        public void Dispose()
        {
            Elapsed = (DateTime.Now - _startTime).Milliseconds;
            
            report += new string(' ', 4 * _level) + _id + new string(' ', 20 - 4 * _level - _id.Length) + ": " + Elapsed + '\n';
        }
    }
}


//report += new string(' ', 4*_level) + _id + new string(' ', 20 - 4*_level - _id.Length) + ": " + Elapsed + '\n';