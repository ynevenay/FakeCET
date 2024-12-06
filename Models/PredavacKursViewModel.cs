using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakeCET.Models
{
    public class PredavacKursViewModel
    {
        public List<PredavacKursViewModel> PredavacKursevi { get; set; }
        public Predavac Predavac { get; set; }
        public Kurs Kurs { get; set; }
    }
}