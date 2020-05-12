using System.Windows;

namespace BowieD.Unturned.NPCMaker
{
    public static class ScreenHelper
    {
        public sealed class WpfScreen
        {
            internal WpfScreen(double sizeX, double sizeY)
            {
                this.Size = new Rect(0, 0, sizeX, sizeY);
            }

            public Rect Size { get; }
        }
        public static WpfScreen GetCurrentScreen() => GetCurrentScreen(MainWindow.Instance);
        public static WpfScreen GetCurrentScreen(this Window window)
        {
            // todo: add multiple screen support
            return new WpfScreen(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
        }
    }
}
