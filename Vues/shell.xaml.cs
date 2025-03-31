using AP4DantecMarket.Vues;
using DantecMarcket.Vues;

namespace AP4DantecMarket
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Enregistrer la route pour VueAccueil
            Routing.RegisterRoute("home", typeof(VueAccueil));
            Routing.RegisterRoute("rechercher", typeof(VueProduit));
            Routing.RegisterRoute("categorie", typeof(VueCategorieParent));
            Routing.RegisterRoute("panier", typeof(VuePanier));
            Routing.RegisterRoute("commandes", typeof(VueCommandes));
            Routing.RegisterRoute("favoris", typeof(VueFavoris));
        }

    }
}
