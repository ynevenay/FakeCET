using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;

namespace FakeCET.Models
{
    public class MestoOdrzavanja
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MestoID { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [StringLength(55)]
        public string Naziv { get; set; }

        public ICollection<Termin> Termin { get; set; }
    }
}