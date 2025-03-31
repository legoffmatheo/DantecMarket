using Newtonsoft.Json;
using System.Collections.Generic;

namespace AP4DantecMarket.Modeles
{
    public class CategorieParent
    {
        #region Attributs
        private int id;
        private string nom;
        private List<Categorie> lesCategories;
        #endregion

        #region Constructeurs
        public CategorieParent(int id, string nom, List<Categorie> lesCategories)
        {
            this.id = id;
            this.nom = nom;
            this.lesCategories = lesCategories;
        }
        #endregion

        #region Getters/Setters
        [JsonProperty("id")]
        public int Id { get => id; set => id = value; }

        [JsonProperty("nom")]
        public string Nom { get => nom; set => nom = value; }

        [JsonProperty("lesCategories")]
        public List<Categorie> LesCategories { get => lesCategories; set => lesCategories = value; }
        public bool IsExpanded { get; internal set; }
        #endregion
    }
}
