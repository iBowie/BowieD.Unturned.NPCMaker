using BowieD.Unturned.NPCMaker.Mistakes;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class MistakeTabViewModel : BaseViewModel
    {
        public MistakeTabViewModel()
        {
            MainWindow.Instance.lstMistakes.SelectionChanged += MistakeList_Selected;
        }
        internal void MistakeList_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (MainWindow.Instance.lstMistakes.SelectedItem != null && MainWindow.Instance.lstMistakes.SelectedItem is Mistake mist)
            {
                mist.OnClick?.Invoke();
            }
        }
    }
}
