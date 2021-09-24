using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public interface ILogger
    {
        void Open();
        void Close();
        void Log(string message, ELogLevel level);
    }
    public interface IAsyncLogger : ILogger
    {
        Task LogAsync(string message, ELogLevel level);
    }
}
