using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RemindXamarin.Models;
using RemindXamarin.Views;
using RemindXamarin.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Diagnostics;
using RemindXamarin.Services;
using System.Collections;
using System.Collections.ObjectModel;

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TachesPage : ContentPage
    {
        TachesViewModel viewModel;
        bool sortDirection = true;

        public TachesPage()
        {
            InitializeComponent();
            permissions();
            viewModel = new TachesViewModel();
            BindingContext = viewModel;

            sortDirection = true;
            this.sortList();
        }

        public async void permissions()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    //Permission granted, do what you want do.
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
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

                var mi = ((Button)sender);
                Tache myTache = ((Tache)mi.CommandParameter);
                myTache.photo = file.Path;
                Debug.Print(myTache.name+"++++++++++++++++++++++++");
                Debug.Print(myTache.photo + "++++++++++++++++++++++++");


                //viewModel.UpdateTache(myTache);
                Tasker.Instance.removeTask(myTache);
                Tasker.Instance.addTask(myTache);
                //Tasker.Instance.getListTasks();

                filterList("");
                /*viewModel.Taches = new ObservableCollection<Tache>(Tasker.Instance.getListTasks().Cast<Tache>().ToList());
                TachesListView.ItemsSource = viewModel.Taches;*/
                file.Dispose();
            }

        }

        void OnSelectPlace(object sender, EventArgs e)
        {
        }

        async void OnTacheSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var tache = args.SelectedItem as Tache;
            if (tache == null)
                return;

            await Navigation.PushModalAsync(new NavigationPage (new EditTachePage(tache)));

            // Manually deselect item.
            TachesListView.SelectedItem = null;
        }

        async void AddTache_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewTachePage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Taches.Count == 0)
                //viewModel.LoadTachesCommand.Execute(null);
                viewModel.Taches = new ObservableCollection<Tache>(Tasker.Instance.getListTasks().Cast<Tache>().ToList());
                //Tasker.Instance.getListTasks();

        }

        public void filterList(String s)
        {
            ObservableCollection<Tache> refs = new ObservableCollection<Tache>(Tasker.Instance.getListTasks().Cast<Tache>().ToList());
            String keywords = s.ToLower();
            IEnumerable<Tache> result = null;
            if (keywords.Equals(""))
            {
                result = refs;
            }
            else
            { 
                result = refs.Where(tache => tache.name.ToLower().Contains(keywords));
            }
            /*Debug.Print(Tasker.Instance.getListTasks().ToString());
            ArrayList result = new ArrayList();
            if (keywords.Equals(""))
            {
                result = Tasker.Instance.getListTasks();
            }
            else
            {
                foreach(Tache t in Tasker.Instance.getListTasks())
                {
                    Debug.Print(t.name+"7777777777777");
                    if (t.name.ToLower().Contains(keywords))
                    {
                        result.Add(t);
                    }
                }
            }*/
            viewModel.Taches.Clear();
            foreach(Tache res in result)
            {
                viewModel.Taches.Add(res);
            }
            //viewModel.Taches = new ObservableCollection<Tache>(result.Cast<Tache>().ToList());
            TachesListView.ItemsSource = viewModel.Taches;

        }

        public void sortList()
        {
          /*  IEnumerable<Tache> result = null;
            if (sortDirection)
            {
                result = viewModel.Taches.OrderBy( tache => tache.name);
            }
            else
            {
                result = viewModel.Taches.OrderByDescending( tache => tache.name);
            }
            viewModel.Taches =  new ObservableCollection<Tache>(result.Cast<Tache>().ToList());
            TachesListView.ItemsSource = viewModel.Taches;
            sortDirection = !sortDirection;
            */
        }

        void OnDelete (object sender, EventArgs e)
        {
            try
            { 
                var mi = ((MenuItem)sender);
                //viewModel.DeleteTache(((Tache) mi.CommandParameter));
                Tasker.Instance.removeTask((Tache) mi.CommandParameter);
                viewModel.Taches = new ObservableCollection<Tache>(Tasker.Instance.getListTasks().Cast<Tache>().ToList());
                TachesListView.ItemsSource = viewModel.Taches;
            }
            catch (Exception x)
            {
                Console.Write(x.Message);
            }
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            this.filterList(SearchBar.Text.ToLower());
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.filterList(SearchBar.Text.ToLower());
        }

        private void TapOrder_Tapped(object sender, EventArgs e)
        {
            this.sortList();
        }

        private void SwitchNotification_Toggled(object sender, ToggledEventArgs e)
        {
            var mi = ((MenuItem)sender);
            ((Tache)mi.CommandParameter).isActivatedNotification = !((Tache)mi.CommandParameter).isActivatedNotification;
        }
    }
}