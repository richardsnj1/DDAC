using System.ComponentModel.DataAnnotations;

namespace DDAC.Models
{
    public class Enroll
    {
        [Key]
        public int enrollID { get; set; }

        [Required]
        [Display(Name = "Child Name: ")]
        public string childName { get; set; }

        [Required]
        [Display(Name = "Parent ID")]
        public string parentID { get; set; }
    }
}
