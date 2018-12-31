using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

            MessagingCenter.Subscribe<EditTachePage, Tache>(this, "UpdateTache", async (obj, tache) =>
            {
                var newTache = tache as Tache;
                var oldTache = Taches.Where((Tache arg) => arg.ID == newTache.ID).FirstOrDefault();
                Taches.Remove(oldTache);
                Taches.Add(newTache);
                await DataStore.UpdateTacheAsync(newTache);
            });

        }

        public async void DeleteTache(Tache tache)
        {
            try
            {
                if (Taches.Contains(tache))
                {
                    Taches.Remove(tache);
                    await DataStore.DeleteTacheAsync(tache.ID);
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