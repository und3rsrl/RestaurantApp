﻿using RestaurantApp.Models;
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
        private int _itemAmount;

		public FoodDetailPage (FoodItem item)
		{
			InitializeComponent ();
            _itemAmount = 0;
            BindData(item);
        }

        protected override void OnAppearing()
        {
            Item_Amount_Entry.Text = "0";
            base.OnAppearing();
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

        private void ImageButton_IncreaseAmount(object sender, EventArgs e)
        {
            _itemAmount++;
            Item_Amount_Entry.Text = _itemAmount.ToString();
        }

        private void ImageButton_DecreaseAmount(object sender, EventArgs e)
        {
            if (_itemAmount > 0)
            {
                _itemAmount--;
                Item_Amount_Entry.Text = _itemAmount.ToString();
            }
        }
    }
}