using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Managers
{
    public interface IUpdateManager
    {
        string Title { get; set; }
        string Content { get; set; }
        void StartUpdate();
        Task<UpdateAvailability> CheckForUpdates();
        UpdateAvailability UpdateAvailability { get; set; }
    }
}
