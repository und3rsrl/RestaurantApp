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
            PriceLabel.Text = item.Price.ToString() + "lei";

            List<string> ingredients = item.Ingredients.Split(',').ToList();

            StringBuilder ingredientsStringBuilder = new StringBuilder();

            foreach (string ingredient in ingredients)
            {
                ingredientsStringBuilder.AppendLine(ingredient);
            }

            IngredientsLabel.Text = ingredientsStringBuilder.ToString();
        }
    }
}