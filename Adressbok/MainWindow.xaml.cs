using Adressbok.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace Adressbok
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ContactPerson> contacts;
        //private string _filePath = $@"C:\AngelicaSkola\Assignments\Test-Inlamning\test2\test.json";
        private string _filePath = $@"C:\AngelicaSkola\Inlamning\Adressbok-WPF\Adressbok.json"; //sökväg var vår json fil ligger och listan sparas i 

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                var result = Read(_filePath);
                contacts = JsonConvert.DeserializeObject<ObservableCollection<ContactPerson>>(Read(_filePath));   
                lv_Contacts.ItemsSource = contacts; // vår listview ska läsa av vår lista "contacts"

                // Knapparna som visas/göms
                btn_Add.Visibility = Visibility.Visible;
                btn_Save.Visibility = Visibility.Collapsed;
                btn_Back.Visibility = Visibility.Hidden;
            }
            catch //gör en ny fil om det inte finns någon
            {
                using (File.Create(_filePath))
                    contacts = new ObservableCollection<ContactPerson>();
                SortList();
            }
           

        }

        private void btn_Add_Click(object sender, RoutedEventArgs e) //Metod som lägger till kontakt och jämför om det finns en kontakt med samma mejladress
        {
            try
            {
                var contact = contacts.FirstOrDefault(x => x.Email == tb_Email.Text); // jämför om det redan finns en användare med samma mejladress 
                if (contact == null)
                {
                    contacts.Add(new ContactPerson // lägger till en ny ContactPerson i vår lista contacts
                    {
                        FirstName = tb_FirstName.Text,
                        LastName = tb_LastName.Text,
                        Email = tb_Email.Text,
                        StreetName = tb_StreetName.Text,
                        PostalCode = tb_PostalCode.Text,
                        City = tb_City.Text
                    });
                    SortList();
                    Save(_filePath, JsonConvert.SerializeObject(contacts));
                }
            }

            catch { }

            ClearFields(); // tömmer fälten
          
          
        }
        private void ClearFields() // metod som tömmer alla fields
        {
            tb_FirstName.Text = "";
            tb_LastName.Text = "";
            tb_Email.Text = "";
            tb_StreetName.Text = "";
            tb_PostalCode.Text = "";
            tb_City.Text = "";
        } 

        private void lv_Contacts_SelectionChanged(object sender, SelectionChangedEventArgs e) // hur vi vill att listan ska visas i listviewn 
        {

            var obj = sender as ListView;
            var contact = (ContactPerson)obj!.SelectedItem;
            if (contact != null)
            {
                tb_FirstName.Text = contact.FirstName;
                tb_LastName.Text = contact.LastName;
                tb_Email.Text = contact.Email;
                tb_StreetName.Text = contact.StreetName;
                tb_PostalCode.Text = contact.PostalCode;
                tb_City.Text = contact.City;


                // Knapparna som visas/göms
                btn_Save.Visibility = Visibility.Visible;
                btn_Add.Visibility = Visibility.Collapsed;
                btn_Back.Visibility = Visibility.Visible;

            }

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) // uppdaterar kontakten genom att hitta indexet på den kontakten vi markerat 
        {

            var contact = (ContactPerson)lv_Contacts.SelectedItems[0]!;
            var index = contacts.IndexOf(contact);
            contacts[index].FirstName = tb_FirstName.Text;
            contacts[index].LastName = tb_LastName.Text;
            contacts[index].Email = tb_Email.Text;
            contacts[index].StreetName = tb_StreetName.Text;
            contacts[index].PostalCode = tb_PostalCode.Text;
            contacts[index].City = tb_City.Text;


            SortList();
            Save(_filePath, JsonConvert.SerializeObject(contacts));
            MessageBox.Show("Kontakten uppdaterad");
            ClearFields();
            btn_Back.Visibility = Visibility.Hidden;
            btn_Add.Visibility = Visibility.Visible;
            btn_Save.Visibility = Visibility.Collapsed;


        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e) //tar bort en kontakt
        {
 
            try

            {
                var button = sender as Button;
                var contact = (ContactPerson)button!.DataContext;

                if (MessageBox.Show("Vill du ta bort kontakten?",
                     "Ta bort kontakt",
                     MessageBoxButton.YesNo,
                     MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    contacts.Remove(contact);

                    SortList();
                    Save(_filePath, JsonConvert.SerializeObject(contacts));
                    ClearFields();
                    btn_Back.Visibility = Visibility.Hidden;
                    btn_Save.Visibility = Visibility.Collapsed;
                    btn_Add.Visibility = Visibility.Visible;
                }

            }
            catch
            {
                MessageBox.Show("Kunde inte ta bort kontakten");
                ClearFields();
                btn_Back.Visibility = Visibility.Hidden;
                btn_Save.Visibility = Visibility.Collapsed;
                btn_Add.Visibility = Visibility.Visible;
            }

        }

        private void btn_Back_Click(object sender, RoutedEventArgs e) // knapp som tömmer fälten så vi kan lägga till en till kontakt
        {
            ClearFields();
            // Knapparna som visas/göms
            btn_Save.Visibility = Visibility.Collapsed;
            btn_Add.Visibility = Visibility.Visible;
            btn_Back.Visibility = Visibility.Hidden;

        }
        private void SortList() //Sorterar vår lista efter förnamn 
        {
            lv_Contacts.ItemsSource = contacts.OrderBy(x => x.FirstName);
        }


        private string Read(string filePath) // läser vår json fil 
        {
            try
            {
                using var sr = new StreamReader(filePath);
                var content = sr.ReadToEnd();
                if (string.IsNullOrEmpty(content))
                    content = "[]";

                return content; 
            }
            catch
            {
                return "[]";
            }
        }

        public void Save(string filePath, string content) //sparar till vår json fil 
        {
            
                using var sw = new StreamWriter(filePath);
                sw.WriteLine(content);
         
            
        }
              
    }
}
