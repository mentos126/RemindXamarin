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
                Lat = 999.999,
                Lng = 999.999,
                Photo = "",
                Description = "description 0",
                Category = CategoryEnum.Sport,
                DateDeb = new DateTime(),
                WarningBefore = 10,
                TimeHour = 14,
                TimeMinutes = 15,
                IsActivatedNotification = true,
                Monday = false,
                Tuesday = false,
                Wednesday = false,
                Thursday = false,
                Friday = false,
                Saturday = false,
                Sunday = false,
            };

            viewModel = new TacheDetailViewModel(tache);
            BindingContext = viewModel;
        }
    }
}