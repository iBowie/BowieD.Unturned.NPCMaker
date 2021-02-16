using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Achievements
{
    public interface IAchievementManager
    {
        void Load();
        void Save();

        void SetStat(string name, double data);
        double GetStat(string name);
        bool TryGiveAchievement(string name);
        bool HasAchievement(string name);

        IEnumerable<string> VisibleAchievements { get; }
        IEnumerable<string> HiddenAchievements { get; }
    }
}
