using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Core
{
    public class ShoppingCart
    {
        public const string CartSessionKey = "HiThaShopCart";
        public List<CartItem> Items { get; set; }
        private static ShoppingCart m_instance = null;
        private ShoppingCart() { }
        public static ShoppingCart Instance
        {
            get{
                if (HttpContext.Current.Session[CartSessionKey] == null)
                {
                    m_instance = new ShoppingCart();
                    m_instance.Items = new List<CartItem>();
                    HttpContext.Current.Session[CartSessionKey] = m_instance;
                }
                else
                {
                    m_instance = (ShoppingCart)HttpContext.Current.Session[CartSessionKey];
                }
                return m_instance;
            }
        }

        public int GetCount()
        {
            return Items.Count;
        }

        public long GetTotal()
        {
            long total = 0;
            foreach (CartItem item in Items)
                total += item.TotalPrice;
            return total;
        }


        public void AddItem(Product product, Color color)
        {
            CartItem newItem = new CartItem(product,color);

            if (Items.Contains(newItem))
            {
                foreach (CartItem item in Items)
                {
                    if (item.Equals(newItem))
                    {
                        item.Quantity++;
                        return;
                    }
                }
            }
            else
            {
                newItem.Quantity = 1;
                Items.Add(newItem);
            }
        }

        public void Remove(Product product, Color color)
        {
            CartItem oldItem = new CartItem(product, color);
            foreach (CartItem item in Items)
            {
                if (item.Equals(oldItem))
                {
                    Items.Remove(oldItem);
                    return;
                }
            }
        }

        public CartItem Update(Product product, Color color, int quantity)
        {
            //CartItem oldItem = new CartItem(product, color);
            CartItem item = Items.Find(x => x.Equals(product, color));
            if (item != null)
            {
                item.Quantity = quantity;
            }
            return item;
        }

        public void Clean()
        {
            Items.Clear();
        }
    }
}