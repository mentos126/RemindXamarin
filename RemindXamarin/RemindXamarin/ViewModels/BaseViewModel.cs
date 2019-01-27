using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using RemindXamarin.Models;
using RemindXamarin.Services;
using System.IO;

namespace RemindXamarin.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
#pragma warning disable CS0246 // Le nom de type ou d'espace de noms 'MockDataStore' est introuvable (vous manque-t-il une directive using ou une référence d'assembly ?)
        public IDataStore<Tache> DataStore => DependencyService.Get<IDataStore<Tache>>() ?? MockDataStore.Database;
#pragma warning restore CS0246 // Le nom de type ou d'espace de noms 'MockDataStore' est introuvable (vous manque-t-il une directive using ou une référence d'assembly ?)

       // public IDataStore<Tache> DataStore => DependencyService.Get<IDataStore<Tache>>() ?? MockDataStore.Database;

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
