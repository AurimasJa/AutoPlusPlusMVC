using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPlusPlusMVC.Models
{
    public class Forum_topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_Forum_topic { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public DateTime creation_date { get; set; }
        [Required]
        public string text { get; set; }
        [Required]
        public int bumped { get; set; }

        [ForeignKey("fk_Userid_User")]
        public User fk_User { get; set; }

        public ICollection<Forum_answer> answers { get; set; }

    }
}
