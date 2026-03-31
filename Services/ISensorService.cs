using DashboardData.Models;
using Microsoft.EntityFrameworkCore;
namespace DashboardData.Services
{
    public interface ISensorService
    {
        Task<List<SensorData>> GetSensorsAsync();
        Task AddSensorAsync(SensorData sensorData);
        Task<List<SensorData>> GetCriticalSensorsAsync(double threshold);

        Task<int> GetTotalCountAsync();
        Task<double> GetAverageValueAsync();
        Task<double> GetMaxValueAsync();

    }

    public class SensorService : ISensorService
    {

        private readonly AppDbContext _context;

        public SensorService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<SensorData>> GetSensorsAsync()
        {
            // EF Core translates Include into a SQL JOIN to the Location table
            return await _context.Sensors
                .Include(s => s.Location)
                .ToListAsync();
        }
        public async Task AddSensorAsync(SensorData sensor)
        {
            // 1. Prepare the addition in memory
            _context.Sensors.Add(sensor);

            // 2. Validate the transaction (Generates the SQL INSERT INTO)
            await _context.SaveChangesAsync();
        }

        public async Task<List<SensorData>> GetCriticalSensorsAsync(double threshold)
        {
            return await _context.Sensors
                .Include(s => s.Location)
                .Where(s => s.Value > threshold) // Translated to: WHERE Value > @threshold
                .OrderByDescending(s => s.Value) // Translated to: ORDER BY Value DESC
                .ToListAsync();                  // Triggers SQL execution
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Sensors.CountAsync();
        }

        public async Task<double> GetAverageValueAsync()
        {
            return await _context.Sensors.AverageAsync(s => s.Value);
        }

        public async Task<double> GetMaxValueAsync()
        {
            return await _context.Sensors.MaxAsync(s => s.Value);
        }


    }


}
