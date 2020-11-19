using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;
using static KinAndCarta.API.Extension;

namespace KinAndCarta.API.Models
{
    public class Contact
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string company { get; set; }
        public string profileImageUri { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string birthdate { get; set; }
        [ValidatePhone]
        [Phone]
        public string workPhone { get; set; }
        [Phone]
        public string personalPhone { get; set; }
        [Required]
        public Address address { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(workPhone) && string.IsNullOrWhiteSpace(workPhone))
            {
                yield return new ValidationResult("Work phone or personal phone must be set");
            }
        }
    }
}