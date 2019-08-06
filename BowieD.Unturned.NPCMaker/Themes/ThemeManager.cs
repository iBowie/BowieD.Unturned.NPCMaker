using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Themes
{
    public static class ThemeManager
    {
        public static Theme CurrentTheme { get; set; }
        public static void Apply(Theme t)
        {
            CurrentTheme?.Remove();
            t.Apply();
            CurrentTheme = t;
        }
        public static readonly Dictionary<string, Theme> Themes = new Dictionary<string, Theme>()
        {
            {
                "Metro/LightGreen", new MetroTheme()
                {
                    Name = "Metro/LightGreen",
                    DictionaryName = "Light.Green",
                    R = 84,
                    G = 142,
                    B = 25
                }
            }
        };
    }
}
