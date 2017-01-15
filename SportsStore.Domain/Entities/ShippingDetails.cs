using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        [Display(Name = "Line 1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Line 2")]
        public string AddressLine2 { get; set; }
        [Display(Name = "Line 3")]
        public string AddressLine3 { get; set; }

        [Required(ErrorMessage = "Please enter city name")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter state name")]
        public string State { get; set; }

        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Please enter country name")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}
