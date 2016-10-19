using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.ViewModels
{
    public class ShippingViewModel
    {
        public int CustomerID { get; set; }
        public String FullName { get; set; }
        public String Phone { get; set; }
        public String Address { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }

        public Province Province { get; set; }
        public District District { get; set; }
        public Ward Ward { get; set; }
    }
}