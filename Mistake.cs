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
        public ImageSource Image => (Importance == IMPORTANCE.ADVICE ? "Resources/ICON_INFO.png".GetImageSource() : Importance == IMPORTANCE.HIGH ? "Resources/ICON_WARNING.png".GetImageSource() : "Resources/ICON_CANCEL.png".GetImageSource());

        public string MistakeName => TranslateName ? MainWindow.Localize(MistakeNameKey) : MistakeNameKey;
        public string MistakeDesc => TranslateDesc ? MainWindow.Localize(MistakeDescKey) : MistakeDescKey;
        public virtual bool IsMistake
        {
            get { return false; }
        }
    }

    public enum IMPORTANCE
    {
        NO_EXPORT, ADVICE, HIGH
    }
}
