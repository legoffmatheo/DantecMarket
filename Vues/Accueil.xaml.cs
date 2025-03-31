using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using AP4DantecMarket.Modeles;
using DantecMarcket.Vues;

namespace AP4DantecMarket.Vues
{
    public partial class VueAccueil : ContentPage
    {
        private ObservableCollection<string> messages;
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

            // Liste des messages
            messages = new ObservableCollection<string>
            {
                "?? Bienvenue sur Dantec Market !",
                "??? D�couvrez nos offres exclusives",
                "?? Profitez de nos promotions !"
            };

            // Lancer le carrousel en t�che de fond
            StartCarousel();
        }

        private async void LoadUserEmail()
        {
            // Simule la r�cup�ration de l'email depuis une source (API, stockage local, etc.)
            await Task.Delay(500); // Simule un petit d�lai r�seau

            string userJson = Preferences.Get("User", null); // R�cup�rer les infos utilisateur

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
            isRunning = false; // Arr�ter la boucle quand l'utilisateur quitte la page
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

    }
}
