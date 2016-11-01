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

        public void UpdateConfig(String configName, String configValue)
        {
            if (configName == null || configValue == null) return;
            var config = FetchAll().FirstOrDefault(c => c.ConfigName.Equals(configName));
            if (config == null) return;
            config.Value = configValue;
        }
    }
}