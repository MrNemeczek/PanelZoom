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

        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            string connectionString = "server = mysql-727668.vipserv.org; user = rafik73_mapy; database = rafik73_mapy; password = Rafaello73";

            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

            try
            {
                mySqlConnection.Open();

                string ContactName;
                string zoom_password;
                string zoom_id;
                string sql = "SELECT * FROM panelrm_contacts";

                bool special;

                int contact_group;
                
                MySqlCommand cmd = new MySqlCommand(sql, mySqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ContactName = rdr.GetString("contact_name");
                    special = rdr.GetBoolean("special");
                    contact_group = rdr.GetInt32("contact_group");
                    zoom_password = rdr.GetString("zoomlink_pass");
                    zoom_id = rdr.GetString("zoomlink_id");
                    
                    Creating_Front(ContactName, special, contact_group, zoom_password, zoom_id);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void Creating_Front(string ContactName, bool special, int contact_group, string Password, string ID)
        {
            Button button = new Button();

            if(special == false)
            {
                button.Background = Brushes.White;
            }

            else if(special == true)
            {
                button.Background = Brushes.Gray;
                button.Foreground = Brushes.White;
            }

            if(contact_group == 0 && special == false)
            {
                button.Foreground = Brushes.Blue;
            }

            button.Content = ContactName;

            button.Click += (s, e) => 
            {
                string link;
                link = "zoommtg://zoom.us/join?confno=" + ID + "&pwd=" + Password;

                Process.Start(link);
            };

            stackpanel.Children.Add(button);
            
        }
    }
}
