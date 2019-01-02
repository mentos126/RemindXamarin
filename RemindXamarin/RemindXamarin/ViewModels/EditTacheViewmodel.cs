using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using RemindXamarin.Models;
using RemindXamarin.Views;
using System.Collections;

namespace RemindXamarin.ViewModels
{

    public class EditTacheViewModel : BaseViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<String> CategoriesName { get; set; }
        public Command LoadCategoriesCommand { get; set; }
        public Tache Tache { get; set; }


        public EditTacheViewModel(Tache tache = null)
        {
            Title = tache?.name;
            Tache = tache;
            Categories = new ObservableCollection<Category>();
            LoadCategoriesCommand = new Command(async () => await ExecuteLoadCategoriesCommand());

            MessagingCenter.Subscribe<NewCategory, Category>(this, "AddCategory", async (obj, category) =>
            {
                var newCategory = category as Category;
                Categories.Add(newCategory);
                await DataStore.UpdateCategoryAsync(newCategory);
            });

            MessagingCenter.Subscribe<EditCategory, Category>(this, "UpdateCategory", async (obj, category) =>
            {
                var newCategory = category as Category;
                var oldCategory = Categories.Where((Category arg) => arg.ID == newCategory.ID).FirstOrDefault();
                Categories.Remove(oldCategory);
                Categories.Add(newCategory);
                await DataStore.UpdateCategoryAsync(newCategory);
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
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
                foreach (Category category in Tasker.Instance.getListCategories())
                {
                    Categories.Add(category);
                }
                CategoriesName = new ObservableCollection<String>();
                foreach (Category c in Categories)
                {
                    CategoriesName.Add(c.name);
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
