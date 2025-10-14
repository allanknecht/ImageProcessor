using Microsoft.Maui.Media;
using SkiaSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

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
                var fileResult = await FilePicker.Default.PickAsync();
                if (fileResult == null) return null;

                byte[] bytes;
                using (var stream = await fileResult.OpenReadAsync())
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
                    FileName = fileResult.FileName ?? "Unknown",
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
                // Primeiro, tenta detectar se é uma imagem TIFF
                if (IsTiffImage(bytes))
                {
                    return ProcessTiffImage(bytes);
                }

                // Para outros formatos, usa SkiaSharp como antes
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
            // Se for uma imagem TIFF, converte para PNG para exibição
            if (IsTiffImage(bytes))
            {
                return CreateTiffImageSource(bytes);
            }
            
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }

        private static bool IsTiffImage(byte[] bytes)
        {
            if (bytes.Length < 4) return false;
            
            // Verifica assinatura TIFF (II* ou MM*)
            return (bytes[0] == 0x49 && bytes[1] == 0x49 && bytes[2] == 0x2A && bytes[3] == 0x00) ||
                   (bytes[0] == 0x4D && bytes[1] == 0x4D && bytes[2] == 0x00 && bytes[3] == 0x2A);
        }

        private static SKColor[,]? ProcessTiffImage(byte[] bytes)
        {
            try
            {
                using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(bytes);
                
                var matrix = new SKColor[image.Height, image.Width];
                
                // Acessa os pixels diretamente
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        var pixel = image[x, y];
                        matrix[y, x] = new SKColor(pixel.R, pixel.G, pixel.B, pixel.A);
                    }
                }

                return matrix;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static ImageSource CreateTiffImageSource(byte[] bytes)
        {
            try
            {
                using var image = SixLabors.ImageSharp.Image.Load(bytes);
                using var memoryStream = new MemoryStream();
                
                // Converte a imagem TIFF para PNG
                image.SaveAsPng(memoryStream);
                var pngBytes = memoryStream.ToArray();
                
                return ImageSource.FromStream(() => new MemoryStream(pngBytes));
            }
            catch (Exception)
            {
                // Se falhar, retorna os bytes originais
                return ImageSource.FromStream(() => new MemoryStream(bytes));
            }
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
