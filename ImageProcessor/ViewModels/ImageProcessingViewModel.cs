using SkiaSharp;
using ImageProcessor.Services;
using Microcharts;
using System.Linq;

namespace ImageProcessor.ViewModels
{
    public partial class ImageProcessingViewModel : BaseViewModel
    {
        private readonly IImageProcessingService _imageProcessingService;
        
        private SKColor[,]? _matrixA;
        private SKColor[,]? _matrixB;
        private bool _isProcessing;
        private ImageSource? _resultImage;
        private bool _showResult;
        private bool _showHistograms;
        private Chart? _histogramBefore;
        private Chart? _histogramAfter;

        public ImageProcessingViewModel(IImageProcessingService imageProcessingService)
        {
            _imageProcessingService = imageProcessingService;
        }

        public SKColor[,]? MatrixA
        {
            get => _matrixA;
            set => SetProperty(ref _matrixA, value);
        }

        public SKColor[,]? MatrixB
        {
            get => _matrixB;
            set => SetProperty(ref _matrixB, value);
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set => SetProperty(ref _isProcessing, value);
        }

        public ImageSource? ResultImage
        {
            get => _resultImage;
            set => SetProperty(ref _resultImage, value);
        }

        public bool ShowResult
        {
            get => _showResult;
            set => SetProperty(ref _showResult, value);
        }

        public bool ShowHistograms
        {
            get => _showHistograms;
            set => SetProperty(ref _showHistograms, value);
        }

        public Chart? HistogramBefore
        {
            get => _histogramBefore;
            set => SetProperty(ref _histogramBefore, value);
        }

        public Chart? HistogramAfter
        {
            get => _histogramAfter;
            set => SetProperty(ref _histogramAfter, value);
        }

        public async Task<ImageSource?> ProcessTwoImagesAsync(Func<SKColor[,], SKColor[,], SKColor[,]> operation)
        {
            if (!_imageProcessingService.ValidateImages(MatrixA, MatrixB))
                return null;

            IsProcessing = true;
            try
            {
                ResultImage = await _imageProcessingService.ProcessImageAsync(MatrixA, MatrixB, operation);
                ShowResult = true;
                ShowHistograms = false;
                return ResultImage;
            }
            finally
            {
                IsProcessing = false;
            }
        }

        public async Task<ImageSource?> ProcessImageWithValueAsync(float value, Func<SKColor[,], float, SKColor[,]> operation)
        {
            if (!_imageProcessingService.ValidateImages(MatrixA))
                return null;

            IsProcessing = true;
            try
            {
                ResultImage = await _imageProcessingService.ProcessImageAsync(MatrixA, value, operation);
                ShowResult = true;
                ShowHistograms = false;
                return ResultImage;
            }
            finally
            {
                IsProcessing = false;
            }
        }

        public async Task<ImageSource?> ProcessImageAsync(Func<SKColor[,], SKColor[,]> operation)
        {
            if (!_imageProcessingService.ValidateImages(MatrixA))
                return null;

            IsProcessing = true;
            try
            {
                ResultImage = await _imageProcessingService.ProcessImageAsync(MatrixA, operation);
                ShowResult = true;
                ShowHistograms = false;
                return ResultImage;
            }
            finally
            {
                IsProcessing = false;
            }
        }

        public async Task<bool> ProcessHistogramEqualizationAsync()
        {
            if (!_imageProcessingService.ValidateImages(MatrixA))
                return false;

            IsProcessing = true;
            try
            {
                var result = await _imageProcessingService.ProcessHistogramEqualizationAsync(MatrixA);
                ResultImage = result.Image;
                ShowResult = true;
                ShowHistograms = true;
                
                HistogramBefore = CreateHistogramChart(result.HistBefore, "#e74c3c");
                HistogramAfter = CreateHistogramChart(result.HistAfter, "#27ae60");
                
                return true;
            }
            finally
            {
                IsProcessing = false;
            }
        }

        public void Reset()
        {
            MatrixA = null;
            MatrixB = null;
            ResultImage = null;
            ShowResult = false;
            ShowHistograms = false;
            HistogramBefore = null;
            HistogramAfter = null;
        }

        public byte[]? GetLastProcessedImageBytes() => _imageProcessingService.GetLastProcessedImageBytes();

        public bool ValidateImages(SKColor[,] matrixA, SKColor[,]? matrixB = null) => 
            _imageProcessingService.ValidateImages(matrixA, matrixB);

        public bool TryParseFloat(string value, out float result) => 
            _imageProcessingService.TryParseFloat(value, out result);

        private Chart CreateHistogramChart(int[] histogram, string color)
        {
            var entries = histogram.Select((value, index) =>
                new ChartEntry(value)
                {
                    Color = SKColor.Parse(color)
                }).ToList();

            return new BarChart
            {
                Entries = entries,
                Margin = 5,
                BackgroundColor = SKColors.White,
            };
        }

    }
}
