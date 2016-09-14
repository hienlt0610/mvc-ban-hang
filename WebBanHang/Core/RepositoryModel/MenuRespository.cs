using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Core.RepositoryModel
{
    public class MenuRespository : RepositoryModel<Menu>
    {
        public MenuRespository(DbContext db) : base(db)
        {

        }
    }
}