using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using System.Linq;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.ViewModels.Character
{
    public sealed class CharacterTabViewModel : BaseViewModel
    {
        public CharacterTabViewModel()
        {
            Character = new NPCCharacter();
        }
        public NPCCharacter Character { get; set; }
        public string DisplayName { get => Character.displayName; set => Character.displayName = value; }
        public string EditorName { get => Character.editorName; set => Character.editorName = value; }
        public ushort ID { get => Character.id; set => Character.id = value; }
        private ICommand saveCommand;
        private ICommand openCommand;
        private ICommand resetCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new BaseCommand(() =>
                    {
                        if (Character.id == 0)
                        {
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Character_ID_Zero"]);
                            return;
                        }
                        MainWindow.CurrentProject.data.characters.RemoveAll(d => d.id == Character.id);
                        MainWindow.CurrentProject.data.characters.Add(Character);
                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["Character_Saved"]);
                        MainWindow.CurrentProject.isSaved = false;
                        App.Logger.LogInfo($"Character {Character.id} saved!");
                    });
                }
                return saveCommand;
            }
        }
        public ICommand OpenCommand
        {
            get
            {
                if (openCommand == null)
                {
                    openCommand = new BaseCommand(() =>
                    {
                        var ulv = new Universal_ListView(MainWindow.CurrentProject.data.characters.OrderBy(d => d.id).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Character, false)).ToList(), Universal_ItemList.ReturnType.Character);
                        if (ulv.ShowDialog() == true)
                        {
                            SaveCommand.Execute(null);
                            Character = ulv.SelectedValue as NPCCharacter;
                            App.Logger.LogInfo($"Opened character {ID}");
                        }
                        MainWindow.CurrentProject.data.characters = ulv.Values.Cast<NPCCharacter>().ToList();
                    });
                }
                return openCommand;
            }
        }
        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new BaseCommand(() =>
                    {
                        Character = new NPCCharacter();
                    });
                }
                return resetCommand;
            }
        }
    }
}
