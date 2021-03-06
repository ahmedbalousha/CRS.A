using CRS.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Core.Dtos
{
   public class CreateAdvertisementDto
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "عنوان الاعلان")]
        public string Title { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "تفاصيل الإعلان")]
        public string Body { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "صورة الاعلان")]
        public IFormFile Image { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Url]
        [Display(Name = "الموقع ")]
        public string WebsiteUrl { get; set; }
        [Display(Name = "البداية ")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "النهاية ")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "صاحب الاعلان ")]
        public string AdvertiserId { get; set; }
        public int? CarId { get; set; }
    }
}
