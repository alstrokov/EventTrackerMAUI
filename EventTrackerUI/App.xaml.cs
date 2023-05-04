namespace EventTrackerUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Activated += Window_Activated;

        return window;
    }

    private void Window_Activated(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("[Window] activated");
    }
}
