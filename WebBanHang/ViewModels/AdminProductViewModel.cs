using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.ViewModels
{
    public class AdminProductViewModel
    {
        public int ProductID { get; set; }
        public String ProductName { get; set; }
        public String Detail { get; set; }
        public long Price { get; set; }
        public long SalePrice { get; set; }
        public int Stock { get; set; }
        public int GroupID { get; set; }
        public bool UseMultiColor { get; set; }
        public bool Active { get; set; }
        public List<Quantity> Quantity { get; set; }

        public AdminProductViewModel()
        {
            Quantity = new List<Quantity>();
        }
    }
}