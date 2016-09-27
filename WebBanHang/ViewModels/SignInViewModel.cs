using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.ViewModels
{
    public class SignInViewModel
    {
        public String FacebookID { get; set; }
        public String GoogleID { get; set; }

        [Required(ErrorMessage="Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage="Email không chính xác")]
        public String Email { get; set; }

        [Required(ErrorMessage="Vui lòng nhập mật khẩu")]
        [MinLength(6,ErrorMessage="Mật khẩu không được dưới 6 ký tự")]
        [MaxLength(16, ErrorMessage="Mật khẩu không quá 16 ký tự")]
        public String Password { get; set; }

        public bool Remember { get; set; }
    }
}