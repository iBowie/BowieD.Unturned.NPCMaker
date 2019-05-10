using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker
{
    public abstract class Mistake
    {
        public virtual bool TranslateName => true;
        public virtual bool TranslateDesc => true;

        public virtual Action OnClick => null;
        public virtual string MistakeNameKey => "";
        public virtual string MistakeDescKey => "";
        public virtual IMPORTANCE Importance => IMPORTANCE.ADVICE;
        public ImageSource Image => (Importance == IMPORTANCE.ADVICE ? "Resources/ICON_INFO.png".GetImageSource() : Importance == IMPORTANCE.WARNING ? "Resources/ICON_WARNING.png".GetImageSource() : "Resources/ICON_CANCEL.png".GetImageSource());

        public string MistakeName => TranslateName ? LocUtil.LocalizeInterface(MistakeNameKey) : MistakeNameKey;
        public string MistakeDesc => TranslateDesc ? LocUtil.LocalizeInterface(MistakeDescKey) : MistakeDescKey;
        public string MistakeImportance => Importance == IMPORTANCE.ADVICE ? LocUtil.LocalizeInterface("IMPORTANCE_ADVICE") : Importance == IMPORTANCE.WARNING ? LocUtil.LocalizeInterface("IMPORTANCE_WARNING") : LocUtil.LocalizeInterface("IMPORTANCE_CRITICAL");
        public virtual bool IsMistake
        {
            get { return false; }
        }
    }

    public enum IMPORTANCE
    {
        ADVICE,
        WARNING,
        CRITICAL
    }
}
