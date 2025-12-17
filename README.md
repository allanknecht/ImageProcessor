# ImageProcessor

Image processing application developed with .NET MAUI featuring various arithmetic operations and convolution filters.

## 📋 About the Project

ImageProcessor is a cross-platform application developed with .NET MAUI that allows performing various image processing operations, including basic arithmetic operations, convolution filters, histogram equalization, and morphological operations.

## ✨ Features

### Arithmetic Operations
- **Addition** - Sum of two images or addition of constant value
- **Subtraction** - Subtraction between two images or subtraction of constant value
- **Multiplication** - Multiplication by scalar value
- **Division** - Division by scalar value
- **Average** - Arithmetic mean between two images
- **Absolute Difference** - Absolute difference between two images
- **Linear Blending** - Linear combination of two images with configurable ratio

### Image Transformations
- **Convert to Grayscale**
- **Image Negative**
- **Flip** - Horizontal and Vertical
- **Thresholding** - Image binarization
- **Histogram Equalization** - With before and after histogram visualization

### Convolution Filters
- **Mean Filter**
- **Min Filter**
- **Max Filter**
- **Median Filter**
- **Order Filter**
- **Conservative Smoothing**
- **Gaussian Blur** - With configurable sigma parameter

### Edge Detection
- **Prewitt** - Prewitt gradient operator
- **Sobel** - Sobel gradient operator
- **Laplacian** - Edge detection using Laplacian

### Morphological Operations
- **Dilation**
- **Erosion**
- **Opening** - Erosion followed by dilation
- **Closing** - Dilation followed by erosion
- **Contour** - Contour extraction

## 🛠️ Technologies Used

- **.NET 9.0** - Base framework
- **.NET MAUI** - Cross-platform framework
- **SkiaSharp** - Image processing and graphics
- **SixLabors.ImageSharp** - Image manipulation
- **CommunityToolkit.Maui** - Components and utilities
- **Microcharts.Maui** - Charts/histograms visualization

## 📦 Supported Platforms

- **Windows** (Windows 10.0.17763.0 or higher)
- **Android** (API 21 or higher)

## 🚀 How to Run

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 or Visual Studio Code with C# extension
- For Android: Android SDK and emulator/physical device configuration

### Installation

1. Clone the repository:
```bash
git clone https://github.com/allanknecht/ImageProcessor.git
cd ImageProcessor
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Run the project:

**Windows:**
```bash
dotnet build
dotnet run --project ImageProcessor/ImageProcessor.csproj
```

**Android:**
```bash
dotnet build -t:Run -f net9.0-android
```

## 📁 Project Structure

```
ImageProcessor/
├── Processing/
│   ├── ArithmeticOperations.cs      # Arithmetic operations
│   └── ConvolutionOperations.cs     # Convolution filters
├── Services/
│   ├── ImageProcessingService.cs    # Main processing service
│   ├── ImageSelectionService.cs     # Image selection
│   ├── ImageSaveService.cs          # Image saving
│   └── ConvolutionService.cs        # Convolution service
├── ViewModels/
│   ├── BaseViewModel.cs             # Base ViewModel
│   ├── ImageProcessingViewModel.cs  # Processing ViewModel
│   └── ConvolutionViewModel.cs      # Convolution ViewModel
└── Views/
    ├── MainPage.xaml                # Main page
    ├── ImageTabsView.xaml           # Image visualization
    └── ConvolutionView.xaml         # Convolution interface
```

## 🎯 Usage

1. Open the application
2. Select one or two images (depending on the operation)
3. Choose the desired operation
4. Configure parameters when necessary (e.g., value for addition/subtraction, sigma for Gaussian Blur)
5. View the result and histograms (when applicable)
6. Save the processed image

## 📝 Notes

- Images must have the same size for operations involving two images
- Some filters preserve the original image borders
- Histogram equalization works better with grayscale images
- Morphological operations are more effective on binary images

## 📄 License

This project was developed as part of an academic work.

## 👤 Author

Developed as an academic image processing project.

---

**Developed with .NET MAUI**
