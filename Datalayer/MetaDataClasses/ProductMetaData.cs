using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Datalayer
{
    public class ProductMetaData
    {
        [Key]
        public int ProductID { get; set; }

        [Display(Name = "عنوان محصول")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(250)]
        public string ProductTitle { get; set; }

        [Display(Name = "توضیح مختصر")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(500)]
        public string ProductShortDescription { get; set; }


        [Display(Name = "متن")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [AllowHtml]
        public string ProductText { get; set; }

        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        public int ProductPrice { get; set; }

        [Display(Name = "تصویر")]
        public string ProductImageName { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public System.DateTime ProductCreateDate { get; set; }
    }

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {

    }
}
