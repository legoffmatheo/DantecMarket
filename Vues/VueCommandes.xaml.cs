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

namespace AP4DantecMarket.Vues
{
    public partial class VueCommandes : ContentPage
    {
        private List<CommandeModel> toutesLesCommandes = new List<CommandeModel>();
        private Picker filtrePicker;

        public VueCommandes()
        {
            InitializeComponent();
            AjouterMenuFiltrage();
            ChargerCommandes();
        }

        private async void ChargerCommandes()
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
                string apiUrl = "http://213.130.144.159/api/mobile/allcommandes";
                var requestData = new { userId = userId };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Erreur", "Échec de la récupération des commandes.", "OK");
                    return;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                toutesLesCommandes = JsonConvert.DeserializeObject<List<CommandeModel>>(responseContent);
                AfficherCommandes(toutesLesCommandes);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Impossible de charger les commandes : {ex.Message}", "OK");
            }
        }

        private void FiltrerCommandes(string etat)
        {
            if (etat == "Tous")
            {
                AfficherCommandes(toutesLesCommandes);
            }
            else
            {
                var commandesFiltrees = toutesLesCommandes.FindAll(cmd => cmd.Etat == etat);
                AfficherCommandes(commandesFiltrees);
            }
        }

        private void AfficherCommandes(List<CommandeModel> commandes)
        {
            CommandesContainer.Children.Clear();
            AjouterMenuFiltrage();

            if (commandes == null || commandes.Count == 0)
            {
                CommandesContainer.Children.Add(new Label
                {
                    Text = "Aucune commande trouvée.",
                    TextColor = Colors.White,
                    FontSize = 18,
                    HorizontalOptions = LayoutOptions.Center
                });
                return;
            }

            foreach (var commande in commandes)
            {
                var supprimerButton = new Button
                {
                    Text = "Supprimer",
                    BackgroundColor = Colors.Red,
                    TextColor = Colors.White,
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                supprimerButton.Clicked += (sender, args) => SupprimerCommande(commande.Id);

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
                            new Label { Text = $"ID : {commande.Id}", FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = Colors.White },
                            new Label { Text = $"Date : {commande.DateCommande:yyyy-MM-dd HH:mm:ss}", FontSize = 14, TextColor = Color.FromArgb("#FFD700") },
                            new Label { Text = $"État : {commande.Etat}", FontSize = 14, TextColor = Color.FromArgb("#32CD32") },
                            supprimerButton
                        }
                    }
                };

                CommandesContainer.Children.Add(frame);
            }
        }

        private void AjouterMenuFiltrage()
        {
            if (filtrePicker == null)
            {
                filtrePicker = new Picker
                {
                    Title = "Filtrer par état",
                    ItemsSource = new List<string> { "Tous", "Livrée", "Traitée", "En cours de traitement", "Confirmée" },
                    SelectedIndex = 0,
                    TextColor = Colors.White,
                    BackgroundColor = Colors.Gray
                };
                filtrePicker.SelectedIndexChanged += (sender, args) =>
                {
                    FiltrerCommandes(filtrePicker.SelectedItem.ToString());
                };
            }
            CommandesContainer.Children.Insert(0, filtrePicker);
        }

        private async void SupprimerCommande(int commandeId)
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
                string apiUrl = "http://213.130.144.159/api/mobile/SupprimerCommande";
                var requestData = new { userId = userId, commandeId = commandeId };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Erreur", "Échec de la suppression de la commande.", "OK");
                    return;
                }

                await DisplayAlert("Succès", "Commande supprimée avec succès.", "OK");
                ChargerCommandes();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Impossible de supprimer la commande : {ex.Message}", "OK");
            }
        }
    }
}
