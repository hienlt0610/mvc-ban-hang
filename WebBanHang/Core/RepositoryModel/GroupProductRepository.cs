using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebBanHang.Models;
using WebBanHang.Utils;

namespace WebBanHang.Core.RepositoryModel
{
    public class GroupProductRepository : RepositoryModel<GroupProduct>
    {
        public GroupProductRepository(DbContext db) : base(db)
        {
            
        }

        public IEnumerable<GroupProduct> GetTopGroupProducts(){
            return FetchAll().Where(item => item.ParentGroupID == null).OrderBy(item => item.GroupName).ToList();
        }

        public List<Product> GetProductInGroups(int group, List<Product> products = null)
        {
            if (products == null) products = new List<Product>();
            var currGroup = FindById(group);
            products.AddRange(currGroup.Products.Where(item => item.Active == true));
            //Find subcategory
            List<GroupProduct> subGroups = FetchAll().Where(item => item.ParentGroupID == group).ToList();
            foreach (GroupProduct subGroup in subGroups)
            {
                GetProductInGroups(subGroup.GroupID,products);
            }
            return products;
        }

        public List<Product> GetProductInGroups(int group, NameValueCollection filter)
        {
            IEnumerable<Product> model = GetProductInGroups(group).OrderByDescending(item => item.CreateDate);

            if (!String.IsNullOrEmpty(filter["color"]))
            {
                model = model.Where(product => product.Colors.Any(color => color.ColorID.ToString().Equals(filter["color"])));
            }

            if (!String.IsNullOrEmpty(filter["range_price"]))
            {
                string[] minMax = filter["range_price"].Split(',');
                if (minMax.Length == 2)
                {
                    long min = minMax[0].ToIntWithDef(0);
                    long max = minMax[1].ToIntWithDef(0);
                    model = model.Where(item => (item.isSale() && item.SalePrice >= min && item.SalePrice <= max) || (!item.isSale() && item.Price >= min && item.Price <= max));
                }
            }
            
            if (!String.IsNullOrEmpty(filter["sort"]))
            {
                switch (filter["sort"])
                {
                    case "name_asc":
                        model = model.OrderBy(item => item.ProductName);
                        break;
                    case "name_desc":
                        model = model.OrderByDescending(item => item.ProductName);
                        break;
                    case "price_asc":
                        model = model.OrderBy(item => item.isSale() ? item.SalePrice : item.Price);
                        break;
                    case "price_desc":
                        model = model.OrderByDescending(item => item.isSale() ? item.SalePrice : item.Price);
                        break;
                }
            }
            return model.ToList();
        }

        public IEnumerable<GroupProduct> GetListSubGroups(int groupID)
        {
            var mainGroupID = GetMainGroup(groupID);
            return FetchAll().Where(item => item.ParentGroupID == mainGroupID)
                .OrderByDescending(item => item.Priority)
                .ToList();
        }

        public int GetMainGroup(int groupID)
        {
            var currGroup = FindById(groupID);
            if (currGroup.ParentGroupID == null)
                return groupID;
            return GetMainGroup(currGroup.ParentGroupID.GetValueOrDefault(0));
        }
    }
}