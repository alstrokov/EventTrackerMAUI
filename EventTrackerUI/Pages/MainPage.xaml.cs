using System.Collections.ObjectModel;
using System.Diagnostics;
using EventTrackerUI.Models;
using EventTrackerUI.Pages;
using EventTrackerUI.Services;

namespace EventTrackerUI;

public partial class MainPage : ContentPage
{
    private ObservableCollection<EventRecord> _events { get; set; }

    public MainPage()
    {
        InitializeComponent();

        _events = new ObservableCollection<EventRecord>(Persistence.Load());
        lvEvents.BindingContext = _events;
    }

    private async void AddEventButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("[MP] add event button clicked");
        await Navigation.PushAsync(new AddEditPage(AddNewEventRecord));
    }

    private async void lvEvents_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is EventRecord selectedEvent)
        {
            Debug.WriteLine($"[MP] (Event selected) `{selectedEvent}`");
            lvEvents.SelectedItem = null;
            await Navigation.PushAsync(new AddEditPage(selectedEvent));
        }
        else
        {
            Debug.WriteLine($"[MP] (Event selected) cannot convert");
        }
    }

    private void StoreButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine($"[MP] (Store)");
        Persistence.Store(_events.ToList());
    }

    private void LoadButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine($"[MP] (Load)");
        lvEvents.BindingContext = null;
        _events = new ObservableCollection<EventRecord>(Persistence.Load());
        lvEvents.BindingContext = _events;
    }

    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        _events.Clear();
    }

    private void AddNewEventRecord(EventRecord record)
    {
        _events.Add(record);
    }
}

