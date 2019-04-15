using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Export_ExportWindow.xaml
    /// </summary>
    public partial class Export_ExportWindow : Window
    {
        public Export_ExportWindow(string baseDir)
        {
            InitializeComponent();
            dir = baseDir;
        }

        public void DoActions(NPCSave save)
        {
            base.Show();
            Start(save);
            Button button = new Button
            {
                Content = new TextBlock
                {
                    Text = MainWindow.Localize("export_Done_Goto")
                }
            };
            Action<object, RoutedEventArgs> action = new Action<object, RoutedEventArgs>((sender, e) => { Process.Start(AppDomain.CurrentDomain.BaseDirectory + $@"results\{save.guid}"); });
            button.Click += new RoutedEventHandler(action);
            MainWindow.NotificationManager.Notify(MainWindow.Localize("export_Done"), buttons: button);
            this.Close();
        }

        
    }
}
