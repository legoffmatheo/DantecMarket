using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using AP4DantecMarket.Modeles;
using projetBase;
using DantecMarcket.Modeles;
using System.ComponentModel.Design;

namespace DantecMarcket.Vues
{
    public partial class VueReserver : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
      
        public ObservableCollection<Planning> Plannings { get; set; } = new();
        private Planning PlanningSelectionne;
        private int CommandeId;

        public VueReserver(int commandeId)
        {
            InitializeComponent();
            BindingContext = this;
            ChargerPlannings();
            CommandeId = commandeId;

        }

        private async void ChargerPlannings()
        {
            try
            {
                var response = await _httpClient.GetAsync(Constantes.BaseApiAddress + "api/mobile/semaine-courante");
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var listePlannings = JsonConvert.DeserializeObject<List<Planning>>(responseString);
                    Plannings.Clear();
                    foreach (var planning in listePlannings)
                    {
                        Plannings.Add(planning);
                    }
                }
                else
                {
                    await DisplayAlert("Erreur", "Impossible de charger les plannings.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                await DisplayAlert("Erreur", "Une erreur s'est produite.", "OK");
            }
        }

        private void OnPlanningSelected(object sender, EventArgs e)
        {
            if (PlanningPicker.SelectedItem is Planning selectedPlanning)
            {
                PlanningSelectionne = selectedPlanning;
                HoraireLabel.Text = $"Plage horaire : {selectedPlanning.PlageHoraire}";
                HoraireLabel.IsVisible = true;
            }
        }

        private async void OnReserverClicked(object sender, EventArgs e)
        {
            if (PlanningSelectionne == null)
            {
                await DisplayAlert("Erreur", "Veuillez sélectionner un planning.", "OK");
                return;
            }

            try
            {
                var requestData = new
                {
                    idUser = Constantes.UserId,
                    jour = PlanningSelectionne.Jour.ToString("yyyy-MM-dd HH:mm:ss"),
                    heureDebut = PlanningSelectionne.HeureDebut.ToString("HH:mm:ss"),
                    commandeId = CommandeId,  // Utiliser l'ID de la commande passé dans le constructeur
                    id = PlanningSelectionne.Id // Id du planning
                };

                string json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(Constantes.BaseApiAddress + "api/mobile/reservermobile", content);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Succès", "Réservation effectuée !", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Erreur", "Impossible d'effectuer la réservation.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                await DisplayAlert("Erreur", "Une erreur s'est produite.", "OK");
            }
        }
    }
}
