using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace DantecMarcket.Modeles
{
    public class CommandeDetail : INotifyPropertyChanged
    {
        #region Attributs
        private int id;
        private string nomProduit;
        private int quantite;
        private double prixretenu;
        private double total;
        private string imageUrl;
        private string etat; // Ajout de l'état de la commande
        private string planningDetails; // Ajout du planning
        public List<ImageProduit> LesImages { get; set; }
        #endregion

        #region Constructeurs

        #endregion

        #region Getters/Setters
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nomProduit")]
        public string NomProduit { get; set; }

        [JsonProperty("quantite")]
        public int Quantite
        {
            get => quantite;
            set
            {
                if (quantite != value)
                {
                    quantite = value;
                    OnPropertyChanged(nameof(Quantite));
                }
            }
        }

        [JsonProperty("prixretenu")]
        public double Prixretenu { get; set; }

        [JsonProperty("total")]
        public double Total
        {
            get => total;
            set
            {
                if (total != value)
                {
                    total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        [JsonProperty("imageUrl")]
        public string Url { get; set; }
        public string FullUrl => Url.StartsWith("http") ? Url : "http://213.130.144.159/" + Url;

        [JsonProperty("etat")]
        public string Etat
        {
            get => etat;
            set
            {
                if (etat != value)
                {
                    etat = value;
                    OnPropertyChanged(nameof(Etat));
                }
            }
        }

        [JsonProperty("planningDetails")]
        public string PlanningDetails
        {
            get => planningDetails;
            set
            {
                if (planningDetails != value)
                {
                    planningDetails = value;
                    OnPropertyChanged(nameof(PlanningDetails));
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}