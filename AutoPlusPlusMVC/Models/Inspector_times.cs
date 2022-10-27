using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPlusPlusMVC.Models
{
    public class Inspector_times
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_Inspector_times { get; set; }
        [Required]

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime interval_start { get; set; }
        [Required]
        public DateTime interval_end { get; set; }
        [Required]
        public float inspection_price { get; set; }
        [ForeignKey("fk_Userid_User")]
        public User fk_User { get; set; }
        public int fk_Userid_User { get; set; }

    }
}
