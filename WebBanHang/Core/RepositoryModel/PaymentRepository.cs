using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Core.RepositoryModel
{
    public class PaymentRepository:RepositoryModel<Payment>
    {
        public PaymentRepository(DbContext dbContext)
            : base(dbContext)
        {
            
        }
    }
}