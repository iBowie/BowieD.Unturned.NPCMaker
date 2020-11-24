using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public struct GameAssetReference<T> : IFileReadable where T : GameAsset
    {
        public Guid GUID { get; set; }
        public void read(IFileReader reader)
        {
            IFileReader formattedFileReader = reader.readObject();
            if (formattedFileReader == null)
            {
                GUID = reader.readValue<Guid>();
            }
            else
            {
                GUID = formattedFileReader.readValue<Guid>("GUID");
            }
        }

        public T get()
        {
            if (GameAssetManager.TryGetAsset<T>(GUID, out var res))
            {
                return res;
            }
            else
            {
                return default;
            }
        }
    }
}
