using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.ViewModels
{
    public class AdminGroupProductViewModel
    {
        public int GroupID { get; set; }
        [Required(ErrorMessage="Vui lòng nhập tên nhóm")]
        public string GroupName { get; set; }
        public string ParentGroupID { get; set; }
        public string Icon { get; set; }
        public int Priority { get; set; }
    }
}