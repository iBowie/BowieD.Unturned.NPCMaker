namespace BowieD.Unturned.NPCMaker.Data
{
    public interface IData<T>
    {
        string FileName { get; }
        void Save();
        void Load(T defaultValue);
    }
}
