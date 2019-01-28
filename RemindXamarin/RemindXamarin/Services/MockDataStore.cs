using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemindXamarin.Models;
using Xamarin.Forms;
using SQLite;
using System.IO;
using System.Diagnostics;

namespace RemindXamarin.Services
{
    public class MockDataStore : SQLiteAsyncConnection, IDataStore<Tache>
    {

        static MockDataStore database;

        public static MockDataStore Database
        {
            get
            {
                if (database == null)
                {
                    database = new MockDataStore(
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                            "TachesSQLite.db3"
                            )
                        );
                    
                }
                return database;
            }
        }

        public MockDataStore(string path) : base(path)
        {
            this.CreateTableAsync<Tache>().Wait();
        }


        public async Task<int> AddTacheAsync(Tache tache)
        {
            return await Database.InsertAsync(tache);
        }

        public async Task<int> UpdateTacheAsync(Tache tache)
        {
            return await Database.UpdateAsync(tache);
        }

        public async Task<int> DeleteTacheAsync(Tache tache)
        {
            return await Database.DeleteAsync(tache);
        }

        public async Task<Tache> GetTacheAsync(int id)
        {
            return await Database.GetAsync<Tache>(id);
        }

        public async Task<IEnumerable<Tache>> GetTachesAsync(bool forceRefresh = false)
        {
            return await Database.Table<Tache>().ToListAsync();
        }

        public async Task<IEnumerable<Tache>> SearchAsync(String recherche)
        {
            var taches = from t in Database.Table<Tache>()
                            orderby t.DateDeb
                            where t.Name.Contains(recherche) || t.Description.Contains(recherche) 
                            select t;

            return await taches.ToListAsync();
        }

    }
}