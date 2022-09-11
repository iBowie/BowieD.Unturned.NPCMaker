using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.NPC.Currency;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameCurrencyAsset : GameAsset, IHasAnimatedThumbnail
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

        public IEnumerable<ImageSource> Thumbnails
        {
            get
            {
                foreach (var p in entries)
                {
                    if (GameAssetManager.TryGetAsset<GameItemAsset>(new Guid(p.ItemGUID), out var itemAsset))
                    {
                        yield return ThumbnailManager.CreateThumbnail(itemAsset.ImagePath);
                    }
                }
            }
        }

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
                if (formattedFileReader.containsKey("Is_Visible_In_Vendor_Menu"))
                {
                    entry.IsVisibleInVendorMenu = formattedFileReader.readValue<bool>("Is_Visible_In_Vendor_Menu");
                }
                else
                {
                    entry.IsVisibleInVendorMenu = true;
                }
                entries[i] = entry;
            }

            Array.Sort(entries, new Comparison<CurrencyEntry>((x, y) =>
            {
                return x.Value.CompareTo(y.Value);
            }));
        }

        public override IEnumerable<string> GetToolTipLines()
        {
            foreach (var b in base.GetToolTipLines())
                yield return b;

            string format;

            try
            {
                string.Format(valueFormat, uint.MaxValue);
                format = valueFormat;
            }
            catch (FormatException)
            {
                format = "{0:N0}";
            }

            foreach (var e in entries)
            {
                if (GameAssetManager.TryGetAsset<GameItemAsset>(new Guid(e.ItemGUID), out var asset) && !string.IsNullOrEmpty(asset.Name))
                {
                    yield return $"[{asset.id}] {asset.Name} - {string.Format(format, e.Value)}";
                }
                else
                {
                    yield return $"{e.ItemGUID} - {string.Format(format, e.Value)}";
                }
            }
        }

        public CurrencyAsset ConvertToProject()
        {
            CurrencyAsset currencyAsset = new CurrencyAsset()
            {
                Entries = entries.Select(d =>
                {
                    return new CurrencyEntry()
                    {
                        ItemGUID = d.ItemGUID,
                        Value = d.Value,
                        IsVisibleInVendorMenu = d.IsVisibleInVendorMenu,
                    };
                }).ToList(), // clone entries
                GUID = guid.ToString("N"),
                ValueFormat = valueFormat,
            };

            return currencyAsset;
        }
    }
}
