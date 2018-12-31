﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RemindXamarin.Models;
using RemindXamarin.ViewModels;

using System.Collections;
using System.ComponentModel;
using RemindXamarin.Services;

namespace RemindXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditTachePage : ContentPage
	{

        EditTacheViewModel viewModel;

        public string title = "Modifier Tâche";
        public Tache tache { get; set; }
        public bool isRepet { get; set; }
        public DateTime myTime { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public DateTime SelectedDate { get; set; }
        public ArrayList Categories { get; set; }
        public ArrayList CategoriesName { get; set; }
        public ArrayList WarningBefore { get; set; }

        public EditTachePage(EditTacheViewModel viewModel)
        {
            InitializeComponent();

            this.tache = new Tache("Erreussssssssr", "Erreur", new Category(Tasker.CATEGORY_SPORT_TAG, "ic_directions_run_black_36dp.png", Color.FromHex("FF6A00")), new DateTime(), 0, 0, 0);
            this.tache = viewModel.Tache;

            MinDate = DateTime.Now;
            MaxDate = DateTime.Now.Add(new TimeSpan(1000, 0, 0, 0, 0));
            SelectedDate = this.tache.getNextDate();
            isRepet = this.tache.dateDeb == null;
            Categories = new ArrayList();

            Categories = Tasker.Instance.getListCategories();
            //viewModel.LoadCategoriesCommand.Execute(null);

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

            this.viewModel = viewModel;
            BindingContext = this;

            pickerWarningBefore.SelectedIndex = tache.warningBefore / 5;
            int index = 0;
            foreach (Category c in Categories)
            {
                if(tache.category.ID == c.ID)
                {
                    break;
                }
                index++;
            }
            pickerCategory.SelectedIndex = index;

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
                tache.category = (Category)Categories[pickerCategory.SelectedIndex];
                if (tache.category.name.Equals(Tasker.CATEGORY_SPORT_TAG) || tache.category.name.Equals(Tasker.CATEGORY_NONE_TAG))
                {
                    edit.IsVisible = false;
                }
                else
                {
                    edit.IsVisible = true;
                }

            }
        }

        void Save_Clicked(object sender, EventArgs e)
        {
            Tache newTache = new Tache(tache.name, tache.description, (Category)Categories[pickerCategory.SelectedIndex], null,
                pickerWarningBefore.SelectedIndex * 5, tache.timeHour, tache.timeMinutes);
            if (tache.name != "")
            {
                if (tache.description != "")
                {
                    if (isRepet)
                    {
                        bool temp = false;
                        foreach (bool b in tache.repete)
                        {
                            temp = temp || b;
                        }
                        if (temp)
                        {
                            newTache.repete = tache.repete;
                            Save(newTache);
                        }
                        else
                        {
                            DependencyService.Get<IMessageToast>().Show("ajouter une répétition !!!");
                        }
                    }
                    else
                    {
                        if (DateTime.Compare(tache.dateDeb.Value, DateTime.Now) < 0)
                        {
                            newTache.dateDeb = tache.dateDeb;
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
            newTache.ID = tache.ID;
            MessagingCenter.Send(this, "UpdateTache", newTache);
            await Navigation.PopModalAsync(false);
        }

        async void OnAddCategory(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewCategory()));
        }

        async void OnEditCategory(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new EditCategory(tache.category)));
        }
    }
}