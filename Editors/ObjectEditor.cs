using BowieD.Unturned.NPCMaker.BetterControls;
using BowieD.Unturned.NPCMaker.BetterForms;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Editors
{
    public class ObjectEditor : IEditor<NPCObject>
    {
        public ObjectEditor()
        {
            MainWindow.Instance.objectsListAddButton.Click += ObjectsListAddButton_Click;
            MainWindow.Instance.objectsListRemoveButton.Click += ObjectsListRemoveButton_Click;
            MainWindow.Instance.objectsList.SelectionChanged += ObjectsList_SelectionChanged;
            MainWindow.Instance.objectsIDbox.ValueChanged += ObjectsIDbox_ValueChanged;
            Current = new NPCObject();
        }

        private void ObjectsIDbox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            Current.ID = (ushort)(e.NewValue.HasValue ? e.NewValue.Value : 0);
        }

        private void ObjectsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Save();
            if (MainWindow.Instance.objectsList.SelectedItem != null)
            {
                LoadObject(MainWindow.Instance.objectsList.SelectedItem as NPCObject);
            }
        }

        private void ObjectsListRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance.objectsList.SelectedItem != null && MainWindow.Instance.objectsList.SelectedItem is NPCObject)
            {
                MainWindow.Instance.objectsList.Items.Remove(MainWindow.Instance.objectsList.SelectedItem);
            }
        }

        private void ObjectsListAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance.objectsList.Items.Contains(d => d is NPCObject obj && obj.ID == 0))
            {
                return;
            }
            NPCObject newObject = new NPCObject();
            MainWindow.Instance.objectsList.Items.Add(newObject);
        }

        public void LoadObject(NPCObject input)
        {
            Reset();
            MainWindow.Instance.objectsIDbox.Value = input.ID;
        }

        public void Save()
        {
            var obj = Current;
            if (obj.ID == 0)
            {
                MainWindow.NotificationManager.Notify(MainWindow.Localize("object_ID_Zero"));
                return;
            }
            var o = MainWindow.CurrentSave.objects.Where(d => d.ID == obj.ID);
            if (o.Count() > 0)
                MainWindow.CurrentSave.objects.Remove(o.ElementAt(0));
            MainWindow.CurrentSave.objects.Add(obj);
            MainWindow.NotificationManager.Notify(MainWindow.Localize("notify_Object_Saved"));
            MainWindow.isSaved = false;
            Logger.Log($"Object {obj.ID} saved!");
        }
        public void Open()
        {
            var ulv = new Universal_ListView(MainWindow.CurrentSave.objects.OrderBy(d => d.ID).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Object, false)).ToList(), Universal_ItemList.ReturnType.Object);
            if (ulv.ShowDialog() == true)
            {
                Save();
                Current = ulv.SelectedValue as NPCObject;
                Logger.Log($"Opened dialogue {MainWindow.Instance.objectsIDbox.Value}");
            }
            MainWindow.CurrentSave.objects = ulv.Values.Cast<NPCObject>().ToList();
        }
        public void Reset()
        {
            MainWindow.Instance.objectsIDbox.Value = 0;
        }
        public NPCObject Current { get; set; }
    }
}
