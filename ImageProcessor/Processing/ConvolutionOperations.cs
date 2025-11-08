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

            // Aplica convolu��o 5x5
            // fica o valor do r como sendo a borda n�o alterada
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

        public static SKColor[,] Prewit(SKColor[,] image)
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

                    var KernelV_R = topLeft.Red + centerLeft.Red + bottomLeft.Red + topRight.Red*(-1) + centerRight.Red*(-1) + bottomRight.Red*(-1);
                    var KernelV_G = topLeft.Green + centerLeft.Green + bottomLeft.Green + topRight.Green * (-1) + centerRight.Green * (-1) + bottomRight.Green * (-1);
                    var KernelV_B = topLeft.Blue + centerLeft.Blue + bottomLeft.Blue + topRight.Blue * (-1) + centerRight.Blue * (-1) + bottomRight.Blue * (-1);

                    var KernelH_G = topLeft.Green + topCenter.Green + topRight.Green + bottomLeft.Green * (-1) + bottomCenter.Green * (-1) + bottomRight.Green * (-1);
                    var KernelH_R = topLeft.Red + topCenter.Red + topRight.Red + bottomLeft.Red * (-1) + bottomCenter.Red * (-1) + bottomRight.Red * (-1);
                    var KernelH_B = topLeft.Blue + topCenter.Blue + topRight.Blue + bottomLeft.Blue * (-1) + bottomCenter.Blue * (-1) + bottomRight.Blue * (-1);

                    byte newR = (byte)Math.Clamp(Math.Sqrt(KernelV_R * KernelV_R + KernelH_R * KernelH_R), 0, 255);
                    byte newG = (byte)Math.Clamp(Math.Sqrt(KernelV_G * KernelV_G + KernelH_G * KernelH_G), 0, 255);
                    byte newB = (byte)Math.Clamp(Math.Sqrt(KernelV_B * KernelV_B + KernelH_B * KernelH_B), 0, 255);


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







        public static SKColor[,] Sobel(SKColor[,] image)
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

                    var KernelV_R = topLeft.Red + centerLeft.Red*2 + bottomLeft.Red + topRight.Red * (-1) + centerRight.Red * (-2) + bottomRight.Red * (-1);
                    var KernelV_G = topLeft.Green + centerLeft.Green*2 + bottomLeft.Green + topRight.Green * (-1) + centerRight.Green * (-2) + bottomRight.Green * (-1);
                    var KernelV_B = topLeft.Blue + centerLeft.Blue*2 + bottomLeft.Blue + topRight.Blue * (-1) + centerRight.Blue * (-2) + bottomRight.Blue * (-1);

                    var KernelH_G = topLeft.Green + topCenter.Green*2 + topRight.Green + bottomLeft.Green * (-1) + bottomCenter.Green * (-2) + bottomRight.Green * (-1);
                    var KernelH_R = topLeft.Red + topCenter.Red*2 + topRight.Red + bottomLeft.Red * (-1) + bottomCenter.Red * (-2) + bottomRight.Red * (-1);
                    var KernelH_B = topLeft.Blue + topCenter.Blue*2 + topRight.Blue + bottomLeft.Blue * (-1) + bottomCenter.Blue * (-2) + bottomRight.Blue * (-1);

                    byte newR = (byte)Math.Clamp(Math.Sqrt(KernelV_R * KernelV_R + KernelH_R * KernelH_R), 0, 255);
                    byte newG = (byte)Math.Clamp(Math.Sqrt(KernelV_G * KernelV_G + KernelH_G * KernelH_G), 0, 255);
                    byte newB = (byte)Math.Clamp(Math.Sqrt(KernelV_B * KernelV_B + KernelH_B * KernelH_B), 0, 255);


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




        public static SKColor[,] Laplacian(SKColor[,] image)
        {
            int h = image.GetLength(0);
            int w = image.GetLength(1);
            var result = new SKColor[h, w];

            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    var centerPixel = image[y, x];

                    var topLeft = image[y - 1, x - 1];
                    var topCenter = image[y - 1, x];
                    var topRight = image[y - 1, x + 1];

                    var centerLeft = image[y, x - 1];
                    var centerRight = image[y, x + 1];

                    var bottomLeft = image[y + 1, x - 1];
                    var bottomCenter = image[y + 1, x];
                    var bottomRight = image[y + 1, x + 1];

                    // Kernel Laplaciano da imagem a/b (KernelV)
                    int KernelV_R = topCenter.Red + centerLeft.Red + centerRight.Red + bottomCenter.Red - 4 * centerPixel.Red;
                    int KernelV_G = topCenter.Green + centerLeft.Green + centerRight.Green + bottomCenter.Green - 4 * centerPixel.Green;
                    int KernelV_B = topCenter.Blue + centerLeft.Blue + centerRight.Blue + bottomCenter.Blue - 4 * centerPixel.Blue;

                    // Kernel Laplaciano da imagem c/d (KernelH)
                    int KernelH_R = 4 * centerPixel.Red - (topCenter.Red + centerLeft.Red + centerRight.Red + bottomCenter.Red);
                    int KernelH_G = 4 * centerPixel.Green - (topCenter.Green + centerLeft.Green + centerRight.Green + bottomCenter.Green);
                    int KernelH_B = 4 * centerPixel.Blue - (topCenter.Blue + centerLeft.Blue + centerRight.Blue + bottomCenter.Blue);

                    // Combina os dois kernels
                    int finalR = (int)Math.Sqrt(KernelV_R * KernelV_R + KernelH_R * KernelH_R);
                    int finalG = (int)Math.Sqrt(KernelV_G * KernelV_G + KernelH_G * KernelH_G);
                    int finalB = (int)Math.Sqrt(KernelV_B * KernelV_B + KernelH_B * KernelH_B);

                    // Normaliza
                    finalR = Math.Clamp(finalR, 0, 255);
                    finalG = Math.Clamp(finalG, 0, 255);
                    finalB = Math.Clamp(finalB, 0, 255);

                    result[y, x] = new SKColor((byte)finalR, (byte)finalG, (byte)finalB, centerPixel.Alpha);
                }
            }

            // Copia bordas
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

        public static SKColor[,] Dilation(SKColor[,] image)
        {
            int h = image.GetLength(0);
            int w = image.GetLength(1);
            var result = new SKColor[h, w];

            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    var centerPixel = image[y, x];
                    var topCenter = image[y - 1, x];
                    var centerLeft = image[y, x - 1];
                    var centerRight = image[y, x + 1];
                    var bottomCenter = image[y + 1, x];

                    // Verifica se algum pixel da cruz é difernente de 0
                    bool anyForeground =
                        (centerPixel.Red != 0 || centerPixel.Green != 0 || centerPixel.Blue != 0) ||
                        (topCenter.Red != 0 || topCenter.Green != 0 || topCenter.Blue != 0) ||
                        (centerLeft.Red != 0 || centerLeft.Green != 0 || centerLeft.Blue != 0) ||
                        (centerRight.Red != 0 || centerRight.Green != 0 || centerRight.Blue != 0) ||
                        (bottomCenter.Red != 0 || bottomCenter.Green != 0 || bottomCenter.Blue != 0);

                    // Se algum pixel for foreground, o pixel central vira branco
                    if (anyForeground)
                    {
                        result[y, x] = new SKColor(255, 255, 255, centerPixel.Alpha);
                    }
                    else
                    {
                        result[y, x] = new SKColor(0, 0, 0, centerPixel.Alpha);
                    }
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



        public static SKColor[,] Erosion(SKColor[,] image)
        {
            int h = image.GetLength(0);
            int w = image.GetLength(1);
            var result = new SKColor[h, w];

            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    var centerPixel = image[y, x];
                    var topCenter = image[y - 1, x];
                    var centerLeft = image[y, x - 1];
                    var centerRight = image[y, x + 1];
                    var bottomCenter = image[y + 1, x];

                    // Verifica se TODOS os pixels da cruz são diferentes de 0
                    bool allForeground =
                        (centerPixel.Red != 0 || centerPixel.Green != 0 || centerPixel.Blue != 0) &&
                        (topCenter.Red != 0 || topCenter.Green != 0 || topCenter.Blue != 0) &&
                        (centerLeft.Red != 0 || centerLeft.Green != 0 || centerLeft.Blue != 0) &&
                        (centerRight.Red != 0 || centerRight.Green != 0 || centerRight.Blue != 0) &&
                        (bottomCenter.Red != 0 || bottomCenter.Green != 0 || bottomCenter.Blue != 0);

                    if (allForeground)
                    {
                        result[y, x] = new SKColor(255, 255, 255, centerPixel.Alpha);
                    }
                    else
                    {
                        result[y, x] = new SKColor(0, 0, 0, centerPixel.Alpha);
                    }
                }
            }

            // Copia bordas originais
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
        
        public static SKColor[,] Opening(SKColor[,] image)
        {
            var eroded = Erosion(image);
            var opened = Dilation(eroded);

            return opened;
        }

        public static SKColor[,] Closing(SKColor[,] image)
        {
            var dilated = Dilation(image);
            var closed = Erosion(dilated);

            return closed;
        }

        public static SKColor[,] Contour(SKColor[,] image)
        {
            int h = image.GetLength(0);
            int w = image.GetLength(1);
            var result = new SKColor[h, w];

            var eroded = Erosion(image);

            // Contorno = Imagem Original - Erosão
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var orig = image[y, x];
                    var ero = eroded[y, x];

                    // Calcula a diferença entre os canais (saturando em 0)
                    byte r = (byte)Math.Max(0, orig.Red - ero.Red);
                    byte g = (byte)Math.Max(0, orig.Green - ero.Green);
                    byte b = (byte)Math.Max(0, orig.Blue - ero.Blue);

                    result[y, x] = new SKColor(r, g, b, orig.Alpha);
                }
            }

            return result;
        }


    }

}