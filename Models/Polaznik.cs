using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeCET.Models
{
    public class Polaznik
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PolaznikID { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [StringLength(25)]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]

        [StringLength(25)]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [StringLength(12)]
        public string KontaktTelefon { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [EmailAddress]
        [StringLength(100)]
        public string MejlAdresa { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Lozinka mora imati najmanje 6 karaktera.")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [Compare("Lozinka", ErrorMessage = "Lozinke se ne podudaraju.")]
        [DataType(DataType.Password)]
        public string PotvrdiLozinku { get; set; }


        public ICollection<PrijavaKurs> PrijavaKurs { get; set; }
    }

}