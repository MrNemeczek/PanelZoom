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
                // krzychu_zoom KuZooM1914KRz
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

                WebRequest webRequest = WebRequest.Create(URLS.CONTACT_LIST_URL);

                webRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                webRequest.Method = "POST";

                StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream());

                streamWriter.Write(token);
                streamWriter.Flush();

                var httpResponse = (HttpWebResponse)webRequest.GetResponse();

                StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream());

                string result = streamReader.ReadToEnd();

               // label.Content = result + UserPlusPass + " " + token;

                JObject person = JObject.Parse(result);
                int personCount = person["all_contacts"].Count();

                for(int i = 0; i < personCount; i++)
                {
                    Console.WriteLine(person["all_contacts"][i]["contact_name"].ToString() + "\n");
                }

               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
