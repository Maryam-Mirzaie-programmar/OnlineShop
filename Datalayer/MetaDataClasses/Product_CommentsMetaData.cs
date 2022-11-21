using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Datalayer
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
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "وبسایت")]
        [MaxLength(350)]
        [Url(ErrorMessage = "آدرس وارد شده معتبر نمی باشد")]
        public string Website { get; set; }


        [Display(Name = "متن دیدگاه")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(800)]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public System.DateTime CreateDate { get; set; }


        public Nullable<int> ParentId { get; set; }
    }

    [MetadataType(typeof(Product_CommentsMetaData))]
    public partial class Product_Comments
    {
    }
}
