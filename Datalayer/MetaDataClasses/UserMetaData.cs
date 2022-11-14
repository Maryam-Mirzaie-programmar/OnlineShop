using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Datalayer
{
    public class UserMetaData
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "نقش")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        public int RoleId { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(100)]
        public string Username { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(100)]
        public string Password { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(250)]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string Email { get; set; }

        [Display(Name = "کد فعالسازی")]
        //[Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(50)]
        public string ActrivationCode { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "تاریخ ثبتنام")]
        [DisplayFormat(DataFormatString = "{0 : yyyy/MM/dd}")]
        public System.DateTime RegisterDate { get; set; }
    }

    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {

    }
}
