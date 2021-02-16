using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Notification;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

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
                    sw.WriteLine($"{s.Key}\t{s.Value}");
            }
        }

        public void SetStat(string name, double data)
        {
            _stats[name] = data;
            
            Save();
        }

        private readonly string[] _visible = new string[]
        {
            "startup",
            "startup10",
            "srsded",
            "justputit",
            "console",
            "trick"
        };

        public IEnumerable<string> VisibleAchievements => _visible;

        private readonly string[] _hidden = new string[]
        {
            "noescape"
        };
        
        public IEnumerable<string> HiddenAchievements => _hidden;
    }
}
