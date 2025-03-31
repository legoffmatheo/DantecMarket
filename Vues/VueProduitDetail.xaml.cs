using AP4DantecMarket.Modeles;
using Newtonsoft.Json;
using projetBase;
using projetBase.Services;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using DantecMarcket.Modeles;

namespace DantecMarcket.Vues
{
    public partial class VueProduitDetail : ContentPage
    {
        private readonly Apis _apiServices = new Apis();
        private Produit ProduitActuel;

        public VueProduitDetail(Produit produit)
        {
            InitializeComponent();
            ProduitActuel = produit;
            BindingContext = ProduitActuel;
        }

        private async void AjouterProduitAuPanier(object sender, EventArgs e)
        {
            try
            {
                int userId = Constantes.UserId;
                if (userId <= 0)
                {
                    await DisplayAlert("Erreur", "Utilisateur non connecté.", "OK");
                    return;
                }

                int quantiteParDefaut = 1;
                var produitData = new AjoutProduitCommandemobile(userId, ProduitActuel.Id, quantiteParDefaut, ProduitActuel.Prix);

                string json = JsonConvert.SerializeObject(produitData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(Constantes.BaseApiAddress + "api/mobile/AjoutProduitCommandemobile", content);

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Succès", "Produit ajouté avec succès !", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Erreur", "Impossible d'ajouter le produit.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Exception : {ex.Message}");
                await DisplayAlert("Erreur", "Une erreur s'est produite.", "OK");
            }
        }

        private async void AjouterAuxFavoris(object sender, EventArgs e)
        {
            try
            {
                int userId = Constantes.UserId;
                if (userId <= 0)
                {
                    await DisplayAlert("Erreur", "Utilisateur non connecté.", "OK");
                    return;
                }

                var favoriData = new { userId = userId, produitId = ProduitActuel.Id };
                string json = JsonConvert.SerializeObject(favoriData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync("http://213.130.144.159/api/mobile/AjoutFavoriMobile", content);

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Succès", "Produit ajouté aux favoris !", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Erreur", "Impossible d'ajouter aux favoris.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Exception : {ex.Message}");
                await DisplayAlert("Erreur", "Une erreur s'est produite.", "OK");
            }
        }
    }
}