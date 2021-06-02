using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using MySql.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PanelZoom
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {        
            InitializeComponent();
        }

        private void Logout_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxButton messageBoxButton = MessageBoxButton.YesNo;

            MessageBoxResult messageBoxResult = MessageBox.Show("Czy naprawdę chcesz się wylogować?", "Wylogowywanie", messageBoxButton);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Properties.Settings.Default.CheckBox = false;
                Properties.Settings.Default.Token = string.Empty;
                Properties.Settings.Default.Save();

                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                Close();
            }
        }

        private void Settings_button_Click(object sender, RoutedEventArgs e)
        {

           /* ImageBrush contacts = new ImageBrush();

            contacts.ImageSource = new BitmapImage(new Uri("Assets/Contacts_icon.png"));

            if (NavigationFrame.Equals(SettingsPage.ContentProperty))
            {
                NavigationFrame.Content = Page.h
            }*/

            SettingsPage settingsPage = new SettingsPage();

            NavigationFrame.Content = settingsPage;
        }

        private void Contacts_button_Click(object sender, RoutedEventArgs e)
        {
            ContactsPage ContactsPage = new ContactsPage();

            NavigationFrame.Content = ContactsPage;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            ContactsPage ContactsPage = new ContactsPage();

            NavigationFrame.Content = ContactsPage;
        }        
    }
}
