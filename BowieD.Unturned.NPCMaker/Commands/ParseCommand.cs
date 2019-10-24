using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Parsing;
using System.IO;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public sealed class ParseCommand : Command
    {
        public override string Name => "parse";
        public override string Syntax => "<fileName>";
        public override string Help => "Parses Unturned .dat file";
        public override void Execute(string[] args)
        {
            if (args.Length < 1)
                App.Logger.LogInfo($"[ParseCommand] - Use: {Name} {Syntax}");
            else
            {
                string joined = string.Join(" ", args);
                if (File.Exists(joined))
                {
                    App.Logger.LogInfo("[ParseCommand] - File found. Checking...");
                    ParseTool pTool = new ParseTool(joined);
                    var type = pTool.GetParseType();
                    switch (type)
                    {
                        case NPC.ParseType.NPC:
                            App.Logger.LogInfo("[ParseCommand] - Started parsing 'NPC'.");
                            MainWindow.CurrentProject.data.characters.Add(pTool.ParseCharacter());
                            App.Logger.LogInfo("[ParseCommand] - 'NPC' parsed and imported into project.");
                            break;
                        case NPC.ParseType.Dialogue:
                            App.Logger.LogInfo("[ParseCommand] - Started parsing 'Dialogue'.");
                            MainWindow.CurrentProject.data.dialogues.Add(pTool.ParseDialogue());
                            App.Logger.LogInfo("[ParseCommand] - 'Dialogue' parsed and imported into project.");
                            break;
                        case NPC.ParseType.Vendor:
                            App.Logger.LogInfo("[ParseCommand] - Vendor parsing not added yet.");
                            break;
                        case NPC.ParseType.Quest:
                            App.Logger.LogInfo("[ParseCommand] - Quest parsing not added yet.");
                            break;
                        default:
                            App.Logger.LogInfo("[ParseCommand] - Invalid file.");
                            break;
                    }
                }
                else
                {
                    App.Logger.LogInfo("[ParseCommand] - File not found.");
                }
            }
        }
    }
}
