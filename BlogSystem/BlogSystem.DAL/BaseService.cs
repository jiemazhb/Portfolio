using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.DAL
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity, new()
    {
        private readonly BlogContext _db;
        public BaseService(BlogContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(T obj, bool saved = true)
        {

            DbSet<T> data = _db.Set<T>();
            data.Add(obj);

            if (saved == true)
            {
                await _db.SaveChangesAsync();
            }
          
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task EditAsync(T obj, bool saved = true)
        {
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.Entry(obj).State = EntityState.Modified;
            if (saved == true)
            {
                await _db.SaveChangesAsync();
            }

        }

        public IQueryable<T> GetAll()
        {
            DbSet<T> data = _db.Set<T>();
            var result = data.Where(a => a.IsRemoved == false).AsNoTracking();
            return result;
        }

        public IQueryable<T> GetAllByOrder(bool asc = true)
        {
            var data = GetAll();
            if (asc)
            {
                data = data.OrderBy(t => t.CreateTime);
            }
            else
            {
                data = data.OrderByDescending(t => t.CreateTime);
            }
            return data;
        }
        /// <summary>
        /// pageNum指的是第几页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public IQueryable<T> GetAllByPage(int pageSize = 10, int pageNum = 0)
        {
            IQueryable<T> result = GetAll().Skip(pageSize * pageNum).Take(pageSize);
            return result;
        }

        public IQueryable<T> GetAllByPageOrder(int pageSize = 10, int pageNum = 0, bool asc = true)
        {
            var result = GetAllByOrder(asc).Skip(pageSize*pageNum).Take(pageSize);
            return result;
        }

        public async Task<T> GetOneByIdAsync(Guid id)
        {
            T result = await GetAll().FirstAsync(t => t.Id == id);
            return result;
        }
        public async Task RemovedAsync(Guid id, bool saved = true)
        {
            _db.Configuration.ValidateOnSaveEnabled = false;
            T t = new T() { Id = id };
            _db.Entry(t).State = EntityState.Unchanged;
            t.IsRemoved = true;
            if (saved == true)
            {
                await _db.SaveChangesAsync();
                _db.Configuration.ValidateOnSaveEnabled = true;
            }
            
        }

        public async Task RemovedAsync(T obj, bool saved = true)
        {
            await RemovedAsync(obj.Id, saved);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
            _db.Configuration.ValidateOnSaveEnabled = true; 
        }
    }
}
