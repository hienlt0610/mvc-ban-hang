using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.ViewModels
{
    public class AdminConfigViewModel
    {
        [Required(ErrorMessage="Vui lòng nhập tên trang web")]
        public String SiteTitle { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập SĐT hỗ trợ")]
        [RegularExpression("^[0][0-9]{9,10}", ErrorMessage = "Số điện thoại nhập vào không hợp lệ")]
        public String Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email hỗ trợ")]
        [EmailAddress(ErrorMessage="Email nhập vào không hợp lệ")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng sản phẩm trong 1 trang")]
        [Range(5,1000, ErrorMessage = "Số lượng hiển thị trong 1 trang phải ít nhất 5 đến 1000 sản phẩm")]
        public int ProductPerPage { get; set; }
    }
}