using FakeCET.Models;
using System.Collections.Generic;

namespace FakeCET.ViewModels
{
    public class PredavacApproveViewModel
    {
        public Predavac Predavac { get; set; } // Assuming Predavac is your model class for instructors
        public List<int> SelectedKursevi { get; set; } // List of selected course IDs
        public List<Kurs> AvailableKursevi { get; set; } // List of available courses to choose from
    }
}
