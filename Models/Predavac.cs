using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeCET.Models
{
    public class Predavac
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PredavacID { get; set; }

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
        [StringLength(255)]
        public string Zvanje { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [StringLength(255)]
        public string OblastRada { get; set; }

        public bool Odobrenje {  get; set; }

        public ICollection<PredavacKurs> PredavacKurs { get; set; }
    }

}