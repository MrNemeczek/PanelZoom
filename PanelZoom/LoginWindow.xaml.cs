using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PanelZoom
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Loggin_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string Password;
                string Username;
                string UserPlusPass;
                string token;

                Password = PasswordBox.Password;
                Username = LoginTextBox.Text;
                Username = Username.ToLower();

                UserPlusPass = Username + Password;

                SHA256 sHA256 = SHA256.Create();
                byte[] hash = sHA256.ComputeHash(Encoding.UTF8.GetBytes(UserPlusPass));

                token = "token=" + GetHashString(hash);

                ServerOperation serverContactList = new ServerOperation(URLS.CONTACT_LIST_URL, "application/x-www-form-urlencoded; charset=utf-8", "POST");

                serverContactList.ServerRequest();
                serverContactList.ServerWriter(token);

                PublicVariables.contacts = serverContactList.ServerReader();

                string permissionString;
                permissionString = "login=" + Username + "&pass=" + Password;
                ServerOperation serverNotRememberedLogin = new ServerOperation(URLS.USER_ON_LOGIN_URL, "application/x-www-form-urlencoded; charset=utf-8", "POST");

                serverNotRememberedLogin.ServerRequest();
                serverNotRememberedLogin.ServerWriter(permissionString);

                PublicVariables.person = serverNotRememberedLogin.ServerReader();
             

                int logowanie = int.Parse(PublicVariables.person["success"].ToString());

                if(logowanie == 1)
                {
                    MainWindow mainWindow = new MainWindow();

                    if (RememberMeCheckBox.IsChecked == true)
                    {
                        Properties.Settings.Default.CheckBox = true;
                        Properties.Settings.Default.Token = token;
                        Properties.Settings.Default.Save();
                    }

                    mainWindow.Show();
                    Close();
                }
                else if(logowanie == 0)
                {
                    if (RememberMeCheckBox.IsChecked == true)
                    {
                        Properties.Settings.Default.CheckBox = false;
                        Properties.Settings.Default.Token = string.Empty;
                        Properties.Settings.Default.Save();
                    }

                    if(Properties.Settings.Default.CheckBox == true)
                    {
                        Properties.Settings.Default.CheckBox = false;
                        Properties.Settings.Default.Token = string.Empty;
                        Properties.Settings.Default.Save();
                    }

                    MessageBox.Show("Złe hasło lub login!");
                }
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Loaded_LoginWindow(object sender, RoutedEventArgs e)
        {
            bool IsInternetLabel = false;

            int Counter = 0;

            while(CheckInternetConnection() == false)
            {
                if (Counter == 5)
                    break;

                if(IsInternetLabel == false)
                {
                    Label ConnectionLost = new Label();

                    ConnectionLost.Foreground = Brushes.Red;
                    ConnectionLost.FontSize = 14;
                    ConnectionLost.Content = "nie ma połączenia z internetem!";

                    stackpanel.Children.Add(ConnectionLost);

                    Loggin_button.IsEnabled = false;

                    IsInternetLabel = true;
                }

                Counter++;
            }

            if (Properties.Settings.Default.CheckBox == true && CheckInternetConnection() == true)
            {
                string token;

                token = Properties.Settings.Default.Token;

                ServerOperation serverRememberedLogin = new ServerOperation(URLS.USER_REMEMBERED_URL, "application/x-www-form-urlencoded; charset=utf-8", "POST");

                serverRememberedLogin.ServerRequest();
                serverRememberedLogin.ServerWriter(token);

                PublicVariables.person = serverRememberedLogin.ServerReader();

                ServerOperation serverContactList = new ServerOperation(URLS.CONTACT_LIST_URL, "application/x-www-form-urlencoded; charset=utf-8", "POST");

                serverContactList.ServerRequest();
                serverContactList.ServerWriter(token);

                PublicVariables.contacts = serverContactList.ServerReader();

                int logowanie = int.Parse(PublicVariables.person["success"].ToString());

                if (logowanie == 1)
                {
                    MainWindow mainWindow = new MainWindow();

                    mainWindow.Show();
                    Close();
                }

                else
                {
                    Properties.Settings.Default.CheckBox = false;
                    Properties.Settings.Default.Token = string.Empty;
                    Properties.Settings.Default.Save();

                    MessageBox.Show("Złe hasło lub login!");
                }
            }
        }
        private bool CheckInternetConnection()
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://www.google.com/");

                httpWebRequest.Timeout = 5000;
                httpWebRequest.Credentials = CredentialCache.DefaultNetworkCredentials;

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private string GetHashString(byte[] hash)
        {
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
