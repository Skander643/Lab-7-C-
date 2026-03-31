using System.ComponentModel.DataAnnotations;
using System.Data;
namespace DashboardData.Models
{
    public class SensorData    {

        [Key]
        public int Id { get; set; }

        [Required] [StringLength(50)]
        public string Name { get; set; }

        public string Type { get; set; } = "Temperature";
        public double Value { get; set; }

        public DateTime LastUpdate {get;set;} = DateTime.Now;

        public int? LocationId { get; set; }
        public Location Location { get; set; }

        public ICollection<Tag> Tags {get;set;} = new List<Tag>();

        public ICollection<SensorValueHistory> Values {get;set;} = new List<SensorValueHistory>();
    }
}
