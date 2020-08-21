using System.ComponentModel.DataAnnotations;

namespace qlkdst.Models
{
    public class LoginModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng nhập user name")]
        public string username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập password")]
        public string password { get; set; }

        public string userId { get; set; }
    }
}