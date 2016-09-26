using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        public String ProductName { get; set; }

        [Required]
        public String Detail { get; set; }
        
        public int GroupID { get; set; }

        [Required]
        public long Price { get; set; }

        [Required]
        public long SalePrice { get; set; }

        [Required]
        public int Stock { get; set; }

        public bool Active { get; set; }
    }
}