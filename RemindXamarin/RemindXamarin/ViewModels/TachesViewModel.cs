using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using RemindXamarin.Models;
using RemindXamarin.Views;

namespace RemindXamarin.ViewModels
{
    public class TachesViewModel : BaseViewModel
    {
        public ObservableCollection<Tache> Taches { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public Command LoadTachesCommand { get; set; }
        public Command LoadCategoryCommand { get; set; }
        public String debug { get; set; }

        public TachesViewModel()
        {
            Title = "Reminds";
            Taches = new ObservableCollection<Tache>();
            LoadTachesCommand = new Command(async () => await ExecuteLoadTachesCommand());

            MessagingCenter.Subscribe<NewTachePage, Tache>(this, "AddTache", async (obj, tache) =>
            {
                var newTache = tache as Tache;
                Taches.Add(newTache);
                await DataStore.AddTacheAsync(newTache);
            });

        }

        public async void DeleteTache(int id)
        {
            try
            {
                // Taches.Clear();
                bool temp = false;
                temp = await DataStore.DeleteTacheAsync(id);
               /* var taches = await DataStore.GetTachesAsync(true);
                foreach (var tache in taches)
                {
                    Taches.Add(tache);
                }*/
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLoadTachesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Taches.Clear();
                var taches = await DataStore.GetTachesAsync(true);
                foreach (var tache in taches)
                {
                    Taches.Add(tache);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}