using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Core
{
    public class RepositoryModel<T> : IRepository<T>
    where T : class
    {
        protected DbContext dbContext;
        private IDbSet<T> dbSet;

        public RepositoryModel(DbContext _dbContext)
        {
            this.dbContext = _dbContext;
            this.dbSet = this.dbContext.Set<T>();
        }

        public RepositoryModel()
        {
            this.dbContext = new ecommerceEntities();
            this.dbSet = this.dbContext.Set<T>();
        }

        public DbContext DbContext
        {
            get
            {
                return this.dbContext;
            }
            set
            {
                this.dbContext = value;
            }
        }


        public T Insert(T entity)
        {
            return dbSet.Add(entity);
        }

        public bool Delete(params object[] keyValues)
        {
            var item = dbSet.Find(keyValues);
            if (item == null)
                return false;
            dbSet.Remove(item);
            return true;
        }

        public T Update(T entity)
        {
            var _updated = dbSet.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            return _updated;
        }

        public IQueryable<T> FetchAll()
        {
            return dbSet;
        }

        public T FindById(params object[] key)
        {
            return dbSet.Find(key);
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

    }
}