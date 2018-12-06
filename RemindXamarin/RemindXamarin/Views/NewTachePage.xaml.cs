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
        public Tache tache { get; set; }
        public string nom { get; set; }
        public string description { get; set; }
        public DateTime myTime;

        public DateTime MinDate = DateTime.Now;
        public DateTime MaxDate = DateTime.Now.Add(new TimeSpan(1000, 0, 0, 0, 0));
        public DateTime SelectedDate = new DateTime();

        public NewTachePage()
        {
            InitializeComponent();

            tache = new Tache("name -1", "description -1", null, new DateTime(), 30, 12, 22);
            nom = tache.getName();
            description = tache.getDescription();

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
            
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddTache", tache);
            await Navigation.PopModalAsync();
        }
    }
}