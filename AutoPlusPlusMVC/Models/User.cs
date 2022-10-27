using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPlusPlusMVC.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_User { get; set; }

        [Required]
        [Display(Name = "Vardas")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Pavardė")]
        public string surname { get; set; }
        [Required]
        [Display(Name = "Slaptažodis")]
        public string password { get; set; }
        [Required]
        [Display(Name = "E-paštas")]
        public string email { get; set; }
        [Required]
        [Display(Name = "Mietas")]
        public string city { get; set; }
        [Required]
        [Display(Name = "Gimimo data")]
        public DateTime date_of_birth { get; set; }
        [Required]
        [Display(Name = "Rolė")]
        public Global.type type { get; set; }
        [Required]
        [Display(Name = "Likutis")]
        public double balance { get; set; }
        [Required]
        [Display(Name = "Tel. numeris")]
        public string phone_number { get; set; }
        [Required]
        [Display(Name = "Galimybės")]
        public Global.abilities abilities { get; set; }
        public ICollection<Inspector_times> times { get; set; }
    }
}
