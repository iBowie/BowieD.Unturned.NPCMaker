using BowieD.Unturned.NPCMaker.Configuration;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker
{
    public static class IDELikeTool
    {
        public static void RegisterOpenCloseBoomerangs(TextBox textBox)
        {
            textBox.PreviewTextInput += (sender, e) =>
            {
                if (AppConfig.Instance.autoCloseOpenBoomerangs)
                {
                    if (e.Text == "<")
                    {
                        var caretPos = textBox.CaretIndex;

                        textBox.Text = $"{textBox.Text.Substring(0, textBox.SelectionStart + textBox.SelectionLength)}>{textBox.Text.Substring(textBox.SelectionStart + textBox.SelectionLength)}";

                        textBox.CaretIndex = caretPos;
                    }
                    else if (e.Text == ">")
                    {
                        var caretPos = textBox.CaretIndex;

                        if (textBox.Text.Length > caretPos && textBox.Text[caretPos] == '>')
                        {
                            e.Handled = true;
                            textBox.CaretIndex++;
                        }
                    }
                }
            };
        }
    }
}
