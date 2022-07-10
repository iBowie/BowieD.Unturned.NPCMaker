using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для AskSkillsView.xaml
    /// </summary>
    public partial class AskSkillsView : Window
    {
        public AskSkillsView()
        {
            InitializeComponent();

            this.selectBeginnerButton.Command = SelectSkillLevelCommand;
            this.selectIntermediateButton.Command = SelectSkillLevelCommand;
            this.selectAdvancedButton.Command = SelectSkillLevelCommand;

            this.selectBeginnerButton.CommandParameter = ESkillLevel.Beginner;
            this.selectIntermediateButton.CommandParameter = ESkillLevel.Intermediate;
            this.selectAdvancedButton.CommandParameter = ESkillLevel.Advanced;
        }

        private ICommand _selectSkillLevelCommand;
        public ICommand SelectSkillLevelCommand
        {
            get
            {
                if (_selectSkillLevelCommand is null)
                {
                    _selectSkillLevelCommand = new BaseCommand((parameter) =>
                    {
                        if (parameter is ESkillLevel skillLevelParam)
                        {
                            this.SelectedSkillLevel = skillLevelParam;
                            this.DialogResult = true;
                            this.Close();
                        }
                    });
                }

                return _selectSkillLevelCommand;
            }
        }

        private ESkillLevel _selectedSkillLevel;
        public ESkillLevel SelectedSkillLevel 
        {
            get => _selectedSkillLevel;
            private set => _selectedSkillLevel = value;
        }
    }
}
