using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using RemindXamarin.Models;
using RemindXamarin.Views;

namespace RemindXamarin.ViewModels
{
    class CategoriesViewModel : BaseViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }
        public Command LoadCategoryCommand { get; set; }

        public CategoriesViewModel()
        {
            Categories = new ObservableCollection<Category>();
            LoadCategoryCommand = new Command(async () => await ExecuteLoadCategoriesCommand());

            MessagingCenter.Subscribe<NewCategory, Category>(this, "AddCategory", async (obj, category) =>
            {
                var newCategory = category as Category;

                Categories.Add(newCategory);

                await DataStore.AddCategoryAsync(newCategory);
            });
        }



        async Task ExecuteLoadCategoriesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Categories.Clear();
                var categories = await DataStore.GetCategoriesAsync(true);
                foreach (var cat in categories)
                {
                    Categories.Add(cat);
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
