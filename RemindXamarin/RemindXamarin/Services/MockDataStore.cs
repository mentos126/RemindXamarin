using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RemindXamarin.Models;

namespace RemindXamarin.Services
{
    public class MockDataStore : IDataStore<Tache>
    {
        List<Tache> taches;

        public MockDataStore()
        {
            taches = new List<Tache>();
            var mockTaches = new List<Tache>
            {
                new Tache("name 1", "description 1", new Category(Tasker.CATEGORY_NONE_TAG, "ic_alarm_black_36dp.png", 12), new DateTime(), 30, 12, 22),
                new Tache("name 2", "description 1", new Category(Tasker.CATEGORY_NONE_TAG, "ic_alarm_black_36dp.png", 12), new DateTime(), 30, 12, 22),
                new Tache("name 3", "description 1", new Category(Tasker.CATEGORY_SPORT_TAG, "ic_directions_run_black_36dp.png", 12), new DateTime(), 30, 12, 22),
                new Tache("name 4", "description 1", new Category(Tasker.CATEGORY_NONE_TAG, "ic_alarm_black_36dp.png", 12), new DateTime(), 30, 12, 22),
                new Tache("name 5", "description 1", new Category(Tasker.CATEGORY_NONE_TAG, "ic_alarm_black_36dp.png", 12), new DateTime(), 30, 12, 22),
            };

            foreach (var tache in mockTaches)
            {
                taches.Add(tache);
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
    }
}