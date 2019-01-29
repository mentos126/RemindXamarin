using RemindXamarin.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using System.Diagnostics;


namespace RemindXamarin.Services
{
    public class MockDataStore2 : SQLiteAsyncConnection, IDataStore2<SportTache>
    {

        static MockDataStore2 database;

        public static MockDataStore2 Database
        {
            get
            {
                if (database == null)
                {
                    database = new MockDataStore2(
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "SportTachesSQLite.db3"
                            )
                        );

                }
                return database;
            }
        }

        public MockDataStore2(string path) : base(path)
        {
            this.CreateTableAsync<SportTache>().Wait();
        }

        public async Task<IEnumerable<SportTache>> GetSportTachesAsync(bool forceRefresh = false)
        {
            return await Database.Table<SportTache>().ToListAsync();
        }

        public async Task<int> AddSportTacheAsync(SportTache tache)
        {
            return await Database.InsertAsync(tache);
        }

        public async Task<int> DeleteSportTacheAsync(SportTache tache)
        {
            return await Database.DeleteAsync(tache);
        }
    }
}
