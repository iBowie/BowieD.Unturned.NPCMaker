using BowieD.Unturned.NPCMaker.Logging;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public sealed class LicenseCommand : Command
    {
        public override string Name => "license";
        public override string Help => "Display license information";
        public override string Syntax => "[part of license]";
        public override void Execute(string[] args)
        {
            App.Logger.LogInfo("Copyright (C) 2019  Anton 'BowieD' Galakhov");
            if (args.Length >= 1)
            {
                switch (args[0].ToLower())
                {
                    case "w":
                        App.Logger.LogInfo("This program is distributed in the hope that it will be useful,");
                        App.Logger.LogInfo("but WITHOUT ANY WARRANTY; without even the implied warranty of");
                        App.Logger.LogInfo("MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the");
                        App.Logger.LogInfo("GNU General Public License for more details.");
                        break;
                    case "c":
                        App.Logger.LogInfo("This program is free software: you can redistribute it and/or modify");
                        App.Logger.LogInfo("it under the terms of the GNU General Public License as published by");
                        App.Logger.LogInfo("the Free Software Foundation, either version 3 of the License, or");
                        App.Logger.LogInfo("(at your option) any later version.");
                        break;
                }
            }
            App.Logger.LogInfo("You should have received a copy of the GNU General Public License");
            App.Logger.LogInfo("along with this program. If not, see <https://www.gnu.org/licenses/>.");
        }
    }
}
