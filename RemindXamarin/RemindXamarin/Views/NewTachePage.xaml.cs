using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RemindXamarin.Models;
using System.ComponentModel;

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTachePage : ContentPage
    {
        public string title = "Nouvelle Tâche";

        public Tache tache { get; set; }

        public DateTime myTime;

        public DateTime MinDate = DateTime.Now;
        public DateTime MaxDate = DateTime.Now.Add(new TimeSpan(1000, 0, 0, 0, 0));
        public DateTime SelectedDate = new DateTime();

        public NewTachePage()
        {
            InitializeComponent();

            tache = new Tache("name -1", "description -1", null, new DateTime(), 30, 12, 22);

            MinDate = DateTime.Now;
            MaxDate = DateTime.Now.Add(new TimeSpan(1000, 0, 0, 0, 0));
            SelectedDate = new DateTime();

            BindingContext = this;
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

            myTime = DateTime.Today + _timePicker.Time;
            if (myTime < DateTime.Now)
            {
                myTime += TimeSpan.FromDays(1);
            }
            tache.timeHour = myTime.Hour;
            tache.timeMinutes = myTime.Minute;
            
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddTache", tache);
            await Navigation.PopModalAsync();
        }
    }
}