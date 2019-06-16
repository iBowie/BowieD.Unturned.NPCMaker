using System;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NPCMakerTests
{
    [TestClass]
    public class ExportTest
    {
        [TestMethod]
        public void ExportFuelObject()
        {
            NPCObject fuelObject = new NPCObject()
            {
                interactability = Object_Interactability.Fuel,
                interactResource = 50,
                interactReset = 60,
                name = "Tank",
                ID = 25000,
                hint = Object_Hint.None,
                assetFilePath = @"G:\Old Drive\Sorted Shit\Unturned\Workshop\BMA_UI\BMA_UI\Bundles\Effects\BMA_UI\BMA_UI.unity3d"
            };
            NPCSave save = new NPCSave()
            {
                objects = new System.Collections.Generic.List<NPCObject>() { fuelObject },
                guid = Guid.NewGuid().ToString("N")
            };
            BowieD.Unturned.NPCMaker.Export.Exporter.ExportNPC(save);
        }
        [TestMethod]
        public void ExportQuestObject()
        {
            NPCObject questObject = new NPCObject()
            {
                interactability = Object_Interactability.Quest,
                interactEffect = 126,
                rewards = new System.Collections.Generic.List<Reward>()
                {
                    new Experience()
                    {
                        Value = 50,
                        Type = RewardType.Experience
                    },
                    new Experience()
                    {
                        Value = 25,
                        Type = RewardType.Experience
                    },
                    new Quest()
                    {
                        Id = 25001,
                        Type = RewardType.Quest
                    }
                },
                ID = 25000,
                assetFilePath = @"G:\Old Drive\Sorted Shit\Unturned\Workshop\BMA_UI\BMA_UI\Bundles\Effects\BMA_UI\BMA_UI.unity3d",
                conditions = new System.Collections.Generic.List<Condition>()
                {
                    new Quest_Cond()
                    {
                        Id = 25001,
                        Logic = Logic_Type.Equal,
                        Status = Quest_Status.None,
                        Reset = false
                    }
                },
                type = Object_Type.LARGE
            };
            NPCSave save = new NPCSave()
            {
                objects = new System.Collections.Generic.List<NPCObject>() { questObject },
                guid = Guid.NewGuid().ToString("N")
            };
            BowieD.Unturned.NPCMaker.Export.Exporter.ExportNPC(save);
        }
    }
}
