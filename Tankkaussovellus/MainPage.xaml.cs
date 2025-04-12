using System.Text.Json;

namespace Tankkaussovellus
{
    public partial class MainPage : ContentPage
    {
        // Muistion tallennuspaikan alustaminen muuttujaksi
        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tankkausmuistio.json");
        List<Tankkaus> tankkaukset = new();

        public MainPage()
        {
            InitializeComponent();

            //Tarkistaan tiedosto ennestään olemassa
            bool doesExist = File.Exists(fileName);

            if (doesExist == true)
            {
                try
                {
                    var json = File.ReadAllText(fileName);
                    tankkaukset = JsonSerializer.Deserialize<List<Tankkaus>>(json) ?? new List<Tankkaus>();
                }
                catch (Exception Ex)
                {
                    DisplayAlert("Virhe.", $"Virhe tiedoston lukemisessa: { Ex.Message}", "OK");
                }
            }

            // Aseta CollectionView:lle itemsource
            tankkausLista.ItemsSource = tankkaukset;

            if (tankkaukset.Count == 0)
            {
                labelIlmoitus.IsVisible = true;
            }
        }

        //Päivitä lista tietojen lisäämisen jälkeen
        public void RefeshList()
        {
            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);
                tankkaukset = JsonSerializer.Deserialize<List<Tankkaus>>(json) ?? new List<Tankkaus>();
                tankkausLista.ItemsSource = null;
                tankkausLista.ItemsSource = tankkaukset;
                labelIlmoitus.IsVisible = tankkaukset.Count == 0;
            }
        }

        //Näytä valitun tiedon keskimääräinen kulutus
        private void itemList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Tankkaus? selectedItem = tankkausLista.SelectedItem as Tankkaus;

            if (selectedItem != null)
            {
                int selectedIndex =  tankkaukset.IndexOf(selectedItem);         

                if(selectedIndex > 0)
                {
                    Tankkaus previousItem = tankkaukset[selectedIndex - 1];
                    double keskikulutus = Tankkaus.LaskeKeskikulutus(previousItem, selectedItem);
                    string formattedResult = $"{keskikulutus:0.##} L/Km";
                    labelKulutus.Text = formattedResult;
                }
                else
                {
                    labelKulutus.Text = "";
                    DisplayAlert("Tietoa ei saatavilla", "Tarvitaan vähintään kaksi tankkausta keskikulutuksen laskemiseen.", "OK");
                }
            }
        }

        //Avaa AddPage-sivun tietojen lisäämiseksi
        private async void addPageBtn_Clicked(object sender, EventArgs e)
        {
            var addPage = new AddPage(this);
            await Shell.Current.Navigation.PushModalAsync(addPage);
        }

        //Poista listan
        private async void poistaNappi_Clicked(object sender, EventArgs e)
        {
            bool vastaus = await DisplayAlert("Vahvistus", "Haluatko varmasti poistaa kakki tiedot?", "Kyllä", "Peruuta");
            if(vastaus)
            {
                tankkaukset.Clear();
                File.WriteAllText(fileName, "[]");
                tankkausLista.ItemsSource = null;
                labelIlmoitus.IsVisible = true;
                labelKulutus.Text = "";
            }

        }
    }
}
