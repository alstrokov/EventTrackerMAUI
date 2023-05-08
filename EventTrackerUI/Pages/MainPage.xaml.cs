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
        UpdateAppTitle();
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
        UpdateAppTitle();
    }

    private void DeleteEventRecord(EventRecord record)
    {
        _events.Remove(record);

        Persistence.Store(_events.ToList());
        UpdateAppTitle();
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        EdSearch.IsVisible = !EdSearch.IsVisible;
        BtnHide.IsVisible = !BtnHide.IsVisible;
    }

    private void EdSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchFullText = e.NewTextValue;
        var tokens = searchFullText.Split(null);
        List<string> tags = new();

        foreach (var token in tokens)
        {
            if (token.StartsWith("#"))
            {
                tags.Add(token);
            }
        }
        foreach (var item in tags)
        {
            searchFullText = searchFullText.Replace($"{item}", "");
        }
        searchFullText = searchFullText.Trim();


        _eventsFiltered = new ObservableCollection<EventRecord>(
            _events.Where(rec => SearchPredicate(rec, searchFullText, tags)).ToList());
        lvEvents.BindingContext = _eventsFiltered;

        UpdateAppTitle();
    }

    private static bool SearchPredicate(EventRecord record, string searchText, List<string> tags)
    {
        Debug.WriteLine($">>>[{searchText}] tags:{string.Join('-', tags)}");

        bool tagPass = false;

        if (tags.Count > 0)
        {
            if (record.Tags != null)
            {
                string[] recordTags = record.Tags.Split(null);

                foreach (var tag in tags)
                {
                    if (recordTags.Any(t => string.Compare(t, tag.Replace("#", ""), true) == 0))
                    {
                        tagPass = true;
                        break;
                    }
                }
            }
        }
        else
        {
            tagPass = true;
        }

        bool pu = (record.Text.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)
            || record.Title.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            && tagPass;

        Debug.WriteLine($">>>result[{pu}]");
        return
            (record.Text.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)
            || record.Title.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            && tagPass;
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
        lvEvents.BindingContext = _events;
        UpdateAppTitle();
    }

    private void UpdateAppTitle()
    {
        Title = $"{AppName} {_eventsFiltered?.Count ?? _events.Count}/{_events.Count}";
        _eventsFiltered?.Recalculate();
    }
}
