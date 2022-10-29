using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Datalayer
{
    public class Product_GroupMetaData
    {
        [Key]
        public int GroupId { get; set; }


        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(250)]
        public string GroupTitle { get; set; }

        [Display(Name = "عنوان سرگروه")]
        public Nullable<int> ParentId { get; set; }
    }


    [MetadataType(typeof(Product_GroupMetaData))]
    public partial class Product_Groups
    {

    }
}
