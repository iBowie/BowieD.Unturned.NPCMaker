using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.XAML
{
    public class Extensions
    {
        public static readonly DependencyProperty OpenCloseBoomerangsProperty = DependencyProperty.RegisterAttached(
            "OCBoomerangs",
            typeof(OCBoomerangsAttach),
            typeof(TextBox),
            new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.None));

        public static void SetOCBoomerangsAttach(TextBox element, OCBoomerangsAttach value)
        {
            element.SetValue(OpenCloseBoomerangsProperty, value);
            value.Register(element);
        }

        public static OCBoomerangsAttach GetOCBoomerangsAttach(TextBox element)
        {
            return (OCBoomerangsAttach)element.GetValue(OpenCloseBoomerangsProperty);
        }
    }

    public sealed class OCBoomerangsAttach
    {
        public void Register(TextBox element)
        {
            IDELikeTool.RegisterOpenCloseBoomerangs(element);
        }
    }
}
