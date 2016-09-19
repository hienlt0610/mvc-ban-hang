using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Core.RepositoryModel
{
    public class GroupProductRepository : RepositoryModel<GroupProduct>
    {
        public GroupProductRepository(DbContext db) : base(db)
        {
            
        }

        public IEnumerable<GroupProduct> GetTopGroupProduct(){
            return FetchAll().Where(item => item.ParentGroupID == null).OrderBy(item => item.GroupName).ToList();
        }

        public List<Product> GetProductInGroup(int group, List<Product> products = null)
        {
            if (products == null) products = new List<Product>();
            var currGroup = FindById(group);
            products.AddRange(currGroup.Products.Where(item => item.Active == true));
            //Find subcategory
            List<GroupProduct> subGroups = FetchAll().Where(item => item.ParentGroupID == group).ToList();
            foreach (GroupProduct subGroup in subGroups)
            {
                GetProductInGroup(subGroup.GroupID,products);
            }
            return products;
        }
    }
}