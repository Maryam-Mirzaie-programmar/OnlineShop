using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer
{
    public class Product_TagsMetaData
    {
        [Key]
        public int ProductTagID { get; set; }

        [Display(Name = "محصول")]
        public int ProductID { get; set; }

        [Display(Name = "کلمات کلیدی")]
        [MaxLength(250)]
        public string ProductTag { get; set; }
    }


    [MetadataType(typeof(Product_TagsMetaData))]
    public partial class Product_Tags
    {
    }
}
