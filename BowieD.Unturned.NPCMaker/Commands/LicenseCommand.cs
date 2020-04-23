namespace BowieD.Unturned.NPCMaker.Commands
{
    public sealed class LicenseCommand : Command
    {
        private static readonly (string lib, string author)[] used = new (string, string)[]
        {
            ("ControlzEx", "ControlzEx"),
            ("Discord RPC CSharp", "Lachee"),
            ("MahApps.Metro", "MahApps"),
            ("MahApps.Metro.IconPacks.Material", "MahApps"),
            ("Newtonsoft.Json", "Newtonsoft"),
            ("Xceed.Wpf.Toolkit", "xceedsoftware"),
            ("XamlBehaviorsWpf", "Microsoft"),
            ("Unturned", "Smartly Dressed Games")
        };
        public override string Name => "license";
        public override string Help => "Display license information";
        public override string Syntax => "[part of license]";
        public override void Execute(string[] args)
        {
            App.Logger.Log("[LicenseCommand] - Copyright (C) 2020  Anton 'BowieD' Galakhov");
            if (args.Length >= 1)
            {
                switch (args[0].ToLower())
                {
                    case "w":
                        App.Logger.Log("[LicenseCommand] - This program is distributed in the hope that it will be useful,");
                        App.Logger.Log("[LicenseCommand] - but WITHOUT ANY WARRANTY; without even the implied warranty of");
                        App.Logger.Log("[LicenseCommand] - MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the");
                        App.Logger.Log("[LicenseCommand] - GNU General Public License for more details.");
                        break;
                    case "c":
                        App.Logger.Log("[LicenseCommand] - This program is free software: you can redistribute it and/or modify");
                        App.Logger.Log("[LicenseCommand] - it under the terms of the GNU General Public License as published by");
                        App.Logger.Log("[LicenseCommand] - the Free Software Foundation, either version 3 of the License, or");
                        App.Logger.Log("[LicenseCommand] - (at your option) any later version.");
                        break;
                    case "l":
                        App.Logger.Log("[LicenseCommand] - Used libraries and other credits:");
                        foreach ((string lib, string author) in used)
                        {
                            App.Logger.Log($"[LicenseCommand] - {lib} - {author}");
                        }
                        break;
                }
            }
            App.Logger.Log("[LicenseCommand] - You should have received a copy of the GNU General Public License");
            App.Logger.Log("[LicenseCommand] - along with this program. If not, see <https://www.gnu.org/licenses/>.");
        }
    }
}
