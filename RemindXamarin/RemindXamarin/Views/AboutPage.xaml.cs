using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RemindXamarin.Models;
using RemindXamarin.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        SportTachesViewModel viewModel;

        public AboutPage()
        {
            InitializeComponent();
            Permissions();
            viewModel = new SportTachesViewModel();
            BindingContext = viewModel;

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

        async void OnSportTacheSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is SportTache tache))
                return;

            await Navigation.PushAsync(new SportTacheDetailPage(new SportTacheDetailViewModel(tache))); ;

            // Manually deselect item.
            SportTachesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.SportTaches.Count == 0)
            {
                viewModel.LoadSportTachesCommand.Execute(null);
            }

        }

        void OnDelete(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                Debug.Print(mi.ToString());
                viewModel.DeleteTache((SportTache)mi.CommandParameter);
            }
            catch (Exception x)
            {
                Console.Write(x.Message);
            }
        }

    }
}