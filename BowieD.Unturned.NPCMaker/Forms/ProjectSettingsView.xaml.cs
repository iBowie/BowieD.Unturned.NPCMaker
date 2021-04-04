using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.ViewModels;
using MahApps.Metro.Controls;
using System.IO;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для ProjectSettingsView.xaml
    /// </summary>
    public partial class ProjectSettingsView : Window
    {
        public readonly NPCProject project;

        private bool _isSaving;

        public ProjectSettingsView(NPCProject contextProject)
        {
            InitializeComponent();

            this.project = contextProject;

            foreach (var dir in project.settings.assetDirs)
            {
                DirectoryInfo di = new DirectoryInfo(dir);

                Universal_ItemList uil = new Universal_ItemList(di, true);

                AddToHookedDirs(uil);
            }

            hookedAddDirButton.Command = new AdvancedCommand(() =>
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog()
                {

                };

                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);

                    Universal_ItemList uil = new Universal_ItemList(di, true);

                    AddToHookedDirs(uil);
                }
            });

            saveButton.Command = new AdvancedCommand(async () =>
            {
                _isSaving = true;

                buttonsPanel.Dispatcher.Invoke(() =>
                {
                    buttonsPanel.Visibility = Visibility.Collapsed;
                });
                progressPanel.Dispatcher.Invoke(() =>
                {
                    progressPanel.Visibility = Visibility.Visible;
                });
                paramsPanel.Dispatcher.Invoke(() =>
                {
                    paramsPanel.IsEnabled = false;
                });

                project.settings.assetDirs.Clear();

                foreach (Universal_ItemList uil in hookedStackPanel.Children)
                {
                    project.settings.assetDirs.Add((uil.Value as DirectoryInfo).FullName);
                }

                GameAssetManager.Purge(EGameAssetOrigin.Hooked);

                if (AppConfig.Instance.importHooked)
                {
                    foreach (var dir in project.settings.assetDirs)
                    {
                        await GameAssetManager.Import(dir, EGameAssetOrigin.Hooked, (index, total) =>
                        {
                            progBar.Dispatcher.Invoke(() =>
                            {
                                progBar.Value = index;
                                progBar.Maximum = total;
                            });
                        });
                    }
                }

                Close();
            }, (p) =>
            {
                return !_isSaving;
            });

            cancelButton.Command = new AdvancedCommand(() =>
            {
                Close();
            }, (p) =>
            {
                return !_isSaving;
            });
        }

        private void AddToHookedDirs(Controls.Universal_ItemList uil)
        {
            uil.deleteButton.Click += (sender, e) =>
            {
                hookedStackPanel.Children.Remove(uil);
            };

            uil.ShowMoveButtons = true;

            if (uil.ShowMoveButtons)
            {
                uil.moveUpButton.Click += (sender, e) =>
                {
                    hookedStackPanel.MoveUp((sender as UIElement).TryFindParent<Universal_ItemList>());
                };
                uil.moveDownButton.Click += (sender, e) =>
                {
                    hookedStackPanel.MoveDown((sender as UIElement).TryFindParent<Universal_ItemList>());
                };
            }
            hookedStackPanel.Children.Add(uil);
            hookedStackPanel.UpdateOrderButtons<Universal_ItemList>();
        }
    }
}
