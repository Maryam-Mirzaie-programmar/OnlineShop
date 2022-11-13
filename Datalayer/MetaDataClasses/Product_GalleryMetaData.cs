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
        public int ProductGalleryID { get; set; }

        [Display(Name = "محصول")]
        public int ProductID { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(50)]
        public string GalleryImageName { get; set; }
    }


    [MetadataType(typeof(Product_GalleryMetaData))]
    public partial class Product_Gallery
    {
    }
}
