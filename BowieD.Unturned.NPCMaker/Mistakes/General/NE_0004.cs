using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Mistakes.General
{
    /// <summary>
    /// Any content in the app has the same id
    /// </summary>
    public sealed class NE_0004 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.CRITICAL;
        public override bool IsMistake
        {
            get
            {
                HashSet<ushort> takenIds = new HashSet<ushort>();
                foreach (var k in MainWindow.CurrentProject.dialogues)
                    takenIds.Add(k.id);
                foreach (var k in MainWindow.CurrentProject.quests)
                    takenIds.Add(k.id);
                foreach (var k in MainWindow.CurrentProject.vendors)
                    takenIds.Add(k.id);
                foreach (var k in MainWindow.CurrentProject.characters)
                    takenIds.Add(k.id);
                return takenIds.Any(d => takenIds.Count(k => k == d) >= 2);
            }
        }
        public override string MistakeNameKey => "NE_0004";
        public override bool TranslateName => false;
        public override string MistakeDescKey => "NE_0004_Desc";
    }
}
