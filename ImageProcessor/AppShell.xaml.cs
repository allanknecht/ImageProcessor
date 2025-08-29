using ImageProcessor.Views;

namespace ImageProcessor;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(ImageTabsView), typeof(ImageTabsView));
    }
}
