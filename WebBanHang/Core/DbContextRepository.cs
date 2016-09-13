using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebBanHang.Core.RepositoryModel;

namespace WebBanHang.Core
{
    public class DbContextRepository
    {
        private Dictionary<Type, object> dict = new Dictionary<Type, object>();
        private DbContext _dbContext;
        public DbContextRepository(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public T Create<T>() where T : class
        {
            if (dict.ContainsKey(typeof(T))) return (T)dict[typeof(T)];
            var result = (T) Activator.CreateInstance(typeof(T), _dbContext);
            if (result != null)
            {
                dict.Add(typeof(T),result);
                return result;
            }
            return null;
        }

        private bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}