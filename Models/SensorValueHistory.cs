using System.ComponentModel.DataAnnotations;
namespace DashboardData.Models;

public class SensorValueHistory
{
    [Key]
    public int Id { get; set; } 
    public double MeasuredValue { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    public int SensorDataId { get; set; }
    public SensorData SensorData { get; set; }
}