using Plugin.Sensors;
using RemindXamarin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RemindXamarin.ViewModels
{
    class SportActivityViewModel : BaseViewModel
    {

        Tache MyTache { get; set; }

        public SportActivityViewModel(Tache Tache)
        {
            Title = "Sport";
            MyTache = Tache;
          

        }

        public async void AddSportTache(SportTache sp)
        {
            await DataStore2.AddSportTacheAsync(sp);
        }

    }

}
