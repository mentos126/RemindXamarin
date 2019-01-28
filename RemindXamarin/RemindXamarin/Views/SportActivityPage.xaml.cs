using RemindXamarin.Models;
using RemindXamarin.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SportActivityPage : ContentPage
    {
        SportActivityViewModel ViewModels { get; set; }
        Tache MyTache { get; set; }

        public SportActivityPage(Tache Tache)
        {
            InitializeComponent();
            ViewModels = new SportActivityViewModel(Tache);
            MyTache = Tache;

            BindingContext = this;
        }

        public void Save_Clicked()
        {

        }
    }
}