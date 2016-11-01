using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Core.RepositoryModel
{
    public class OrderDetailRepository : RepositoryModel<OrderDetail>
    {
        public OrderDetailRepository(DbContext dbContext)
            : base(dbContext)
        {
            
        }
    }
}