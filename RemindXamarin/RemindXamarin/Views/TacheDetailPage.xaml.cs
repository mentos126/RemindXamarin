using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RemindXamarin.Models;
using RemindXamarin.ViewModels;

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TacheDetailPage : ContentPage
    {
        TacheDetailViewModel viewModel;

        public TacheDetailPage(TacheDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public TacheDetailPage()
        {
            InitializeComponent();

            var tache = new Tache("tache 0", "description 0", new Category(Tasker.CATEGORY_SPORT_TAG, "ic_directions_run_black_36dp.png", 3), new DateTime(), 10, 14, 15);

            viewModel = new TacheDetailViewModel(tache);
            BindingContext = viewModel;
        }
    }
}