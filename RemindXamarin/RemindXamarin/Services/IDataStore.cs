using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace RemindXamarin.Services
{
    public interface IDataStore<T>
    {
        Task<int> AddTacheAsync(T tache);
        Task<int> UpdateTacheAsync(T tache);
        Task<int> DeleteTacheAsync(T tache);
        Task<T> GetTacheAsync(int id);
        Task<IEnumerable<T>> GetTachesAsync(bool forceRefresh = false);
        Task<IEnumerable<T>> SearchAsync(String recherche);
    }
}

