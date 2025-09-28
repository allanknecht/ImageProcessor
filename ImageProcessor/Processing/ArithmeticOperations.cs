using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor.Processing
{
    public static class ArithmeticOperations
    {
        public static SKColor[,] Add(SKColor[,] a, SKColor[,] b)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            if (h != b.GetLength(0) || w != b.GetLength(1))
                throw new ArgumentException("Imagens não têm o mesmo tamanho");

            var result = new SKColor[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var colorA = a[y, x];
                    var colorB = b[y, x];

                    int red = colorA.Red + colorB.Red;
                    int green = colorA.Green + colorB.Green;
                    int blue = colorA.Blue + colorB.Blue;

                    byte rOut = (byte)Math.Min(red, 255);
                    byte gOut = (byte)Math.Min(green, 255);
                    byte bOut = (byte)Math.Min(blue, 255);

                    result[y, x] = new SKColor(rOut, gOut, bOut, 255);
                }
            }
            return result;
        }

        public static SKColor[,] Subt(SKColor[,] a, SKColor[,] b)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            if (h != b.GetLength(0) || w != b.GetLength(1))
                throw new ArgumentException("Imagens não têm o mesmo tamanho");

            var result = new SKColor[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var colorA = a[y, x];
                    var colorB = b[y, x];

                    int red = colorA.Red - colorB.Red;
                    int green = colorA.Green - colorB.Green;
                    int blue = colorA.Blue - colorB.Blue;

                    byte rOut = (byte)Math.Max(red, 0);
                    byte gOut = (byte)Math.Max(green, 0);
                    byte bOut = (byte)Math.Max(blue, 0);

                    result[y, x] = new SKColor(rOut, gOut, bOut, 255);
                }
            }
            return result;
        }

        public static SKColor[,] AddValue(SKColor[,] a, float sumValue)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            var result = new SKColor[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var colorA = a[y, x];

                    float red = colorA.Red + sumValue;
                    float green = colorA.Green + sumValue;
                    float blue = colorA.Blue + sumValue;

                    byte rOut = (byte)Math.Min(red, 255);
                    byte gOut = (byte)Math.Min(green, 255);
                    byte bOut = (byte)Math.Min(blue, 255);

                    result[y, x] = new SKColor(rOut, gOut, bOut, colorA.Alpha);
                }
            }
            return result;
        }

        public static SKColor[,] SubtValue(SKColor[,] a, float subtValue)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            var result = new SKColor[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var colorA = a[y, x];

                    float red = colorA.Red - subtValue;
                    float green = colorA.Green - subtValue;
                    float blue = colorA.Blue - subtValue;

                    byte rOut = (byte)Math.Max(red, 0);
                    byte gOut = (byte)Math.Max(green, 0);
                    byte bOut = (byte)Math.Max(blue, 0);

                    result[y, x] = new SKColor(rOut, gOut, bOut, colorA.Alpha);
                }
            }
            return result;
        }

        public static SKColor[,] Multiplication(SKColor[,] a, float multiplicationValue)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            var result = new SKColor[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var colorA = a[y, x];

                    float red = colorA.Red * multiplicationValue;
                    float green = colorA.Green * multiplicationValue;
                    float blue = colorA.Blue * multiplicationValue;

                    byte rOut = (byte)MathF.Min(MathF.Max(red, 0f), 255f);
                    byte gOut = (byte)MathF.Min(MathF.Max(green, 0f), 255f);
                    byte bOut = (byte)MathF.Min(MathF.Max(blue, 0f), 255f);

                    result[y, x] = new SKColor(rOut, gOut, bOut, colorA.Alpha);
                }
            }
            return result;
        }

        public static SKColor[,] Division(SKColor[,] a, float divisionValue)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            var result = new SKColor[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var colorA = a[y, x];

                    float red = colorA.Red / divisionValue;
                    float green = colorA.Green / divisionValue;
                    float blue = colorA.Blue / divisionValue;

                    byte rOut = (byte)MathF.Min(MathF.Max(red, 0f), 255f);
                    byte gOut = (byte)MathF.Min(MathF.Max(green, 0f), 255f);
                    byte bOut = (byte)MathF.Min(MathF.Max(blue, 0f), 255f);

                    result[y, x] = new SKColor(rOut, gOut, bOut, colorA.Alpha);
                }
            }
            return result;
        }

        public static SKColor[,] ConvertToGrayScale(SKColor[,] a)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            var result = new SKColor[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var colorA = a[y, x];

                    float grayScalePixelValue = (colorA.Red + colorA.Green + colorA.Blue) / 3f;

                    byte grayValue = (byte)MathF.Min(MathF.Max(grayScalePixelValue, 0f), 255f);

                    result[y, x] = new SKColor(grayValue, grayValue, grayValue, colorA.Alpha);
                }
            }
            return result;
        }

        public static SKColor[,] FlipLeftToRight(SKColor[,] a)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            var result = new SKColor[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int invertedX = w - 1 - x;
                    result[y, invertedX] = a[y, x];
                }
            }
            return result;
        }

        public static SKColor[,] FlipTopToBottom(SKColor[,] a)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            var result = new SKColor[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int invertedY = h - 1 - y;
                    result[invertedY, x] = a[y, x];
                }
            }
            return result;
        }

        public static SKColor[,] LinearBlending(SKColor[,] a, SKColor[,] b, float blendingRatio)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            if (h != b.GetLength(0) || w != b.GetLength(1))
                throw new ArgumentException("Imagens não têm o mesmo tamanho");

            var result = new SKColor[h, w];

            blendingRatio = MathF.Min(MathF.Max(blendingRatio, 0f), 1f);
            float complementRatio = 1f - blendingRatio;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var colorA = a[y, x];
                    var colorB = b[y, x];

                    float red = (blendingRatio * colorA.Red) + (complementRatio * colorB.Red);
                    float green = (blendingRatio * colorA.Green) + (complementRatio * colorB.Green);
                    float blue = (blendingRatio * colorA.Blue) + (complementRatio * colorB.Blue);
                    float alpha = (blendingRatio * colorA.Alpha) + (complementRatio * colorB.Alpha);

                    byte rOut = (byte)MathF.Min(MathF.Max(red, 0f), 255f);
                    byte gOut = (byte)MathF.Min(MathF.Max(green, 0f), 255f);
                    byte bOut = (byte)MathF.Min(MathF.Max(blue, 0f), 255f);
                    byte aOut = (byte)MathF.Min(MathF.Max(alpha, 0f), 255f);

                    result[y, x] = new SKColor(rOut, gOut, bOut, aOut);
                }
            }
            return result;
        }
        public static SKColor[,] AbsoluteDifference(SKColor[,] a, SKColor[,] b)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            if (h != b.GetLength(0) || w != b.GetLength(1))
                throw new ArgumentException("Imagens não têm o mesmo tamanho");

            var result = new SKColor[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var ca = a[y, x];
                    var cb = b[y, x];

                    byte rOut = (byte)Math.Abs(ca.Red - cb.Red);
                    byte gOut = (byte)Math.Abs(ca.Green - cb.Green);
                    byte bOut = (byte)Math.Abs(ca.Blue - cb.Blue);

                    result[y, x] = new SKColor(rOut, gOut, bOut, 255);
                }
            }
            return result;
        }

        public static SKColor[,] Average(SKColor[,] a, SKColor[,] b)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            if (h != b.GetLength(0) || w != b.GetLength(1))
                throw new ArgumentException("Imagens não têm o mesmo tamanho");

            var result = new SKColor[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var colorA = a[y, x];
                    var colorB = b[y, x];

                    // Soma com clipping em 255 (R1 = P + Q)
                    int redSum = Math.Min(colorA.Red + colorB.Red, 255);
                    int greenSum = Math.Min(colorA.Green + colorB.Green, 255);
                    int blueSum = Math.Min(colorA.Blue + colorB.Blue, 255);
                    int alphaSum = Math.Min(colorA.Alpha + colorB.Alpha, 255);

                    // Divide por 2 (R2 = R1 / 2)
                    byte red = (byte)(redSum / 2);
                    byte green = (byte)(greenSum / 2);
                    byte blue = (byte)(blueSum / 2);
                    byte alpha = (byte)(alphaSum / 2);

                    result[y, x] = new SKColor(red, green, blue, alpha);
                }
            }
            return result;
        }
    }
}