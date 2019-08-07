using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.Text;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public sealed class InfoCommand : Command
    {
        public override string Name => "info";
        public override string Help => "";
        public override string Syntax => "";
        public override void Execute(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            var cfg = AppConfig.Instance;
            sb.AppendLine("== Debug Information ==");
            sb.AppendLine("");
            sb.AppendLine($"OS: {WindowsVersion}");
            sb.AppendLine($"Is Elevated: {IsElevated}");
            sb.AppendLine($"Time: {DateTime.Now}");
            sb.AppendLine($"Settings:");
            sb.AppendLine($".Experimental Features: {cfg.experimentalFeatures}");
            sb.AppendLine($".Language: {cfg.locale}");
            sb.AppendLine($".Autosave Option: {cfg.autosaveOption}");
            sb.AppendLine($".GUID Generation: {cfg.generateGuids}");
            sb.AppendLine($".Detailed Discord Rich Presence: {cfg.enableDiscord}");
            sb.AppendLine($".Animation: {cfg.animateControls}");
            sb.AppendLine($".Scale: {cfg.scale}");
            sb.AppendLine($".Theme: {cfg.currentTheme}");
            sb.AppendLine("");
            sb.AppendLine("== End of Debug Information ==");
            Logging.Logger.Log(sb);
        }
        private OperatingSystem OS => Environment.OSVersion;
        private string WindowsVersion
        {
            get
            {
                string version = OS.Version.Major + "." + OS.Version.Minor;
                switch (version)
                {
                    case "10.0":
                        return "Windows 10";
                    case "6.3":
                        return "Windows 8.1";
                    case "6.2":
                        return "Windows 8";
                    case "6.1":
                        return "Windows 7";
                    case "6.0":
                        return "Windows Vista";
                    case "5.2":
                        return "Windows XP 64-Bit Edition";
                    case "5.1":
                        return "Windows XP";
                    case "5.0":
                        return "Windows 2000";
                    default:
                        return "Unknown";
                }
            }
        }
        private bool IsElevated
        {
            get
            {
                var id = System.Security.Principal.WindowsIdentity.GetCurrent();
                return id.Owner != id.User;
            }
        }
    }
}
