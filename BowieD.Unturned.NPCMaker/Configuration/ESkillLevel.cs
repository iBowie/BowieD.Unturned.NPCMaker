using System;

namespace BowieD.Unturned.NPCMaker.Configuration
{
    public enum ESkillLevel
    {
        None = 0,
        /// <summary>
        /// Conditions, rewards, quests, advanced dialogue options and currency are locked
        /// </summary>
        Beginner = 1,
        /// <summary>
        /// Advanced conditions and rewards, advanced dialogue options are locked
        /// </summary>
        Intermediate = 2,
        /// <summary>
        /// Everything is unlocked
        /// </summary>
        Advanced = 3,
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SkillLockAttribute : Attribute
    {
        public SkillLockAttribute(ESkillLevel level)
        {
            this.Level = level;
        }

        public ESkillLevel Level { get; }
    }
}
