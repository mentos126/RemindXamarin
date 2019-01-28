using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RemindXamarin.Models;
using RemindXamarin.ViewModels;
using Plugin.LocalNotifications;
using Xamarin.Forms.Maps;

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

            var pos = new Position(viewModel.Tache.Lat, viewModel.Tache.Lng);
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = pos,
                Label = "",
                Address = ""
            };
            MyMap.Pins.Add(pin);
            MyMap.WidthRequest = 320;
            MyMap.HeightRequest = 200;
            MyMap.MoveToRegion(new MapSpan(pos, 1, 1));
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

            var pos = new Position(viewModel.Tache.Lat, viewModel.Tache.Lng);
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = pos,
                Label = "",
                Address = ""
            };
            MyMap.Pins.Add(pin);
            MyMap.WidthRequest = 320;
            MyMap.HeightRequest = 200;
            MyMap.MoveToRegion(new MapSpan(pos, 1, 1));

        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            viewModel.UpdateTache(viewModel.Tache);
            if (viewModel.Tache.IsActivatedNotification)
            {
                CrossLocalNotifications.Current.Show(viewModel.Tache.Name, viewModel.Tache.CatName + ": " + viewModel.Tache.Description, viewModel.Tache.Id, viewModel.Tache.GetNextDate());
            }
            else
            {
                CrossLocalNotifications.Current.Cancel(viewModel.Tache.Id);
            }
        }

        async private void OnLaunchSport()
        {
            await Navigation.PushModalAsync(new NavigationPage(new SportActivityPage(this.viewModel.Tache)));
        }
    }
}