using Plugin.Geolocator;
using RemindXamarin.Models;
using RemindXamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Threading;
using RemindXamarin.Services;

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SportActivityPage : ContentPage
    {
       
        SportActivityViewModel ViewModel { get; set; }
        Tache MyTache { get; set; }
        Timer MyTimer { get; set; }

        public int Steps { get; set; }
        public int Distance { get; set; }
        public int Duration { get; set; }
        public List<Position> Coordinates { get; set; }
        public bool IsReady { get; set; }

        public String GetSteps
        {
            get
            {
                return Steps + "  pas";
            }
        }

        public String GetDistance
        {
            get
            {
                return Distance + "  km parcourus";
            }
        }

        public String GetTime
        {
            get
            {
                return Duration + "  secondes";
            }
        }

        public bool IsNotReady
        {
            get
            {
                return !IsReady;
            }
        }

        public SportActivityPage(Tache Tache)
        {
            InitializeComponent();
            ViewModel = new SportActivityViewModel(Tache);
            MyTache = Tache;
            MyTimer = null; ;

            Steps = 0;
            Distance = 0;
            Duration = 0;
            Coordinates = new List<Position>();

            IsReady = false;

            BindingContext = this;

            _duration.Text = GetTime;
            _distance.Text = GetDistance;
            _steps.Text = GetSteps;
        }

        public void OnPressStart()
        {
            IsReady = true;
            _start.IsVisible = false;
            _save.IsVisible = true;

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(1);

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                LaunchRegister();
                return IsReady;
            });

        }

        public void LaunchRegister()
        {
            // time
            Duration = Duration  + 1;
            _duration.Text = GetTime;

            //geo  
            GeoLocation();

            //distance
            CaculateDistance();

            //step

            // TODO find plugin...

        }

        async void OnPressEnd()
        {
            IsReady = false;
            _start.IsVisible = true;
            _save.IsVisible = false;
            String res = JsonConvert.SerializeObject(Coordinates);

            SportTache s = new SportTache()
            {
                Coordinates = res,
                Distance = this.Distance,
                Steps = this.Steps,
                Duration = this.Duration,
                
                Id = 0,
                Name = MyTache.Name,
                Description = MyTache.Description,
                DateDeb = MyTache.GetNextDate(),
                TimeHour = MyTache.TimeHour,
                TimeMinutes = MyTache.TimeMinutes,

            };
            MessagingCenter.Send(this, "AddSportTache", s);
            await Navigation.PopModalAsync();

        }

        async void GeoLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5), null, true);
                if (position == null)
                {
                    return;
                }
                else
                {
                    Position pos = new Position(position.Latitude, position.Longitude);
                    Coordinates.Add(pos);

                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = pos,
                        Label = "",
                        Address = ""
                    };
                    MyMap.Pins.Add(pin);
                    MyMap.MoveToRegion(new MapSpan(pos, 0, 0));

                    return;
                }
            }
            catch (Exception) { }
        }

        async public void Exit_Clicked()
        {
            await Navigation.PopModalAsync();
        }

         public void CaculateDistance()
       {
           double res = 0;
           for (int i = 0; i < Coordinates.Count; i++)
           {
               if (i != 0)
               {
                   res += DistanceBetween((Position)Coordinates[i - 1], (Position)Coordinates[i]);
               }
           }
            Distance = (int)res + 0;
       }

         private double DistanceBetween(Position C1, Position C2)
         {
            double LatC1 = C1.Latitude;
            double LngC1 = C1.Longitude;
            double LatC2 = C2.Latitude;
            double LngC2 = C2.Longitude;

             int R = 6371; // Radius of the earth
             double latDistance = this.ConvertDegreesToRadians(LatC2 - LatC1);
             double lonDistance = this.ConvertDegreesToRadians(LngC2 - LngC1);
             double a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2)
                     + Math.Cos(this.ConvertDegreesToRadians(LatC1)) * Math.Cos(this.ConvertDegreesToRadians(LatC2))
                     * Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);
             double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
             double distance = R * c * 1000; // convert to meters

             //double height = c1.h - c2.h;
             double height = 0;

             distance = Math.Pow(distance, 2) + Math.Pow(height, 2);

             return Math.Sqrt(distance);
         }

         public double ConvertDegreesToRadians(double degrees)
         {
             double radians = (Math.PI / 180) * degrees;
             return (radians);
         }

    }
}