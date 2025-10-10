using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Microcharts.Maui;
using ImageProcessor.Services;
using ImageProcessor.ViewModels;

namespace ImageProcessor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMicrocharts() 
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register services
            builder.Services.AddSingleton<IImageProcessingService, ImageProcessingService>();
            builder.Services.AddSingleton<IImageSelectionService, ImageSelectionService>();
            builder.Services.AddSingleton<IImageSaveService, ImageSaveService>();
            builder.Services.AddSingleton<IConvolutionService, ConvolutionService>();
            builder.Services.AddTransient<ImageProcessingViewModel>();
            builder.Services.AddTransient<ConvolutionViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}