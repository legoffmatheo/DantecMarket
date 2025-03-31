using Newtonsoft.Json;
using System;

namespace AP4DantecMarket.Modeles
{
    public class CommandeModel
    {
        #region Attributs
        private int id;
        private DateTime dateCommande;
        private string etat;
        #endregion

        #region Constructeurs
        public CommandeModel(int id, DateTime dateCommande, string etat)
        {
            this.id = id;
            this.dateCommande = dateCommande;
            this.etat = etat;
        }
        #endregion

        #region Getters/Setters
        [JsonProperty("id")]
        public int Id { get => id; set => id = value; }

        [JsonProperty("dateCommande")]
        public DateTime DateCommande { get => dateCommande; set => dateCommande = value; }

        [JsonProperty("etat")]
        public string Etat { get => etat; set => etat = value; }
        #endregion
    }
}
