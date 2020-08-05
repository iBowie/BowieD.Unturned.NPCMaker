using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace BowieD.Unturned.NPCMaker.Markup
{
    public sealed class Markdown : IMarkup
    {
        public void Markup(TextBlock textBlock, string text)
        {
            textBlock.Text = string.Empty;
            textBlock.Inlines.Clear();

            if (string.IsNullOrEmpty(text))
                return;

            StringReader reader = new StringReader(text);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                List<Inline> inlines = null;
                Inline inline = null;

                if (line.StartsWith("# "))
                {
                    inline = new Run(line.Substring(2));
                    inline.FontSize = 32;
                }
                else if (line.StartsWith("## "))
                {
                    inline = new Run(line.Substring(3));
                    inline.FontSize = 24;
                }
                else if (line.StartsWith("### "))
                {
                    inline = new Run(line.Substring(4));
                    inline.FontSize = 18.72;
                }
                else if (line.StartsWith("#### "))
                {
                    inline = new Run(line.Substring(5));
                    inline.FontSize = 16;
                }
                else if (line.StartsWith("##### "))
                {
                    inline = new Run(line.Substring(6));
                    inline.FontSize = 13.28;
                }
                else if (line.StartsWith("###### "))
                {
                    inline = new Run(line.Substring(7));
                    inline.FontSize = 10.72;
                }
                else
                {
                    string pr;
                    if (line.StartsWith("* ") || line.StartsWith("- ") || line.StartsWith("+ "))
                    {
                        pr = $"● {line.Substring(2)}";
                    }
                    else if (line.StartsWith(" * ") || line.StartsWith(" - ") || line.StartsWith(" + "))
                    {
                        pr = $" ○ {line.Substring(3)}";
                    }
                    else
                    {
                        pr = line;
                    }
                    inlines = new List<Inline>();
                    StringBuilder sb = new StringBuilder();
                    bool isBold = false, isItalic = false;
                    void toggleBold()
                    {
                        flush();
                        isBold = !isBold;
                    }
                    void toggleItalic()
                    {
                        flush();
                        isItalic = !isItalic;
                    }
                    void flush()
                    {
                        if (sb.Length > 0)
                        {
                            Inline inl = new Run(sb.ToString());
                            if (isBold)
                                inl.FontWeight = System.Windows.FontWeights.Bold;
                            if (isItalic)
                                inl.FontStyle = System.Windows.FontStyles.Italic;
                            inlines.Add(inl);
                        }
                        sb.Clear();
                    }
                    for (int i = 0; i < pr.Length; i++)
                    {
                        char? prev = i > 0 ? pr[i - 1] : (char?)null;
                        char current = pr[i];
                        char? next = i < pr.Length - 1 ? pr[i + 1] : (char?)null;
                        char? nextNext = i < pr.Length - 2 ? pr[i + 2] : (char?)null;

                        switch (current)
                        {
                            case '*' when prev != '\\':
                                {
                                    if (next == '*')
                                    {
                                        if (nextNext == '*')
                                        {
                                            toggleBold();
                                            toggleItalic();
                                            i += 2;
                                        }
                                        else
                                        {
                                            toggleBold();
                                            i += 1;
                                        }
                                    }
                                    else
                                    {
                                        toggleItalic();
                                    }
                                }
                                break;
                            case '[' when prev != '\\':
                                flush();
                                string workplace1 = pr.Substring(i);
                                string workplace2 = workplace1.Substring(0, workplace1.IndexOf(')'));
                                string urlText = workplace2.Substring(0, workplace2.IndexOf(']')).Trim('[', ']');
                                string urlPart = workplace2.Substring(workplace2.IndexOf('(')).Trim('(', ')');
                                string[] vs = urlPart.Split(' ');
                                string urlLink = vs[0];
                                string urlTitle;
                                if (vs.Length > 1)
                                    urlTitle = string.Join(" ", vs.Skip(1)).Trim('"');
                                else
                                    urlTitle = string.Empty;
                                var hl = new Hyperlink
                                {
                                    NavigateUri = new System.Uri(urlLink),
                                    ToolTip = urlTitle
                                };
                                hl.RequestNavigate += (object sender, RequestNavigateEventArgs e) =>
                                {
                                    System.Diagnostics.Process.Start(e.Uri.ToString());
                                };
                                hl.Inlines.Add(new Run(urlText));
                                i += workplace2.Length;
                                inlines.Add(hl);
                                break;
                            default:
                                sb.Append(current);
                                break;
                        }
                    }
                    flush();
                }

                if (inlines != null && inlines.Count > 0)
                {
                    foreach (var i in inlines)
                    {
                        textBlock.Inlines.Add(i);
                    }
                    textBlock.Inlines.Add(new LineBreak());
                    textBlock.Inlines.Add(new LineBreak());
                }
                else if (inline != null)
                {
                    textBlock.Inlines.Add(inline);
                    textBlock.Inlines.Add(new LineBreak());
                    textBlock.Inlines.Add(new LineBreak());
                }
            }
        }
    }
}
