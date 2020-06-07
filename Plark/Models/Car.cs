using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.Models
{
    public class Car
    {
        public long Id { get; set; }
        [Required]
        public string NickName { get; set; }
        [Required]
        public string NumberPlate { get; set; }
    }
}
