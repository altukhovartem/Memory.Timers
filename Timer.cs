using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.Timers
{ 
    public class Timer : IDisposable
    {
        [field: ThreadStatic]
        public static string Report { get; private set; }
        private static int _level = -1;

        [ThreadStatic]
        private static Timer _current;
        private Timer _parent;
        
        private string _id = "*";
        private DateTime _startTime;
        private DateTime _stopTime;
        public int Elapsed;


        public Timer()
        {
            _startTime = DateTime.Now;
            ++_level;

            _parent = _current;
            _current = this;
        }

        public Timer(string timerID)
            :this()
        {
            _id = timerID;
        }

        public static IDisposable Start() => new Timer();
        public static IDisposable Start(string timerID) => new Timer(timerID);

        public void Dispose()
        {
            _stopTime = DateTime.Now;
            Elapsed = (int)(Math.Round(Decimal.Divide((_stopTime - _startTime).Milliseconds, 100)) * 100);
            Report += new string(' ', 4 * _level) + _id + new string(' ', 20 - 4 * _level - _id.Length) + ": " + Elapsed + '\n';
            --_level;
        }
    }
}
