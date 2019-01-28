using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RemindXamarin.Models;
using RemindXamarin.ViewModels;
using RemindXamarin.Services;

using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using Plugin.Media;
using Plugin.Geolocator.Abstractions;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;
using Plugin.LocalNotifications;

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTachePage : ContentPage
    {
        public string title { get; set; }
        public Tache Tache { get; set; }
        public bool IsRepet { get; set; }
        public TimeSpan MyTime { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MyDate { get; set; }
        public List<CategoryEnum> Categories { get; set; }
        public List<String> CategoriesName { get; set; }
        public ArrayList WarningBefore { get; set; }
        public bool IsInGeo { get; set; }

        public NewTachePage()
        {
            InitializeComponent();

            IsInGeo = false;
            title = "Nouvelle Tâche";
            DateTime temp = DateTime.Now.AddMinutes(5.0);

            MyTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(5));

            Tache = new Tache() {
                Id = 0,
                Name = "votre nom",
                Lat = 999.999,
                Lng = 999.999,
                Photo = "",
                Description = "votre description",
                Category = CategoryEnum.Sport,
                DateDeb = DateTime.Now,
                WarningBefore = 35,
                TimeHour = temp.Hour,
                TimeMinutes = temp.Minute,
                IsActivatedNotification = true,
                Monday = false,
                Tuesday = false,
                Wednesday = false,
                Thursday = false,
                Friday = false,
                Saturday = false,
                Sunday = false,
            };
            MyDate = DateTime.Now;
            MinDate = DateTime.Now;
            IsRepet = false;

            Categories = new List<CategoryEnum>()
            {
                CategoryEnum.Sport,
                CategoryEnum.RDV,
                CategoryEnum.Loisirs,
                CategoryEnum.Anniversaire,
                CategoryEnum.Vacances,
                CategoryEnum.Congés,
                CategoryEnum.Santé,
                CategoryEnum.Courses,
                CategoryEnum.Autres,
            };

            CategoriesName = new List<String>();
            foreach (CategoryEnum c in Categories)
            {
                CategoriesName.Add(c.ToString());
            }
            WarningBefore = new ArrayList();
            for (int i = 0; i <= 90; i += 5)
            {
                WarningBefore.Add(i);
            }

            BindingContext = this;

            pickerWarningBefore.SelectedIndex = 7;
            pickerCategory.SelectedIndex = 0;
        }

        void OnSwitchToggled(object sender, ToggledEventArgs args)
        {
            this.IsRepet = !this.IsRepet;
            datePicker.IsVisible = !datePicker.IsVisible;
            datePickerLabel.IsVisible = datePicker.IsVisible;
            controlGrid.IsVisible = !controlGrid.IsVisible;

        }

        void OnSwitchDay()
        {
            this.Tache.Monday = Monday.IsToggled;
            this.Tache.Tuesday = Tuesday.IsToggled;
            this.Tache.Wednesday = Wednesday.IsToggled;
            this.Tache.Thursday = Thursday.IsToggled;
            this.Tache.Friday = Friday.IsToggled;
            this.Tache.Saturday = Saturday.IsToggled;
            this.Tache.Sunday = Sunday.IsToggled;
        }

        void OnCategoryChanged()
        {
            if (pickerCategory.SelectedIndex != -1)
            {
                Tache.Category = Categories[pickerCategory.SelectedIndex];
            }
        }

        void Save_Clicked(object sender, EventArgs e)
        {
            Tache newTache = new Tache {
                Id = 0,
                Name = Tache.Name,
                Lat = Tache.Lat,
                Lng = Tache.Lng,
                Photo = Tache.Photo,
                Description = Tache.Description,
                Category = Categories[pickerCategory.SelectedIndex],
                DateDeb = null,
                WarningBefore = pickerWarningBefore.SelectedIndex * 5,
                TimeHour = MyTime.Hours,
                TimeMinutes = MyTime.Minutes,
                IsActivatedNotification = true,
                Monday = false,
                Tuesday = false,
                Wednesday = false,
                Thursday = false,
                Friday = false,
                Saturday = false,
                Sunday = false,
             };
            if (Tache.Name != "")
            {
                if (Tache.Description != "")
                {
                    if (IsRepet)
                    {
                        if (this.Tache.Monday || this.Tache.Tuesday || this.Tache.Wednesday || this.Tache.Thursday || this.Tache.Friday || this.Tache.Saturday || this.Tache.Sunday)
                        {
                            newTache.Monday = this.Tache.Monday ;
                            newTache.Tuesday = this.Tache.Tuesday;
                            newTache.Wednesday = this.Tache.Wednesday;
                            newTache.Thursday = this.Tache.Thursday;
                            newTache.Friday = this.Tache.Friday;
                            newTache.Saturday = this.Tache.Saturday;
                            newTache.Sunday = this.Tache.Sunday;
                            Save(newTache);
                        }
                        else
                        {
                            DependencyService.Get<IMessageToast>().Show("ajouter une répétition !!!");
                        }
                    }
                    else
                    {
                        if(DateTime.Compare(Tache.DateDeb.Value , DateTime.Now ) < 0)
                        {
                            newTache.DateDeb = MyDate;
                            Save(newTache);
                        }
                        else
                        {
                            DependencyService.Get<IMessageToast>().Show("ajouter une date après maintement !!!");
                        }
                    }
                }
                else
                {
                    DependencyService.Get<IMessageToast>().Show("ajouter une description !!!");
                }
            }
            else
            {
                DependencyService.Get<IMessageToast>().Show("ajouter un nom !!!");
            }
        }

        async void Save(Tache newTache)
        {
            MessagingCenter.Send(this, "AddTache", newTache);
            CrossLocalNotifications.Current.Show(newTache.Name, newTache.CatName + ": " + newTache.Description, newTache.Id, newTache.GetNextDate());
            await Navigation.PopModalAsync();
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
                Name = DateTime.Now + "_pic.jpg"
            }))
            {
                if (file == null)
                    return;
                if (string.IsNullOrWhiteSpace(file?.Path))
                {
                    return;
                }
                Tache.Photo = file.Path;
                file.Dispose();
                MyPhoto.Source = ImageSource.FromFile(Tache.Photo);
            }
        }

        async void OnSelectPlace(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                bt.IsEnabled = false;
                this.IsInGeo = true;

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5), null, true);

                if (position == null)
                {
                    bt.Text = "Impossible de vous géolocaliser. Voulez vous vous géolocaliser???";
                    this.IsInGeo = false;
                    bt.IsEnabled = true;
                    return;
                }
                else
                {
                    bt.Text = "Trouver!!! Voulez vous vous géolocaliser?";
                    Tache.Lat= position.Latitude;
                    Tache.Lng= position.Longitude;

                    var pos = new Position(position.Latitude, position.Longitude); 
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


                    bt.IsEnabled = true;
                    this.IsInGeo = false;
                    return;
                }

            }
            catch (Exception ex)
            {
                Button bt = (Button)sender;
                Debug.Write(ex.ToString());
                await DisplayAlert("Oups", "Une erreur est survenu.", "OK");
                bt.Text = "Impossible de vous géolocaliser. Voulez vous vous géolocaliser???";
                bt.IsEnabled = true;
                this.IsInGeo = false;
            }
        }

    }
}