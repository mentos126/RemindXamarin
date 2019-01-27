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

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TachesPage : ContentPage
    {
        TachesViewModel viewModel;

        bool SortDirection { get; set; }

        public TachesPage()
        {
            InitializeComponent();
            Permissions();
            viewModel = new TachesViewModel();
            BindingContext = viewModel;
            SortDirection = true;

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
        
        public async void OnTakePhoto(object sender, EventArgs e)
        {
            var initialize = await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.IsSupported || !initialize)
            {
                DependencyService.Get<IMessageToast>().Show(":( No camera available.");
                return;
            }

            using (var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Name = DateTime.Now + "_pic.jpg"
            }))
            {
                if (file == null)
                    return;
                if (string.IsNullOrWhiteSpace(file?.Path))
                {
                    return;
                }

                var mi = ((Button)sender);
                Tache myTache = ((Tache)mi.CommandParameter);
                myTache.Photo = file.Path;
                Debug.Print(myTache.Name+"++++++++++++++++++++++++");
                Debug.Print(myTache.Photo + "++++++++++++++++++++++++");


                FilterList("");
                file.Dispose();
            }

        }

        void OnSelectPlace(object sender, EventArgs e)
        {
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
                viewModel.LoadTachesCommand.Execute(null);

        }

        public void FilterList(String s)
        {
                viewModel.Search(s);
        }

        public void SortList()
        {
          /*  IEnumerable<Tache> result = null;
            if (sortDirection)
            {
                result = viewModel.Taches.OrderBy( tache => tache.name);
            }
            else
            {
                result = viewModel.Taches.OrderByDescending( tache => tache.name);
            }
            viewModel.Taches =  new ObservableCollection<Tache>(result.Cast<Tache>().ToList());
            TachesListView.ItemsSource = viewModel.Taches;
            sortDirection = !sortDirection;
            */
        }

        void OnDelete (object sender, EventArgs e)
        {
            try
            { 
                var mi = ((MenuItem)sender);
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

        private void TapOrder_Tapped(object sender, EventArgs e)
        {
            this.SortList();
        }

        private void SwitchNotification_Toggled(object sender, ToggledEventArgs e)
        {
            /*var mi = ((MenuItem)sender);
            ((Tache)mi.CommandParameter).IsActivatedNotification = !((Tache)mi.CommandParameter).IsActivatedNotification;*/
        }
    }
}