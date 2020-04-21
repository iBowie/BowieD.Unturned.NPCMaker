namespace BowieD.Unturned.NPCMaker.Data
{
    public delegate void DataLoaded<T>();
    public delegate void DataSaved<T>();
    public interface IData<T>
    {
        string FileName { get; }
        bool Save();
        bool Load(T defaultValue);

        event DataLoaded<T> OnDataLoaded;
        event DataSaved<T> OnDataSaved;
    }
}
