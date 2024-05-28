using BlogSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.IDAL
{
    public interface IBaseService<T>:IDisposable where T : BaseEntity
    {
        Task CreateAsync(T obj, bool saved = true);
        Task EditAsync(T obj, bool saved = true);
        IQueryable<T> GetAll();
        IQueryable<T> GetAllByOrder(bool asc = true);
        IQueryable<T> GetAllByPage(int pageSize = 10, int pageIndex = 0);
        IQueryable<T> GetAllByPageOrder(int pageSize = 10, int pageIndex = 0, bool asc = true);
        IQueryable<T> GetOneByIdAsync(Guid id);
        Task RemovedAsync(Guid id, bool saved = true);
        Task RemovedAsync(T obj, bool saved = true);
        Task SaveAsync();
    }
}
