using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace PanelZoom
{
    /// <summary>
    /// Logika interakcji dla klasy ContactsPage.xaml
    /// </summary>
    public partial class ContactsPage : Page
    {
        public ContactsPage()
        {
            InitializeComponent();
            Creating_Front();
        }

        void Creating_Front()
        {
            int Json_counter; int MyGroup;

            Json_counter = PublicVariables.contacts["all_contacts"].Count();
            MyGroup = int.Parse(PublicVariables.person["group"].ToString());

            for (int i = 0; i < Json_counter; i++)
            {
                Button button = new Button();

                button.Height = 30;
                button.FontSize = 14;
                button.BorderBrush = Brushes.Black;
                button.FontWeight = FontWeights.Medium;

                string zoom_id; string zoom_password; string ContactName; string nadzorca_grupy_imie; string nadzorca_grupy_nazwisko;

                int contact_type;//typ kontaktu (0-specjalny, 1-ukryty, 2-grupowy, 99-prywatny)
                int contact_group;

                zoom_id = PublicVariables.contacts["all_contacts"][i]["zoomlink_id"].ToString();
                zoom_password = PublicVariables.contacts["all_contacts"][i]["zoomlink_pass"].ToString();
                ContactName = PublicVariables.contacts["all_contacts"][i]["contact_name"].ToString();
                contact_type = int.Parse(PublicVariables.contacts["all_contacts"][i]["contact_type"].ToString());
                nadzorca_grupy_imie = PublicVariables.contacts["all_contacts"][i]["nad_name"].ToString();
                nadzorca_grupy_nazwisko = PublicVariables.contacts["all_contacts"][i]["nad_surname"].ToString();
                contact_group = int.Parse(PublicVariables.contacts["all_contacts"][i]["contact_group"].ToString());

                if (contact_type == 0)
                {
                    button.Background = Brushes.White;
                }

                else if (contact_type == 1)
                {
                    button.Background = Brushes.Gray;
                    button.Foreground = Brushes.White;
                }

                else if (contact_type == 2)
                {
                    button.Foreground = Brushes.Blue;
                }

                if (contact_group != 0)
                {
                    if (contact_group == MyGroup)
                    {
                        button.Content = "MOJA GRUPA" + " " + "(" + nadzorca_grupy_imie + " " + nadzorca_grupy_nazwisko + ")";
                        button.Foreground = Brushes.Red;
                    }
                    else
                    {
                        button.Content = ContactName + " " + "(" + nadzorca_grupy_imie + " " + nadzorca_grupy_nazwisko + ")";
                    }
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
