using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.Text;

namespace BowieD.Unturned.NPCMaker.Common.Utility
{
    internal static class DebugUtility
    {
        private const string UNDEFINED = "Undefined";
        internal static string GetDebugInformation()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($".NET Version: {NETHelper.GetVersionString()}");
            sb.Append("Platform: ");
            try
            {
                sb.AppendLine(Environment.OSVersion.Platform.ToString());
            }
            catch (InvalidOperationException)
            {
                sb.AppendLine(UNDEFINED);
            }
            sb.Append("Command Line Args: ");
            try
            {
                sb.AppendLine($"{string.Join(", ", Environment.GetCommandLineArgs())}");
            }
            catch (Exception e) when (e is NotSupportedException || e is InvalidOperationException)
            {
                sb.AppendLine(UNDEFINED);
            }
            sb.AppendLine($"Is Elevated: {IsElevated}");
            sb.AppendLine($"Time: {DateTime.Now}");
            sb.AppendLine($"Alternate Path: {AppConfig.AlternatePath}");
            sb.AppendLine($"Settings:");
            try
            {
                var cfg = AppConfig.Instance;
                sb.AppendLine($".animateControls: {cfg.animateControls}");
                sb.AppendLine($".autosaveOption: {cfg.autosaveOption}");
                sb.AppendLine($".autoUpdate: {cfg.autoUpdate}");
                sb.AppendLine($".currentTheme: {cfg.currentTheme}");
                sb.AppendLine($".enableDiscord: {cfg.enableDiscord}");
                sb.AppendLine($".experimentalFeatures: {cfg.experimentalFeatures}");
                sb.AppendLine($".generateGuids: {cfg.generateGuids}");
                sb.AppendLine($".language: {cfg.language}");
                sb.AppendLine($".scale: {cfg.scale}");
                sb.AppendLine($".downloadPrerelease: {cfg.downloadPrerelease}");
            }
            catch
            {
                sb.AppendLine("!Unable to get further settings");
            }
            return sb.ToString();
        }
        private static bool IsElevated
        {
            get
            {
                var id = System.Security.Principal.WindowsIdentity.GetCurrent();
                return id.Owner != id.User;
            }
        }
    }
}
