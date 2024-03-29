﻿using MvvmHelpers;
using Plugin.Toasts;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class EditCategorieViewModel : BaseViewModel
    {
        private CategoriesApiService _categoriesApiService = new CategoriesApiService();

        public int Id { get; set; }

        public string Name { get; set; }

        public event EventHandler RefreshCategories;

        public event EventHandler SuccesfulEdit;

        public ICommand EditCategorieCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        var notificator = DependencyService.Get<IToastNotificator>();
                        var options = new NotificationOptions()
                        {
                            Title = "Categories",
                            ClearFromHistory = true,
                            Description = "Please provide a name for category!",
                            IsClickable = false // Set to true if you want the result Clicked to come back (if the user clicks it)
                        };

                        await notificator.Notify(options);
                    }
                    else
                    {
                        var response = await _categoriesApiService.EditCategorie(Id, Name);

                        if (!string.IsNullOrEmpty(response))
                            await Application.Current.MainPage.DisplayAlert("Edit Categorie", response, "Ok");
                        else
                        {
                            RefreshCategories?.Invoke(this, EventArgs.Empty);
                            SuccesfulEdit?.Invoke(this, EventArgs.Empty);
                        }
                    }
                });
            }
        }
    }
}
