using System.Linq;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();


        Task<T> GetIdAsync(int id);


        Task CreateAsync(T entity);


        Task UpdateAsync(T entity);


        Task DeleteAsync(T entity);


        Task<bool> ExistsAsync(int id);
    }
}
