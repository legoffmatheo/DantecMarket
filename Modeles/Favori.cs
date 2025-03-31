using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AP4DantecMarket.Modeles
{
    public class Favori
    {
        #region Attributs
        private int id;
        private string nomProduit;
        private double prix;
        private string imageUrl;
        #endregion

        #region Constructeurs
        public Favori(int id, string nomProduit, double prix, string imageUrl)
        {
            this.id = id;
            this.nomProduit = nomProduit;
            this.prix = prix;
            this.imageUrl = imageUrl;
        }
        #endregion

        #region Getters/Setters
        [JsonProperty("id")]
        public int Id { get => id; set => id = value; }

        [JsonProperty("nomProduit")]
        public string NomProduit { get => nomProduit; set => nomProduit = value; }

        [JsonProperty("prix")]
        public double Prix { get => prix; set => prix = value; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get => imageUrl; set => imageUrl = value; }

        // Génère l'URL complète de l'image
        public string ImageFullUrl
        {
            get
            {
                return $"http://213.130.144.159/{imageUrl}";
            }
        }
        #endregion
    }
}
