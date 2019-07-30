namespace BowieD.NPCMaker.Data
{
    public interface IConfig
    {
        string FileName { get; }
        void LoadDefaults();
        void Load(string filePath);
        void Save(string filePath);
    }
}
