using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AP4DantecMarket.Modeles
{
    public class Promotion
    {
        #region Attributs
        private int id;
        private double prixPromo;
        private string dateDebut;
        private string dateFin;
        #endregion

        #region Constructeurs
        public Promotion(int id, double prixPromo, string dateDebut, string dateFin)
        {
            this.id = id;
            this.prixPromo = prixPromo;
            this.dateDebut = dateDebut;
            this.dateFin = dateFin;
   
        }
        #endregion

        #region Getters/Setters
        [JsonProperty("id")]
        public int Id { get => id; set => id = value; }

        [JsonProperty("dateDebut")]
        public string DateDebut { get => dateDebut; set => dateDebut = value; }

        [JsonProperty("dateFin")]
        public string DateFin { get => dateFin; set => dateFin = value; }
        [JsonProperty("prix")]
        public double PrixPromo { get => prixPromo; set => prixPromo = value; }
        [JsonIgnore]
        public Produit Produit { get; set; }

        #endregion
    }
}
