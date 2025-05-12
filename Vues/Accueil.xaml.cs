using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using AP4DantecMarket.Modeles;
using DantecMarcket.Vues;
using System.Net.Http; // Ajout pour HttpClient

namespace AP4DantecMarket.Vues
{
    public partial class VueAccueil : ContentPage
    {
        private ObservableCollection<string> messages;
        private ObservableCollection<Promotion> promotions = new ObservableCollection<Promotion>(); // Déclaration
        private int currentIndex = 0;
        private bool isRunning = true;

        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                currentIndex = value;
                OnPropertyChanged(nameof(CurrentIndex));
            }
        }

        public VueAccueil()
        {
            InitializeComponent();
            BindingContext = this;

            // Charger l'email de l'utilisateur
            LoadUserEmail();
            LoadPromotions(); // Charger les promos

            // Liste des messages
            messages = new ObservableCollection<string>
            {
                "?? Bienvenue sur Dantec Market !",
                "?? Découvrez nos offres exclusives",
                "?? Profitez de nos promotions !"
            };

            // Lancer le carrousel en tâche de fond
            StartCarousel();
        }

        private async void LoadUserEmail()
        {
            await Task.Delay(500); // Simule un petit délai réseau

            string userJson = Preferences.Get("User", null); // Récupérer les infos utilisateur

            if (!string.IsNullOrEmpty(userJson))
            {
                User currentUser = JsonConvert.DeserializeObject<User>(userJson);
                UserEmailLabel.Text = $" {currentUser.Email}";
            }
            else
            {
                UserEmailLabel.Text = " Email inconnu";
            }
        }

        private async void StartCarousel()
        {
            while (isRunning)
            {
                await Task.Delay(3000); // Attendre 3 secondes
                CurrentIndex = (CurrentIndex + 1) % messages.Count;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            isRunning = false; // Arrêter la boucle quand l'utilisateur quitte la page
        }

        private async void OnVoirProduitsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("rechercher");
        }

        private async void OnVoirCategorieClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("categorie");
        }

        private async void OnVoirPanierClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("panier");
        }

        private async void AllerCommandes(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VueCommandes());
        }

        private async void OnVoirFavorisClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("favoris");
        }
        private async void LoadPromotions()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync("http://213.130.144.159/api/mobile/produits");
                    var produits = JsonConvert.DeserializeObject<List<Produit>>(response);

                    promotions.Clear();
                    foreach (var produit in produits)
                    {
                        if (produit.LesPromos != null && produit.LesPromos.Any())
                        {
                            foreach (var promo in produit.LesPromos)
                            {
                                promo.Produit = produit; // Associez le produit à la promotion
                                promotions.Add(promo);
                            }
                        }
                    }

                    // Vérification des données
                    Console.WriteLine($"Promotions chargées : {promotions.Count}");
                    foreach (var promo in promotions)
                    {
                        Console.WriteLine($"Produit : {promo.Produit.NomProduit} - Promo : {promo.PrixPromo}€");
                    }

                    PromotionListView.ItemsSource = promotions;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du chargement des promotions : {ex.Message}");
                }
            }
        }



        private async void OnPromoClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var promo = button?.BindingContext as Promotion;

            if (promo != null && promo.Produit != null)
            {
                await Navigation.PushAsync(new VueProduitDetail(promo.Produit));
            }
            else
            {
                await DisplayAlert("Erreur", "Aucune promotion sélectionnée ou produit non associé.", "OK");
            }
        }


    }
}
