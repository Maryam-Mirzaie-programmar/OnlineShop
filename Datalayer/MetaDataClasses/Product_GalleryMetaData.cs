using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer
{
    public class Product_GalleryMetaData
    {
        [Key]
        public int ProductGalleryId { get; set; }

        [Display(Name = "محصول")]
        public int ProductID { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(50)]
        public string ImageName { get; set; }

        [Display(Name = "عنوان تصویر")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(250)]
        public string ImageTitle { get; set; }
    }


    [MetadataType(typeof(Product_GalleryMetaData))]
    public partial class Product_Gallery
    {
    }
}
