using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Mistakes
{
    public static class MistakesManager
    {
        public static void FindMistakes()
        {
            if (CheckMistakes == null)
            {
                CheckMistakes = new HashSet<Mistake>();
                string[] nspaces = {
                    "BowieD.Unturned.NPCMaker.Mistakes.Character",
                    "BowieD.Unturned.NPCMaker.Mistakes.Dialogue",
                    "BowieD.Unturned.NPCMaker.Mistakes.Vendor",
                    "BowieD.Unturned.NPCMaker.Mistakes.Quest"
                };
                var q = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass && nspaces.Contains(t.Namespace) select t;
                foreach (Type t in q)
                {
                    try
                    {
                        var mistake = Activator.CreateInstance(t);
                        if (mistake is Mistake mist)
                            CheckMistakes.Add(mist);
                    }
                    catch { }
                }
            }
            MainWindow.Instance.lstMistakes.Items.Clear();
            FoundMistakes.Clear();
            foreach (Mistake m in CheckMistakes)
            {
                var mistakes = m.CheckMistake();
                foreach (var fm in mistakes)
                {
                    FoundMistakes.Add(fm);
                    MainWindow.Instance.lstMistakes.Items.Add(fm);
                }
            }
            if (MainWindow.Instance.lstMistakes.Items.Count == 0)
            {
                MainWindow.Instance.lstMistakes.Visibility = Visibility.Collapsed;
                MainWindow.Instance.noErrorsLabel.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.lstMistakes.Visibility = Visibility.Visible;
                MainWindow.Instance.noErrorsLabel.Visibility = Visibility.Collapsed;
            }
        }
        private static HashSet<Mistake> CheckMistakes;
        public static int Advices_Count => FoundMistakes.Count(d => d.Importance == IMPORTANCE.ADVICE);
        public static int Warnings_Count => FoundMistakes.Count(d => d.Importance == IMPORTANCE.WARNING);
        public static int Criticals_Count => FoundMistakes.Count(d => d.Importance == IMPORTANCE.CRITICAL);
        public static HashSet<Mistake> FoundMistakes { get; } = new HashSet<Mistake>();
    }
}
