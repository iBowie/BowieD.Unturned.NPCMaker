﻿using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes
{
    public class Mistake
    {
        public Mistake() { }
        public string MistakeName { get; set; }
        public string MistakeDesc { get; set; }
        public string MistakeImportance => Importance == IMPORTANCE.ADVICE ? LocUtil.LocalizeInterface("IMPORTANCE_ADVICE") : Importance == IMPORTANCE.WARNING ? LocUtil.LocalizeInterface("IMPORTANCE_WARNING") : LocUtil.LocalizeInterface("IMPORTANCE_CRITICAL");
        public Action OnClick { get; set; }
        public IMPORTANCE Importance = IMPORTANCE.ADVICE;
        public virtual IEnumerable<Mistake> CheckMistake()
        {
            return new List<Mistake>();
        }
    }

    public enum IMPORTANCE
    {
        ADVICE,
        WARNING,
        CRITICAL
    }
}
