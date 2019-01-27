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

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTachePage : ContentPage
    {
        public string title = "Nouvelle Tâche";
        public Tache Tache { get; set; }
        public bool IsRepet { get; set; }
        public DateTime MyTime { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public DateTime SelectedDate { get; set; }
        public List<CategoryEnum> Categories { get; set; }
        public List<String> CategoriesName { get; set; }
        public ArrayList WarningBefore { get; set; }


        public NewTachePage()
        {
            InitializeComponent();

            DateTime temp = DateTime.Now.AddMinutes(5.0);

            Tache = new Tache() {
                Id = 0,
                Name = "votre nom",
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
            MinDate = DateTime.Now;
            MaxDate = DateTime.Now.Add(new TimeSpan(1000, 0, 0, 0, 0));
            SelectedDate = new DateTime();
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

        void OnTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Time")
            {
                SetTriggerTime();
            }
        }

        void SetTriggerTime()
        {

            MyTime = DateTime.Today + _timePicker.Time;
            if (MyTime < DateTime.Now)
            {
                MyTime += TimeSpan.FromDays(1);
            }
            Tache.TimeHour = MyTime.Hour;
            Tache.TimeMinutes = MyTime.Minute;
            
        }

        void OnDateSelected(object sender, DateChangedEventArgs args)
        {
            Tache.DateDeb = SelectedDate;
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
                Photo = "",
                Description = Tache.Description,
                Category = Categories[pickerCategory.SelectedIndex],
                DateDeb = null,
                WarningBefore = pickerWarningBefore.SelectedIndex * 5,
                TimeHour = Tache.TimeHour,
                TimeMinutes = Tache.TimeMinutes,
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
                            newTache.DateDeb = Tache.DateDeb;
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
            await Navigation.PopModalAsync();
        }

    }
}