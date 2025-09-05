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



            int hA = a.GetLength(0); // altura de A
            int wA = a.GetLength(1); // largura de A
            int hB = b.GetLength(0);
            int wB = b.GetLength(1);

            if (hA != hB || wA != wB)
            {
                throw new ArgumentException("Imagens não tem o mesmo tamanho");
            }

            var result = new SKColor[hA, wA];

            for (int y = 0; y < hA; y++)
            {
                for (int x = 0; x < wA; x++)
                {
                    var colorA = a[y, x];
                    var colorB = b[y, x];

                    int red = colorA.Red + colorB.Red;
                    int green = colorA.Green + colorB.Green;
                    int blue = colorA.Blue + colorB.Blue;
                    int alpha = colorA.Alpha + colorB.Alpha;


                    byte rOut = (byte)Math.Min(red, 255);
                    byte gOut = (byte)Math.Min(green, 255);
                    byte bOut = (byte)Math.Min(blue, 255);
                    byte aOut = (byte)Math.Min(alpha, 255);

                    result[y, x] = new SKColor(rOut, gOut, bOut, aOut);
                }
            }
            return result;
        }
    }
}
