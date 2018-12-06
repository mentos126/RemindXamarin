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

            BindingContext = viewModel = new TachesViewModel();
        }

        async void OnTacheSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var tache = args.SelectedItem as Tache;
            if (tache == null)
                return;

            await Navigation.PushAsync(new TacheDetailPage(new TacheDetailViewModel(tache)));

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
    }
}