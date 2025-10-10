using Microsoft.Maui.Media;
using SkiaSharp;

namespace ImageProcessor.Services
{
    public interface IImageSelectionService
    {
        Task<ImageSelectionResult?> PickImageAsync();
        SKColor[,]? ProcessImageBytes(byte[] bytes);
        ImageSource CreateImageSource(byte[] bytes);
    }

    public class ImageSelectionResult
    {
        public byte[] Bytes { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = string.Empty;
        public SKColor[,] Matrix { get; set; } = new SKColor[0, 0];
    }

    public class ImageSelectionService : IImageSelectionService
    {
        public async Task<ImageSelectionResult?> PickImageAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result == null) return null;

                byte[] bytes;
                using (var stream = await result.OpenReadAsync())
                using (var ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);
                    bytes = ms.ToArray();
                }

                var matrix = ProcessImageBytes(bytes);
                if (matrix == null) return null;

                return new ImageSelectionResult
                {
                    Bytes = bytes,
                    FileName = result.FileName ?? "Unknown",
                    Matrix = matrix
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public SKColor[,]? ProcessImageBytes(byte[] bytes)
        {
            try
            {
                using var bitmap = SKBitmap.Decode(bytes);
                if (bitmap == null) return null;

                return ToMatrix(bitmap);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ImageSource CreateImageSource(byte[] bytes)
        {
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }

        private static SKColor[,] ToMatrix(SKBitmap bitmap)
        {
            var matrix = new SKColor[bitmap.Height, bitmap.Width];
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    matrix[y, x] = bitmap.GetPixel(x, y);
                }
            }
            return matrix;
        }
    }
}
