using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeCET.Models
{
    public class Kurs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KursID { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [StringLength(75)]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        public int Trajanje { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        public decimal Cena { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        public string Opis { get; set; }

        [ForeignKey("Oblast")]
        public int? OblastID { get; set; }
        public Oblast Oblast { get; set; }

        public ICollection<PredavacKurs> PredavacKurs { get; set; }
    }


}