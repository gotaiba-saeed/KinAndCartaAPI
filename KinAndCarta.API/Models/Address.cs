using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KinAndCarta.API.Models
{
    public class Address
    {
        public int id { get; set; }       
        public string city { get; set; }
        [Required]
        public string street { get; set; }
        public string state { get; set; }
    }
}