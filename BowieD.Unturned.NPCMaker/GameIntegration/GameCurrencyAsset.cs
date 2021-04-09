using BowieD.Unturned.NPCMaker.NPC.Currency;
using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameCurrencyAsset : GameAsset
    {
        public GameCurrencyAsset(string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
        {

        }
        public GameCurrencyAsset(CurrencyAsset asset, EGameAssetOrigin origin) : base(asset.ValueFormat, 0, Guid.Parse(asset.GUID), "currency", origin)
        {
            valueFormat = asset.ValueFormat;
            entries = asset.Entries.ToArray();
        }
        public GameCurrencyAsset(Guid guid, EGameAssetOrigin origin) : base(guid, origin)
        {

        }

        public string valueFormat;
        public CurrencyEntry[] entries;

        public override EIDDef IDDef => EIDDef.GUID;

        protected override void readAsset(IFileReader reader)
        {
            base.readAsset(reader);
            valueFormat = reader.readValue("ValueFormat");
            name = valueFormat;
            int num = reader.readArrayLength("Entries");
            entries = new CurrencyEntry[num];
            for (int i = 0; i < num; i++)
            {
                IFileReader formattedFileReader = reader.readObject(i);
                CurrencyEntry entry = new CurrencyEntry();
                entry.ItemGUID = formattedFileReader.readValue<GameAssetReference<GameItemAsset>>("Item").GUID.ToString("N");
                entry.Value = formattedFileReader.readValue<uint>("Value");
                entries[i] = entry;
            }

            Array.Sort(entries, new Comparison<CurrencyEntry>((x, y) =>
            {
                return x.Value.CompareTo(y.Value);
            }));
        }
    }
}
