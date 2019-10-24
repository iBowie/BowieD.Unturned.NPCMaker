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
            App.Logger.LogInfo("[LicenseCommand] - Copyright (C) 2019  Anton 'BowieD' Galakhov");
            if (args.Length >= 1)
            {
                switch (args[0].ToLower())
                {
                    case "w":
                        App.Logger.LogInfo("[LicenseCommand] - This program is distributed in the hope that it will be useful,");
                        App.Logger.LogInfo("[LicenseCommand] - but WITHOUT ANY WARRANTY; without even the implied warranty of");
                        App.Logger.LogInfo("[LicenseCommand] - MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the");
                        App.Logger.LogInfo("[LicenseCommand] - GNU General Public License for more details.");
                        break;
                    case "c":
                        App.Logger.LogInfo("[LicenseCommand] - This program is free software: you can redistribute it and/or modify");
                        App.Logger.LogInfo("[LicenseCommand] - it under the terms of the GNU General Public License as published by");
                        App.Logger.LogInfo("[LicenseCommand] - the Free Software Foundation, either version 3 of the License, or");
                        App.Logger.LogInfo("[LicenseCommand] - (at your option) any later version.");
                        break;
                }
            }
            App.Logger.LogInfo("[LicenseCommand] - You should have received a copy of the GNU General Public License");
            App.Logger.LogInfo("[LicenseCommand] - along with this program. If not, see <https://www.gnu.org/licenses/>.");
        }
    }
}
