using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage="Vui lòng nhập họ tên")]
        [MaxLength(50,ErrorMessage="Đặt tên quá dài")]
        //[RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$",ErrorMessage="Tên không hợp lệ")]
        public String FullName { get; set; }


        [Required(ErrorMessage="Email không được để trống!")]
        [EmailAddress(ErrorMessage="Email vừa nhập không hợp lệ!")]
        public String Email { get; set; }

        [Required(ErrorMessage="Vui lòng nhập mật khẩu")]
        [MinLength(6,ErrorMessage="Mật khẩu phải lớn hơn 6 ký tự!")]
        [MaxLength(16,ErrorMessage="Mật khẩu không được quá 16 ký tự!")]
        public String Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [System.Web.Mvc.Compare("Password",ErrorMessage="Mật khẩu không giống nhau!")]
        public String ConfirmPassword { get; set; }
        public String Captcha { get; set; }
    }
}