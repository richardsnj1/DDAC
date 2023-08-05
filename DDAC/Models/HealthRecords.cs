using System.ComponentModel.DataAnnotations;
namespace DDAC.Models
{
    public class HealthRecords
    {
        [Key]
        public int recordID { get; set; }

        [Required]
        [Display(Name = "Child Name: ")]
        public string childName { get; set; }

        [Required]
        [Display(Name = "Sick Date: ")]
        [DataType(DataType.Date)]
        public string sickDate { get; set; }
    }
}
