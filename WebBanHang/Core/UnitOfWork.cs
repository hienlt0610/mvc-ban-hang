using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebBanHang.Core.RepositoryModel;

namespace WebBanHang.Core
{
    public class UnitOfWork
    {
        private Dictionary<Type, object> dict = new Dictionary<Type, object>();
        private DbContext _dbContext;

        //Private members corresponding to each concrete repository
        private ColorRepository colorRepository;
        private ConfigRepository configRepository;
        private CustomerRepository customerRepository;
        private GroupProductRepository groupProductRepository;
        private MenuRepository menuRepository;
        private ProductRepository productRepository;
        private UserRepository userRepository;

        public UnitOfWork(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Color Repository
        /// </summary>
        public ColorRepository Color
        {
            get
            {
                if (colorRepository == null)
                    colorRepository = new ColorRepository(_dbContext);
                return colorRepository;
            }

        }

        /// <summary>
        /// Config Repository
        /// </summary>
        public ConfigRepository Config
        {
            get
            {
                if (configRepository == null)
                    configRepository = new ConfigRepository(_dbContext);
                return configRepository;
            }

        }

        /// <summary>
        /// Customer Repository
        /// </summary>
        public CustomerRepository Customer
        {
            get
            {
                if (customerRepository == null)
                    customerRepository = new CustomerRepository(_dbContext);
                return customerRepository;
            }

        }

        /// <summary>
        /// GroupProduct Repository
        /// </summary>
        public GroupProductRepository GroupProduct 
        {
            get
            {
                if (groupProductRepository == null)
                    groupProductRepository = new GroupProductRepository(_dbContext);
                return groupProductRepository;
            }

        }

        /// <summary>
        /// Menu Repository
        /// </summary>
        public MenuRepository Menu
        {
            get
            {
                if (menuRepository == null)
                    menuRepository = new MenuRepository(_dbContext);
                return menuRepository;
            }

        }
        /// <summary>
        /// Product Repository
        /// </summary>
        public ProductRepository Product
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(_dbContext);
                return productRepository;
            }

        }

        public UserRepository User
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(_dbContext);
                return userRepository;
            }
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