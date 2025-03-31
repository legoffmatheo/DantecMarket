using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace AP4DantecMarket.Modeles
{
    public class Produit
    {
        #region Attributs
        private int id;
        private string nomProduit;
        private double prix;
        private string descriptioncourte;
        private string description;
        private int quantiteDisponible;
        private List<ImageProduit> lesImages;
        #endregion

        #region Constructeurs
        public Produit(int id, string nomProduit, double prix, string descriptioncourte,string description, int quantiteDisponible, List<ImageProduit> lesImages)
        {
            this.id = id;
            this.nomProduit = nomProduit;
            this.prix = prix;
            this.descriptioncourte = descriptioncourte;
            this.description = description;
            this.quantiteDisponible = quantiteDisponible;
            this.lesImages = lesImages;
        }
        #endregion

        #region Getters/Setters
        [JsonProperty("id")]
        public int Id { get => id; set => id = value; }

        [JsonProperty("nomProduit")]
        public string NomProduit { get => nomProduit; set => nomProduit = value; }

        [JsonProperty("prix")]
        public double Prix { get => prix; set => prix = value; }

        [JsonProperty("descriptioncourte")]
        public string Descriptioncourte { get => descriptioncourte; set => descriptioncourte = value; }

        [JsonProperty("description")]
        public string Description { get => description; set => description = value; }

        [JsonProperty("quantiteDisponible")]
        public int QuantiteDisponible { get => quantiteDisponible; set => quantiteDisponible = value; }

        [JsonProperty("lesImages")]
        public List<ImageProduit> LesImages { get => lesImages; set => lesImages = value; }

        public string DescriptionNettoyee
        {
            get
            {

                string desc = string.IsNullOrWhiteSpace(description) ? descriptioncourte : description;
                string sansBalises = Regex.Replace(desc, "<.*?>", string.Empty);
                return sansBalises.Replace("&nbsp;", " "); // Remplace &nbsp; par un espace normal
            }
        }



        #endregion

    }
}
