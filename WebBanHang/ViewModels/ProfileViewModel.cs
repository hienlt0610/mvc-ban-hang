using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.ViewModels
{
    public class ProfileViewModel
    {
        public int CustomerID { get; set; }

        [Display(Name = "Mật khẩu")]
        [StringLength(16,MinimumLength=6,ErrorMessage= "Mật khẩu từ {2} đến {1} ký tự")]
        public string Passwrord { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Mật khẩu từ {2} đến {1} ký tự")]
        public string NewPasswrord { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Mật khẩu từ {2} đến {1} ký tự")]
        public string ConfirmPasswrord { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên đầy đủ")]
        [StringLength(50,ErrorMessage="Tên không được dài quá {1} ký tự")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [Display(Name = "Địa chỉ")]
        [StringLength(100, ErrorMessage = "Địa chỉ không được dài quá {1} ký tự")]
        public string Address { get; set; }

        [Display(Name = "Tỉnh/Thành")]
        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành")]
        public int ProvinceID { get; set; }

        [Display(Name = "Quận/Huyện")]
        [Required(ErrorMessage = "Vui lòng chọn Quận/Huyện")]
        public int DistrictID { get; set; }

        [Display(Name = "Phường/Xã")]
        [Required(ErrorMessage = "Vui lòng chọn Phường/Xã")]
        public int WardID { get; set; }

        [Required(ErrorMessage="Vui lòng nhập số điện thoại")]
        [Display(Name="Số ĐT")]
        [RegularExpression("^[0][0-9]{9,10}",ErrorMessage="Số điện thoại nhập vào không hợp lệ")]
        public string Phone { get; set; }
    }
}