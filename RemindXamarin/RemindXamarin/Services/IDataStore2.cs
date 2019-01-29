using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace RemindXamarin.Services
{
    public interface IDataStore2<T>
    {
        Task<IEnumerable<T>> GetSportTachesAsync(bool forceRefresh = false);
        Task<int> AddSportTacheAsync(T tache);
        Task<int> DeleteSportTacheAsync(T tache);
    }
}
