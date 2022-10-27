using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPlusPlusMVC.Models
{
    public class Listing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_Listing { get; set; }
        [Required]
        public DateTime listing_creation_date { get; set; }
        [Required]
        public string fuel_type { get; set; }
        [Required]
        public double mileage { get; set; }
        [Required]
        public string vin { get; set; }
        [Required]
        public string engine_displacement { get; set; }
        [Required]
        public string model { get; set; }
        [Required]
        public string make { get; set; }
        [Required]
        public int number_of_doors { get; set; }
        [Required]
        public string gearbox { get; set; }
        [Required]
        public string body_type { get; set; }
        [Required]
        public DateTime year { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public int drive_wheels { get; set; }
        [Required]
        public string power { get; set; }
        [Required]
        public string color { get; set; }
        [Required]
        public int number_of_seats { get; set; }
        [Required]
        public string steering_wheel_position { get; set; }
        [Required]
        public string first_registration_country { get; set; }
        [Required]
        public double co2_emissions { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string phone_number { get; set; }
        [Required]
        public bool bumped { get; set; }
        
        [Required]
        public int fk_Userid_User { get; set; }

        public ICollection<Photo> photos { get; set; }
        public Inspection inspection { get; set; }

        public ICollection<listing_remembrance> remembrances { get; set; }
    }
}
