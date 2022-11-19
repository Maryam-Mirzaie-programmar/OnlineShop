using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Datalayer
{
    public class FeatureMetaData
    {
        [Key]
        public int FeatureId { get; set; }

        [Display(Name = "ویژگی")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(150)]
        public string FeatureTitle { get; set; }
    }


    [MetadataType(typeof(FeatureMetaData))]
    public partial class Feature
    {
    }
}
