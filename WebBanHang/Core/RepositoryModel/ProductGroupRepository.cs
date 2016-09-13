using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebBanHang.Core.RepositoryModel
{
    public class ProductGroupRepository : RepositoryModel<ProductGroupRepository>
    {
        public ProductGroupRepository(DbContext db) : base(db)
        {

        }
    }
}