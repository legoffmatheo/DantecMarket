using AP4DantecMarket.Modeles;
using System.Collections.ObjectModel;

namespace DantecMarcket.Vues;

public partial class VueCategorie : ContentPage
{
    public ObservableCollection<Categorie> SousCategories { get; set; }

    public VueCategorie(CategorieParent categorieParent)
    {
        InitializeComponent();
        SousCategories = new ObservableCollection<Categorie>(categorieParent.LesCategories);
        BindingContext = this;
    }

    private async void OnSubCategoryTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Categorie selectedSubCategory)
        {
            await Task.Delay(100);
            await Navigation.PushAsync(new VueProduits(selectedSubCategory));
        }
    }



}