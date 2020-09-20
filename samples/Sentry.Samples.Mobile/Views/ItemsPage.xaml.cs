﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Sentry.Samples.Mobile.Models;
using Sentry.Samples.Mobile.Views;
using Sentry.Samples.Mobile.ViewModels;

namespace Sentry.Samples.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Item)layout.BindingContext;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
            SentrySdk.AddBreadcrumb(
                "Item selected",
                "user.interaction",
                "ui");
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
            SentrySdk.AddBreadcrumb(
                "Item added to cart",
                "user.interaction",
                "event");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
            {
                viewModel.IsBusy = true;
            }

            SentrySdk.AddBreadcrumb(
                "OnAppearing",
                "app.lifecycle",
                "event",
                new Dictionary<string, string>
                {
                {"IsBusy", viewModel.IsBusy.ToString()}
            });
        }
    }
}