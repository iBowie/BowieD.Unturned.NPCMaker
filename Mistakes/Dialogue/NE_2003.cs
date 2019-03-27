﻿using System;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// Dialogue duplicate
    /// </summary>
    public class NE_2003 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.NO_EXPORT;
        public override string MistakeNameKey => "NE_2003";
        public override bool TranslateName => false;
        public override string MistakeDescKey => "NE_2003_Desc";
        public override bool IsMistake => MainWindow.CurrentNPC.dialogues.Any(d => MainWindow.CurrentNPC.dialogues.Count(k => k.id == d.id) > 1);
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 2;
        };
    }
}