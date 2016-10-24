using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.ViewModels
{
    public class AdminProductViewModel
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage="Vui lòng nhập tên sản phẩm")]
        public String ProductName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên nội dung sản phẩm")]
        [AllowHtml]
        public String Detail { get; set; }

       [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
        public long Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá giảm")]
        public long SalePrice { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhóm sản phẩm")]
        public int GroupID { get; set; }

        public bool UseMultiColor { get; set; }

        public bool Active { get; set; }
        public List<ProductColor> ProductColor { get; set; }
        public List<ProductAttribute> ProductAttribute { get; set; }

        public AdminProductViewModel()
        {
            ProductColor = new List<ProductColor>();
            ProductAttribute = new List<ProductAttribute>();
        }
    }
}