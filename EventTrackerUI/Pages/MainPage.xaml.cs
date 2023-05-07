using System.Collections.ObjectModel;
using System.Diagnostics;
using EventTrackerUI.Helpers;
using EventTrackerUI.Models;
using EventTrackerUI.Pages;
using EventTrackerUI.Services;

namespace EventTrackerUI;

public partial class MainPage : ContentPage
{
    private const string AppName = "EventTracker";
    private ObservableCollection<EventRecord> _eventsFiltered { get; set; }
    private ObservableCollection<EventRecord> _events { get; set; }

    public MainPage()
    {
        InitializeComponent();

        _events = new ObservableCollection<EventRecord>(Persistence.Load());
        lvEvents.BindingContext = _events;
        UpdateTitle();
    }


    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Persistence.Store(_events.ToList());
        Debug.WriteLine(">>>[MainPage] OnDisappearing, store data");
    }

    private async void AddEventButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine(">>>[MainPage] add event button clicked");
        await Navigation.PushAsync(new AddEditPage(AddNewEventRecord));
    }

    private async void lvEvents_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is EventRecord selectedEvent)
        {
            Debug.WriteLine($">>>[MainPage] (Event selected) `{selectedEvent}`");
            lvEvents.SelectedItem = null;
            await Navigation.PushAsync(new AddEditPage(selectedEvent, DeleteEventRecord));
        }
        else
        {
            Debug.WriteLine($">>>[MainPage] (Event selected) cannot convert");
        }
    }

    private void AddNewEventRecord(EventRecord record)
    {
        HideSearch();

        var list = _events.ToList();

        list = list.Reorder();
        list.Insert(0, record);

        _events = new ObservableCollection<EventRecord>(list);
        lvEvents.BindingContext = _events;

        Persistence.Store(_events.ToList());
        UpdateTitle();
    }

    private void DeleteEventRecord(EventRecord record)
    {
        _events.Remove(record);

        Persistence.Store(_events.ToList());
        UpdateTitle();
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        EdSearch.IsVisible = !EdSearch.IsVisible;
        BtnHide.IsVisible = !BtnHide.IsVisible;
    }

    private void EdSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        _eventsFiltered = new ObservableCollection<EventRecord>(_events
            .Where(ev => ev.Text.Contains(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase)).ToList());
        lvEvents.BindingContext = _eventsFiltered;

        UpdateTitle();
    }

    private void HideButton_Clicked(object sender, EventArgs e)
    {
        HideSearch();
    }

    private void HideSearch()
    {
        EdSearch.Text = "";
        EdSearch.IsVisible = false;
        BtnHide.IsVisible = false;
        _eventsFiltered = null;
    }

    private void UpdateTitle()
    {
        Title = $"{AppName} {_eventsFiltered?.Count ?? _events.Count}/{_events.Count}";
    }
}
