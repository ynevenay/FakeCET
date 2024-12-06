using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FakeCET.Models;

namespace FakeCET.ViewModels
{
    public class KursViewModel
    {
        public int KursID {get; set;}

        [Required(ErrorMessage = "Obavezno polje")]
        [StringLength(75)]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        public int Trajanje { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        public decimal Cena { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        public int? OblastID { get; set; }

        public IEnumerable<Oblast> Oblasti { get; set; }
    }
}
