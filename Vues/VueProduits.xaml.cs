using AP4DantecMarket.Modeles;
using System.Collections.ObjectModel;

namespace DantecMarcket.Vues;

public partial class VueProduits : ContentPage
{
    public ObservableCollection<Produit> Produits { get; set; }

    public VueProduits(Categorie categorie)
    {
        InitializeComponent();
        Produits = categorie.Produits;
        BindingContext = this;
    }
    private async void OnProduitTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Produit selectedProduit)
        {
            await Navigation.PushAsync(new VueProduitDetail(selectedProduit));
        }
    }

}