namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }
        public MainWindow MainWindow { get; set; }
    }
}
