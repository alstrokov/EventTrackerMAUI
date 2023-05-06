using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace EventTrackerUI.Models
{
    public class EventRecord : INotifyPropertyChanged
    {
        public EventRecord() { }

        public EventRecord(string DataToParse)
        {
            if (!string.IsNullOrEmpty(DataToParse))
            {
                FromStore(DataToParse);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private DateOnly _date;

        public DateOnly Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public string TimeTo => $"{GetTimeTo()}";

        private string GetTimeTo()
        {
            int days = DateOnly.FromDateTime(DateTime.Now.Date).DayNumber - _date.DayNumber;

            if (days > 365)
            {
                return $"{days / 365}y ago";
            }
            else if (days > 30)
            {
                return $"{days / 30}m ago";
            }
            else if (days == 0)
            {
                return "Today";
            }
            else
            {
                return $"{days}d ago";
            }
        }

        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"[EventRecord] @{Date.ToShortDateString()} {Title}-{Text}";
        }

        public string ToStore()
        {
            return $"{Date.ToShortDateString()}|{Title}|{Text}";
        }

        public void FromStore(string StringToParse)
        {
            var tokens = StringToParse.Split('|');

            if (tokens.Length != 3) return;

            _date = DateOnly.Parse(tokens[0]);
            _title = tokens[1];
            _text = tokens[2];
        }
    }
}
