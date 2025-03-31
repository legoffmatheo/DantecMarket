using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Maui.Controls;
using AP4DantecMarket.Modeles;
using DantecMarcket.Vues;

namespace AP4DantecMarket.Vues
{
    public partial class VueProduit : ContentPage
    {
        private const string ApiUrl = "http://213.130.144.159/api/mobile/produits";
        private List<Produit> _produits;

        public VueProduit()
        {
            InitializeComponent();
            LoadProduits();
        }

        private async void LoadProduits()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync(ApiUrl);
                    _produits = JsonConvert.DeserializeObject<List<Produit>>(response);

                    TrierProduits(); // Appliquer le tri dès le chargement
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Impossible de charger les produits : {ex.Message}", "OK");
            }
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            TrierProduits();
        }

        private void OnSortPickerChanged(object sender, EventArgs e)
        {
            TrierProduits();
        }

        private void TrierProduits()
        {
            string searchText = ProductSearchBar.Text?.ToLower() ?? "";
            var produitsFiltres = string.IsNullOrWhiteSpace(searchText)
                ? _produits
                : _produits.Where(p => p.NomProduit.ToLower().Contains(searchText)).ToList();

            switch (SortPicker.SelectedIndex)
            {
                case 1:
                    ProduitsListView.ItemsSource = produitsFiltres.OrderByDescending(p => p.NomProduit).ToList();
                    break;
                case 2:
                    ProduitsListView.ItemsSource = produitsFiltres.OrderBy(p => p.Prix).ToList(); // Prix croissant
                    break;
                case 3:
                    ProduitsListView.ItemsSource = produitsFiltres.OrderByDescending(p => p.Prix).ToList(); // Prix décroissant
                    break;
                default:
                    ProduitsListView.ItemsSource = produitsFiltres.OrderBy(p => p.NomProduit).ToList(); // Par défaut : Nom A ? Z
                    break;
            }
        }

        private async void OnProduitTapped(object sender, EventArgs e)
        {
            var produit = ((VisualElement)sender).BindingContext as Produit;
            if (produit != null)
            {
                await Navigation.PushAsync(new VueProduitDetail(produit));
            }
        }
    }
}
