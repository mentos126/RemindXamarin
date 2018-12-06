using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RemindXamarin.Models;

namespace RemindXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTachePage : ContentPage
    {
        public Tache Tache { get; set; }

        public NewTachePage()
        {
            InitializeComponent();

            Tache = new Tache("", "", new Category("", 0, 0), new DateTime(), 0,0,0);

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddTache", Tache);
            await Navigation.PopModalAsync();
        }
    }
}