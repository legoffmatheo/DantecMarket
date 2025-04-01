using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using AP4DantecMarket.Modeles;
using DantecMarcket.Vues;
using projetBase;

namespace AP4DantecMarket.Vues
{
    public partial class VueFavoris : ContentPage
    {
        private List<Favori> tousLesFavoris = new List<Favori>();

        public VueFavoris()
        {
            InitializeComponent();
            ChargerFavoris();
        }

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
                var requestData = new { userId };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Erreur", "Échec de la récupération des favoris.", "OK");
                    return;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                tousLesFavoris = JsonConvert.DeserializeObject<List<Favori>>(responseContent);
                AfficherFavoris(tousLesFavoris);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Impossible de charger les favoris : {ex.Message}", "OK");
            }
        }

        private void AfficherFavoris(List<Favori> favoris)
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

            foreach (var favori in favoris)
            {
                var imageProduit = new Image
                {
                    Source = favori.ImageFullUrl,
                    HeightRequest = 100,
                    WidthRequest = 100,
                    Aspect = Aspect.AspectFit
                };

                var detailsButton = new Button
                {
                    Text = "Voir Détails",
                    BackgroundColor = Colors.Blue,
                    TextColor = Colors.White,
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                detailsButton.Clicked += async (sender, args) => await VoirDetailsProduit(favori.Id);

                var supprimerButton = new Button
                {
                    Text = "Supprimer",
                    BackgroundColor = Colors.Red,
                    TextColor = Colors.White,
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                supprimerButton.Clicked += async (sender, args) => await SupprimerFavori(favori.Id);

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
                            imageProduit,
                            new Label { Text = $"Nom : {favori.NomProduit}", FontSize = 14, TextColor = Color.FromArgb("#FFD700") },
                            new Label { Text = $"Prix : {favori.Prix} €", FontSize = 14, TextColor = Color.FromArgb("#32CD32") },
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

        private async Task VoirDetailsProduit(int produitId)
        {
            try
            {
                using HttpClient client = new HttpClient();
                string apiUrl = "http://213.130.144.159/api/mobile/produits";

                var response = await client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Erreur", "Impossible de récupérer la liste des produits.", "OK");
                    return;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var listeProduits = JsonConvert.DeserializeObject<List<Produit>>(responseContent);

                // Chercher le produit par son ID
                var produitDetail = listeProduits?.Find(p => p.Id == produitId);

                if (produitDetail != null)
                {
                    await Navigation.PushAsync(new VueProduitDetail(produitDetail));
                }
                else
                {
                    await DisplayAlert("Erreur", "Produit introuvable.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Erreur lors du chargement du produit : {ex.Message}", "OK");
            }
        }

        private async Task SupprimerFavori(int produitId)
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
                var requestData = new { userId, produitId };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Erreur", "Échec de la suppression du favori.", "OK");
                    return;
                }

                await DisplayAlert("Succès", "Favori supprimé avec succès.", "OK");
                ChargerFavoris();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Impossible de supprimer le favori : {ex.Message}", "OK");
            }
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            ChargerFavoris();
        }
    }
}
