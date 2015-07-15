﻿using Newtonsoft.Json;
using SunSync.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SunSync
{
    /// <summary>
    /// Interaction logic for QuickStartPage.xaml
    /// </summary>
    public partial class QuickStartPage : Page
    {
        private MainWindow mainWindow;
        private Dictionary<int, string> syncRecordDict;
        public QuickStartPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.syncRecordDict = new Dictionary<int, string>();
        }

        private void SetAccount_EventHandler(object sender, MouseButtonEventArgs e)
        {
            this.mainWindow.GotoAccountPage();
        }

        private void CreateNewSyncJob_EventHandler(object sender, MouseButtonEventArgs e)
        {
            this.mainWindow.GotoSyncSettingPage(null);
        }

        public void LoadSyncRecords(List<SyncRecord> syncRecords)
        {
            this.SyncHistoryListBox.Items.Clear();
            this.syncRecordDict.Clear();
            int index = 0;
            foreach (SyncRecord record in syncRecords)
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                Style ctlStyle = Application.Current.TryFindResource("jobListItemResource") as Style;
                listBoxItem.DataContext = record;
                listBoxItem.Style = ctlStyle;
                listBoxItem.MouseDoubleClick += listBoxItem_MouseDoubleClick;
                this.syncRecordDict.Add(index, record.FilePath);
                this.SyncHistoryListBox.Items.Add(listBoxItem);
                index += 1;
            }
        }

        private void listBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = this.SyncHistoryListBox.SelectedIndex;
            if (selectedIndex != -1)
            {
                string jobPath = this.syncRecordDict[selectedIndex];
                try
                {
                    using (StreamReader sr = new StreamReader(jobPath, Encoding.UTF8))
                    {
                        SyncSetting syncSetting = JsonConvert.DeserializeObject<SyncSetting>(sr.ReadToEnd());
                        if (syncSetting.SyncLocalDir != "" && syncSetting.SyncTargetBucket != "")
                        {
                            this.mainWindow.GotoSyncSettingPage(syncSetting);
                        }
                    }
                }
                catch (Exception)
                {
                    //todo where is the fuck job file
                }
            }
        }
    }
}
