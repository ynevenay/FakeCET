using FakeCET.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeCET.ViewModels
{
    public class TerminViewModel
    {
        public Termin Termin { get; set; }

        [Display(Name = "Kurs")]
        public int KursID { get; set; }
        public IEnumerable<Kurs> Kursevi { get; set; }

        [Display(Name = "Predavač")]
        public int PredavacID { get; set; }
        public IEnumerable<Predavac> Predavaci { get; set; }

        [Display(Name = "Mesto održavanja")]
        public int MestoID { get; set; }
        public IEnumerable<MestoOdrzavanja> MestaOdrzavanja { get; set; }

        public string KursNaziv { get; set; }
        public string PredavacIme { get; set; }
        public string PredavacPrezime { get; set; }
        public string MestoNaziv { get; set; }
    }
}
