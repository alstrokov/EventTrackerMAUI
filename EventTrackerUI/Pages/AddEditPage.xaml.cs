using EventTrackerUI.Models;

namespace EventTrackerUI.Pages;

public partial class AddEditPage : ContentPage
{
    private EventRecord _eventRecord = null;
    private bool isEditMode = false;
    private Action<EventRecord> _addNewEventRecord;
    private Action<EventRecord> _deleteEventRecord;


    public AddEditPage(EventRecord eventRecord, Action<EventRecord> deleteEventRecord)
    {
        InitializeComponent();

        _eventRecord = eventRecord;
        _deleteEventRecord = deleteEventRecord;
        isEditMode = true;
        EntryTitle.Text = eventRecord.Title;
        EntryTags.Text = eventRecord.Tags;
        EditorText.Text = eventRecord.Text;

        btnAdd.IsVisible = false;
        btnSave.IsVisible = false;
        btnDelete.IsVisible = true;
    }

    public AddEditPage(Action<EventRecord> addNewEventRecord)
    {
        InitializeComponent();

        _addNewEventRecord = addNewEventRecord;
        isEditMode = false;

        btnAdd.IsVisible = true;
        btnSave.IsVisible = false;
        btnDelete.IsVisible = false;
        btnCancel.IsVisible = true;
    }

    private async void AddButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();

        EventRecord eventRecord = new()
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            Title = EntryTitle.Text,
            Text = EditorText.Text,
            Tags = EntryTags.Text
        };

        _addNewEventRecord(eventRecord);
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        _eventRecord.Title = EntryTitle.Text;
        _eventRecord.Text = EditorText.Text;
        _eventRecord.Tags = EntryTags.Text;

        await Navigation.PopAsync();
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();

        _deleteEventRecord(_eventRecord);
    }

    private void Title_TextChanged(object sender, TextChangedEventArgs e)
    {
        SetDirtyFlag();
    }

    private void Text_TextChanged(object sender, TextChangedEventArgs e)
    {
        SetDirtyFlag();
    }


    private void EntryTags_TextChanged(object sender, TextChangedEventArgs e)
    {
        SetDirtyFlag();
    }

    private void SetDirtyFlag()
    {
        if (isEditMode)
        {
            btnCancel.IsVisible = true;
            btnSave.IsVisible = true;
        }
    }
}