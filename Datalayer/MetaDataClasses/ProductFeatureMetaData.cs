using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Datalayer
{
    public class ProductFeatureMetaData
    {
        [Key]
        public int ProductFeatureId { get; set; }

        [Display(Name = "ویژگی")]
        public int FeatureId { get; set; }

        [Display(Name = "محصول")]
        public int ProductID { get; set; }

        [Display(Name = "مقدار")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(150)]
        public string Value { get; set; }
    }

    [MetadataType(typeof(ProductFeatureMetaData))]
    public partial class ProductFeature
    {

    }
}
