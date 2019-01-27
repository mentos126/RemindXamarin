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

            var tache = new Tache() {
                Id = 0,
                Name = "tache 0",
                /*Photo = "",
                Description = "description 0",
                Category = CategoryEnum.Sport,
                DateDeb = new DateTime(),
                WarningBefore = 10,
                TimeHour = 14,
                TimeMinutes = 15,
                IsActivatedNotification = true,
                Repete = new bool[] { false, false, false, false, false, false, false, }*/
            };

            viewModel = new TacheDetailViewModel(tache);
            BindingContext = viewModel;
        }
    }
}