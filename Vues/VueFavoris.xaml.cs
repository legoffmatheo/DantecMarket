using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using AP4DantecMarket.Modeles;
using projetBase;
using Microsoft.Maui.Graphics;
using DantecMarcket.Vues;

namespace AP4DantecMarket.Vues
{
    public partial class VueFavoris : ContentPage
    {
        private List<Produit> tousLesFavoris = new List<Produit>();

        public VueFavoris()
        {
            InitializeComponent();
            ChargerFavoris();
        }

        // ?? Charger les favoris depuis l'API
        private async void ChargerFavoris()
        {
            try
            {
                int userId = Constantes.UserId;
                if (userId <= 0)
                {
                    await DisplayAlert("Erreur", "Utilisateur non connecté.", "OK");
                    return;
                }

                using HttpClient client = new HttpClient();
                string apiUrl = "http://213.130.144.159/api/mobile/getListeFavorisMobile";
                var requestData = new { userId = userId };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Erreur", "Échec de la récupération des favoris.", "OK");
                    return;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                tousLesFavoris = JsonConvert.DeserializeObject<List<Produit>>(responseContent);
                AfficherFavoris(tousLesFavoris);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Impossible de charger les favoris : {ex.Message}", "OK");
            }
        }

        private void AfficherFavoris(List<Produit> favoris)
        {
            FavorisContainer.Children.Clear();

            if (favoris == null || favoris.Count == 0)
            {
                FavorisContainer.Children.Add(new Label
                {
                    Text = "Aucun favori trouvé.",
                    TextColor = Colors.White,
                    FontSize = 18,
                    HorizontalOptions = LayoutOptions.Center
                });
                return;
            }

            foreach (var produit in favoris)
            {
                var supprimerButton = new Button
                {
                    Text = "Supprimer",
                    BackgroundColor = Colors.Red,
                    TextColor = Colors.White,
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                supprimerButton.Clicked += (sender, args) => SupprimerFavori(produit.Id);

                var detailsButton = new Button
                {
                    Text = "Voir Détails",
                    BackgroundColor = Colors.Blue,
                    TextColor = Colors.White,
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                detailsButton.Clicked += (sender, args) => Navigation.PushAsync(new VueProduitDetail(produit));

                var frame = new Frame
                {
                    Padding = 10,
                    Margin = new Thickness(5, 5, 5, 5),
                    BackgroundColor = Color.FromArgb("#2E2E30"),
                    BorderColor = Color.FromArgb("#FFD700"),
                    CornerRadius = 10,
                    HasShadow = false,
                    Content = new StackLayout
                    {
                        Spacing = 5,
                        Children =
                {
                    new Label { Text = $"ID : {produit.Id}", FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = Colors.White },
                    new Label { Text = $"Nom : {produit.NomProduit}", FontSize = 14, TextColor = Color.FromArgb("#FFD700") },
                    new Label { Text = $"Prix : {produit.Prix} €", FontSize = 14, TextColor = Color.FromArgb("#32CD32") },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 10,
                        Children = { detailsButton, supprimerButton }
                    }
                }
                    }
                };

                FavorisContainer.Children.Add(frame);
            }
        }
        private async void SupprimerFavori(int produitId)
        {
            try
            {
                int userId = Constantes.UserId;
                if (userId <= 0)
                {
                    await DisplayAlert("Erreur", "Utilisateur non connecté.", "OK");
                    return;
                }

                using HttpClient client = new HttpClient();
                string apiUrl = "http://213.130.144.159/api/mobile/SupprimerFavoriMobile";
                var requestData = new { userId = userId, produitId = produitId };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Erreur", "Échec de la suppression du favori.", "OK");
                    return;
                }

                await DisplayAlert("Succès", "Favori supprimé avec succès.", "OK");
                ChargerFavoris(); // Recharge la liste après suppression
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Impossible de supprimer le favori : {ex.Message}", "OK");
            }
        }

        // ?? Bouton d'actualisation pour recharger la liste
        private void OnRefreshClicked(object sender, EventArgs e)
        {
            ChargerFavoris();
        }
    }
}
