using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakeCET.Models
{
    public class PrijavaKursViewModel
    {
        public int PrijavaID { get; set; }
        public int TerminID { get; set; }
        public string NazivKursa { get; set; }
        public string ImePredavaca { get; set; }
        public string PrezimePredavaca { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public bool JePrijavljen { get; set; }
        public string ImePolaznika { get; set; }
        public string PrezimePolaznika { get; set; }
        public int Status { get; set; }

    }

}