using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemindXamarin.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddTacheAsync(T tache);
        Task<bool> UpdateTacheAsync(T tache);
        Task<bool> DeleteTacheAsync(int id);
        Task<T> GetTacheAsync(int id);
        Task<IEnumerable<T>> GetTachesAsync(bool forceRefresh = false);
    }
}
