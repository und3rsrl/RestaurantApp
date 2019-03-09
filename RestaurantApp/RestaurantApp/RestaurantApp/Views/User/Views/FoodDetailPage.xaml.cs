using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views.User.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FoodDetailPage : ContentPage
	{
		public FoodDetailPage (FoodItem item)
		{
			InitializeComponent ();

            BindData(item);
        }

        private void BindData(FoodItem item)
        {
            FoodNameLabel.Text = item.Name;
            FoodImage.Source = item.ImageUrl;
            PriceLabel.Text = item.Price.ToString("c");

            StringBuilder ingredients = new StringBuilder("Ingredients: ");

            foreach (string ingredient in item.Ingredients)
            {
                ingredients.Append(ingredient);
                ingredients.Append(", ");
            }

            ingredients.Remove(ingredients.Length - 2, 2);

            IngredientsLabel.Text = ingredients.ToString();
        }
    }
}