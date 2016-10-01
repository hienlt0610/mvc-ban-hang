using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Core.RepositoryModel
{
    public class UserRepository : RepositoryModel<User>
    {
        public UserRepository(DbContext db) : base(db) { }

        public User FindByUserName(String username)
        {
            if (username == null) return null;
            username = username.Trim();
            return FetchAll().Where(item => item.Username.Equals(username)).FirstOrDefault();
        }
    }
}