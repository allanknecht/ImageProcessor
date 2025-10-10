using SkiaSharp;
using ImageProcessor.Services;
using ImageProcessor.Processing;
using Microcharts;
using System.Linq;

namespace ImageProcessor.ViewModels
{
    public partial class ConvolutionViewModel : BaseViewModel
    {
        private readonly IConvolutionService _convolutionService;
        
        private SKColor[,]? _matrix;
        private bool _isProcessing;
        private ImageSource? _resultImage;
        private bool _showResult;

        public ConvolutionViewModel(IConvolutionService convolutionService)
        {
            _convolutionService = convolutionService;
        }

        public SKColor[,]? Matrix
        {
            get => _matrix;
            set => SetProperty(ref _matrix, value);
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

        public async Task<ImageSource?> ProcessImageAsync(Func<SKColor[,], SKColor[,]> operation)
        {
            if (!_convolutionService.ValidateImage(Matrix))
                return null;

            IsProcessing = true;
            try
            {
                ResultImage = await _convolutionService.ProcessImageAsync(Matrix, operation);
                ShowResult = true;
                return ResultImage;
            }
            finally
            {
                IsProcessing = false;
            }
        }

        public async Task<ImageSource?> ProcessImageWithParameterAsync(int parameter, Func<SKColor[,], int, SKColor[,]> operation)
        {
            if (!_convolutionService.ValidateImage(Matrix))
                return null;

            IsProcessing = true;
            try
            {
                ResultImage = await _convolutionService.ProcessImageWithParameterAsync(Matrix, parameter, operation);
                ShowResult = true;
                return ResultImage;
            }
            finally
            {
                IsProcessing = false;
            }
        }

        public async Task<ImageSource?> ProcessImageWithDoubleParameterAsync(double parameter, Func<SKColor[,], double, SKColor[,]> operation)
        {
            if (!_convolutionService.ValidateImage(Matrix))
                return null;

            IsProcessing = true;
            try
            {
                ResultImage = await _convolutionService.ProcessImageWithDoubleParameterAsync(Matrix, parameter, operation);
                ShowResult = true;
                return ResultImage;
            }
            finally
            {
                IsProcessing = false;
            }
        }

        public async Task<ImageSource?> ProcessImageWithKernelAsync(float[,] kernel, Func<SKColor[,], float[,], SKColor[,]> operation)
        {
            if (!_convolutionService.ValidateImage(Matrix))
                return null;

            IsProcessing = true;
            try
            {
                ResultImage = await _convolutionService.ProcessImageWithKernelAsync(Matrix, kernel, operation);
                ShowResult = true;
                return ResultImage;
            }
            finally
            {
                IsProcessing = false;
            }
        }

        public void Reset()
        {
            Matrix = null;
            ResultImage = null;
            ShowResult = false;
        }

        public byte[]? GetLastProcessedImageBytes() => _convolutionService.GetLastProcessedImageBytes();

        public bool ValidateImage(SKColor[,] matrix) => _convolutionService.ValidateImage(matrix);

        public bool TryParseInt(string value, out int result) => _convolutionService.TryParseInt(value, out result);

        public bool TryParseDouble(string value, out double result) => _convolutionService.TryParseDouble(value, out result);
    }
}
