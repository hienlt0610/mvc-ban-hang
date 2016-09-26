using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Core.RepositoryModel
{
    public class CustomerRepository : RepositoryModel<Customer>
    {
        public CustomerRepository(DbContext db)
            : base(db)
        {

        }

        public bool checkExistCustomer(String username)
        {
            return FetchAll().Any(item => item.Username == username);
        }

        public Customer FindByEmail(String email)
        {
            return FetchAll().Where(item => item.Email.Equals(email)).FirstOrDefault();
        }
    }
}