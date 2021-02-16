using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Notification;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Achievements
{
    public sealed class AchievementManager : IAchievementManager
    {
        private readonly INotificationManager notifications;

        private readonly Dictionary<string, double> _stats;
        private readonly HashSet<string> _achievements;

        private readonly string pathAchievements, pathStats;
        public AchievementManager(INotificationManager notificationManager)
        {
            this.notifications = notificationManager;

            _stats = new Dictionary<string, double>();
            _achievements = new HashSet<string>();

            pathAchievements = Path.Combine(AppConfig.Directory, "achievements.txt");
            pathStats = Path.Combine(AppConfig.Directory, "stats.txt");
        }

        public double GetStat(string name)
        {
            if (_stats.TryGetValue(name, out var val))
                return val;
            return 0;
        }

        public bool TryGiveAchievement(string name)
        {
            bool res = _achievements.Add(name);


            if (res)
            {
                Save();

                string locName = LocalizationManager.Current.Achievements[$"{name}_Name"];
                string locDesc = LocalizationManager.Current.Achievements[$"{name}_Desc"];

                notifications.NotifyAchievement(locName, locDesc);

                rebuildTab();
            }

            return res;
        }

        public bool HasAchievement(string name)
        {
            return _achievements.Contains(name);
        }

        public void Load()
        {
            _achievements.Clear();
            _stats.Clear();

            if (File.Exists(pathAchievements))
            {
                using (StreamReader sr = new StreamReader(pathAchievements))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (string.IsNullOrEmpty(line))
                            continue;

                        _achievements.Add(line);
                    }
                }
            }

            if (File.Exists(pathStats))
            {
                using (StreamReader sr = new StreamReader(pathStats))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (string.IsNullOrEmpty(line))
                            continue;

                        string[] sep = line.Split('\t');

                        _stats[sep[0]] = double.Parse(sep[1], CultureInfo.InvariantCulture);
                    }
                }
            }

            rebuildTab();
        }

        public void Save()
        {
            using (StreamWriter sw = new StreamWriter(pathAchievements))
            {
                foreach (var a in _achievements)
                    sw.WriteLine(a);
            }

            using (StreamWriter sw = new StreamWriter(pathStats))
            {
                foreach (var s in _stats)
                    sw.WriteLine($"{s.Key}\t{s.Value.ToString(CultureInfo.InvariantCulture)}");
            }
        }

        public void SetStat(string name, double data, bool immediateSave = true)
        {
            _stats[name] = data;

            if (immediateSave)
                Save();
        }

        private readonly string[] _visible = new string[]
        {
            "startup",
            "startup10",
            "srsded",
            "justputit",
            "console",
            "sweep",
            "trick"
        };

        public IEnumerable<string> VisibleAchievements => _visible;

        private readonly string[] _hidden = new string[]
        {
            "noescape",
            "ohno",
            "sus"
        };

        public IEnumerable<string> HiddenAchievements => _hidden;

        void rebuildTab()
        {
            MainWindow.Instance.achStackPanel.Children.Clear();

            UIElement buildAchievement(string name, bool isHidden, out int order)
            {
                Grid g = new Grid()
                {
                    Margin = new Thickness(5)
                };

                g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                g.RowDefinitions.Add(new RowDefinition());

                Label l1 = new Label();
                TextBlock t1 = new TextBlock()
                {
                    FontSize = 18
                };
                l1.Content = t1;

                Label l2 = new Label();
                TextBlock t2 = new TextBlock();
                l2.Content = t2;

                g.Children.Add(l1);
                g.Children.Add(l2);

                Grid.SetRow(l2, 1);

                string locName = LocalizationManager.Current.Achievements[$"{name}_Name"];
                string locDesc = LocalizationManager.Current.Achievements[$"{name}_Desc"];

                if (locDesc.StartsWith("<nowrap>"))
                    locDesc = locDesc.Substring(8);

                if (isHidden)
                {
                    if (HasAchievement(name))
                    {
                        t1.Text = locName;
                        t2.Text = locDesc;

                        order = -1;
                    }
                    else
                    {
                        t1.Text = "???";
                        t2.Text = "???";

                        g.Opacity = 0.5;

                        order = 1;
                    }
                }
                else
                {
                    t1.Text = locName;
                    t2.Text = locDesc;

                    if (HasAchievement(name))
                    {
                        order = -1;
                    }
                    else
                    {
                        g.Opacity = 0.5;

                        order = 0;
                    }
                }

                return g;
            }

            List<Tuple<UIElement, int>> lst = new List<Tuple<UIElement, int>>();

            foreach (var va in VisibleAchievements)
            {
                var res = buildAchievement(va, false, out int order);

                lst.Add(new Tuple<UIElement, int>(res, order));
            }
            foreach (var va in HiddenAchievements)
            {
                var res = buildAchievement(va, true, out int order);

                lst.Add(new Tuple<UIElement, int>(res, order));
            }

            foreach (var item in lst.OrderBy(d => d.Item2))
            {
                MainWindow.Instance.achStackPanel.Children.Add(item.Item1);
            }
        }
    }
}
