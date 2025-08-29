namespace ImageProcessor.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnProcessImagesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ImageTabsView));
    }
}
