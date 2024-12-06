using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeCET.Models
{
    public class PrijavaKurs
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrijavaID { get; set; }
        
        [Key, Column(Order = 1)]
        public int TerminID { get; set; }
        public Termin Termin { get; set; }
        
        [Key, Column(Order = 2)]
        public int PolaznikID { get; set; }
        public Polaznik Polaznik { get; set; }

        [Required]
        public DateTime DatumPrijave { get; set; }

        [Required]
        public int Status { get; set; } = -1;

        public PrijavaKurs()
        {
            DatumPrijave = DateTime.Now;
        }
    }

}