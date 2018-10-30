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

        private static Dictionary<string, string> timerDictionary = new Dictionary<string, string>();

        [ThreadStatic]
        private Timer _parent;
        private static Timer _current;
        public int _level = 0;
        
        private string _id = "*";
        private DateTime _startTime;
        public int Elapsed;
        private static bool _printed;

        public Timer()
        {
            if(_printed) ClearPrintedReport();
            
            _startTime = DateTime.Now;
            
            CreateReferences();

            timerDictionary.Add(_id, string.Empty);

            _level = (_parent == null) ? 0 : _parent._level + 1;
        }

        public Timer(string timerID)
          : this()
        {
            _id = timerID;
        }

        public void Dispose()
        {
            Elapsed = (int)(Math.Round(Decimal.Divide((DateTime.Now - _startTime).Milliseconds, 100)) * 100);
            _report += new string(' ', 4 * _level) + _id + new string(' ', 20 - 4 * _level - _id.Length) + ": " + Elapsed + '\n';
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
            timerDictionary.Clear();
        }

        private void RemovesReferenses()
        {
            _current = _parent;
            _parent = _current?._parent;
        }
    }
}
