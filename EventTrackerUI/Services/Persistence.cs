using System.Text;
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

            StringBuilder sb = new();

            foreach (var e in events)
            {
                sb.AppendLine(e.ToStore());
            }

            File.WriteAllText(_filename, sb.ToString());
        }

        internal static List<EventRecord> Load()
        {
            System.Diagnostics.Debug.WriteLine($"[Persistence>Load]\tLoad from [{_filename}]");

            List<EventRecord> records = new();

            if (!File.Exists(_filename))
            {
                return records;
            }

            var data = File.ReadAllLines(_filename);

            foreach (var line in data)
            {
                records.Add(new EventRecord(line));
            }

            System.Diagnostics.Debug.WriteLine($"[Persistence>Load]\tTotal records {records.Count}");
            return records.OrderByDescending(r => r.Date).ToList();
        }
    }
}


