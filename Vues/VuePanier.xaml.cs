using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using DantecMarcket.Modeles;
using projetBase;

namespace DantecMarcket.Vues
{
    public partial class VuePanier : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private List<CommandeDetail> ProduitsPanier = new();

        public VuePanier()
        {
            InitializeComponent();
            ChargerPanier();
        }

        private async void ChargerPanier()
        {
            try
            {
                int userId = Constantes.UserId;
                if (userId <= 0)
                {
                    await DisplayAlert("Erreur", "Utilisateur non connecté.", "OK");
                    return;
                }

                var requestData = new { userId };
                string json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)); // Timeout de 10s
                var response = await _httpClient.PostAsync(Constantes.BaseApiAddress + "api/mobile/commandenonvalideemobile", content, cts.Token);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var commandes = JsonConvert.DeserializeObject<List<CommandeDetail>>(responseString);

                        if (commandes == null || commandes.Count == 0)
                        {
                            await DisplayAlert("Info", "Votre panier est vide.", "OK");
                            return;
                        }

                        ProduitsPanier = commandes;
                        AfficherProduits();
                        CalculerTotalGeneral();
                    }
                    catch (JsonException jsonEx)
                    {
                        await DisplayAlert("Erreur", "Problème de format des données reçues.", "OK");
                        Console.WriteLine($"Erreur JSON : {jsonEx.Message}\nRéponse brute : {responseString}");
                    }
                }
                else
                {
                    await DisplayAlert("Erreur", $"Impossible de charger le panier.\nCode : {response.StatusCode}", "OK");
                    Console.WriteLine($"Réponse API : {responseString}");
                }
            }
            catch (TaskCanceledException)
            {
                await DisplayAlert("Erreur", "La requête a pris trop de temps.", "OK");
            }
            catch (HttpRequestException httpEx)
            {
                await DisplayAlert("Erreur", "Problème de connexion à l'API.", "OK");
                Console.WriteLine($"Erreur HTTP : {httpEx.Message}");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Une erreur s'est produite : {ex.Message}", "OK");
                Console.WriteLine($"Exception : {ex.Message}");
            }
        }
        private void AfficherProduits()
        {
            ProduitsContainer.Children.Clear(); // Vider le conteneur avant d'ajouter de nouvelles cartes.

            foreach (var produit in ProduitsPanier)
            {
                try
                {
                    Console.WriteLine($"Affichage du produit : {produit.NomProduit}");

                    var frame = new Frame
                    {
                        Padding = 15,
                        Margin = new Thickness(10),
                        BackgroundColor = Color.FromArgb("#2E2E30"),
                        BorderColor = Colors.Gold,
                        CornerRadius = 15,
                        HasShadow = true,
                        WidthRequest = 400,
                        HeightRequest = -1, // Ajusté pour s'adapter au contenu
                        Shadow = new Shadow
                        {
                            Brush = Brush.Black,
                            Opacity = 0.5f,
                            Radius = 10
                        }
                    };

                    var grid = new Grid
                    {
                        RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },  // Titre
                    new RowDefinition { Height = GridLength.Auto },  // Image (optionnelle)
                    new RowDefinition { Height = GridLength.Auto },  // Quantité + Prix + Total
                    new RowDefinition { Height = GridLength.Auto }   // Bouton Supprimer
                },
                        ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star }  // Centrage des éléments
                }
                    };

                    // Titre du produit
                    var titreLabel = new Label
                    {
                        Text = produit.NomProduit,
                        FontSize = 18,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Colors.White,
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        LineBreakMode = LineBreakMode.WordWrap,
                        WidthRequest = 300
                    };
                    Grid.SetRow(titreLabel, 0);
                    grid.Children.Add(titreLabel);

                    // Image du produit (si disponible)
                    if (!string.IsNullOrWhiteSpace(produit.FullUrl))
                    {
                        var imageProduit = new Image
                        {
                            Source = new UriImageSource
                            {
                                Uri = new Uri(produit.FullUrl),
                                CachingEnabled = true,
                                CacheValidity = TimeSpan.FromDays(1)
                            },
                            WidthRequest = 100,
                            HeightRequest = 100,
                            Aspect = Aspect.AspectFill,
                            HorizontalOptions = LayoutOptions.Center
                        };
                        Grid.SetRow(imageProduit, 1);
                        grid.Children.Add(imageProduit);
                    }
                    else
                    {
                        // Ajuster la disposition si l'image est absente
                        grid.RowDefinitions[1] = new RowDefinition { Height = 0 }; // Réduire l'espace de l'image
                    }

                    // Zone de gestion de la quantité et prix
                    var actionsLayout = new HorizontalStackLayout
                    {
                        Spacing = 10,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                {
                    new Button
                    {
                        Text = "-",
                        FontSize = 16,
                        BackgroundColor = Colors.Orange,
                        TextColor = Colors.White,
                        WidthRequest = 35,
                        HeightRequest = 35,
                        Command = new Command(() => ModifierQuantite(produit, produit.Quantite - 1))
                    },
                    new Label
                    {
                        Text = $"Quantité: {produit.Quantite}",
                        FontSize = 16,
                        TextColor = Colors.White,
                        HorizontalTextAlignment = TextAlignment.Center
                    },
                    new Button
                    {
                        Text = "+",
                        FontSize = 16,
                        BackgroundColor = Colors.Orange,
                        TextColor = Colors.White,
                        WidthRequest = 35,
                        HeightRequest = 35,
                        Command = new Command(() => ModifierQuantite(produit, produit.Quantite + 1))
                    }
                }
                    };

                    var prixLayout = new HorizontalStackLayout
                    {
                        Spacing = 15,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                {
                    new Label
                    {
                        Text = $"Prix: {produit.Prixretenu:F2} €",
                        FontSize = 14,
                        TextColor = Colors.Green,
                        HorizontalTextAlignment = TextAlignment.Center
                    },
                    new Label
                    {
                        Text = $"Total: {produit.Total:F2} €",
                        FontSize = 14,
                        TextColor = Colors.Cyan,
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                }
                    };

                    var actionsStack = new VerticalStackLayout
                    {
                        Spacing = 10,
                        HorizontalOptions = LayoutOptions.Center,
                        Children = { actionsLayout, prixLayout }
                    };
                    Grid.SetRow(actionsStack, 2);
                    grid.Children.Add(actionsStack);

                    // Bouton Supprimer
                    var supprimerButton = new Button
                    {
                        Text = "Supprimer",
                        BackgroundColor = Colors.Red,
                        TextColor = Colors.White,
                        FontSize = 14,
                        CornerRadius = 10,
                        HeightRequest = 40,
                        WidthRequest = 140,
                        Command = new Command(() => SupprimerProduit(produit)),
                        HorizontalOptions = LayoutOptions.Center
                        };
                        Grid.SetRow(supprimerButton, 3);
                        grid.Children.Add(supprimerButton);

                        frame.Content = grid;
                        ProduitsContainer.Children.Add(frame);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'affichage du produit {produit.NomProduit} : {ex.Message}");
                }
            }
        }



        private void RecalculerTotalProduit(CommandeDetail produit)
        {
            produit.Total = produit.Quantite * produit.Prixretenu;
        }

        private async void ModifierQuantite(CommandeDetail produit, int nouvelleQuantite)
        {
            if (nouvelleQuantite < 0) return; // Évite les quantités négatives

            try
            {
                var requestData = new
                {
                    userId = Constantes.UserId,
                    nomProduit = produit.NomProduit,
                    quantite = nouvelleQuantite
                };

                string json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(Constantes.BaseApiAddress + "api/mobile/MajProduitCommandemobile", content);
                if (response.IsSuccessStatusCode)
                {
                    if (nouvelleQuantite == 0)
                    {
                        ProduitsPanier.Remove(produit); // Supprime si la quantité devient 0
                    }
                    else
                    {
                        produit.Quantite = nouvelleQuantite;
                        RecalculerTotalProduit(produit);
                    }

                    CalculerTotalGeneral();
                    AfficherProduits(); // Rafraîchit l'affichage
                }
                else
                {
                    await DisplayAlert("Erreur", "Impossible de modifier la quantité.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                await DisplayAlert("Erreur", "Une erreur s'est produite lors de la mise à jour.", "OK");
            }
        }

        private void SupprimerProduit(CommandeDetail produit)
        {
            ModifierQuantite(produit, 0);
            ProduitsPanier.Remove(produit);
            AfficherProduits();
            CalculerTotalGeneral();
        }

        private void CalculerTotalGeneral()
        {
            double totalGeneral = 0;
            foreach (var produit in ProduitsPanier)
            {
                totalGeneral += produit.Total;
            }
            TotalGeneralLabel.Text = $"Total : {totalGeneral:F2} €";
        }

        private async void OnReserverMobileClicked(object sender, EventArgs e)
        {
            if (ProduitsPanier.Count > 0)
            {
                try
                {
                    // Assurez-vous que l'ID est de type int
                    int commandeId = ProduitsPanier[0].Id;  // Accédez à l'ID du premier produit dans le panier
                    await Navigation.PushAsync(new VueReserver(commandeId)); // Passez l'ID à la page VueReserver
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erreur", "Erreur lors de l'accès à l'ID de la commande.", "OK");
                    Console.WriteLine($"Erreur : {ex.Message}");
                }
            }
            else
            {
                await DisplayAlert("Erreur", "Le panier est vide.", "OK");
            }
        }
    }
}
