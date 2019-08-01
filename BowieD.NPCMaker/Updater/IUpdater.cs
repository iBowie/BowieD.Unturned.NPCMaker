namespace BowieD.NPCMaker.Updater
{
    public interface IUpdater
    {
        ECheckResult CheckUpdate();
        void DownloadUpdater(string path);
    }
}
