using System;
using System.Diagnostics;
using RemindXamarin.Models;

namespace RemindXamarin.ViewModels
{
    public class TacheDetailViewModel : BaseViewModel
    {
        public Tache Tache { get; set; }

        public TacheDetailViewModel(Tache tache = null)
        {
            Title = tache?.Name;
            Tache = tache;
        }

        public async void UpdateTache(Tache tache)
        {
            try
            {
                await DataStore.UpdateTacheAsync(tache);
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
