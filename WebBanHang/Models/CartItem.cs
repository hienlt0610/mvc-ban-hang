using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Utils;

namespace WebBanHang.Models
{
    public class CartItem : IEquatable<CartItem>
    {
        public int ProductID { get; set; }
        public int ColorID { get; set; }
        public Color Color { get; set; }
        public long Price
        {
            get
            {
                if (Product == null) return 0;
                if (Product.isSale()) return Product.SalePrice;
                return Product.Price;
            }
        }
        public int Quantity { get; set; }
        public long TotalPrice
        {
            get { return Price * Quantity; }
        }
        public Product Product { get; set; }

        public CartItem(Product product, Color color)
        {
            if(product!=null)
                ProductID = product.ProductID;
            if (color != null)
                ColorID = color.ColorID;
            Color = color;
            Product = product;
        }


        public bool Equals(CartItem item)
        {
            if (item.Color == null || item.Color.ColorID == 0)
                return (item.ProductID == ProductID);
            return (item.ProductID == ProductID && item.ColorID == ColorID);
        }

        public bool Equals(Product product, Color color)
        {
            if (color == null || color.ColorID == 0)
            {
                return (product.ProductID == ProductID);
            }
            return (product.ProductID == ProductID && color.ColorID == ColorID);
        }
    }
}