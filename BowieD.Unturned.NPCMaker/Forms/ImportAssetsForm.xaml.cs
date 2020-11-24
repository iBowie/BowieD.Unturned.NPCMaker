using BowieD.Unturned.NPCMaker.Common.Utility;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для ImportAssetsForm.xaml
    /// </summary>
    public partial class ImportAssetsForm : Window
    {
        public ImportAssetsForm()
        {
            InitializeComponent();

            Loaded += async (sender, e) =>
            {
                if (string.IsNullOrEmpty(AppConfig.Instance.unturnedDir) || !Directory.Exists(AppConfig.Instance.unturnedDir))
                {
                    var res = MessageBox.Show(LocalizationManager.Current.Interface.Translate("StartUp_ImportGameAssets_Content"), LocalizationManager.Current.Interface.Translate("StartUp_ImportGameAssets_Title"), MessageBoxButton.YesNo);

                    if (res == MessageBoxResult.Yes)
                    {
                    ask:
                        {
                            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog
                            {
                                Description = LocalizationManager.Current.Interface.Translate("StartUp_ImportGameAssets_fbd")
                            };
                            switch (fbd.ShowDialog())
                            {
                                case System.Windows.Forms.DialogResult.Yes:
                                case System.Windows.Forms.DialogResult.OK:
                                    {
                                        if (PathUtility.IsUnturnedPath(fbd.SelectedPath))
                                        {
                                            AppConfig.Instance.unturnedDir = fbd.SelectedPath;
                                            AppConfig.Instance.Save();

                                            await ImportGameAssets(fbd.SelectedPath);
                                        }
                                        else
                                        {
                                            goto ask;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    await ImportGameAssets(AppConfig.Instance.unturnedDir);
                }
            };
        }

        private async Task ImportGameAssets(string mainPath)
        {
            GameIntegration.GameAssetManager.Purge();

            stepText.Text = LocalizationManager.Current.Interface.Translate("StartUp_ImportGameAssets_Window_Step_Unturned");

            await GameIntegration.GameAssetManager.Import(mainPath, GameIntegration.EGameAssetOrigin.Unturned, (index, total) =>
            {
                stepProgress.Value = index;
                stepProgress.Maximum = total;
            });

            string workshopPath = PathUtility.GetUnturnedWorkshopPathFromUnturnedPath(mainPath);

            stepText.Text = LocalizationManager.Current.Interface.Translate("StartUp_ImportGameAssets_Window_Step_Workshop");

            await GameIntegration.GameAssetManager.Import(workshopPath, GameIntegration.EGameAssetOrigin.Workshop, (index, total) =>
            {
                stepProgress.Value = index;
                stepProgress.Maximum = total;
            });

            Close();
        }
    }
}
