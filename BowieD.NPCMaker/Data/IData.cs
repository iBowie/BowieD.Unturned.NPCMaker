namespace BowieD.NPCMaker.Data
{
    public interface IData<T> where T : new()
    {
        string FileName { get; }
        void Save(T value);
        T Load();
    }
}
