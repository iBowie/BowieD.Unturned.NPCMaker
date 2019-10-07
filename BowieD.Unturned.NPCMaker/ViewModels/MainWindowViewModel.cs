using BowieD.Unturned.NPCMaker.ViewModels.Character;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            CharacterTabViewModel = new CharacterTabViewModel();
        }
        public MainWindow MainWindow { get; set; }
        public CharacterTabViewModel CharacterTabViewModel { get; set; }
    }
}
