using BowieD.Unturned.NPCMaker.Coloring;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BowieD.Unturned.NPCMaker.Markup
{
    public sealed class RichText : IMarkup
    {
        public void Markup(TextBlock textBlock, string text)
        {
            textBlock.Text = string.Empty;
            textBlock.Inlines.Clear();

            if (string.IsNullOrEmpty(text))
                return;

            List<RichTag> openedTags = new List<RichTag>();

            StringBuilder sb = new StringBuilder();

            void flush()
            {
                Inline inl = new Run(sb.ToString());
                foreach (var t in openedTags)
                    t.Modify(inl);
                sb.Clear();

                textBlock.Inlines.Add(inl);
            }

            for (int i = 0; i < text.Length; i++)
            {
                char? prev = i > 0 ? text[i - 1] : (char?)null;
                char current = text[i];

                if (current == '<' && prev != '\\')
                {
                    string tag = text.Substring(i, text.IndexOf('>', i) - i);
                    i += tag.Length;

                    string clearTag = tag.Trim('<', '>');
                    int num = clearTag.IndexOf('/');
                    string clearerTag = clearTag.Trim('/');
                    string[] splittedTag = clearerTag.Split('=');
                    RichTag t;
                    if (splittedTag.Length == 1)
                        t = new RichTag(splittedTag[0]);
                    else
                        t = new RichTag(splittedTag[0], splittedTag[1]);
                    if (num != -1) // close
                    {
                        flush();
                        RemoveFromEnd(openedTags, t);
                    }
                    else // open
                    {
                        flush();
                        openedTags.Add(t);
                    }
                }
                else
                {
                    sb.Append(current);
                }
            }

            flush();
        }
        static void RemoveFromEnd(List<RichTag> list, RichTag element)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == element)
                {
                    list.RemoveAt(i);
                    return;
                }
            }
        }
        class RichTag
        {
            public RichTag(string name, string value = null)
            {
                this.Name = name;
                this.Value = value;

                Open = $"<{Name}{(Value == null ? "" : $"={Value}")}>";
                Close = $"</{Name}>";
            }

            public string Name { get; }
            public string Value { get; }

            public string Open { get; }
            public string Close { get; }

            public void Modify(Inline inline)
            {
                switch (Name)
                {
                    case "color":
                        {
                            var c = ColorConverter.ParseColor(Value);
                            TextElement.SetForeground(inline, c);
                        }
                        break;
                    case "b":
                        {
                            TextElement.SetFontWeight(inline, FontWeights.Bold);
                        }
                        break;
                    case "i":
                        {
                            TextElement.SetFontStyle(inline, FontStyles.Italic);
                        }
                        break;
                }
            }

            public static bool operator ==(RichTag a, RichTag b)
            {
                return a.Name == b.Name;
            }
            public static bool operator !=(RichTag a, RichTag b)
            {
                return a.Name != b.Name;
            }
        }
    }
}
