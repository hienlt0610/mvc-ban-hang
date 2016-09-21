﻿using System;
using System.Collections.Generic;
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

        public List<Product> GetProductInGroups(int group, String sort)
        {
            List<Product> model = null;
            switch (sort)
            {
                case "name_asc":
                    model = GetProductInGroups(group).OrderBy(item => item.ProductName).ToList();
                    break;
                case "name_desc":
                    model = GetProductInGroups(group).OrderByDescending(item => item.ProductName).ToList();
                    break;
                case "price_asc":
                    model = GetProductInGroups(group).
                        OrderBy(item => item.isSale() ? item.SalePrice : item.Price)
                        .ToList();
                    break;
                case "price_desc":
                    model = GetProductInGroups(group).OrderByDescending(item => item.isSale() ? item.SalePrice : item.Price).ToList();
                    break;
                default:
                    model = GetProductInGroups(group).OrderByDescending(item => item.CreateDate).ToList();
                    break;
            }
            return model;
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