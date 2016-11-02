using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage="Vui lòng nhập họ tên")]
        [Display(Name="Họ và tên")]
        public String FullName { get; set; }

        [Required(ErrorMessage="Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage="Email không hợp lệ")]
        public String Email { get; set; }

        [Required(ErrorMessage="Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression("^[0][0-9]{9,10}", ErrorMessage = "Số điện thoại nhập vào không hợp lệ")]
        public String Phone { get; set; }

        [Required(ErrorMessage="Vui lòng nhập nội dung liên hệ")]
        [Display(Name = "Nội dung")]
        public String Message { get; set; }
    }
}