using EventTrackerUI.Models;

namespace EventTrackerUI.Pages;

public partial class AddEditPage : ContentPage
{
    private EventRecord _eventRecord = null;
    private Action<EventRecord> _addNewEventRecord;


    public AddEditPage(EventRecord eventRecord)
    {
        InitializeComponent();

        _eventRecord = eventRecord;
        eTitle.Text = eventRecord.Title;
        eText.Text = eventRecord.Text;

        btnAdd.IsVisible = false;
        btnSave.IsVisible = true;
    }

    public AddEditPage(Action<EventRecord> addNewEventRecord)
    {
        InitializeComponent();

        _addNewEventRecord = addNewEventRecord;

        btnAdd.IsVisible = true;
        btnSave.IsVisible = false;
    }

    private async void AddButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();

        EventRecord eventRecord = new()
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            Title = eTitle.Text,
            Text = eText.Text,
        };

        _addNewEventRecord(eventRecord);
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        _eventRecord.Title = eTitle.Text;
        _eventRecord.Text = eText.Text;

        await Navigation.PopAsync();
    }
}