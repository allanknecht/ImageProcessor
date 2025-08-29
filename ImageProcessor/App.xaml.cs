namespace ImageProcessor;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Usa o Shell como janela principal
        MainPage = new AppShell();
    }
}
