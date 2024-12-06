using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeCET.Models
{
    public class PredavacKurs
    {
        [Key, Column(Order = 0)]
        public int KursID { get; set; }
        public Kurs Kurs { get; set; }

        [Key, Column(Order = 1)]
        public int PredavacID { get; set; }
        public Predavac Predavac { get; set; }

        public ICollection<Termin> Termin { get; set; }
    }

}