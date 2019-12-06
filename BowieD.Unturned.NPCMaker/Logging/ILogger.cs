using System;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public interface ILogger
    {
        void Open();
        void Close();
        Task Log(string message, ELogLevel level);
    }
}
