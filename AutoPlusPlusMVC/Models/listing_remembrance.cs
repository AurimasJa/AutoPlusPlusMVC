using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPlusPlusMVC.Models
{
    public class listing_remembrance
    {
        [Key]
        public int id_Listing_remembrance { get; set; }
        public DateTime date { get; set; }
        [ForeignKey("fk_Listingid_Listing")]
        public Listing Listing { get; set; }

        [ForeignKey("fk_Userid_User")]
        public User User { get; set; }

       
    }
}
