using Microsoft.Maui.Storage;
using System.Text.Json;

namespace Tankkaussovellus; 

public partial class AddPage : ContentPage
{
	private MainPage _mainPage;
    string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tankkausmuistio.json");
    List<Tankkaus> tankkaukset = new();

    public AddPage(MainPage mainPage)
	{
        InitializeComponent();
		_mainPage = mainPage;
	}

    private async void tallennusNappi_Clicked(object sender, EventArgs e)
    {

        // Tarkista, ett‰ k‰ytt‰j‰n syˆtteet ovat oikeassa muodossa
        if (!int.TryParse(kmEntry.Text, out var km) ||
            !int.TryParse(litratEntry.Text, out var lit) ||
            !double.TryParse(summaEntry.Text, out var sum))
        {
            await DisplayAlert("Virhe", "Tarkista syˆtteet, vain numerot sallittu.", "OK");
            return;
        }
        var uusiTankkaus = new Tankkaus
        {
            Pvm = DateTime.Now.ToString("dd.MM.yyyy"),
            Kilometrit = km,
            Litraa = lit,
            Summa = sum,
        };

        //Tarkistaan tiedosto ennest‰‰n olemassa
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
                await DisplayAlert("Virhe", $"Virhe tiedoston lukemisessa: {Ex.Message}", "OK");
            }
        }
        
        //Lis‰t‰‰n listaan
        tankkaukset.Add(uusiTankkaus);

        //Sarjaa p‰ivitetty lista takaisin JSON-muotoon
        var updatedJson = JsonSerializer.Serialize(tankkaukset, new JsonSerializerOptions { WriteIndented = true });

        //P‰ivit‰‰n updatedJson 
        File.WriteAllText(fileName, updatedJson);

        //P‰ivit‰ CollectionView p‰‰sivulla tietojen lis‰‰misen j‰lkeen
        _mainPage.RefeshList();

        await DisplayAlert("Vahvistus", "Tankkaus tallennettu.", "Ok");

        //Poista tiedot lamakkeelta
        kmEntry.Text = "";
        litratEntry.Text = "";
        summaEntry.Text = "";

        // Return to MainPage (which will reload data)
        await Shell.Current.Navigation.PopAsync();
    }

    private void palautusNappi_Clicked(object sender, EventArgs e)
    {
        Shell.Current.Navigation.PopAsync();
    }
}