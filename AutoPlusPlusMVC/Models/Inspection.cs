using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPlusPlusMVC.Models
{
    public class Inspection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_Inspection { get; set; }
        [Required]
        public DateTime order_date { get; set; }
        [Required]
        public DateTime inspection_date { get; set; }
        [ForeignKey("fk_Userid_User")]
        public User fk_Inspector { get; set; }
        [ForeignKey("fk_Userid_User1")]
        public User fk_User { get; set; }
        [ForeignKey("fk_Listingid_Listing")]
        public Listing fk_Listing { get; set; }

    }
}
