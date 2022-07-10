using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.Collections.Generic;
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
            {
                App.Logger.Log($"[ParseCommand] - Use: {Name} {Syntax}");
            }
            else
            {
                string joined = string.Join(" ", args);
                if (File.Exists(joined))
                {
                    App.Logger.Log("[ParseCommand] - File found. Checking...");
                    try
                    {
                        var ext = Path.GetExtension(joined);

                        if (string.Equals(ext, ".dat", StringComparison.OrdinalIgnoreCase))
                        {
                            ParseTool pTool = new ParseTool(joined);
                            NPC.ParseType type = pTool.GetParseType();
                            switch (type)
                            {
                                case NPC.ParseType.NPC:
                                    App.Logger.Log("[ParseCommand] - Started parsing 'NPC'.");
                                    MainWindow.CurrentProject.data.characters.Add(pTool.ParseCharacter());
                                    App.Logger.Log("[ParseCommand] - 'NPC' parsed and imported into project.");
                                    LastResult = true;
                                    break;
                                case NPC.ParseType.Dialogue:
                                    App.Logger.Log("[ParseCommand] - Started parsing 'Dialogue'.");
                                    MainWindow.CurrentProject.data.dialogues.Add(pTool.ParseDialogue());
                                    App.Logger.Log("[ParseCommand] - 'Dialogue' parsed and imported into project.");
                                    LastResult = true;
                                    break;
                                case NPC.ParseType.Vendor:
                                    App.Logger.Log("[ParseCommand] - Started parsing 'Vendor'.");
                                    MainWindow.CurrentProject.data.vendors.Add(pTool.ParseVendor());
                                    App.Logger.Log("[ParseCommand] - 'Vendor' parsed and imported into project.");
                                    LastResult = true;
                                    break;
                                case NPC.ParseType.Quest:
                                    App.Logger.Log("[ParseCommand] - Started parsing 'Quest'.");
                                    MainWindow.CurrentProject.data.quests.Add(pTool.ParseQuest());
                                    App.Logger.Log("[ParseCommand] - 'Quest' parsed and imported into project.");
                                    LastResult = true;
                                    break;
                                default:
                                    LastResult = false;
                                    App.Logger.Log("[ParseCommand] - Invalid file.");
                                    break;
                            }
                        }
                        else if (string.Equals(ext, ".asset", StringComparison.OrdinalIgnoreCase))
                        {
                            var res = GameIntegration.GameAssetManager.TryReadAssetFile(joined, GameIntegration.EGameAssetOrigin.Project);

                            LastResult = res.Item1;

                            if (res.Item1)
                            {
                                switch (res.Item2)
                                {
                                    case GameIntegration.GameCurrencyAsset gameCurrencyAsset:
                                        MainWindow.CurrentProject.data.currencies.Add(gameCurrencyAsset.ConvertToProject());
                                        App.Logger.Log("[ParseCommand] - 'Currency' parsed and imported into project.");
                                        break;
                                }
                            }
                            else
                            {
                                App.Logger.Log("[ParseCommand] - Could not recognize .asset type.");
                            }
                        }
                        else
                        {
                            LastResult = false;
                            App.Logger.Log("[ParseCommand] - Could not recognize asset file format.");
                        }
                    }
                    catch (Exception ex)
                    {
                        LastResult = false;
                        App.Logger.LogException("[ParseCommand] - Invalid asset file.", ex: ex);
                    }
                }
                else
                {
                    LastResult = false;
                    App.Logger.Log("[ParseCommand] - File not found.");
                }
            }
        }
        public bool LastResult { get; private set; } = false;
    }
    public sealed class ParseDirCommand : Command
    {
        public override string Name => "parsedir";
        public override string Syntax => "<directory>";
        public override string Help => "Parses all valid Unturned .dat files";
        public override void Execute(string[] args)
        {
            LastImported = 0;
            LastSkipped = 0;
            if (args.Length < 1)
            {
                App.Logger.Log($"[ParseDirCommand] - Use {Name} {Syntax}.");
            }
            else
            {
                string joined = string.Join(" ", args);
                if (Directory.Exists(joined))
                {
                    ParseCommand pCommand = Command.GetCommand<ParseCommand>() as ParseCommand;

                    List<string> toProcess = new List<string>();

                    toProcess.AddRange(Directory.GetFiles(joined, "*.dat", SearchOption.AllDirectories));
                    toProcess.AddRange(Directory.GetFiles(joined, "*.asset", SearchOption.AllDirectories));

                    string[] argCache = new string[1];
                    foreach (string fi in toProcess)
                    {
                        argCache[0] = fi;
                        pCommand.Execute(argCache);
                        if (pCommand.LastResult)
                        {
                            LastImported++;
                        }
                        else
                        {
                            LastSkipped++;
                        }
                    }
                    LastResult = true;
                }
                else
                {
                    App.Logger.Log("[ParseDirCommand] - Directory not found.");
                    LastResult = false;
                }
            }
        }
        public bool LastResult { get; private set; } = false;
        public int LastSkipped { get; private set; } = 0;
        public int LastImported { get; private set; } = 0;
    }
}
