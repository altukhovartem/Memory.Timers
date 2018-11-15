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
        private static string _report;

        public static string Report
        {
            get 
            { 
                _printed = true;
                return _report; 
            }
        }
        
        [ThreadStatic]
        private Timer _parent;
        private static Timer _current;

        public int _level = 0;
        private string _id = string.Empty;
        private DateTime _startTime;
        public int Elapsed;
        private static bool _printed;

        public static Queue<string> _queueOfReports = new Queue<string>();

        public Timer()
        {
            if(_printed) ClearPrintedReport();
            _startTime = DateTime.Now;
            _level = (_parent == null) ? 0 : _parent._level + 1;
            _id = "*";
            
            CreateReferences();
        }

        public Timer(string timerID)
        {
            if (_printed) ClearPrintedReport();
            CreateReferences();

            _startTime = DateTime.Now;
            _level = (_parent == null) ? 0 : _parent._level + 1;
            _id = timerID;
        }

        public void Dispose()
        {
            Elapsed = (int)(Math.Round(Decimal.Divide((DateTime.Now - _startTime).Milliseconds, 100)) * 100);
            _queueOfReports.Enqueue(new string(' ', 4 * _level) + _id + new string(' ', 20 - 4 * _level - _id.Length) + ": " + Elapsed + '\n');
            RemovesReferenses();

            _level = (_current == null) ? 0 : this._level - 1;
        }

        public static IDisposable Start() => new Timer();
        public static IDisposable Start(string timerID) => new Timer(timerID);

        private void CreateReferences()
        {
            _parent = _current;
            _current = this;
        }

        private void ClearPrintedReport()
        {
            _printed = false;
            _report = string.Empty;
            _queueOfReports.Clear();
        }

        private void RemovesReferenses()
        {
            _current = _parent;
            _parent = _current?._parent;
        }
    }
}
