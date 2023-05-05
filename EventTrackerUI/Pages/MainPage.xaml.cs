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

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Persistence.Store(_events.ToList());
        Debug.WriteLine("[MainPage] OnDisappearing, store data");
    }

    private async void AddEventButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("[MainPage] add event button clicked");
        await Navigation.PushAsync(new AddEditPage(AddNewEventRecord));
    }

    private async void lvEvents_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is EventRecord selectedEvent)
        {
            Debug.WriteLine($"[MainPage] (Event selected) `{selectedEvent}`");
            lvEvents.SelectedItem = null;
            await Navigation.PushAsync(new AddEditPage(selectedEvent));
        }
        else
        {
            Debug.WriteLine($"[MainPage] (Event selected) cannot convert");
        }
    }

    private void StoreButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine($"[MainPage] (Store)");
        Persistence.Store(_events.ToList());
    }

    //private void LoadButton_Clicked(object sender, EventArgs e)
    //{
    //    Debug.WriteLine($"[MainPage] (Load)");
    //    lvEvents.BindingContext = null;
    //    _events = new ObservableCollection<EventRecord>(Persistence.Load());
    //    lvEvents.BindingContext = _events;
    //}

    private void AddNewEventRecord(EventRecord record)
    {
        _events.Add(record);
    }
}

