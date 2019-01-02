﻿using System;
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

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TachesPage : ContentPage
    {
        TachesViewModel viewModel;
        bool sortDirection = true;

        public TachesPage()
        {
            InitializeComponent();
            viewModel = new TachesViewModel();

            BindingContext = viewModel;

            sortDirection = true;
            this.sortList();
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
                PhotoSize = PhotoSize.Medium,
                Directory = "Images",
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
                myTache.photo = file.Path;
                Debug.Print(myTache.name+"++++++++++++++++++++++++");
                Debug.Print(myTache.photo + "++++++++++++++++++++++++");
                viewModel.UpdateTache(myTache);
                file.Dispose();
            }



            /*using (var memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                var myfile = memoryStream.ToArray();
                mysfile = myfile;
            }

            PhotoIDImage.Source = ImageSource.FromFile(file.Path);*/


            /*var mi = ((MenuItem)sender);
            ((Tache)mi.CommandParameter).photo = file.Path; */


        }

        void OnSelectPlace(object sender, EventArgs e)
        {
        }

        async void OnTacheSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var tache = args.SelectedItem as Tache;
            if (tache == null)
                return;

            await Navigation.PushModalAsync(new NavigationPage (new EditTachePage(tache)));

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

        public void filterList()
        {
            String keywords = SearchBar.Text.ToLower();
            IEnumerable<Tache> result = null;
            if (keywords.Equals(""))
            {
                result = viewModel.Taches;
            }
            else
            { 
                result = viewModel.Taches.Where(tache => tache.name.ToLower().Contains(keywords));
            }
            TachesListView.ItemsSource = result;
        }

        public void sortList()
        {
            IEnumerable<Tache> result = null;
            if (sortDirection)
            {
                result = viewModel.Taches.OrderBy( tache => tache.name);
            }
            else
            {
                result = viewModel.Taches.OrderByDescending( tache => tache.name);
            }
            TachesListView.ItemsSource = result;
            sortDirection = !sortDirection;
        }

        void OnDelete (object sender, EventArgs e)
        {
            try
            { 
                var mi = ((MenuItem)sender);
                viewModel.DeleteTache(((Tache) mi.CommandParameter));
            }
            catch (Exception x)
            {
                Console.Write(x.Message);
            }
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            this.filterList();
        }

        private void TapOrder_Tapped(object sender, EventArgs e)
        {
            this.sortList();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.sortList();
        }

        private void SwitchNotification_Toggled(object sender, ToggledEventArgs e)
        {
            var mi = ((MenuItem)sender);
            ((Tache)mi.CommandParameter).isActivatedNotification = !((Tache)mi.CommandParameter).isActivatedNotification;
        }
    }
}