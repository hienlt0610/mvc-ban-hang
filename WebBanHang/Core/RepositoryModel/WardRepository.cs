using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Core.RepositoryModel
{
    public class WardRepository : RepositoryModel<Ward>
    {
        public WardRepository(DbContext dbContext)
            : base(dbContext)
        {
            
        }
    }
}