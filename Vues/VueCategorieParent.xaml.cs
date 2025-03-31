using AP4DantecMarket.Modeles;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using projetBase.Services;
using DantecMarcket.Vues;

namespace AP4DantecMarket.Vues
{
    public partial class VueCategorieParent : ContentPage
    {
        private readonly Apis _apiService = new Apis();
        public ObservableCollection<CategorieParent> CategoriesParent { get; set; }

        public VueCategorieParent()
        {
            InitializeComponent();
            CategoriesParent = new ObservableCollection<CategorieParent>();
            BindingContext = this;
            ChargerCategoriesParentes();
        }

        private async void ChargerCategoriesParentes()
        {
            try
            {
                IsBusy = true; // D�sactiver les interactions utilisateur pendant le chargement des donn�es
                var categoriesParent = await _apiService.GetAllAsync<CategorieParent>("api/mobile/allcategoriesParent");
                CategoriesParent.Clear();
                foreach (var categorieParent in categoriesParent)
                {
                    CategoriesParent.Add(categorieParent);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Impossible de charger les cat�gories parentes. D�tails : {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false; // R�activer les interactions utilisateur apr�s le chargement des donn�es
            }
        }

        private async void OnCategoryTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is CategorieParent selectedCategory)
            {
                await Task.Delay(100); // Assurer un petit d�lai pour �viter les soucis d�input
                await Navigation.PushAsync(new VueCategorie(selectedCategory), false);
            }
        }



    }
}
