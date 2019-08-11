using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Updating
{
    public interface IUpdateManager
    {
        string Title { get; set; }
        string Content { get; set; }
        void StartUpdate();
        Task<UpdateAvailability> CheckForUpdates();
    }
}
