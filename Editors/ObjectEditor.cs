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
            if (MainWindow.Instance.objectsList.SelectedItem != null)
                MainWindow.Instance.objectsList.Items[MainWindow.Instance.objectsList.Items.IndexOf(MainWindow.Instance.objectsList.SelectedItem)] = Current;
            else
                MainWindow.Instance.objectsList.Items.Add(Current);
        }
        public void Open()
        {

        }
        public void Reset()
        {
            MainWindow.Instance.objectsIDbox.Value = 0;
        }
        public NPCObject Current { get; set; }
    }
}
