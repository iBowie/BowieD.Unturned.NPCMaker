using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.Themes
{
    public static class ThemeManager
    {
        static readonly BrushConverter _converter = new BrushConverter();

        public static void Init(Coloring.Color newColor, bool isDarkMode)
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml") });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml") });

            Apply(newColor, isDarkMode);
        }

        public static void Apply(Coloring.Color newColor, bool isDarkMode)
        {
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

            ApplyAccentColorToUI(newColor, isDarkMode);
        }

        private static void ApplyAccentColorToUI(Coloring.Color color, bool isDarkMode)
        {
            CreateVariant(byte.MaxValue, color, out var accentBaseBrush, out var accentBaseColor);
            CreateVariant(0xCC, color, out var accentBrush, out var accentColor);
            CreateVariant(0x99, color, out var accent2Brush, out var accent2Color);
            CreateVariant(0x66, color, out var accent3Brush, out var accent3Color);
            CreateVariant(0x33, color, out var accent4Brush, out var accent4Color);
            CreateVariant(byte.MaxValue, Color.Multiply(color, 0.75f), out var highlightBrush, out var highlightColor);

            App.Current.Resources["AccentColor"] = accentBaseBrush;

            if (isDarkMode)
            {
                App.Current.Resources["ForegroundColor"] = _converter.ConvertFromString("#FFFFFF");
                App.Current.Resources["BackgroundColor"] = _converter.ConvertFromString("#252525");
            }
            else
            {
                App.Current.Resources["ForegroundColor"] = _converter.ConvertFromString("#000000");
                App.Current.Resources["BackgroundColor"] = _converter.ConvertFromString("#FFFFFF");
            }

            App.Current.Resources["MahApps.Colors.AccentBase"] = accentBaseColor;
            App.Current.Resources["MahApps.Colors.Accent"] = accentColor;
            App.Current.Resources["MahApps.Colors.Accent2"] = accent2Color;
            App.Current.Resources["MahApps.Colors.Accent3"] = accent3Color;
            App.Current.Resources["MahApps.Colors.Accent4"] = accent4Color;
            App.Current.Resources["MahApps.Colors.Highlight"] = highlightColor;

            App.Current.Resources["MahApps.Brushes.AccentBase"] = accentBaseBrush;
            App.Current.Resources["MahApps.Brushes.Accent"] = accentBrush;
            App.Current.Resources["MahApps.Brushes.Accent2"] = accent2Brush;
            App.Current.Resources["MahApps.Brushes.Accent3"] = accent3Brush;
            App.Current.Resources["MahApps.Brushes.Accent4"] = accent4Brush;
            App.Current.Resources["MahApps.Brushes.Highlight"] = highlightBrush;

            App.Current.Resources["MahApps.Brushes.WindowTitle"] = accentBrush;
            App.Current.Resources["MahApps.Brushes.TextBlock.FloatingMessage"] = accentBaseBrush;
            App.Current.Resources["MahApps.Brushes.Badged.Background"] = accentBaseBrush;
            App.Current.Resources["MahApps.Brushes.Dialog.Background.Accent"] = highlightBrush;
            App.Current.Resources["MahApps.Brushes.Dialog.Glow"] = accentBrush;
            App.Current.Resources["MahApps.Brushes.CheckmarkFill"] = accentBrush;
            App.Current.Resources["MahApps.Brushes.RightArrowFill"] = accentBrush;

            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.Background"] = accentBrush;
            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.Background.Inactive"] = accent3Brush;
            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.Background.MouseOver"] = accent2Brush;
            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.BorderBrush"] = accentBrush;
            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.BorderBrush.Focus"] = accentBrush;
            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.BorderBrush.Inactive"] = accent3Brush;
            App.Current.Resources["MahApps.Brushes.DataGrid.Selection.BorderBrush.MouseOver"] = accent2Brush;

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

        private static void CreateVariant(byte opacity, Coloring.Color color, out SolidColorBrush scb, out Color resultColor)
        {
            resultColor = Color.FromArgb(opacity, color.R, color.G, color.B);
            scb = new SolidColorBrush(resultColor);

            scb.Freeze();
        }
    }
}
