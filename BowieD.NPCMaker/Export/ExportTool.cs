using BowieD.NPCMaker.Configuration;
using BowieD.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Export
{
    public static class ExportTool
    {
        public static bool ExportCharacter(Character character, string directory)
        {
            try
            {
                string workDir = $"{directory}Characters{Path.DirectorySeparatorChar}{character.editorName}_{character.id}{Path.DirectorySeparatorChar}";
                Directory.CreateDirectory(workDir);
                using (StreamWriter assetWriter = new StreamWriter(workDir + "Asset.dat"))
                using (StreamWriter assetWriter = new StreamWriter(workDir + "English.dat"))
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
