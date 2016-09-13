using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebBanHang.Core
{
    public class WebDbContext : DbContext
    {
        public WebDbContext(String connectionString) : base(connectionString)
        {
            
        }
    }
}