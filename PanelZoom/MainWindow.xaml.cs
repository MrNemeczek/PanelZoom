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
        public MainWindow(JObject jObject)
        {        
            InitializeComponent();
            Creating_Front(jObject);
        }

        void Creating_Front(JObject dane_z_serwera)
        {
            int Json_counter;

            Json_counter = dane_z_serwera["all_contacts"].Count();

            for (int i = 0; i < Json_counter; i++)
            {
                Button button = new Button();

                string zoom_id; string zoom_password; string ContactName; string nadzorca_grupy_imie; string nadzorca_grupy_nazwisko;

                int contact_type;//typ kontaktu (0-specjalny, 1-ukryty, 2-grupowy, 99-prywatny)

                zoom_id = dane_z_serwera["all_contacts"][i]["zoomlink_id"].ToString();
                zoom_password = dane_z_serwera["all_contacts"][i]["zoomlink_pass"].ToString();
                ContactName = dane_z_serwera["all_contacts"][i]["contact_name"].ToString();
                contact_type = int.Parse(dane_z_serwera["all_contacts"][i]["contact_type"].ToString());
                nadzorca_grupy_imie = dane_z_serwera["all_contacts"][i]["nad_name"].ToString();
                nadzorca_grupy_nazwisko = dane_z_serwera["all_contacts"][i]["nad_surname"].ToString();

                if (contact_type == 0)
                {
                    button.Background = Brushes.White;
                }

                else if(contact_type == 1)
                {
                    button.Background = Brushes.Gray;
                    button.Foreground = Brushes.White;
                }

                else if(contact_type == 2)
                {
                    button.Foreground = Brushes.Blue;
                }

                if (nadzorca_grupy_imie.Length > 0 && nadzorca_grupy_nazwisko.Length > 0)
                {
                    button.Content = ContactName + " " + "(" + nadzorca_grupy_imie + " " + nadzorca_grupy_nazwisko + ")";
                }

                else
                {
                    button.Content = ContactName;
                }

                button.Click += (s, e) =>
                {
                    string link;
                    link = "zoommtg://zoom.us/join?confno=" + zoom_id + "&pwd=" + zoom_password;

                    Process.Start(link);
                };

                stackpanel.Children.Add(button);
            }
        }
    }
}
