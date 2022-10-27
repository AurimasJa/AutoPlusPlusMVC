using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPlusPlusMVC.Models
{
    public class Photo
    {
        [Key]
        public int id_Photo { get; set; }
        [Required]
        public string path { get; set; }
        [ForeignKey("fk_Listingid_Listing")]
        public Listing Listing { get; set; }

       
    }
}
