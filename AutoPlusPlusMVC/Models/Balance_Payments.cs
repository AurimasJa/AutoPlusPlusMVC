using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPlusPlusMVC.Models
{
    public class Balance_payments
    {
        [Key]
        public int id_Balance_payments { get; set; }
        [Required]
        public double sum { get; set; }
        [ForeignKey("fk_Userid_User")]
        public User User { get; set; }

       
    }
}
