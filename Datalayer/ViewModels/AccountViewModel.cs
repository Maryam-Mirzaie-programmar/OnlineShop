using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Datalayer
{
    public class RegisterViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(100)]
        public string Username { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(250)]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "لطفا رمز عبور را در این فیلد تکرار نمایید")]
        public string RePassword { get; set; }
    }


    public class LoginViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(250)]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }

    }
}
