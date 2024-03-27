using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Themes;
using BowieD.Unturned.NPCMaker.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Form_About.xaml
    /// </summary>
    public partial class Form_About : MetroWindow
    {
        private static readonly int[] _trickKeys = new int[]
        {
            24, 24, 26, 26, 23, 25, 23, 25, 45, 44
        };

        public Form_About()
        {
            InitializeComponent();
            string aboutText = LocalizationManager.Current.Interface.Translate("App_About", LocalizationManager.Current.Author, App.Version);
            mainText.Text = aboutText;
            double scale = AppConfig.Instance.scale;
            Height *= scale;
            Width *= scale;

            if (!(App.Package is null))
			{
				if (!(App.Package.Patrons is null))
				{
					foreach (string patron in App.Package.Patrons)
					{
						patronsList.Items.Add(patron);
					}
				}

                if (!(App.Package.Credits is null))
				{
					foreach (var credit in App.Package.Credits)
					{
						creditsList.Items.Add(credit);
					}
				}
			}

            PreviewKeyDown += Form_About_PreviewKeyDown;

            makerLicenseButton.Command = new BaseCommand(() =>
            {
                string licText;

                var sinfo = App.GetResourceStream(new Uri("pack://application:,,,/Resources/LICENSE.txt"));
                using (var stream = sinfo.Stream)
                using (var sr = new StreamReader(stream))
                {
                    licText = sr.ReadToEnd();
                }

                PlainTextWindow ptw = new PlainTextWindow(LocalizationManager.Current.Interface["App_About_Licenses_NPCMaker_Title"], licText);
                ptw.ShowDialog();
            });

            thirdPartyLicenseButton.Command = new BaseCommand(() =>
            {
                string licText;

                var sinfo = App.GetResourceStream(new Uri("pack://application:,,,/Resources/THIRD_PARTY_LICENSES.txt"));
                using (var stream = sinfo.Stream)
                using (var sr = new StreamReader(stream))
                {
                    licText = sr.ReadToEnd();
                }

                PlainTextWindow ptw = new PlainTextWindow(LocalizationManager.Current.Interface["App_About_Licenses_ThirdParty_Title"], licText);
                ptw.ShowDialog();
            });
        }

        private List<int> _currentTrickKeys = new List<int>();
        private void Form_About_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (AppConfig.Instance.hasUnlockedSecretThemes)
                return;

            if (_currentTrickKeys.Count == 0)
            {
                if (e.Key == System.Windows.Input.Key.Up)
                {
                    e.Handled = true;

                    _currentTrickKeys.Add((int)e.Key);
                    BeginShake(new Point(0, -10));
                }
            }
            else
            {
                _currentTrickKeys.Add((int)e.Key);
                bool isGood = true;

                for (int i = 0; i < _currentTrickKeys.Count; i++)
                {
                    if (_currentTrickKeys[i] != _trickKeys[i])
                    {
                        isGood = false;
                        break;
                    }
                }

                if (isGood)
                {
                    e.Handled = true;

                    switch (e.Key)
                    {
                        case System.Windows.Input.Key.Up:
                            BeginShake(new Point(0, -10));
                            break;
                        case System.Windows.Input.Key.Down:
                            BeginShake(new Point(0, 10));
                            break;
                        case System.Windows.Input.Key.Left:
                            BeginShake(new Point(-10, 0));
                            break;
                        case System.Windows.Input.Key.Right:
                            BeginShake(new Point(10, 0));
                            break;
                        case System.Windows.Input.Key.A:
                            BeginScale(1.25);
                            break;
                        case System.Windows.Input.Key.B:
                            BeginScale(0.75);
                            break;
                    }

                    if (_currentTrickKeys.Count == _trickKeys.Length)
                    {
                        AppConfig.Instance.hasUnlockedSecretThemes = true;
                        AppConfig.Instance.themeType = Themes.EThemeType.Rainbow;
                        AppConfig.Instance.Save();
                        ThemeManager.Apply(AppConfig.Instance.accentColor, AppConfig.Instance.useDarkMode, AppConfig.Instance.useCuteTheme);
                    }
                }
                else
                {
                    _currentTrickKeys.Clear();
                }
            }
        }

        private CancellationTokenSource _cts;
        private void BeginShake(Point shakeAmount)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }

            _cts = new CancellationTokenSource();

            Task.Factory.StartNew(async () =>
            {
                await Shake(shakeAmount, _cts.Token);
            });
        }
        private async Task Shake(Point shakeAmount, CancellationToken token)
        {
            await Dispatcher.Invoke(async () =>
            {
                var left = this.Left;
                var top = this.Top;

                Stopwatch sw = new Stopwatch();
                sw.Start();
                const double duration = 0.2;
                const double halfDur = duration / 2.0;

                double ts;

                while ((ts = sw.Elapsed.TotalSeconds) < duration)
                {
                    if (token.IsCancellationRequested)
                        break;

                    if (ts >= halfDur)
                    {
                        var t = 1.0 - ((ts - halfDur) / halfDur);

                        this.Left = left + (shakeAmount.X * t);
                        this.Top = top + (shakeAmount.Y * t);
                    }
                    else
                    {
                        var t = ts / halfDur;

                        this.Left = left + (shakeAmount.X * t);
                        this.Top = top + (shakeAmount.Y * t);
                    }

                    await Task.Delay(10, token);
                }

                this.Left = left;
                this.Top = top;
            });
        }

        private void BeginScale(double amount)
        {
            DoubleAnimation da = new DoubleAnimation()
            {
                From = 1.0,
                To = amount,
                AutoReverse = true,
                Duration = new Duration(TimeSpan.FromSeconds(0.1)),
                FillBehavior = FillBehavior.Stop,
            };

            secretScale.CenterX = ActualWidth / 2.0;
            secretScale.CenterY = ActualHeight / 2.0;

            secretScale.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            secretScale.BeginAnimation(ScaleTransform.ScaleYProperty, da);
        }
    }
}
