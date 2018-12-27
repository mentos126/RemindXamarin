using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RemindXamarin.Models;
using Xamarin.Forms;

namespace RemindXamarin.Services
{
    public class MockDataStore : IDataStore<Tache, Category>
    {
        List<Tache> taches;
        List<Category> categories;

        public MockDataStore()
        {
            taches = new List<Tache>();
            var mockTaches = new List<Tache>
            {
                new Tache("name 1", "description 1", new Category(Tasker.CATEGORY_NONE_TAG, "ic_alarm_black_36dp.png", Color.FromHex("FF6A00")), new DateTime(), 30, 12, 22),
                new Tache("name 2", "description 1", new Category(Tasker.CATEGORY_NONE_TAG, "ic_alarm_black_36dp.png", Color.FromHex("FF6A00")), new DateTime(), 30, 12, 22),
                new Tache("name 3", "description 1", new Category(Tasker.CATEGORY_SPORT_TAG, "ic_directions_run_black_36dp.png", Color.FromHex("FFFF92FF")), new DateTime(), 30, 12, 22),
                new Tache("name 4", "description 1", new Category(Tasker.CATEGORY_NONE_TAG, "ic_alarm_black_36dp.png", Color.FromHex("FF6A00")), new DateTime(), 30, 12, 22),
                new Tache("name 5", "description 1", new Category(Tasker.CATEGORY_NONE_TAG, "ic_alarm_black_36dp.png", Color.FromHex("FF6A00")), new DateTime(), 30, 12, 22),
            };

            foreach (var tache in mockTaches)
            {
                //taches.Add(tache);
            }

            categories = new List<Category>();

            var mockCategories = new List<Category>
            {

            };

            foreach (var cat in mockCategories)
            {
                categories.Add(cat);
            }
        }

        public async Task<bool> AddTacheAsync(Tache tache)
        {
            taches.Add(tache);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateTacheAsync(Tache tache)
        {
            var oldTache = taches.Where((Tache arg) => arg.ID == tache.ID).FirstOrDefault();
            taches.Remove(oldTache);
            taches.Add(tache);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteTacheAsync(int id)
        {
            var oldTache = taches.Where((Tache arg) => arg.ID == id).FirstOrDefault();
            taches.Remove(oldTache);

            return await Task.FromResult(true);
        }

        public async Task<Tache> GetTacheAsync(int id)
        {
            return await Task.FromResult(taches.FirstOrDefault(s => s.ID == id));
        }

        public async Task<IEnumerable<Tache>> GetTachesAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(taches);
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            categories.Add(category);
            return await Task.FromResult(true);

        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            var oldCategory = categories.Where((Category arg) => arg.ID == category.ID).FirstOrDefault();
            categories.Remove(oldCategory);
            categories.Add(category);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var oldCategory = categories.Where((Category arg) => arg.ID == id).FirstOrDefault();
            categories.Remove(oldCategory);

            return await Task.FromResult(true);
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await Task.FromResult(categories.FirstOrDefault(s => s.ID == id));
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(categories);
        }
    }
}