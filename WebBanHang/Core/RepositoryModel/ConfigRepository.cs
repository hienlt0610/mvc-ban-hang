using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Core.RepositoryModel
{
    public class ConfigRepository : RepositoryModel<Configuration>
    {
        public ConfigRepository(DbContext db) : base(db)
        {
        }
    }
}