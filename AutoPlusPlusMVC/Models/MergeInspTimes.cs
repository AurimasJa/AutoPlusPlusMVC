using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPlusPlusMVC.Models
{
    public class MergeInspTimes
    {
        public List<Inspection> inspections { get; set; }
        public List<Inspector_times> inspector_Times { get; set; }
        public Inspector_times inspector { get; set; }

    }
}
