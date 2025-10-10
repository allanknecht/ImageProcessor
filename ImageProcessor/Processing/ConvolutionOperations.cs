using SkiaSharp;

namespace ImageProcessor.Processing
{
    public static class ConvolutionOperations
    {
        public static SKColor[,] MeanFilter(SKColor[,] image)
        {
            int h = image.GetLength(0);
            int w = image.GetLength(1);
            var result = new SKColor[h, w];

            // Evitar bordas (padding = 1)
            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    // PIXEL CENTRAL: f(x, y) = p
                    var centerPixel = image[y, x];
                    
                    // VIZINHOS - LINHA SUPERIOR (y-1)
                    var topLeft = image[y - 1, x - 1];     // f(x-1, y-1)
                    var topCenter = image[y - 1, x];       // f(x, y-1)
                    var topRight = image[y - 1, x + 1];    // f(x+1, y-1)
                    
                    // VIZINHOS - LINHA CENTRAL (y)
                    var centerLeft = image[y, x - 1];      // f(x-1, y)
                    // centerPixel já é f(x, y)
                    var centerRight = image[y, x + 1];     // f(x+1, y)
                    
                    // VIZINHOS - LINHA INFERIOR (y+1)
                    var bottomLeft = image[y + 1, x - 1];  // f(x-1, y+1)
                    var bottomCenter = image[y + 1, x];    // f(x, y+1)
                    var bottomRight = image[y + 1, x + 1]; // f(x+1, y+1)
                    
                    // Calcular média dos 9 vizinhos
                    int totalR = 0, totalG = 0, totalB = 0;

                    totalR += topLeft.Red + topCenter.Red + topRight.Red +
                              centerLeft.Red + centerPixel.Red + centerRight.Red +
                              bottomLeft.Red + bottomCenter.Red + bottomRight.Red;

                    totalG += topLeft.Green + topCenter.Green + topRight.Green +
                              centerLeft.Green + centerPixel.Green + centerRight.Green +
                              bottomLeft.Green + bottomCenter.Green + bottomRight.Green;

                    totalB += topLeft.Blue + topCenter.Blue + topRight.Blue +
                              centerLeft.Blue + centerPixel.Blue + centerRight.Blue +
                              bottomLeft.Blue + bottomCenter.Blue + bottomRight.Blue;

                    // Dividir por 9 e criar novo pixel
                    byte newR = (byte)(totalR / 9);
                    byte newG = (byte)(totalG / 9);
                    byte newB = (byte)(totalB / 9);

                    result[y, x] = new SKColor(newR, newG, newB, centerPixel.Alpha);
                }
            }
            
            // Copiar bordas sem processamento
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (y == 0 || y == h - 1 || x == 0 || x == w - 1)
                    {
                        result[y, x] = image[y, x];
                    }
                }
            }

            return result;
        }

        public static SKColor[,] MinFilter(SKColor[,] image)
        {
            int h = image.GetLength(0);
            int w = image.GetLength(1);
            var result = new SKColor[h, w];

            // Evitar bordas (padding = 1)
            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    // PIXEL CENTRAL: f(x, y) = p
                    var centerPixel = image[y, x];

                    // VIZINHOS - LINHA SUPERIOR (y-1)
                    var topLeft = image[y - 1, x - 1];     // f(x-1, y-1)
                    var topCenter = image[y - 1, x];       // f(x, y-1)
                    var topRight = image[y - 1, x + 1];    // f(x+1, y-1)

                    // VIZINHOS - LINHA CENTRAL (y)
                    var centerLeft = image[y, x - 1];      // f(x-1, y)
                    // centerPixel já é f(x, y)
                    var centerRight = image[y, x + 1];     // f(x+1, y)

                    // VIZINHOS - LINHA INFERIOR (y+1)
                    var bottomLeft = image[y + 1, x - 1];  // f(x-1, y+1)
                    var bottomCenter = image[y + 1, x];    // f(x, y+1)
                    var bottomRight = image[y + 1, x + 1]; // f(x+1, y+1)

                    byte minR = Math.Min(Math.Min(Math.Min(Math.Min(topLeft.Red, topCenter.Red), topRight.Red),
                                                  Math.Min(Math.Min(centerLeft.Red, centerPixel.Red), centerRight.Red)),
                                         Math.Min(Math.Min(bottomLeft.Red, bottomCenter.Red), bottomRight.Red));

                    byte minG = Math.Min(Math.Min(Math.Min(Math.Min(topLeft.Green, topCenter.Green), topRight.Green),
                                                  Math.Min(Math.Min(centerLeft.Green, centerPixel.Green), centerRight.Green)),
                                         Math.Min(Math.Min(bottomLeft.Green, bottomCenter.Green), bottomRight.Green));

                    byte minB = Math.Min(Math.Min(Math.Min(Math.Min(topLeft.Blue, topCenter.Blue), topRight.Blue),
                                                  Math.Min(Math.Min(centerLeft.Blue, centerPixel.Blue), centerRight.Blue)),
                                         Math.Min(Math.Min(bottomLeft.Blue, bottomCenter.Blue), bottomRight.Blue));

                    result[y, x] = new SKColor(minR, minG, minB, centerPixel.Alpha);
                }
            }

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (y == 0 || y == h - 1 || x == 0 || x == w - 1)
                    {
                        result[y, x] = image[y, x];
                    }
                }
            }

            return result;
        }

        public static SKColor[,] MaxFilter(SKColor[,] image)
        {
            int h = image.GetLength(0);
            int w = image.GetLength(1);
            var result = new SKColor[h, w];

            // Evitar bordas (padding = 1)
            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    // PIXEL CENTRAL: f(x, y) = p
                    var centerPixel = image[y, x];

                    // VIZINHOS - LINHA SUPERIOR (y-1)
                    var topLeft = image[y - 1, x - 1];     // f(x-1, y-1)
                    var topCenter = image[y - 1, x];       // f(x, y-1)
                    var topRight = image[y - 1, x + 1];    // f(x+1, y-1)

                    // VIZINHOS - LINHA CENTRAL (y)
                    var centerLeft = image[y, x - 1];      // f(x-1, y)
                    // centerPixel já é f(x, y)
                    var centerRight = image[y, x + 1];     // f(x+1, y)

                    // VIZINHOS - LINHA INFERIOR (y+1)
                    var bottomLeft = image[y + 1, x - 1];  // f(x-1, y+1)
                    var bottomCenter = image[y + 1, x];    // f(x, y+1)
                    var bottomRight = image[y + 1, x + 1]; // f(x+1, y+1)

                    byte maxR = Math.Max(Math.Max(Math.Max(Math.Max(topLeft.Red, topCenter.Red), topRight.Red),
                                                  Math.Max(Math.Max(centerLeft.Red, centerPixel.Red), centerRight.Red)),
                                         Math.Max(Math.Max(bottomLeft.Red, bottomCenter.Red), bottomRight.Red));

                    byte maxG = Math.Max(Math.Max(Math.Max(Math.Max(topLeft.Green, topCenter.Green), topRight.Green),
                                                  Math.Max(Math.Max(centerLeft.Green, centerPixel.Green), centerRight.Green)),
                                         Math.Max(Math.Max(bottomLeft.Green, bottomCenter.Green), bottomRight.Green));

                    byte maxB = Math.Max(Math.Max(Math.Max(Math.Max(topLeft.Blue, topCenter.Blue), topRight.Blue),
                                                  Math.Max(Math.Max(centerLeft.Blue, centerPixel.Blue), centerRight.Blue)),
                                         Math.Max(Math.Max(bottomLeft.Blue, bottomCenter.Blue), bottomRight.Blue));

                    result[y, x] = new SKColor(maxR, maxG, maxB, centerPixel.Alpha);
                }
            }

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (y == 0 || y == h - 1 || x == 0 || x == w - 1)
                    {
                        result[y, x] = image[y, x];
                    }
                }
            }

            return result;
        }
    }
}