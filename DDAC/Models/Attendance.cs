using System.ComponentModel.DataAnnotations;

namespace DDAC.Models
{
    public class Attendance
    {

        [Key]
        public int attendanceId { get; set; }

        [Required]
        [Display(Name = "Enroll Id")]
        public string enrollId { get; set; }

        [Required]
        [Display(Name = "Attendance Date")]
        [DataType(DataType.Date)]
        public string attendaceDate { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string status { get; set; }
    }

}