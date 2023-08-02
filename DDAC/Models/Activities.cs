using System.ComponentModel.DataAnnotations;

namespace DDAC.Models
{
    public class Activities
    {
        [Key]
        public int activityId { get; set; }

        [Required]
        [Display(Name = "Activity Name")]
        public string activityName { get; set; }

        [Required]
        [Display(Name = "Activity Date")]
        [DataType(DataType.Date)]
        public string date { get; set; }
    }
}
