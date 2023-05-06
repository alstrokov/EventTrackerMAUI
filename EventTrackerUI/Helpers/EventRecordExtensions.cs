using EventTrackerUI.Models;

namespace EventTrackerUI.Helpers
{
    public static class EventRecordExtensions
    {
        public static List<EventRecord> Reorder(this List<EventRecord> records)
        {
            return records.OrderByDescending(r => r.Date).ToList();
        }
    }
}
