using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AP4DantecMarket.Modeles
{
    public class Categorie
    {
        #region Attributs
        private int id;
        private string nom;
        private ObservableCollection<Produit> produits;
        #endregion

        #region Constructeurs
        public Categorie(int id, string nom, ObservableCollection<Produit> produits)
        {
            this.id = id;
            this.nom = nom;
            this.produits = produits;
        }
        #endregion

        #region Getters/Setters
        [JsonProperty("id")]
        public int Id { get => id; set => id = value; }

        [JsonProperty("nom")]
        public string Nom { get => nom; set => nom = value; }

        [JsonProperty("lesProduits")] // CHANGEMENT ICI
        public ObservableCollection<Produit> Produits
        {
            get => produits ??= new ObservableCollection<Produit>();
            set => produits = value;
        }
        #endregion
    }
}
