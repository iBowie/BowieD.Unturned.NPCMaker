using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
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
            sb.AppendLine($"NPC Maker {App.Version}");
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
            sb.AppendLine($"Settings:");
            try
            {
                AppConfig cfg = AppConfig.Instance;
                sb.AppendLine($".animateControls: {cfg.animateControls}");
                sb.AppendLine($".autosaveOption: {cfg.autosaveOption}");
                sb.AppendLine($".autoUpdate: {cfg.autoUpdate}");
                sb.AppendLine($".enableDiscord: {cfg.enableDiscord}");
                sb.AppendLine($".experimentalFeatures: {cfg.experimentalFeatures}");
                sb.AppendLine($".generateGuids: {cfg.generateGuids}");
                sb.AppendLine($".language: {cfg.language}");
                sb.AppendLine($".scale: {cfg.scale}");
                sb.AppendLine($".downloadPrerelease: {cfg.downloadPrerelease}");
                sb.AppendLine($".alternateLogicTranslation: {cfg.alternateLogicTranslation}");
                sb.AppendLine($".replaceMissingKeysWithEnglish: {cfg.replaceMissingKeysWithEnglish}");
                sb.AppendLine($".useCommentsInsteadOfData: {cfg.useCommentsInsteadOfData}");
                sb.AppendLine($".unturnedDir: {cfg.unturnedDir ?? "NULL"}");
                sb.AppendLine($".importVanilla: {cfg.importVanilla}");
                sb.AppendLine($".importWorkshop: {cfg.importWorkshop}");
                sb.AppendLine($".generateThumbnailsBeforehand: {cfg.generateThumbnailsBeforehand}");
                sb.AppendLine($".highlightSearch: {cfg.highlightSearch}");

                try
                {
                    if (GameAssetManager.HasImportedAssets)
                    {
                        sb.AppendLine($"+Imported {GameAssetManager.ImportedAssetCount} assets");
                    }
                    if (cfg.generateThumbnailsBeforehand)
                    {
                        sb.AppendLine($"+Generated {ThumbnailManager.GeneratedThumbnailCount} thumbnails");
                    }
                }
                catch
                {
                    sb.AppendLine("-Could not get asset info");
                }
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
                System.Security.Principal.WindowsIdentity id = System.Security.Principal.WindowsIdentity.GetCurrent();
                return id.Owner != id.User;
            }
        }
    }
}
