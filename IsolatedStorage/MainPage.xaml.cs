using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;

namespace IsolatedStorage
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += (s, e) => LoadSettingsList();
        }

        private void WriteButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtain the virtual store for the application.
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

            // Create a new folder and call it "MyFolder".
            myStore.CreateDirectory("MyFolder");

            // Specify the file path and options.
            using (var isoFileStream = new
                IsolatedStorageFileStream("MyFolder\\myFile.txt", FileMode.OpenOrCreate, myStore))
            {
                //Write the data
                using (var isoFileWriter = new StreamWriter(isoFileStream))
                {
                    isoFileWriter.WriteLine(WriteText.Text);
                }
            }
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {    // Obtain a virtual store for the application.
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

            try
            {
                // Specify the file path and options.
                using (var isoFileStream = new IsolatedStorageFileStream("MyFolder\\myFile.txt", FileMode.Open, myStore))
                {
                    // Read the data.
                    using (var isoFileReader = new StreamReader(isoFileStream))
                    {
                        ReadText.Text = isoFileReader.ReadToEnd();
                    }
                }
            }
            catch
            {
                // Handle the case when the user attempts to click the Read button first.
                ReadText.Text = "Need to create directory and the file first.";
            }
        }

        readonly IsolatedStorageSettings settings =
            IsolatedStorageSettings.ApplicationSettings;

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            if (!settings.Contains(SettingKey.Text))
            {
                settings.Add(SettingKey.Text, SettingValue.Text);
            }
            else
            {
                settings[SettingKey.Text] = SettingValue.Text;
            }
            settings.Save();
            LoadSettingsList();
        }

        private void LoadSettingsList()
        {
            var list = new List<string>();
            foreach (var item in settings)
            {
                var value = String.Format("{0} = {1}", item.Key, item.Value);
                list.Add(value);
            }
            SettingsListBox.ItemsSource = list;
        }
    }
}