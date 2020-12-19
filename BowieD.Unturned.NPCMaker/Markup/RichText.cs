using BowieD.Unturned.NPCMaker.Coloring;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.Markup
{
    public class RichText : IMarkup
    {
        public virtual void Markup(TextBlock textBlock, string text)
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
                int tagEndPos = text.IndexOf('>', i);

                if (current == '<' && prev != '\\' && tagEndPos != -1)
                {
                    string tag = text.Substring(i, tagEndPos - i);
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
                    def:
                    sb.Append(current);
                }
            }

            flush();
        }
        protected static void RemoveFromEnd(List<RichTag> list, RichTag element)
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
        protected class RichTag
        {
            public const string SEARCH_NAME = "NPCMAKERSEARCH";

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
                            var c = Coloring.ColorConverter.ParseColor(Value);
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
                    case SEARCH_NAME:
                        {
                            inline.Background = App.Current.Resources["AccentColor"] as Brush;
                            TextElement.SetForeground(inline, App.Current.Resources["ForegroundColor"] as Brush);
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
    public sealed class SearchRichText : RichText
    {
        public string searchText;
        public SearchRichText() : this(string.Empty)
        {

        }
        public SearchRichText(string searchText)
        {
            this.searchText = searchText;
        }

        public override void Markup(TextBlock textBlock, string text)
        {
            textBlock.Text = string.Empty;
            textBlock.Inlines.Clear();

            if (string.IsNullOrEmpty(text))
                return;

            int pos = text.ToLowerInvariant().IndexOf(searchText.ToLowerInvariant());

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
                if (i == pos)
                {
                    flush();
                    openedTags.Add(new RichTag(RichTag.SEARCH_NAME));
                }
                else if (i == pos + searchText.Length)
                {
                    flush();
                    RemoveFromEnd(openedTags, new RichTag(RichTag.SEARCH_NAME));
                }

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
    }
}
