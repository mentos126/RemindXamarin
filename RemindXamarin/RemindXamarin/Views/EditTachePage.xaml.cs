using System;
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
using System.Diagnostics;

namespace RemindXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditTachePage : ContentPage
	{

        public EditTacheViewModel viewModel { get; set; }

        public string title { get; set; }
        public Tache tache { get; set; }
        public bool isRepet { get; set; }
        public DateTime myTime { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public DateTime SelectedDate { get; set; }
        //public ArrayList Categories { get; set; }
        //public ArrayList CategoriesName { get; set; }
        public ArrayList WarningBefore { get; set; }

        public EditTachePage(Tache tache)
        {
            InitializeComponent();
            viewModel = new EditTacheViewModel(tache);
            viewModel.LoadCategoriesCommand.Execute(null);
            
            title = "Modifier Tâche";
            this.tache = new Tache("Erreussssssssr", "Erreur", new Category(Tasker.CATEGORY_SPORT_TAG, "ic_directions_run_black_36dp.png", 
                Color.FromHex("FF6A00")), new DateTime(), 0, 0, 0);
            this.tache = viewModel.Tache;

            MinDate = DateTime.Now;
            MaxDate = DateTime.Now.Add(new TimeSpan(1000, 0, 0, 0, 0));
            SelectedDate = this.tache.getNextDate();
            isRepet = this.tache.dateDeb == null;

            /*viewModel.Categories = new ArrayList();
            viewModel.Categories = Tasker.Instance.getListCategories();*/

            WarningBefore = new ArrayList();
            for (int i = 0; i <= 90; i += 5)
            {
                WarningBefore.Add(i);
            }

            BindingContext = this;

            pickerWarningBefore.SelectedIndex = tache.warningBefore / 5;

            Debug.Print("Tasker.Instance.getListCategories().ToString()");
            Debug.Print(Tasker.Instance.getListCategories().Count + "");


            int index = 0;
            pickerCategory.SelectedIndex = index;
            foreach (Category c in viewModel.Categories)
            {
                if(tache.category.ID == c.ID)
                {
                    pickerCategory.SelectedIndex = index;
                    break;
                }
                index++;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //if (viewModel.Categories.Count == 0)
            viewModel.LoadCategoriesCommand.Execute(null);
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
                tache.category = (Category)viewModel.Categories[pickerCategory.SelectedIndex];
                if (tache.category.name.Equals(Tasker.CATEGORY_SPORT_TAG) || 
                    tache.category.name.Equals(Tasker.CATEGORY_NONE_TAG) || 
                    tache.category.name.Equals(""))
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
            Tache newTache = new Tache(tache.name, tache.description, (Category)viewModel.Categories[pickerCategory.SelectedIndex], null,
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
            tache = newTache;
            Tasker.Instance.removeTask(newTache);
            Tasker.Instance.addTask(newTache);
            Debug.Print(Tasker.Instance.getTaskByID(newTache.ID).name+"8888888888888888888");
            //MessagingCenter.Send(this, "UpdateTache", newTache);
            await Navigation.PopModalAsync();
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