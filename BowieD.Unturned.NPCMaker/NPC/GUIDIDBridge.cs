using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.ViewModels;
using System;
using System.Globalization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public struct GUIDIDBridge
    {
        public GUIDIDBridge(Guid guid)
        {
            this.Guid = guid;
            this.ID = null;
        }
        public GUIDIDBridge(ushort id)
        {
            this.Guid = null;
            this.ID = id;
        }

        public Guid? Guid { get; set; }
        public ushort? ID { get; set; }

        public ushort ResolveLegacyID<T>() where T : GameAsset
        {
            if (ID.HasValue)
                return ID.Value;

            if (GameAssetManager.TryGetAsset<T>(this, out var res))
            {
                return res.id;
            }

            return 0;
        }

        public bool IsEmpty
        {
            get
            {
                if (!Guid.HasValue && !ID.HasValue)
                    return true;

                if (Guid.HasValue && Guid.Value == System.Guid.Empty)
                    return true;

                if (ID.HasValue && ID.Value == 0)
                    return true;

                return false;
            }
        }

        public static explicit operator GUIDIDBridge(ushort id)
        {
            return new GUIDIDBridge(id);
        }
        public static explicit operator GUIDIDBridge(Guid guid)
        {
            return new GUIDIDBridge(guid);
        }

        public string ExportValue
        {
            get
            {
                if (Guid.HasValue && Guid.Value != System.Guid.Empty)
                    return Guid.Value.ToString("N");

                if (ID.HasValue && ID.Value > 0)
                    return ID.Value.ToString();

                return "0";
            }
        }

        public static GUIDIDBridge Parse(string input)
        {
            GUIDIDBridge bridge = new GUIDIDBridge();

            if (!string.IsNullOrEmpty(input) && (input.Length != 1 || input[0] != '0'))
            {
                if (ushort.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var legacyId))
                {
                    bridge.ID = legacyId;
                    bridge.Guid = null;
                    return bridge;
                }

                if (System.Guid.TryParse(input, out var guid))
                {
                    bridge.Guid = guid;
                    bridge.ID = null;
                    return bridge;
                }
            }

            return bridge;
        }

        public override string ToString() => ExportValue;
    }
}
