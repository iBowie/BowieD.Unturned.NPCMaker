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
                "Metro/LightAmber", new MetroTheme()
                {
                    Name = "Metro/LightAmber",
                    DictionaryName = "Light.Amber",
                    AccentColor = "#F0A30A"
                }
            },
            {
                "Metro/LightBlue", new MetroTheme()
                {
                    Name = "Metro/LightBlue",
                    DictionaryName = "Light.Blue",
                    AccentColor = "#119EDA"
                }
            },
            {
                "Metro/LightBrown", new MetroTheme()
                {
                    Name = "Metro/LightBrown",
                    DictionaryName = "Light.Brown",
                    AccentColor = "#825A2C"
                }
            },
            {
                "Metro/LightCobalt", new MetroTheme()
                {
                    Name = "Metro/LightCobalt",
                    DictionaryName = "Light.Cobalt",
                    AccentColor = "#0050EF"
                }
            },
            {
                "Metro/LightCrimson", new MetroTheme()
                {
                    Name = "Metro/LightCrimson",
                    DictionaryName = "Light.Crimson",
                    AccentColor = "#A20025"
                }
            },
            {
                "Metro/LightCyan", new MetroTheme()
                {
                    Name = "Metro/LightCyan",
                    DictionaryName = "Light.Cyan",
                    AccentColor = "#1BA1E2"
                }
            },
            {
                "Metro/LightEmerald", new MetroTheme()
                {
                    Name = "Metro/LightEmerald",
                    DictionaryName = "Light.Emerald",
                    AccentColor = "#008A00"
                }
            },
            {
                "Metro/LightGreen", new MetroTheme()
                {
                    Name = "Metro/LightGreen",
                    DictionaryName = "Light.Green",
                    AccentColor = "#60A917"
                }
            },
            {
                "Metro/LightIndigo", new MetroTheme()
                {
                    Name = "Metro/LightIndigo",
                    DictionaryName = "Light.Indigo",
                    AccentColor = "#6A00FF"
                }
            },
            {
                "Metro/LightLime", new MetroTheme()
                {
                    Name = "Metro/LightLime",
                    DictionaryName = "Light.Lime",
                    AccentColor = "#A4C400"
                }
            },
            {
                "Metro/LightMagenta", new MetroTheme()
                {
                    Name = "Metro/LightMagenta",
                    DictionaryName = "Light.Magenta",
                    AccentColor = "#D80073"
                }
            },
            {
                "Metro/LightMauve", new MetroTheme()
                {
                    Name = "Metro/LightMauve",
                    DictionaryName = "Light.Mauve",
                    AccentColor = "#76608A"
                }
            },
            {
                "Metro/LightOlive", new MetroTheme()
                {
                    Name = "Metro/LightOlive",
                    DictionaryName = "Light.Olive",
                    AccentColor = "#6D8764"
                }
            },
            {
                "Metro/LightOrange", new MetroTheme()
                {
                    Name = "Metro/LightOrange",
                    DictionaryName = "Light.Orange",
                    AccentColor = "#FA6800"
                }
            },
            {
                "Metro/LightPink", new MetroTheme()
                {
                    Name = "Metro/LightPink",
                    DictionaryName = "Light.Pink",
                    AccentColor = "#F472D0"
                }
            },
            {
                "Metro/LightPurple", new MetroTheme()
                {
                    Name = "Metro/LightPurple",
                    DictionaryName = "Light.Purple",
                    AccentColor = "#6459DF"
                }
            },
            {
                "Metro/LightRed", new MetroTheme()
                {
                    Name = "Metro/LightRed",
                    DictionaryName = "Light.Red",
                    AccentColor = "#E51400"
                }
            },
            {
                "Metro/LightSienna", new MetroTheme()
                {
                    Name = "Metro/LightSienna",
                    DictionaryName = "Light.Sienna",
                    AccentColor = "#A0522D"
                }
            },
            {
                "Metro/LightSteel", new MetroTheme()
                {
                    Name = "Metro/LightSteel",
                    DictionaryName = "Light.Steel",
                    AccentColor = "#647687"
                }
            },
            {
                "Metro/LightTaupe", new MetroTheme()
                {
                    Name = "Metro/LightTaupe",
                    DictionaryName = "Light.Taupe",
                    AccentColor = "#87794E"
                }
            },
            {
                "Metro/LightTeal", new MetroTheme()
                {
                    Name = "Metro/LightTeal",
                    DictionaryName = "Light.Teal",
                    AccentColor = "#00ABA9"
                }
            },
            {
                "Metro/LightViolet", new MetroTheme()
                {
                    Name = "Metro/LightViolet",
                    DictionaryName = "Light.Violet",
                    AccentColor = "#AA00FF"
                }
            },
            {
                "Metro/LightYellow", new MetroTheme()
                {
                    Name = "Metro/LightYellow",
                    DictionaryName = "Light.Yellow",
                    AccentColor = "#FEDE06"
                }
            },
            {
                "Metro/DarkAmber", new MetroTheme()
                {
                    Name = "Metro/DarkAmber",
                    DictionaryName = "Dark.Amber",
                    AccentColor = "#F0A30A"
                }
            },
            {
                "Metro/DarkBlue", new MetroTheme()
                {
                    Name = "Metro/DarkBlue",
                    DictionaryName = "Dark.Blue",
                    AccentColor = "#119EDA"
                }
            },
            {
                "Metro/DarkBrown", new MetroTheme()
                {
                    Name = "Metro/DarkBrown",
                    DictionaryName = "Dark.Brown",
                    AccentColor = "#825A2C"
                }
            },
            {
                "Metro/DarkCobalt", new MetroTheme()
                {
                    Name = "Metro/DarkCobalt",
                    DictionaryName = "Dark.Cobalt",
                    AccentColor = "#0050EF"
                }
            },
            {
                "Metro/DarkCrimson", new MetroTheme()
                {
                    Name = "Metro/DarkCrimson",
                    DictionaryName = "Dark.Crimson",
                    AccentColor = "#A20025"
                }
            },
            {
                "Metro/DarkCyan", new MetroTheme()
                {
                    Name = "Metro/DarkCyan",
                    DictionaryName = "Dark.Cyan",
                    AccentColor = "#1BA1E2"
                }
            },
            {
                "Metro/DarkEmerald", new MetroTheme()
                {
                    Name = "Metro/DarkEmerald",
                    DictionaryName = "Dark.Emerald",
                    AccentColor = "#008A00"
                }
            },
            {
                "Metro/DarkGreen", new MetroTheme()
                {
                    Name = "Metro/DarkGreen",
                    DictionaryName = "Dark.Green",
                    AccentColor = "#60A917"
                }
            },
            {
                "Metro/DarkIndigo", new MetroTheme()
                {
                    Name = "Metro/DarkIndigo",
                    DictionaryName = "Dark.Indigo",
                    AccentColor = "#6A00FF"
                }
            },
            {
                "Metro/DarkLime", new MetroTheme()
                {
                    Name = "Metro/DarkLime",
                    DictionaryName = "Dark.Lime",
                    AccentColor = "#A4C400"
                }
            },
            {
                "Metro/DarkMagenta", new MetroTheme()
                {
                    Name = "Metro/DarkMagenta",
                    DictionaryName = "Dark.Magenta",
                    AccentColor = "#D80073"
                }
            },
            {
                "Metro/DarkMauve", new MetroTheme()
                {
                    Name = "Metro/DarkMauve",
                    DictionaryName = "Dark.Mauve",
                    AccentColor = "#76608A"
                }
            },
            {
                "Metro/DarkOlive", new MetroTheme()
                {
                    Name = "Metro/DarkOlive",
                    DictionaryName = "Dark.Olive",
                    AccentColor = "#6D8764"
                }
            },
            {
                "Metro/DarkOrange", new MetroTheme()
                {
                    Name = "Metro/DarkOrange",
                    DictionaryName = "Dark.Orange",
                    AccentColor = "#FA6800"
                }
            },
            {
                "Metro/DarkPink", new MetroTheme()
                {
                    Name = "Metro/DarkPink",
                    DictionaryName = "Dark.Pink",
                    AccentColor = "#F472D0"
                }
            },
            {
                "Metro/DarkPurple", new MetroTheme()
                {
                    Name = "Metro/DarkPurple",
                    DictionaryName = "Dark.Purple",
                    AccentColor = "#6459DF"
                }
            },
            {
                "Metro/DarkRed", new MetroTheme()
                {
                    Name = "Metro/DarkRed",
                    DictionaryName = "Dark.Red",
                    AccentColor = "#E51400"
                }
            },
            {
                "Metro/DarkSienna", new MetroTheme()
                {
                    Name = "Metro/DarkSienna",
                    DictionaryName = "Dark.Sienna",
                    AccentColor = "#A0522D"
                }
            },
            {
                "Metro/DarkSteel", new MetroTheme()
                {
                    Name = "Metro/DarkSteel",
                    DictionaryName = "Dark.Steel",
                    AccentColor = "#647687"
                }
            },
            {
                "Metro/DarkTaupe", new MetroTheme()
                {
                    Name = "Metro/DarkTaupe",
                    DictionaryName = "Dark.Taupe",
                    AccentColor = "#87794E"
                }
            },
            {
                "Metro/DarkTeal", new MetroTheme()
                {
                    Name = "Metro/DarkTeal",
                    DictionaryName = "Dark.Teal",
                    AccentColor = "#00ABA9"
                }
            },
            {
                "Metro/DarkViolet", new MetroTheme()
                {
                    Name = "Metro/DarkViolet",
                    DictionaryName = "Dark.Violet",
                    AccentColor = "#AA00FF"
                }
            },
            {
                "Metro/DarkYellow", new MetroTheme()
                {
                    Name = "Metro/DarkYellow",
                    DictionaryName = "Dark.Yellow",
                    AccentColor = "#FEDE06"
                }
            }
        };
    }
}
