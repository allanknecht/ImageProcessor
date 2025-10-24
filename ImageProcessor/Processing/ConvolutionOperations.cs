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

            // Avoid borders (padding = 1)
            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    // CENTRAL PIXEL: f(x, y) = p
                    var centerPixel = image[y, x];

                    // NEIGHBORS - TOP ROW (y-1)
                    var topLeft = image[y - 1, x - 1];     // f(x-1, y-1)
                    var topCenter = image[y - 1, x];       // f(x, y-1)
                    var topRight = image[y - 1, x + 1];    // f(x+1, y-1)

                    // NEIGHBORS - CENTRAL ROW (y)
                    var centerLeft = image[y, x - 1];      // f(x-1, y)
                    // centerPixel is already f(x, y)
                    var centerRight = image[y, x + 1];     // f(x+1, y)

                    // NEIGHBORS - BOTTOM ROW (y+1)
                    var bottomLeft = image[y + 1, x - 1];  // f(x-1, y+1)
                    var bottomCenter = image[y + 1, x];    // f(x, y+1)
                    var bottomRight = image[y + 1, x + 1]; // f(x+1, y+1)

                    // Calculate average of 9 neighbors
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

                    // Divide by 9 and create new pixel
                    byte newR = (byte)(totalR / 9);
                    byte newG = (byte)(totalG / 9);
                    byte newB = (byte)(totalB / 9);

                    result[y, x] = new SKColor(newR, newG, newB, centerPixel.Alpha);
                }
            }

            // Copy borders without processing
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

            // Avoid borders (padding = 1)
            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    // CENTRAL PIXEL: f(x, y) = p
                    var centerPixel = image[y, x];

                    // NEIGHBORS - TOP ROW (y-1)
                    var topLeft = image[y - 1, x - 1];     // f(x-1, y-1)
                    var topCenter = image[y - 1, x];       // f(x, y-1)
                    var topRight = image[y - 1, x + 1];    // f(x+1, y-1)

                    // NEIGHBORS - CENTRAL ROW (y)
                    var centerLeft = image[y, x - 1];      // f(x-1, y)
                    // centerPixel is already f(x, y)
                    var centerRight = image[y, x + 1];     // f(x+1, y)

                    // NEIGHBORS - BOTTOM ROW (y+1)
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

            // Avoid borders (padding = 1)
            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    // CENTRAL PIXEL: f(x, y) = p
                    var centerPixel = image[y, x];

                    // NEIGHBORS - TOP ROW (y-1)
                    var topLeft = image[y - 1, x - 1];     // f(x-1, y-1)
                    var topCenter = image[y - 1, x];       // f(x, y-1)
                    var topRight = image[y - 1, x + 1];    // f(x+1, y-1)

                    // NEIGHBORS - CENTRAL ROW (y)
                    var centerLeft = image[y, x - 1];      // f(x-1, y)
                    // centerPixel is already f(x, y)
                    var centerRight = image[y, x + 1];     // f(x+1, y)

                    // NEIGHBORS - BOTTOM ROW (y+1)
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

        public static SKColor[,] MedianFilter(SKColor[,] image)
        {
            int h = image.GetLength(0);
            int w = image.GetLength(1);
            var result = new SKColor[h, w];

            // Avoid borders (padding = 1)
            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    // CENTRAL PIXEL: f(x, y) = p
                    var centerPixel = image[y, x];

                    // NEIGHBORS - TOP ROW (y-1)
                    var topLeft = image[y - 1, x - 1];     // f(x-1, y-1)
                    var topCenter = image[y - 1, x];       // f(x, y-1)
                    var topRight = image[y - 1, x + 1];    // f(x+1, y-1)

                    // NEIGHBORS - CENTRAL ROW (y)
                    var centerLeft = image[y, x - 1];      // f(x-1, y)
                    // centerPixel is already f(x, y)
                    var centerRight = image[y, x + 1];     // f(x+1, y)

                    // NEIGHBORS - BOTTOM ROW (y+1)
                    var bottomLeft = image[y + 1, x - 1];  // f(x-1, y+1)
                    var bottomCenter = image[y + 1, x];    // f(x, y+1)
                    var bottomRight = image[y + 1, x + 1]; // f(x+1, y+1)



                    var MedianArrayR = new byte[] {
                        topLeft.Red,    topCenter.Red,    topRight.Red,
                        centerLeft.Red, centerPixel.Red,  centerRight.Red,
                        bottomLeft.Red, bottomCenter.Red, bottomRight.Red
                    };
                                        var MedianArrayG = new byte[] {
                        topLeft.Green,    topCenter.Green,    topRight.Green,
                        centerLeft.Green, centerPixel.Green,  centerRight.Green,
                        bottomLeft.Green, bottomCenter.Green, bottomRight.Green
                    };
                                        var MedianArrayB = new byte[] {
                        topLeft.Blue,    topCenter.Blue,    topRight.Blue,
                        centerLeft.Blue, centerPixel.Blue,  centerRight.Blue,
                        bottomLeft.Blue, bottomCenter.Blue, bottomRight.Blue
                    };

                     Array.Sort(MedianArrayR);
                     Array.Sort(MedianArrayG);
                     Array.Sort(MedianArrayB);

                     const int mid = 4; // 9 elements -> central index (median)
                     byte newR = MedianArrayR[mid];
                     byte newG = MedianArrayG[mid];
                     byte newB = MedianArrayB[mid];


                    result[y, x] = new SKColor(newR, newG, newB, centerPixel.Alpha);
                }
            }

            // Copy borders without processing
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







        public static SKColor[,] OrderFilter(SKColor[,] image, int order)
        {
            int h = image.GetLength(0);
            int w = image.GetLength(1);
            var result = new SKColor[h, w];

            // Avoid borders (padding = 1)
            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    // CENTRAL PIXEL: f(x, y) = p
                    var centerPixel = image[y, x];

                    // NEIGHBORS - TOP ROW (y-1)
                    var topLeft = image[y - 1, x - 1];     // f(x-1, y-1)
                    var topCenter = image[y - 1, x];       // f(x, y-1)
                    var topRight = image[y - 1, x + 1];    // f(x+1, y-1)

                    // NEIGHBORS - CENTRAL ROW (y)
                    var centerLeft = image[y, x - 1];      // f(x-1, y)
                    // centerPixel is already f(x, y)
                    var centerRight = image[y, x + 1];     // f(x+1, y)

                    // NEIGHBORS - BOTTOM ROW (y+1)
                    var bottomLeft = image[y + 1, x - 1];  // f(x-1, y+1)
                    var bottomCenter = image[y + 1, x];    // f(x, y+1)
                    var bottomRight = image[y + 1, x + 1]; // f(x+1, y+1)



                    var MedianArrayR = new byte[] {
                        topLeft.Red,    topCenter.Red,    topRight.Red,
                        centerLeft.Red, centerPixel.Red,  centerRight.Red,
                        bottomLeft.Red, bottomCenter.Red, bottomRight.Red
                    };
                    var MedianArrayG = new byte[] {
                        topLeft.Green,    topCenter.Green,    topRight.Green,
                        centerLeft.Green, centerPixel.Green,  centerRight.Green,
                        bottomLeft.Green, bottomCenter.Green, bottomRight.Green
                    };
                    var MedianArrayB = new byte[] {
                        topLeft.Blue,    topCenter.Blue,    topRight.Blue,
                        centerLeft.Blue, centerPixel.Blue,  centerRight.Blue,
                        bottomLeft.Blue, bottomCenter.Blue, bottomRight.Blue
                    };

                    Array.Sort(MedianArrayR);
                    Array.Sort(MedianArrayG);
                    Array.Sort(MedianArrayB);

                    byte newR = MedianArrayR[order];
                    byte newG = MedianArrayG[order];
                    byte newB = MedianArrayB[order];


                    result[y, x] = new SKColor(newR, newG, newB, centerPixel.Alpha);
                }
            }

            // Copy borders without processing
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


        public static SKColor[,] ConservativeSmoothing(SKColor[,] image)
        {
            int h = image.GetLength(0);
            int w = image.GetLength(1);
            var result = new SKColor[h, w];

            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    var center = image[y, x];

                    // Pega todos os 8 vizinhos (sem o pixel central)
                    var neighbors = new[]
                    {
                        image[y - 1, x - 1], image[y - 1, x], image[y - 1, x + 1],
                        image[y, x - 1],                      image[y, x + 1],
                        image[y + 1, x - 1], image[y + 1, x], image[y + 1, x + 1]
                    };

                    // Calcula min e max para cada canal (R, G, B)
                    byte minR = neighbors.Min(p => p.Red);
                    byte maxR = neighbors.Max(p => p.Red);
                    byte minG = neighbors.Min(p => p.Green);
                    byte maxG = neighbors.Max(p => p.Green);
                    byte minB = neighbors.Min(p => p.Blue);
                    byte maxB = neighbors.Max(p => p.Blue);

                    byte newR = center.Red;
                    byte newG = center.Green;
                    byte newB = center.Blue;

                    // Regra do filtro conservativo
                    if (center.Red > maxR) newR = maxR;
                    else if (center.Red < minR) newR = minR;

                    if (center.Green > maxG) newG = maxG;
                    else if (center.Green < minG) newG = minG;

                    if (center.Blue > maxB) newB = maxB;
                    else if (center.Blue < minB) newB = minB;

                    result[y, x] = new SKColor(newR, newG, newB, center.Alpha);
                }
            }

            // Copia bordas sem alterar
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (y == 0 || y == h - 1 || x == 0 || x == w - 1)
                        result[y, x] = image[y, x];
                }
            }

            return result;
        }


        public static SKColor[,] GaussianBlur(SKColor[,] image, double sigma)
        {
            int h = image.GetLength(0);
            int w = image.GetLength(1);
            var result = new SKColor[h, w];

            // Kernel fixo 5x5
            int r = 2;
            int kernelSize = 5;
            double[,] kernel = new double[kernelSize, kernelSize];
            double sum = 0.0;

            // Calcula o kernel Gaussiano
            double coeff = 1.0 / (2.0 * Math.PI * sigma * sigma);
            double twoSigma2 = 2.0 * sigma * sigma;

            for (int y = -r; y <= r; y++)
            {
                for (int x = -r; x <= r; x++)
                {
                    double value = coeff * Math.Exp(-(x * x + y * y) / twoSigma2);
                    kernel[y + r, x + r] = value;
                    sum += value;
                }
            }

            // Normaliza para somar 1
            for (int y = 0; y < kernelSize; y++)
            {
                for (int x = 0; x < kernelSize; x++)
                {
                    kernel[y, x] /= sum;
                }
            }

            // Aplica convolução 5x5
            // fica o valor do r como sendo a borda não alterada
            for (int y = r; y < h - r; y++)
            {
                for (int x = r; x < w - r; x++)
                {
                    double accR = 0, accG = 0, accB = 0;

                    for (int ky = -r; ky <= r; ky++)
                    {
                        for (int kx = -r; kx <= r; kx++)
                        {
                            var p = image[y + ky, x + kx];
                            double wgt = kernel[ky + r, kx + r];
                            accR += p.Red * wgt;
                            accG += p.Green * wgt;
                            accB += p.Blue * wgt;
                        }
                    }

                    byte r8 = (byte)Math.Clamp(Math.Round(accR), 0, 255);
                    byte g8 = (byte)Math.Clamp(Math.Round(accG), 0, 255);
                    byte b8 = (byte)Math.Clamp(Math.Round(accB), 0, 255);
                    result[y, x] = new SKColor(r8, g8, b8, image[y, x].Alpha);
                }
            }

            // Copia bordas sem alterar
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (y < r || y >= h - r || x < r || x >= w - r)
                        result[y, x] = image[y, x];
                }
            }

            return result;
        }





}









}