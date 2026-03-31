using DashboardData.Models;
using Microsoft.EntityFrameworkCore;
namespace DashboardData.Services
{
    public interface ISensorService
    {
        Task<List<SensorData>> GetSensorsAsync();
        Task AddSensorAsync(SensorData sensor);
        Task<List<SensorData>> GetCriticalSensorsAsync(double threshold);

        Task<int> GetTotalCountAsync();
        Task<double> GetAverageValueAsync();
        Task<double> GetMaxValueAsync();

        Task<List<Location>> GetLocationsAsync();
        Task<SensorData> GetSensorByIdAsync(int id);
        Task UpdateSensorAsync(SensorData sensor);
        Task DeleteSensorAsync(int id);

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

       public async Task<List<Location>> GetLocationsAsync()
{
    return await _context.Locations.ToListAsync();
}

public async Task<SensorData> GetSensorByIdAsync(int id)
{
    // FindAsync cherche directement par la Clé Primaire (Id)
    return await _context.Sensors.FindAsync(id);
}

public async Task AddSensorAsync(SensorData sensor)
{
    sensor.LastUpdate = DateTime.Now;
    
    // Historisation de la valeur initiale (TP5)
    sensor.Values.Add(new SensorValueHistory {
        MeasuredValue = sensor.Value,
        Date = DateTime.Now
    });

    _context.Sensors.Add(sensor);
    await _context.SaveChangesAsync();
}

public async Task UpdateSensorAsync(SensorData sensor)
{
    sensor.LastUpdate = DateTime.Now; // Mise à jour de la date
    
    // Ajout à l'historique lors d'une modification (TP5)
    sensor.Values.Add(new SensorValueHistory {
        MeasuredValue = sensor.Value,
        Date = DateTime.Now
    });

    _context.Sensors.Update(sensor);
    await _context.SaveChangesAsync();
}

public async Task DeleteSensorAsync(int id)
{
    var sensor = await _context.Sensors.FindAsync(id);
    if (sensor != null)
    {
        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync();
    }
}

    }


}
