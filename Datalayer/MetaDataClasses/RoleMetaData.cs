using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Datalayer
{
    public class RoleMetaData
    {
        [Key]
        public int RoleId { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(150)]
        public string RoleTitle { get; set; }

        [Display(Name = "عنوان سیستمی نقش")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(150)]
        public string RoleName { get; set; }
    }


    [MetadataType(typeof(RoleMetaData))]
    public partial class Role
    {

    }
}
