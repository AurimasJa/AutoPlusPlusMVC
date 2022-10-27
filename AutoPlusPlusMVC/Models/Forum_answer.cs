using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPlusPlusMVC.Models
{
    public class Forum_answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_Forum_answer { get; set; }
        [Required]
        public string text { get; set; }
        [Required]
        public DateTime date { get; set; }

        [ForeignKey("fk_Userid_User")]
        public User fk_User { get; set; }

        [ForeignKey("fk_Forum_topicid_Forum_topic")]
        public Forum_topic fk_Forum_topic { get; set; }

    }
}
