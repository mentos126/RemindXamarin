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
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsTakePhotoSupported && !CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Message","prise de photo non supporter","OK");
                return;
            }
            else
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Images",
                    Name = DateTime.Now+"_pic.jpg"
                });

                if(file == null)
                {
                    return;
                }

                var mi = ((MenuItem)sender);
                ((Tache)mi.CommandParameter).photo = file.Path;
                
            }
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
    }
}