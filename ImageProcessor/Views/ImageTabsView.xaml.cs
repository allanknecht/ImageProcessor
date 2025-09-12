using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Media;
using SkiaSharp;
using ImageProcessor.Processing;

namespace ImageProcessor.Views
{
    public partial class ImageTabsView : ContentPage
    {
        // Bytes das imagens selecionadas
        private byte[] _imageABytes;
        private byte[] _imageBBytes;

        // Matrizes de pixels
        private SkiaSharp.SKColor[,] _matrixA;
        private SkiaSharp.SKColor[,] _matrixB;

        public ImageTabsView()
        {
            InitializeComponent();
        }

        private void OnReloadClicked(object sender, EventArgs e)
        {
            SelectedImageA.Source = null;
            SelectedImageA.IsVisible = false;
            SelectedImageB.Source = null;
            SelectedImageB.IsVisible = false;
            ResultImage.Source = null;
            ResultImage.IsVisible = false;

            // Esconde os loading indicators
            LoadingIndicatorA.IsVisible = false;
            LoadingIndicatorA.IsRunning = false;
            LoadingIndicatorB.IsVisible = false;
            LoadingIndicatorB.IsRunning = false;
            LoadingIndicatorOperation.IsVisible = false;
            LoadingIndicatorOperation.IsRunning = false;

            ButtonImageA.Text = "Select Image";
            ButtonImageB.Text = "Select Image";
        }

        private async void OnSelectImageAClicked(object sender, EventArgs e)
        {
            await PickAndShowAsync(
                onBytesSet: bytes => ProcessImage(bytes, "A"),
                setImageSource: src => { SelectedImageA.Source = src; SelectedImageA.IsVisible = true; },
                setButtonText: text => ButtonImageA.Text = text,
                loadingIndicator: LoadingIndicatorA
            );
        }

        private async void OnSelectImageBClicked(object sender, EventArgs e)
        {
            await PickAndShowAsync(
                onBytesSet: bytes => ProcessImage(bytes, "B"),
                setImageSource: src => { SelectedImageB.Source = src; SelectedImageB.IsVisible = true; },
                setButtonText: text => ButtonImageB.Text = text,
                loadingIndicator: LoadingIndicatorB
            );
        }

        private static async Task PickAndShowAsync(
            Action<byte[]> onBytesSet,
            Action<ImageSource> setImageSource,
            Action<string> setButtonText,
            ActivityIndicator loadingIndicator)
        {
            // Mostra o loading
            loadingIndicator.IsVisible = true;
            loadingIndicator.IsRunning = true;

            var result = await MediaPicker.PickPhotoAsync();

            if (result == null)
            {
                loadingIndicator.IsVisible = false;
                loadingIndicator.IsRunning = false;
                return;
            }

            byte[] bytes;
            using (var picked = await result.OpenReadAsync())
            using (var ms = new MemoryStream())
            {
                await picked.CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            // Processa a imagem
            onBytesSet?.Invoke(bytes);

            // ImageSource segura que sempre cria um novo stream quando necessário
            var imageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
            setImageSource?.Invoke(imageSource);

            setButtonText?.Invoke(result.FileName);

            // Esconde o loading
            loadingIndicator.IsVisible = false;
            loadingIndicator.IsRunning = false;
        }

        private void ProcessImage(byte[] bytes, string id)
        {
            using var bmp = SkiaSharp.SKBitmap.Decode(bytes);
            if (bmp == null)
            {
                DisplayAlert("Erro", $"Não foi possível decodificar a imagem {id}.", "OK");
                return;
            }

            var matrix = ToMatrix(bmp); // função que monta a matriz RGBA

            if (id == "A")
            {
                _imageABytes = bytes;
                _matrixA = matrix;
            }
            else if (id == "B")
            {
                _imageBBytes = bytes;
                _matrixB = matrix;
            }
        }

        private static SkiaSharp.SKColor[,] ToMatrix(SkiaSharp.SKBitmap bmp)
        {
            var m = new SkiaSharp.SKColor[bmp.Height, bmp.Width];
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                    m[y, x] = bmp.GetPixel(x, y);
            return m;
        }

        private ImageSource MatrixToImageSource(SKColor[,] matrix)
        {
            int h = matrix.GetLength(0);
            int w = matrix.GetLength(1);

            // 1) Copiar a matriz para um SKBitmap
            using var bmp = new SKBitmap(w, h, true);
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    bmp.SetPixel(x, y, matrix[y, x]);

            // 2) Encodar como PNG em memória
            using var img = SKImage.FromBitmap(bmp);
            using var data = img.Encode(SKEncodedImageFormat.Png, 100);
            var bytes = data.ToArray();

            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }

        private async void OnSumClicked(object sender, EventArgs e)
        {
            if (_matrixA == null || _matrixB == null)
            {
                await DisplayAlert("Atenção", "Selecione as duas imagens (A e B) antes de somar.", "OK");
                return;
            }

            // Mostra loading
            LoadingIndicatorOperation.IsVisible = true;
            LoadingIndicatorOperation.IsRunning = true;

            // Executa operações pesadas em background
            var result = await Task.Run(() =>
            {
                var sumMatrix = ImageProcessor.Processing.ArithmeticOperations.Add(_matrixA, _matrixB);
                var src = MatrixToImageSource(sumMatrix);
                return src;
            });

            ResultImage.Source = result;
            ResultImage.IsVisible = true;

            // Esconde loading
            LoadingIndicatorOperation.IsVisible = false;
            LoadingIndicatorOperation.IsRunning = false;
            ResultLabel.IsVisible = true;
        }


        private async void SubtButton_Clicked(object sender, EventArgs e)
        {
            if (_matrixA == null || _matrixB == null)
            {
                await DisplayAlert("Atenção", "Selecione as duas imagens (A e B) antes de subtrair.", "OK");
                return;
            }

            // Mostra loading
            LoadingIndicatorOperation.IsVisible = true;
            LoadingIndicatorOperation.IsRunning = true;

            // Executa operações pesadas em background
            var result = await Task.Run(() =>
            {
                var sumMatrix = ImageProcessor.Processing.ArithmeticOperations.Subt(_matrixA, _matrixB);
                var src = MatrixToImageSource(sumMatrix);
                return src;
            });

            ResultImage.Source = result;
            ResultImage.IsVisible = true;

            // Esconde loading
            LoadingIndicatorOperation.IsVisible = false;
            LoadingIndicatorOperation.IsRunning = false;
            ResultLabel.IsVisible = true;
        }

        private async void MultButton_Clicked(object sender, EventArgs e)
        {
            if (_matrixA == null)
            {
                await DisplayAlert("Atenção", "Selecione a imagem A antes de multiplicar.", "OK");
                return;
            }

            string valueText = MultiplicationValue.Text?.Replace(',', '.') ?? "0";

            if (!float.TryParse(valueText,
                                System.Globalization.NumberStyles.Float,
                                System.Globalization.CultureInfo.InvariantCulture,
                                out float multiplicationValueVar))
            {
                await DisplayAlert("Erro", "Digite um valor numérico válido", "OK");
                return;
            }

            // Resto do código permanece igual...
            LoadingIndicatorOperation.IsVisible = true;
            LoadingIndicatorOperation.IsRunning = true;

            var result = await Task.Run(() =>
            {
                var sumMatrix = ImageProcessor.Processing.ArithmeticOperations.Multiplication(_matrixA, multiplicationValueVar);
                var src = MatrixToImageSource(sumMatrix);
                return src;
            });

            ResultImage.Source = result;
            ResultImage.IsVisible = true;
            LoadingIndicatorOperation.IsVisible = false;
            LoadingIndicatorOperation.IsRunning = false;
            ResultLabel.IsVisible = true;
        }
    }
}