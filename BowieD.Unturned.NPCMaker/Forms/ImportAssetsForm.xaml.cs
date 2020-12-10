using BowieD.Unturned.NPCMaker.Common.Utility;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Localization;
using System.IO;
using System.Linq;
using System.Threading;
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

                                            tokenSource = new CancellationTokenSource();

                                            importTask = new Task(async () =>
                                            {
                                                await Dispatcher.Invoke(async () =>
                                                {
                                                    await ImportGameAssets(AppConfig.Instance.unturnedDir);
                                                });
                                            }, tokenSource.Token);
                                            importTask.Start();
                                        }
                                        else
                                        {
                                            goto ask;
                                        }
                                    }
                                    break;
                                case System.Windows.Forms.DialogResult.No:
                                case System.Windows.Forms.DialogResult.Cancel:
                                    {
                                        Close();
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Close();
                    }
                }
                else
                {
                    tokenSource = new CancellationTokenSource();

                    importTask = new Task(async () =>
                    {
                        await Dispatcher.Invoke(async () =>
                        {
                            await ImportGameAssets(AppConfig.Instance.unturnedDir);
                        });
                    }, tokenSource.Token);
                    importTask.Start();
                }
            };
            Closing += (sender, e) =>
            {
                if (!hasDone)
                {
                    App.Logger.Log("User aborted asset loading");

                    tokenSource?.Cancel(true);

                    GameIntegration.GameAssetManager.Purge();
                    ThumbnailManager.Purge();
                }
            };
        }

        CancellationTokenSource tokenSource;
        Task importTask;
        bool hasDone = false;

        private async Task ImportGameAssets(string mainPath)
        {
            GameIntegration.GameAssetManager.Purge();

            if (AppConfig.Instance.importVanilla)
            {
                stepText.Text = LocalizationManager.Current.Interface.Translate("StartUp_ImportGameAssets_Window_Step_Unturned");

                await GameIntegration.GameAssetManager.Import(mainPath, GameIntegration.EGameAssetOrigin.Unturned, (index, total) =>
                {
                    stepProgress.Value = index;
                    stepProgress.Maximum = total;
                }, tokenSource);

                if (tokenSource.IsCancellationRequested)
                {
                    await App.Logger.Log("Cancelled after import", Logging.ELogLevel.TRACE);
                    GameIntegration.GameAssetManager.Purge();
                    ThumbnailManager.Purge();
                    return;
                }

                GameAssetManager.HasImportedVanilla = true;
            }

            if (AppConfig.Instance.importWorkshop)
            {
                string workshopPath = PathUtility.GetUnturnedWorkshopPathFromUnturnedPath(mainPath);

                stepText.Text = LocalizationManager.Current.Interface.Translate("StartUp_ImportGameAssets_Window_Step_Workshop");

                await GameIntegration.GameAssetManager.Import(workshopPath, GameIntegration.EGameAssetOrigin.Workshop, (index, total) =>
                {
                    stepProgress.Value = index;
                    stepProgress.Maximum = total;
                }, tokenSource);

                if (tokenSource.IsCancellationRequested)
                {
                    await App.Logger.Log("Cancelled after import", Logging.ELogLevel.TRACE);
                    GameIntegration.GameAssetManager.Purge();
                    ThumbnailManager.Purge();
                    return;
                }

                GameAssetManager.HasImportedWorkshop = true;
            }

            if (GameAssetManager.HasImportedAssets)
            {
                stepText.Text = LocalizationManager.Current.Interface.Translate("StartUp_ImportGameAssets_Window_Step_Thumbnails");

                IHasIcon[] assetsWithIcons = GameIntegration.GameAssetManager.GetAllAssetsWithIcons().ToArray();
                for (int i = 0; i < assetsWithIcons.Length; i++)
                {
                    IHasIcon a = assetsWithIcons[i];

                    if (i % 25 == 0)
                    {
                        stepProgress.Value = i;
                        stepProgress.Maximum = assetsWithIcons.Length;

                        await Task.Delay(1);
                    }
                    else
                        await Task.Yield();

                    ThumbnailManager.CreateThumbnail(a.ImagePath);
                }

                if (tokenSource.IsCancellationRequested)
                {
                    await App.Logger.Log("Cancelled after import", Logging.ELogLevel.TRACE);
                    GameIntegration.GameAssetManager.Purge();
                    ThumbnailManager.Purge();
                    return;
                }
            }

            hasDone = true;

            Close();
        }
    }
}
