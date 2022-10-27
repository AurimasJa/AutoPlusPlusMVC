using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPlusPlusMVC.Models
{
    public class MergePhotosListings
    {
        public Listing listing { get; set; }
        public List<Photo> photos { get; set; }
    }
}
