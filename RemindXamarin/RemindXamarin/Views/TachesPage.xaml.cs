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

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TachesPage : ContentPage
    {
        TachesViewModel viewModel;

        public TachesPage()
        {
            InitializeComponent();
            viewModel = new TachesViewModel();

            BindingContext = viewModel;
        }

        void OnTakePhoto()
        {
        }

        void OnSelectPlace()
        {
        }

        async void OnTacheSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var tache = args.SelectedItem as Tache;
            if (tache == null)
                return;

            await Navigation.PushModalAsync(new NavigationPage (new EditTachePage(new EditTacheViewModel(tache))));

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
    }
}