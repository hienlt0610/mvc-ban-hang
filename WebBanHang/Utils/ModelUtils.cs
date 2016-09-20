using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Utils
{
    public static class ModelUtils
    {
        public static int GetLevel(this GroupProduct group, int level = 0)
        {
            if (group.ParentGroupID == null || group.ParentGroupID == 0) return level;
            level++;
            var category = group.GroupProduct1;
            return category.GetLevel(level);
        }

        public static bool isSale(this Product product)
        {
            return (product.SalePrice != 0 && (product.Price - product.SalePrice) > 0);
        }
    }
}