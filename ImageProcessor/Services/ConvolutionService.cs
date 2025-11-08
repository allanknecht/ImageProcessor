using SkiaSharp;
using System.Globalization;
using ImageProcessor.Processing;

namespace ImageProcessor.Services
{
    public interface IConvolutionService
    {
        Task<ImageSource> ProcessImageAsync(SKColor[,] matrix, Func<SKColor[,], SKColor[,]> operation);
        Task<ImageSource> ProcessImageWithParameterAsync(SKColor[,] matrix, int parameter, Func<SKColor[,], int, SKColor[,]> operation);
        Task<ImageSource> ProcessImageWithDoubleParameterAsync(SKColor[,] matrix, double parameter, Func<SKColor[,], double, SKColor[,]> operation);
        Task<ImageSource> ProcessImageWithKernelAsync(SKColor[,] matrix, float[,] kernel, Func<SKColor[,], float[,], SKColor[,]> operation);
        bool ValidateImage(SKColor[,] matrix);
        bool ValidateBinaryImage(SKColor[,] matrix);
        bool TryParseInt(string value, out int result);
        bool TryParseDouble(string value, out double result);
        byte[]? GetLastProcessedImageBytes();
    }

    public class ConvolutionService : IConvolutionService
    {
        private byte[]? _lastProcessedImageBytes;

        public async Task<ImageSource> ProcessImageAsync(SKColor[,] matrix, Func<SKColor[,], SKColor[,]> operation)
        {
            return await Task.Run(() =>
            {
                var result = operation(matrix);
                return MatrixToImageSource(result);
            });
        }

        public async Task<ImageSource> ProcessImageWithParameterAsync(SKColor[,] matrix, int parameter, Func<SKColor[,], int, SKColor[,]> operation)
        {
            return await Task.Run(() =>
            {
                var result = operation(matrix, parameter);
                return MatrixToImageSource(result);
            });
        }

        public async Task<ImageSource> ProcessImageWithDoubleParameterAsync(SKColor[,] matrix, double parameter, Func<SKColor[,], double, SKColor[,]> operation)
        {
            return await Task.Run(() =>
            {
                var result = operation(matrix, parameter);
                return MatrixToImageSource(result);
            });
        }

        public async Task<ImageSource> ProcessImageWithKernelAsync(SKColor[,] matrix, float[,] kernel, Func<SKColor[,], float[,], SKColor[,]> operation)
        {
            return await Task.Run(() =>
            {
                var result = operation(matrix, kernel);
                return MatrixToImageSource(result);
            });
        }

        public bool ValidateImage(SKColor[,] matrix)
        {
            return matrix != null;
        }

        public bool ValidateBinaryImage(SKColor[,] matrix)
        {
            if (matrix == null)
                return false;

            int h = matrix.GetLength(0);
            int w = matrix.GetLength(1);

            // Verifica se todos os pixels têm apenas valores 0 ou 255 em cada canal RGB
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var pixel = matrix[y, x];
                    
                    // Verifica se cada canal é 0 ou 255
                    if ((pixel.Red != 0 && pixel.Red != 255) ||
                        (pixel.Green != 0 && pixel.Green != 255) ||
                        (pixel.Blue != 0 && pixel.Blue != 255))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool TryParseInt(string value, out int result)
        {
            string normalizedValue = value?.Replace(',', '.') ?? "0";
            return int.TryParse(normalizedValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
        }

        public bool TryParseDouble(string value, out double result)
        {
            string normalizedValue = value?.Replace(',', '.') ?? "0";
            return double.TryParse(normalizedValue, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
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
