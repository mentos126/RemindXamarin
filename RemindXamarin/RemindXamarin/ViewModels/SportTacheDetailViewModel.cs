using RemindXamarin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RemindXamarin.ViewModels
{
    public class SportTacheDetailViewModel : BaseViewModel
    {
        public SportTache SportTache { get; set; }

        public SportTacheDetailViewModel(SportTache sportTache = null)
        {
            Title = sportTache?.Name;
            SportTache = sportTache;
        }

    }
}
