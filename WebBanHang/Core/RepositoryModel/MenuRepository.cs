using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Core.RepositoryModel
{
    public class MenuRepository : RepositoryModel<Menu>
    {
        public MenuRepository(DbContext db) : base(db)
        {

        }
    }
}