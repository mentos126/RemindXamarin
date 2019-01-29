using RemindXamarin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using RemindXamarin.Views;

namespace RemindXamarin.ViewModels
{
    public class SportTachesViewModel : BaseViewModel
    {

        public ObservableCollection<SportTache> SportTaches { get; set; }
        public Command LoadSportTachesCommand { get; set; }

        public SportTachesViewModel()
        {
            Title = "Reminds";
            SportTaches = new ObservableCollection<SportTache>();
            LoadSportTachesCommand = new Command(async () => await ExecuteLoadSportTachesCommand());

            MessagingCenter.Subscribe<SportActivityPage, SportTache>(this, "AddSportTache", async (obj, tache) =>
            {
                var newTache = tache as SportTache;
                SportTaches.Add(newTache);
                await DataStore2.AddSportTacheAsync(newTache);

            });

        }

        public async void DeleteTache(SportTache sportTache)
        {
            try
            {
                if (SportTaches.Contains(sportTache))
                {
                    SportTaches.Remove(sportTache);
                    await DataStore2.DeleteSportTacheAsync(sportTache);
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

        async Task ExecuteLoadSportTachesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                SportTaches.Clear();
                var sportTaches = await DataStore2.GetSportTachesAsync(true);
                foreach (var t in sportTaches)
                {
                    SportTaches.Add(t);
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
