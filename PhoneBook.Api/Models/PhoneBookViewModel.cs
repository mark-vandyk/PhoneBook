using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Api.Models
{
    public class PhoneBookViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Book Name")]
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Name { get; set; }

        public int Entries { get; set; }
    }
}
