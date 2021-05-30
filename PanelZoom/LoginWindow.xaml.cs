using System;
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
using Flurl.Http;
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

        string GetHashString (byte[] hash)
        {
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        
        private void Loggin_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // krzychu_zoom Skrm1914
                string Password;
                string Username;
                string UserPlusPass;
                string token;
                
                JObject ServerRequestJSON = new JObject();

                Password = PasswordBox.Password;
                Username = LoginTextBox.Text;
                Username = Username.ToLower();

                SHA256 sHA = SHA256.Create();
                byte[] sHAPassword = sHA.ComputeHash(Encoding.UTF8.GetBytes(Password));

                ServerRequestJSON.Add("login", Username);
                ServerRequestJSON.Add("pass", GetHashString(sHAPassword));

                Console.WriteLine(ServerRequestJSON);

                UserPlusPass = Username + Password;

                SHA256 sHA256 = SHA256.Create();
                byte[] hash = sHA256.ComputeHash(Encoding.UTF8.GetBytes(UserPlusPass));

                token = "token=" + GetHashString(hash);

                ServerOperation serverContactList = new ServerOperation(URLS.CONTACT_LIST_URL, "application/x-www-form-urlencoded; charset=utf-8", "POST");

                serverContactList.ServerRequest();
                serverContactList.ServerWriter(token);

                ServerOperation serverNotRememberedLogin = new ServerOperation(URLS.USER_ON_LOGIN_URL, "application/x-www-form-urlencoded; charset=utf-8", "POST");

                serverNotRememberedLogin.ServerRequest();
                serverNotRememberedLogin.ServerWriter(ServerRequestJSON);
                
                JObject person = serverNotRememberedLogin.ServerReader();

                string wyswietl = person.ToString();
                Console.WriteLine(wyswietl);

                int logowanie = int.Parse(person["success"].ToString());

                if(logowanie == 1)
                {
                    MainWindow mainWindow = new MainWindow(person);

                    mainWindow.Show();
                    Close();
                }
                else if(logowanie == 0)
                {
                    MessageBox.Show("Złe hasło lub login!");
                }
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
