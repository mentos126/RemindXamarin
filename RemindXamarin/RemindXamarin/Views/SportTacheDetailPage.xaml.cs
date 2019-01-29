using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RemindXamarin.Models;
using RemindXamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SportTacheDetailPage : ContentPage
    {
        SportTacheDetailViewModel viewModel;

        public SportTacheDetailPage(SportTacheDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;

            AddPinsInMap();

        }

        public String GetSteps
        {
            get
            {
                return this.viewModel.SportTache.Steps + "  pas";
            }
        }

        public String GetDistance
        {
            get
            {
                return this.viewModel.SportTache.Distance + "  km parcourus";
            }
        }

        public String GetTime
        {
            get
            {
                return this.viewModel.SportTache.Duration + "  secondes";
            }
        }

        public SportTacheDetailPage()
        {
            InitializeComponent();

            var sportTache = new SportTache()
            {
                Id = 0,
                Name = "tache 0",
                Description = "description 0",
                DateDeb = DateTime.MinValue,
                TimeHour = 14,
                TimeMinutes = 15,
                Coordinates = "error",
                Distance = 0,
                Duration = 0,
                Steps = 0,
            };

            viewModel = new SportTacheDetailViewModel(sportTache);
            BindingContext = viewModel;

            AddPinsInMap();

        }

        private void AddPinsInMap()
        {
            _steps.Text = GetSteps;
            _distance.Text = GetDistance;
            _duration.Text = GetTime;
            if (!this.viewModel.SportTache.Coordinates.Equals("error"))
            {
                String temp = this.viewModel.SportTache.Coordinates;
                List<Position> res = JsonConvert.DeserializeObject<List<Position>>(temp);

                bool ok = false;
                foreach(Position p in res)
                {
                    if (!ok)
                    {
                        MyMap.MoveToRegion(new MapSpan(p, 1, 1));
                        ok = !ok;
                    }
                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = p,
                        Label = "",
                        Address = ""
                    };
                    MyMap.Pins.Add(pin);
                }
            }
        }


    }
}