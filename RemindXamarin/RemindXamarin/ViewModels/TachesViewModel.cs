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
        public Command LoadTachesCommand { get; set; }

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