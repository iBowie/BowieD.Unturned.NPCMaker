namespace BowieD.Unturned.NPCMaker.Editors
{
    public interface IEditor<T>
    {
        void Save();
        void Open();
        void Reset();
        T Current { get; set; }
    }
}
