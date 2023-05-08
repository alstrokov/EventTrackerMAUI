using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using EventTrackerUI.Models;

namespace EventTrackerUI.Helpers
{
    public static class EventRecordExtensions
    {
        public static List<EventRecord> Reorder(this List<EventRecord> records)
        {
            return records.OrderByDescending(r => r.Date).ToList();
        }

        public static List<EventRecord> Recalculate(this List<EventRecord> records)
        {
            for(int i = 0; i < records.Count -1; i++)
            {
                records[i].TimeToPrev =
                    $"+{Helper.DaysDiffToString(Helper.GetDaysBetweenDates(records[i].Date, records[i + 1].Date))}";
            }

            return records.OrderByDescending(r => r.Date).ToList();
        }

        public static ObservableCollection<EventRecord> Recalculate(this ObservableCollection<EventRecord> records)
        {
            for (int i = 0; i < records.Count - 1; i++)
            {
                records[i].TimeToPrev =
                    $"+{Helper.DaysDiffToString(Helper.GetDaysBetweenDates(records[i].Date, records[i + 1].Date))}";
            }

            return records;
        }
    }
}
