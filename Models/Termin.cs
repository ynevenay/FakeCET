using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeCET.Models
{
    public class Termin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TerminID { get; set; }


        public int KursID { get; set; }
        public int PredavacID { get; set; }

        [ForeignKey("KursID, PredavacID")]
        public PredavacKurs PredavacKurs { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        public DateTime? DatumPocetka { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        public DateTime? DatumZavrsetka { get; set; }
        [Required]
        public bool Status { get; set; } = false;

        [ForeignKey("MestoOdrzavanja")]
        public int MestoID { get; set; }
        public MestoOdrzavanja MestoOdrzavanja { get; set; }

        public ICollection<PrijavaKurs> PrijavaKurs { get; set; }
    }

}