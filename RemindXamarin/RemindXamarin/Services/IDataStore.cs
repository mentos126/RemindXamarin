using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemindXamarin.Services
{
    public interface IDataStore<T, C>
    {
        Task<bool> AddTacheAsync(T tache);
        Task<bool> UpdateTacheAsync(T tache);
        Task<bool> DeleteTacheAsync(int id);
        Task<T> GetTacheAsync(int id);
        Task<IEnumerable<T>> GetTachesAsync(bool forceRefresh = false);

        Task<bool> AddCategoryAsync(C category);
        Task<bool> UpdateCategoryAsync(C category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<C> GetCategoryAsync(int id);
        Task<IEnumerable<C>> GetCategoriesAsync(bool forceRefresh = false);

    }
}
