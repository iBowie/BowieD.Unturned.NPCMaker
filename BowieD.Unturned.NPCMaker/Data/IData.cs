namespace BowieD.Unturned.NPCMaker.Data
{
    public interface IData<T>
    {
        string FileName { get; }
        bool Save();
        bool Load(T defaultValue);
    }
}
