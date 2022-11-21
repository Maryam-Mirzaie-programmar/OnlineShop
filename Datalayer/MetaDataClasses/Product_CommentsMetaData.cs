using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Datalayer.MetaDataClasses
{
    public class Product_CommentsMetaData
    {
        [Key]
        public int CommentId { get; set; }

        [Display(Name = "محصول")]
        public int ProductID { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(150)]
        public string Name { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(250)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "وبسایت")]
        [MaxLength(350)]
        [Url]
        public string Website { get; set; }


        [Display(Name = "متن نظر")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(800)]
        public string Comment { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public System.DateTime CreateDate { get; set; }


        public Nullable<int> ParentId { get; set; }
    }

    public partial class Product_Comments
    {
    }
}
