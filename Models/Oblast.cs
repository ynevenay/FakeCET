using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeCET.Models
{
    public class Oblast
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OblastID { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [StringLength(55)]
        public string Naziv { get; set; }

        public ICollection<Kurs> Kursevi { get; set; }
    }

}