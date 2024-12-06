using System.Collections.Generic;
using FakeCET.Models;

namespace FakeCET.ViewModels
{
    public class DodeliPredavacaViewModel
    {
        public int PredavacID { get; set; }
        public int KursID { get; set; }
        public List<Predavac> Predavaci { get; set; }
        public List<Kurs> Kursevi { get; set; }
        public string NazivKursa { get; set; } // Assuming you might need this based on previous interactions
    }
}
