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
                            App.Logger.LogInfo("[ParseCommand] - Started parsing 'Vendor'.");
                            MainWindow.CurrentProject.data.vendors.Add(pTool.ParseVendor());
                            App.Logger.LogInfo("[ParseCommand] - 'Vendor' parsed and imported into project.");
                            break;
                        case NPC.ParseType.Quest:
                            App.Logger.LogInfo("[ParseCommand] - Started parsing 'Quest'.");
                            MainWindow.CurrentProject.data.quests.Add(pTool.ParseQuest());
                            App.Logger.LogInfo("[ParseCommand] - 'Quest' parsed and imported into project.");
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
    public sealed class ParseDirCommand : Command
    {
        public override string Name => "parsedir";
        public override string Syntax => "<directory>";
        public override string Help => "Parses all valid Unturned .dat files";
        public override void Execute(string[] args)
        {
            if (args.Length < 1)
            {
                App.Logger.LogInfo($"[ParseDirCommand] - Use {Name} {Syntax}.");
            }
            else
            {
                string joined = string.Join(" ", args);
                if (Directory.Exists(joined))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(joined);
                    foreach (var fi in dirInfo.GetFiles("Asset.dat", SearchOption.AllDirectories))
                    {
                        Command.GetCommand<ParseCommand>().Execute(new string[] { fi.FullName });
                    }
                }
                else
                {
                    App.Logger.LogInfo("[ParseDirCommand] - Directory not found.");
                }
            }
        }
    }
}
