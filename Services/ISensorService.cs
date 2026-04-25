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

        Task ReloadSensorAsync(SensorData sensor);

        Task<List<Location>> GetLocationsAsync();
        Task<SensorData> GetSensorByIdAsync(int id);
        Task UpdateSensorAsync(SensorData sensor);
        Task DeleteSensorAsync(int id);

        Task<List<LocationStat>> GetAverageValueByLocationAsync();

        Task<List<SensorData>> SearchSensorsAsync(string? locationName, string? searchText);

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
            sensor.Values.Add(new SensorValueHistory
            {
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
            sensor.Values.Add(new SensorValueHistory
            {
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

        public async Task ReloadSensorAsync(SensorData sensor)
        {
            await _context.Entry(sensor).ReloadAsync();
        }

        public async Task<List<LocationStat>> GetAverageValueByLocationAsync()
        {
            // EF Core traduit ceci en : SELECT Location, AVG(Value) FROM Sensors GROUP BY Location
            return await _context.Sensors
                .Include(s => s.Location)
                .GroupBy(s => s.Location.Name)
                .Select(g => new LocationStat
                {
                    LocationName = g.Key ?? "Inconnu",
                    AverageValue = g.Average(s => s.Value)
                })
                .ToListAsync();
        }

        public async Task<List<SensorData>> SearchSensorsAsync(string? locationName, string? searchText)
        {
            // AsQueryable() prepares a query without executing it
            IQueryable<SensorData> query = _context.Sensors.Include(s => s.Location).AsQueryable();

            // If a location is provided, we add a WHERE to the SQL
            if (!string.IsNullOrEmpty(locationName))
            {
                query = query.Where(s => s.Location.Name == locationName);
            }

            // If text is provided, we add another WHERE (LIKE) to the SQL
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(s => s.Name.Contains(searchText));
            }

            // The SQL execution (SELECT ...) only happens here, with ToListAsync()!
            return await query.ToListAsync();
        }

    }


}
