using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BowieD.Unturned.NPCMaker.Themes
{
    public static class ThemeManager
    {
        static readonly BrushConverter _converter = new BrushConverter();

        public static void Init(Coloring.Color newColor, bool isDarkMode, bool isCuteTheme)
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml") });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml") });

			Apply(newColor, isDarkMode, isCuteTheme);
        }
        public static void Apply(Coloring.Color newColor, bool isDarkMode, bool isCuteTheme)
        {
            if (isCuteTheme)
            {
                isDarkMode = false;
            }

            List<ResourceDictionary> metroThemes = (from d in Application.Current.Resources.MergedDictionaries
                                                    where d.Source != null && d.Source.OriginalString.StartsWith("pack://application:,,,/MahApps.Metro;component/Styles/Themes/")
                                                    select d).ToList();

            if (metroThemes?.Count() > 0)
            {
                foreach (ResourceDictionary dic in metroThemes)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(dic);
                }
            }

            ResourceDictionary resourceDictionary = new ResourceDictionary() { Source = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/{(isDarkMode ? "Dark" : "Light")}.Green.xaml") };

            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            if (isCuteTheme)
			{
				ApplyNormalAccentColorToUI(new Coloring.Color(0xFF, 0xD1, 0xDC), false);
				return;
            }

            if (Configuration.AppConfig.Instance.hasUnlockedSecretThemes)
            {
                switch (Configuration.AppConfig.Instance.themeType)
                {
                    case EThemeType.Rainbow:
                        ApplyRainbow(isDarkMode);
                        break;
                    case EThemeType.ExtraDark:
                        ApplyExtraDarkMode(newColor);
                        break;
                    default:
                        ApplyNormalAccentColorToUI(newColor, isDarkMode);
                        break;
                }
            }
            else
            {
                ApplyNormalAccentColorToUI(newColor, isDarkMode);
            }
        }

        private static void ApplyNormalAccentColorToUI(Coloring.Color color, bool isDarkMode)
        {
            CreateVariant(byte.MaxValue, color, out var accentBaseBrush, out var accentBaseColor);
            CreateVariant(0xCC, color, out var accentBrush, out var accentColor);
            CreateVariant(0x99, color, out var accent2Brush, out var accent2Color);
            CreateVariant(0x66, color, out var accent3Brush, out var accent3Color);
            CreateVariant(0x33, color, out var accent4Brush, out var accent4Color);
            CreateVariant(byte.MaxValue, Color.Multiply(color, 0.75f), out var highlightBrush, out var highlightColor);

            UpdateBrushResourceDictionary(accentBaseBrush, accentBrush, accent2Brush, accent3Brush, accent4Brush, highlightBrush, isDarkMode);
            UpdateColorResourceDictionary(accentBaseColor, accentColor, accent2Color, accent3Color, accent4Color, highlightColor);

            LinearGradientBrush progr = new LinearGradientBrush()
            {
                StartPoint = new Point(1.002, 0.5),
                EndPoint = new Point(0.001, 0.5),
            };

            progr.GradientStops.Add(new GradientStop(highlightColor, 0));
            progr.GradientStops.Add(new GradientStop(accent3Color, 1));

            progr.Freeze();

            App.Current.Resources["MahApps.Brushes.Progress"] = progr;
        }
        private static void ApplyRainbow(bool isDarkMode)
        {
            var accentBaseBrush = CreateRainbowVariant(byte.MaxValue);
            var accentBrush = CreateRainbowVariant(0xCC);
            var accent2Brush = CreateRainbowVariant(0x99);
            var accent3Brush = CreateRainbowVariant(0x66);
            var accent4Brush = CreateRainbowVariant(0x33);
            var highlightBrush = CreateRainbowVariant(byte.MaxValue);

            UpdateBrushResourceDictionary(accentBaseBrush, accentBrush, accent2Brush, accent3Brush, accent4Brush, highlightBrush, isDarkMode);

            LinearGradientBrush progr = new LinearGradientBrush()
            {
                StartPoint = new Point(1.002, 0.5),
                EndPoint = new Point(0.001, 0.5),
            };

            progr.GradientStops.Add(CreateRainbowGradientStop(0xFF, 0));
            progr.GradientStops.Add(CreateRainbowGradientStop(0x66, 1));

            App.Current.Resources["MahApps.Brushes.Progress"] = progr;
        }
        private static void ApplyExtraDarkMode(Coloring.Color color)
        {
            ApplyNormalAccentColorToUI(color, true);

            // const string hex = "#121212";
            const string hex = "#000000";

            Brush back = (Brush)_converter.ConvertFromString(hex);

            back.Freeze();

            App.Current.Resources["BackgroundColor"] = back;

            App.Current.Resources["MahApps.Colors.ThemeBackground"] = CreateColor(hex, 0xFF);
            App.Current.Resources["MahApps.Brushes.ThemeBackground"] = back;
            App.Current.Resources["MahApps.Brushes.Control.Background"] = back;
            App.Current.Resources["MahApps.Brushes.Dialog.Background"] = back;
            App.Current.Resources["MahApps.Brushes.Window.Background"] = back;
            App.Current.Resources["MahApps.Brushes.Menu.Background"] = back;
            App.Current.Resources["MahApps.Brushes.ContextMenu.Background"] = back;
            App.Current.Resources["MahApps.Brushes.SubMenu.Background"] = back;
            App.Current.Resources[SystemColors.WindowBrushKey] = back;
            App.Current.Resources["MahApps.Brushes.MenuItem.Background"] = back;
            App.Current.Resources["MahApps.Brushes.DataGridColumnHeader.Background"] = back;
        }

        #region Util
        private static void CreateVariant(byte opacity, Coloring.Color color, out SolidColorBrush scb, out Color resultColor)
        {
            resultColor = Color.FromArgb(opacity, color.R, color.G, color.B);
            scb = new SolidColorBrush(resultColor);

            scb.Freeze();
        }
        private static Brush CreateRainbowVariant(byte opacity)
        {
            var anim = CreateRainbowAnimation(opacity);

            SolidColorBrush scb = new SolidColorBrush
            {
                Color = CreateColor("#E51400", opacity),
            };

            scb.BeginAnimation(SolidColorBrush.ColorProperty, anim);

            return scb;
        }
        private static GradientStop CreateRainbowGradientStop(byte opacity, double offset)
        {
            var anim = CreateRainbowAnimation(opacity);

            GradientStop gs = new GradientStop
            {
                Color = CreateColor("#E51400", opacity),
                Offset = offset,
            };

            gs.BeginAnimation(GradientStop.ColorProperty, anim);

            return gs;
        }
        private static ColorAnimationBase CreateRainbowAnimation(byte opacity)
        {
            ColorAnimationUsingKeyFrames caukf = new ColorAnimationUsingKeyFrames();

            caukf.KeyFrames.Add(new LinearColorKeyFrame(CreateColor("#E51400", opacity), KeyTime.Uniform));
            caukf.KeyFrames.Add(new LinearColorKeyFrame(CreateColor("#FA6800", opacity), KeyTime.Uniform));
            caukf.KeyFrames.Add(new LinearColorKeyFrame(CreateColor("#FEDE06", opacity), KeyTime.Uniform));
            caukf.KeyFrames.Add(new LinearColorKeyFrame(CreateColor("#60A917", opacity), KeyTime.Uniform));
            caukf.KeyFrames.Add(new LinearColorKeyFrame(CreateColor("#1BA1E2", opacity), KeyTime.Uniform));
            caukf.KeyFrames.Add(new LinearColorKeyFrame(CreateColor("#119EDA", opacity), KeyTime.Uniform));
            caukf.KeyFrames.Add(new LinearColorKeyFrame(CreateColor("#6459DF", opacity), KeyTime.Uniform));

            caukf.AutoReverse = true;
            caukf.RepeatBehavior = RepeatBehavior.Forever;
            caukf.Duration = new Duration(TimeSpan.FromSeconds(21));

            return caukf;
        }
        private static Color CreateColor(string hex, byte opacity)
        {
            var color = new Coloring.Color(hex);

            return Color.FromArgb(opacity, color.R, color.G, color.B);
        }
        #endregion

        private static void UpdateBrushResourceDictionary(Brush accentBase, Brush accent, Brush accent2, Brush accent3, Brush accent4, Brush highlight, bool isDarkMode)
        {
            App.Current.Resources["AccentColor"] = accentBase;

            if (isDarkMode)
            {
                App.Current.Resources["ForegroundColor"] = _converter.ConvertFromString("#FFFFFF");
                App.Current.Resources["MahApps.Brushes.Text"] = _converter.ConvertFromString("#FFFFFF");
                App.Current.Resources["BackgroundColor"] = _converter.ConvertFromString("#252525");
            }
            else
            {
                App.Current.Resources["ForegroundColor"] = _converter.ConvertFromString("#000000");
                App.Current.Resources["MahApps.Brushes.Text"] = _converter.ConvertFromString("#000000");
                App.Current.Resources["BackgroundColor"] = _converter.ConvertFromString("#FFFFFF");
            }

            App.Current.Resources["MahApps.Brushes.AccentBase"] = accentBase;
            App.Current.Resources["MahApps.Brushes.Accent"] = accent;
            App.Current.Resources["MahApps.Brushes.Accent2"] = accent2;
            App.Current.Resources["MahApps.Brushes.Accent3"] = accent3;
            App.Current.Resources["MahApps.Brushes.Accent4"] = accent4;
            App.Current.Resources["MahApps.Brushes.Highlight"] = highlight;

            App.Current.Resources["MahApps.Brushes.WindowTitle"] = accent;
            App.Current.Resources["MahApps.Brushes.TextBlock.FloatingMessage"] = accentBase;
            App.Current.Resources["MahApps.Brushes.Badged.Background"] = accentBase;
            App.Current.Resources["MahApps.Brushes.Dialog.Background.Accent"] = highlight;
            App.Current.Resources["MahApps.Brushes.Dialog.Glow"] = accent;
            App.Current.Resources["MahApps.Brushes.CheckmarkFill"] = accent;
            App.Current.Resources["MahApps.Brushes.RightArrowFill"] = accent;

            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.Background"] = accent;
            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.Background.Inactive"] = accent3;
            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.Background.MouseOver"] = accent2;
            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.BorderBrush"] = accent;
            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.BorderBrush.Focus"] = accent;
            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.BorderBrush.Inactive"] = accent3;
            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.BorderBrush.MouseOver"] = accent2;

            App.Current.Resources["MahApps.Brushes.SystemControlBackgroundAccent"] = accentBase;
            App.Current.Resources["MahApps.Brushes.SystemControlDisabledAccent"] = accentBase;
            App.Current.Resources["MahApps.Brushes.SystemControlForegroundAccent"] = accentBase;
            App.Current.Resources["MahApps.Brushes.SystemControlHighlightAccent"] = accentBase;
            App.Current.Resources["MahApps.Brushes.SystemControlHighlightAltAccent"] = accentBase;
        }
        private static void UpdateColorResourceDictionary(Color accentBase, Color accent, Color accent2, Color accent3, Color accent4, Color highlight)
        {
            App.Current.Resources["MahApps.Colors.AccentBase"] = accentBase;
            App.Current.Resources["MahApps.Colors.Accent"] = accent;
            App.Current.Resources["MahApps.Colors.Accent2"] = accent2;
            App.Current.Resources["MahApps.Colors.Accent3"] = accent3;
            App.Current.Resources["MahApps.Colors.Accent4"] = accent4;
            App.Current.Resources["MahApps.Colors.Highlight"] = highlight;

            App.Current.Resources["MahApps.Colors.SystemAccent"] = accentBase;
        }
    }
}
