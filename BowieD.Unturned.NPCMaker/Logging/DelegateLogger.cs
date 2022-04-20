using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public sealed class DelegateLogger : ILogger
    {
        private readonly Queue<string> _stalled = new Queue<string>();

        public delegate void LineAdded(string newLine);

        public event LineAdded OnLineAdded;

        public void Close() { }

        public void Log(string message, ELogLevel level)
        {
            if (OnLineAdded is null)
            {
                _stalled.Enqueue(message);
            }
            else
            {
                while (_stalled.Count > 0)
                {
                    OnLineAdded.Invoke(_stalled.Dequeue());
                }

                OnLineAdded.Invoke(message);
            }
        }

        public void Open() { }
    }
}
