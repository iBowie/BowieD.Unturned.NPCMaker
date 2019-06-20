using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Managers
{
    public interface IUpdateManager
    {
        void StartUpdate();
        Task<UpdateAvailability> CheckForUpdates();
        UpdateAvailability UpdateAvailability { get; set; }
    }
}
