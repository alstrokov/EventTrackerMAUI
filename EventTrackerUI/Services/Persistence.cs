using System.Text.Json;
using EventTrackerUI.Models;

namespace EventTrackerUI.Services
{
    internal static class Persistence
    {
        static readonly string _filename = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EventTracker.txt");

        internal static void Store(List<EventRecord> events)
        {
            System.Diagnostics.Debug.WriteLine($"[Persistence>Store]\tSave to [{_filename}]");

            JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

            string output = JsonSerializer.Serialize(events, jsonSerializerOptions);

            File.WriteAllText(_filename, output);
        }

        internal static List<EventRecord> Load()
        {
            System.Diagnostics.Debug.WriteLine($"[Persistence>Load]\tLoad from [{_filename}]");

            List<EventRecord> records = new();

            if (!File.Exists(_filename))
            {
                return records;
            }

            string data = File.ReadAllText(_filename);

            records = JsonSerializer.Deserialize<List<EventRecord>>(data);
            if (records == null)
            {
                return records;
            }

            System.Diagnostics.Debug.WriteLine($"[Persistence>Load]\tTotal records {records.Count}");
            return records.OrderByDescending(r => r.Date).ToList();
        }
    }
}


