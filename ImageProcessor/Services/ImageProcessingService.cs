using SkiaSharp;
using System.Globalization;

namespace ImageProcessor.Services
{
    public interface IImageProcessingService
    {
        Task<ImageSource> ProcessImageAsync(SKColor[,] matrixA, SKColor[,] matrixB, Func<SKColor[,], SKColor[,], SKColor[,]> operation);
        Task<ImageSource> ProcessImageAsync(SKColor[,] matrixA, float value, Func<SKColor[,], float, SKColor[,]> operation);
        Task<ImageSource> ProcessImageAsync(SKColor[,] matrixA, Func<SKColor[,], SKColor[,]> operation);
        Task<(ImageSource Image, int[] HistBefore, int[] HistAfter)> ProcessHistogramEqualizationAsync(SKColor[,] matrixA);
        bool ValidateImages(SKColor[,] matrixA, SKColor[,]? matrixB = null);
        bool TryParseFloat(string value, out float result);
        byte[]? GetLastProcessedImageBytes();
    }

    public class ImageProcessingService : IImageProcessingService
    {
        private byte[]? _lastProcessedImageBytes;

        public async Task<ImageSource> ProcessImageAsync(SKColor[,] matrixA, SKColor[,] matrixB, Func<SKColor[,], SKColor[,], SKColor[,]> operation)
        {
            return await Task.Run(() =>
            {
                var result = operation(matrixA, matrixB);
                return MatrixToImageSource(result);
            });
        }

        public async Task<ImageSource> ProcessImageAsync(SKColor[,] matrixA, float value, Func<SKColor[,], float, SKColor[,]> operation)
        {
            return await Task.Run(() =>
            {
                var result = operation(matrixA, value);
                return MatrixToImageSource(result);
            });
        }

        public async Task<ImageSource> ProcessImageAsync(SKColor[,] matrixA, Func<SKColor[,], SKColor[,]> operation)
        {
            return await Task.Run(() =>
            {
                var result = operation(matrixA);
                return MatrixToImageSource(result);
            });
        }

        public async Task<(ImageSource Image, int[] HistBefore, int[] HistAfter)> ProcessHistogramEqualizationAsync(SKColor[,] matrixA)
        {
            return await Task.Run(() =>
            {
                var gray = Processing.ArithmeticOperations.ConvertToGrayScale(matrixA);
                var histBefore = Processing.ArithmeticOperations.GetHistogram(gray);
                var equalized = Processing.ArithmeticOperations.HistogramEqualization(gray);
                var histAfter = Processing.ArithmeticOperations.GetHistogram(equalized);
                
                return (MatrixToImageSource(equalized), histBefore, histAfter);
            });
        }

        public bool ValidateImages(SKColor[,] matrixA, SKColor[,]? matrixB = null)
        {
            if (matrixA == null)
                return false;

            if (matrixB != null)
            {
                int h = matrixA.GetLength(0), w = matrixA.GetLength(1);
                if (h != matrixB.GetLength(0) || w != matrixB.GetLength(1))
                    return false;
            }

            return true;
        }

        public bool TryParseFloat(string value, out float result)
        {
            string normalizedValue = value?.Replace(',', '.') ?? "0";
            return float.TryParse(normalizedValue, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
        }

        public byte[]? GetLastProcessedImageBytes() => _lastProcessedImageBytes;

        private ImageSource MatrixToImageSource(SKColor[,] matrix)
        {
            int h = matrix.GetLength(0);
            int w = matrix.GetLength(1);

            using var bmp = new SKBitmap(w, h, true);
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    bmp.SetPixel(x, y, matrix[y, x]);

            using var img = SKImage.FromBitmap(bmp);
            using var data = img.Encode(SKEncodedImageFormat.Png, 100);
            
            _lastProcessedImageBytes = data.ToArray();
            return ImageSource.FromStream(() => new MemoryStream(_lastProcessedImageBytes));
        }
    }
}
