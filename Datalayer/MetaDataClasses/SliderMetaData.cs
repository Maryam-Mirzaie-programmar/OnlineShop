using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Datalayer
{
    public class SliderMetaData
    {
        [Key]
        public int SliderId { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد نمایید")]
        [MaxLength(250)]
        public string Title { get; set; }

        [Display(Name = "تصویر")]
        public string ImageName { get; set; }

        [Display(Name = "تاریخ شروع")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}" , ApplyFormatInEditMode = true)]
        public System.DateTime StartDate { get; set; }

        [Display(Name = "تاریخ پایان")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime EndDate { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        [Display(Name = "url")]
        [MaxLength(450)]
        [Url(ErrorMessage = "آدرس وارد شده معتبر نمی باشد")]
        public string Url { get; set; }
    }

    [MetadataType(typeof(SliderMetaData))]
    public partial class Slider
    {

    }
}
