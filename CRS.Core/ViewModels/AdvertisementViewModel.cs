using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Core.ViewModels
{
    public class AdvertisementViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
        public UserViewModel Advertiser { get; set; }
        //public CarViewModel Car { get; set; }

    }
}
