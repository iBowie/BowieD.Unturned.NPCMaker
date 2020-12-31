using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameItemGunAsset : GameItemWeaponAsset
    {
        public GameItemGunAsset(DataReader data, DataReader local, string dirName, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(data, local, dirName, name, id, guid, type, origin)
        {
            hasSight = data.Has("Hook_Sight");
            hasTactical = data.Has("Hook_Tactical");
            hasGrip = data.Has("Hook_Grip");
            hasBarrel = data.Has("Hook_Barrel");

            int num2 = data.ReadInt32("Magazine_Calibers", 0);
            if (num2 > 0)
            {
                magazineCalibers = new ushort[num2];
                for (int j = 0; j < num2; j++)
                {
                    magazineCalibers[j] = data.ReadUInt16($"Magazine_Caliber_{j}", 0);
                }
                int num3 = data.ReadInt32("Attachment_Calibers");
                if (num3 > 0)
                {
                    attachmentCalibers = new ushort[num3];
                    for (int k = 0; k < num3; k++)
                    {
                        attachmentCalibers[k] = data.ReadUInt16($"Attachment_Caliber_{k}", 0);
                    }
                }
                else
                {
                    attachmentCalibers = magazineCalibers;
                }
            }
            else
            {
                magazineCalibers = new ushort[1];
                magazineCalibers[0] = data.ReadUInt16("Caliber", 0);
                attachmentCalibers = magazineCalibers;
            }
        }

        public bool hasSight, hasTactical, hasGrip, hasBarrel;

        public readonly ushort[] magazineCalibers;
        public readonly ushort[] attachmentCalibers;
    }
}
