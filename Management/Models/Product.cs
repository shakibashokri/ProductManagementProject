using System;
using System.ComponentModel.DataAnnotations;

namespace Management.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The IsAvailable field is required.")]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "The ManufactureEmail field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string ManufactureEmail { get; set; }

        [Required(ErrorMessage = "The ManufacturePhone field is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string ManufacturePhone { get; set; }

        [Required(ErrorMessage = "The ProduceDate field is required.")]
        public DateTime ProduceDate { get; set; }
        
    }

}
