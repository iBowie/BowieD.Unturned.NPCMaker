using BowieD.Unturned.NPCMaker.ViewModels;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для UGC_CreateUpdateView.xaml
    /// </summary>
    public partial class UGC_CreateUpdateView : Window
    {
        public UGC_CreateUpdateView()
        {
            InitializeComponent();

            createButton.Command = new BaseCommand(() =>
            {
                Result = EResult.Create;
                DialogResult = true;
                Close();
            });

            updateButton.Command = new BaseCommand(() =>
            {
                Result = EResult.Update;
                DialogResult = true;
                Close();
            });

            cancelButton.Command = new BaseCommand(() =>
            {
                Result = EResult.None;
                DialogResult = false;
                Close();
            });
        }

        public EResult Result { get; private set; }

        public enum EResult
        {
            None,
            Create,
            Update
        }
    }
}
