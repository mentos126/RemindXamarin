using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RemindXamarin.Models;
using System.ComponentModel;
using System.Collections;

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTachePage : ContentPage
    {
        public string title = "Nouvelle Tâche";
        public Tache tache { get; set; }
        public bool isRepet { get; set; }
        public DateTime myTime { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public DateTime SelectedDate { get; set; }
        public ArrayList Categories { get; set; }
        public ArrayList CategoriesName { get; set; }
        public ArrayList WarningBefore { get; set; }

        public NewTachePage()
        {
            InitializeComponent();

            tache = new Tache("votre nom", "votre description", null, new DateTime(), 30, 12, 22);
            MinDate = DateTime.Now;
            MaxDate = DateTime.Now.Add(new TimeSpan(1000, 0, 0, 0, 0));
            SelectedDate = new DateTime();
            isRepet = false;
            Categories = new ArrayList();
            Categories = Tasker.Instance.getListCategories();
            CategoriesName = new ArrayList();
            foreach (Category c in Categories)
            {
                CategoriesName.Add(c.name);
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

            myTime = DateTime.Today + _timePicker.Time;
            if (myTime < DateTime.Now)
            {
                myTime += TimeSpan.FromDays(1);
            }
            tache.timeHour = myTime.Hour;
            tache.timeMinutes = myTime.Minute;
            
        }

        void OnDateSelected(object sender, DateChangedEventArgs args)
        {
            tache.dateDeb = SelectedDate;
        }

        void OnSwitchToggled(object sender, ToggledEventArgs args)
        {
            this.isRepet = !this.isRepet;
            datePicker.IsVisible = !datePicker.IsVisible;
            datePickerLabel.IsVisible = datePicker.IsVisible;
            controlGrid.IsVisible = !controlGrid.IsVisible;

        }

        void OnSwitchDay()
        {
            this.tache.repete = new Boolean[] {
                    Monday.IsToggled,
                    Tuesday.IsToggled,
                    Wednesday.IsToggled,
                    Thursday.IsToggled,
                    Friday.IsToggled,
                    Saturday.IsToggled,
                    Sunday.IsToggled,
                    };
        }

        void OnCategoryChanged()
        {
            if (pickerCategory.SelectedIndex != -1)
            {
                tache.category = (Category) Categories[pickerCategory.SelectedIndex];
                if(tache.category.name.Equals(Tasker.CATEGORY_SPORT_TAG) || tache.category.name.Equals(Tasker.CATEGORY_NONE_TAG))
                {
                    edit.IsVisible = false;
                }
                else
                {
                    edit.IsVisible = true;
                }

            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            //TODO : verifier si tache correcte

            MessagingCenter.Send(this, "AddTache", tache);
            await Navigation.PopModalAsync();
        }

        async void OnAddCategory(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewCategory()));
        }

        async void OnEditCategory(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new EditCategory()));
        }

    }
}