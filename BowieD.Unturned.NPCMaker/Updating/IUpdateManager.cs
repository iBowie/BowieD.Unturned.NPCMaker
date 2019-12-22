using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Updating
{
    public interface IUpdateManager
    {
        void StartUpdate();
        Task<UpdateAvailability> CheckForUpdates(bool checkForPrerelease);
    }
}
