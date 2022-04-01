using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Themes;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;

            if (AppConfig.Instance.animateControls)
            {
                DoubleAnimation da = new DoubleAnimation(0, 1, new Duration(new System.TimeSpan(0, 0, 1)));
                authorText.BeginAnimation(OpacityProperty, da);
            }

            foreach (string patron in App.Package.Patrons)
            {
                patronsList.Items.Add(patron);
            }

            foreach (System.Collections.Generic.KeyValuePair<string, string> credit in App.Package.Credits)
            {
                creditsList.Items.Add(credit);
            }

            PreviewKeyDown += Form_About_PreviewKeyDown;
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
                    }

                    if (_currentTrickKeys.Count == _trickKeys.Length)
                    {
                        AppConfig.Instance.hasUnlockedSecretThemes = true;
                        AppConfig.Instance.themeType = Themes.EThemeType.Rainbow;
                        AppConfig.Instance.Save();
                        ThemeManager.Apply(AppConfig.Instance.accentColor, AppConfig.Instance.useDarkMode);
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
    }
}
