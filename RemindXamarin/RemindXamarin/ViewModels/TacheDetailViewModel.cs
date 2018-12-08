using System;

using RemindXamarin.Models;

namespace RemindXamarin.ViewModels
{
    public class TacheDetailViewModel : BaseViewModel
    {
        public Tache Tache { get; set; }
        public TacheDetailViewModel(Tache tache = null)
        {
            Title = tache?.name;
            Tache = tache;
        }
    }
}
