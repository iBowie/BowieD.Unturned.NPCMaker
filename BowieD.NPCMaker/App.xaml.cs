using BowieD.NPCMaker.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BowieD.NPCMaker
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        public void runCharacterExportTest()
        {
            Project proj = new Project();
            proj.characters.Add(new NPC.Character()
            {
                beard = 1,
                christmasClothing = new NPC.Outfit(),
                defaultClothing = new NPC.Outfit()
                {
                    shirt = 250,
                    pants = 251
                },
                dialogueId = 25,
                id = 24,
                displayName = new Dictionary<NPC.ELanguage, string>()
                {
                    { NPC.ELanguage.English, "Export Display Test" },
                    { NPC.ELanguage.Russian, "Тест Экспорта Видимое имя" }
                },
                editorName = new Dictionary<NPC.ELanguage, string>()
                {
                    { NPC.ELanguage.English, "Export Editor Test" },
                    { NPC.ELanguage.Russian, "Тест Экспорта Имя в редакторе" }
                },
                equippedSlot = NPC.ESlotType.NONE,
                equipPrimary = 0,
                equipSecondary = 0,
                equipTertiary = 0,
                face = 1,
                hair = 5,
                hairColor = new Coloring.Color(0,0,0),
                skinColor = new Coloring.Color(0,0,0),
                halloweenClothing = new NPC.Outfit(),
                leftHanded = false,
                pose = NPC.ENPCPose.STAND,
                visibilityConditions = new List<NPC.Condition.Condition>()
            });
            Export.ExportTool.ExportCharacter(proj.characters[0], $@"K:\User\Desktop\npctest\");
        }
        public void runConditionExportTest()
        {
            NPC.Condition.Condition cnd = new NPC.Condition.ConditionCompareFlags()
            {
                Allow_A_Unset = false,
                Allow_B_Unset = true,
                A_ID = 1,
                B_ID = 2,
                localization = new Dictionary<NPC.ELanguage, string>()
                {
                    { NPC.ELanguage.English, "no text" }
                },
                Logic = NPC.ENPCLogicType.EQUAL,
                Reset = true
            };
            string cnd1 = Export.ExportTool.ExportCondition(cnd, "Response_0_", 0);
            string cnd2 = Export.ExportTool.ExportCondition(cnd, "", 0);
            MessageBox.Show($"cnd1: {Environment.NewLine}{cnd1}");
            MessageBox.Show($"cnd2: {Environment.NewLine}{cnd2}");
        }
    }
}
