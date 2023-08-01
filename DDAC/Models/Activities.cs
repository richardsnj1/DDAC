using System.ComponentModel.DataAnnotations;

namespace DDAC.Models
{
    public class Activities
    {
        [Key]
        public int activityId { get; set; }

        public string parentName { get; set; }

        public string activityName { get; set; }

        public DateTime date { get; set; }
    }
}
