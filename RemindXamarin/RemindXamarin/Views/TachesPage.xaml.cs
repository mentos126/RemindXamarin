using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RemindXamarin.Models;
using RemindXamarin.Views;
using RemindXamarin.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Diagnostics;
using RemindXamarin.Services;
using System.Collections;
using System.Collections.ObjectModel;
using Xamarin.Forms.Maps;
using Plugin.LocalNotifications;

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TachesPage : ContentPage
    {
        TachesViewModel viewModel;

        bool SortDirection { get; set; }
        String Recherche { get; set; }

        public TachesPage()
        {
            InitializeComponent();
            Permissions();
            viewModel = new TachesViewModel();
            BindingContext = viewModel;
            SortDirection = true;
            Recherche = "";

        }

        public async void Permissions()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    //Permission granted, do what you want do.
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        async void OnTacheSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Tache tache))
                return;

            await Navigation.PushAsync(new TacheDetailPage (new TacheDetailViewModel(tache)));

            // Manually deselect item.
            TachesListView.SelectedItem = null;
        }

        async void AddTache_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewTachePage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Taches.Count == 0)
            {
                viewModel.LoadTachesCommand.Execute(null);
            }

        }

        public void FilterList(String s)
        {
            this.Recherche = s;
            viewModel.Search(s);
        }

        void OnDelete (object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                Debug.Print(mi.ToString());
                viewModel.DeleteTache((Tache) mi.CommandParameter);
            }
            catch (Exception x)
            {
                Console.Write(x.Message);
            }
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            this.FilterList(SearchBar.Text.ToLower());
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.FilterList(SearchBar.Text.ToLower());
        }


    }
}