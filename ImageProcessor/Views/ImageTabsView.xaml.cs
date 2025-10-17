using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SkiaSharp;
using ImageProcessor.Processing;
using ImageProcessor.Services;
using ImageProcessor.ViewModels;
using ImageProcessor.Constants;
using Microcharts;
using System.Linq;

namespace ImageProcessor.Views
{
    public partial class ImageTabsView : ContentPage
    {
        private readonly ImageProcessingViewModel _viewModel;
        private readonly IImageSelectionService _imageSelectionService;
        private readonly IImageSaveService _imageSaveService;

        // Default constructor for XAML instantiation
        public ImageTabsView()
        {
            InitializeComponent();
            // Note: This constructor is required for XAML but won't work properly
            // The proper constructor with DI should be used instead
        }

        public ImageTabsView(ImageProcessingViewModel viewModel, IImageSelectionService imageSelectionService, IImageSaveService imageSaveService)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _imageSelectionService = imageSelectionService;
            _imageSaveService = imageSaveService;
            BindingContext = _viewModel;
        }

        private void OnReloadClicked(object sender, EventArgs e)
        {
            _viewModel.Reset();
            ResetUI();
        }

        private void ResetUI()
        {
            SelectedImageA.Source = null;
            SelectedImageA.IsVisible = false;
            SelectedImageB.Source = null;
            SelectedImageB.IsVisible = false;
            ResultImage.Source = null;
            ResultImage.IsVisible = false;
            LoadingIndicatorA.IsVisible = false;
            LoadingIndicatorA.IsRunning = false;
            LoadingIndicatorB.IsVisible = false;
            LoadingIndicatorB.IsRunning = false;
            LoadingIndicatorOperation.IsVisible = false;
            LoadingIndicatorOperation.IsRunning = false;
            ButtonImageA.Text = AppConstants.UI.SelectImageA;
            ButtonImageB.Text = AppConstants.UI.SelectImageB;
            SaveResultButton.IsVisible = false;
            HistogramChartBefore.Chart = null;
            HistogramChartAfter.Chart = null;
            HistogramContainer.IsVisible = false;
        }

        private async void OnSelectImageAClicked(object sender, EventArgs e)
        {
            await SelectImageAsync(isImageA: true);
        }

        private async void OnSelectImageBClicked(object sender, EventArgs e)
        {
            await SelectImageAsync(isImageA: false);
        }

        private async Task SelectImageAsync(bool isImageA)
        {
            var loadingIndicator = isImageA ? LoadingIndicatorA : LoadingIndicatorB;
            var imageControl = isImageA ? SelectedImageA : SelectedImageB;
            var buttonControl = isImageA ? ButtonImageA : ButtonImageB;

            loadingIndicator.IsVisible = true;
            loadingIndicator.IsRunning = true;

            try
            {
                var result = await _imageSelectionService.PickImageAsync();
                if (result != null)
                {
                    if (isImageA)
                        _viewModel.MatrixA = result.Matrix;
                    else
                        _viewModel.MatrixB = result.Matrix;

                    imageControl.Source = _imageSelectionService.CreateImageSource(result.Bytes);
                    imageControl.IsVisible = true;
                    buttonControl.Text = result.FileName;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", string.Format(AppConstants.Messages.ImageLoadError, ex.Message), "OK");
            }
            finally
            {
                loadingIndicator.IsVisible = false;
                loadingIndicator.IsRunning = false;
            }
        }

        private async Task ProcessOperationAsync(Func<Task<ImageSource?>> operation, string errorMessage)
        {
            LoadingIndicatorOperation.IsVisible = true;
            LoadingIndicatorOperation.IsRunning = true;

            try
            {
                var result = await operation();
                if (result != null)
                {
                    ResultImage.Source = result;
                    ResultImage.IsVisible = true;
                    ResultLabel.IsVisible = true;
                    SaveResultButton.IsVisible = true;
                }
                else
                {
                    await DisplayAlert("Attention", errorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", string.Format(AppConstants.Messages.ProcessingError, ex.Message), "OK");
            }
            finally
            {
                LoadingIndicatorOperation.IsVisible = false;
                LoadingIndicatorOperation.IsRunning = false;
            }
        }

        // Arithmetic Operations - Two Images
        private async void OnSumClicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessTwoImagesAsync(Processing.ArithmeticOperations.Add),
                AppConstants.Messages.SelectTwoImages
            );
        }

        private async void SubtButton_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessTwoImagesAsync(Processing.ArithmeticOperations.Subt),
                "Select both images (A and B) before subtracting."
            );
        }

        private async void AbsoluteDifferenceButton_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessTwoImagesAsync(Processing.ArithmeticOperations.AbsoluteDifference),
                "Select both images (A and B) before calculating the difference."
            );
        }

        private async void AverageButton_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessTwoImagesAsync(Processing.ArithmeticOperations.Average),
                "Select both images (A and B) before calculating the average."
            );
        }

        // Arithmetic Operations - Single Image with Value
        private async void SumValueButton_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.ValidateImages(_viewModel.MatrixA))
            {
                await DisplayAlert("Attention", "Select image A before adding.", "OK");
                return;
            }

            if (!_viewModel.TryParseFloat(SumValue.Text, out float value))
            {
                await DisplayAlert("Error", "Enter a valid numeric value", "OK");
                return;
            }

            await ProcessOperationAsync(
                () => _viewModel.ProcessImageWithValueAsync(value, Processing.ArithmeticOperations.AddValue),
                "Error processing image."
            );
        }

        private async void SubtValueButton_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.ValidateImages(_viewModel.MatrixA))
            {
                await DisplayAlert("Attention", "Select image A before subtracting.", "OK");
                return;
            }

            if (!_viewModel.TryParseFloat(SubtValue.Text, out float value))
            {
                await DisplayAlert("Error", "Enter a valid numeric value", "OK");
                return;
            }

            await ProcessOperationAsync(
                () => _viewModel.ProcessImageWithValueAsync(value, Processing.ArithmeticOperations.SubtValue),
                "Error processing image."
            );
        }

        private async void MultButton_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.ValidateImages(_viewModel.MatrixA))
            {
                await DisplayAlert("Attention", "Select image A before multiplying.", "OK");
                return;
            }

            if (!_viewModel.TryParseFloat(MultiplicationValue.Text, out float value))
            {
                await DisplayAlert("Error", "Enter a valid numeric value", "OK");
                return;
            }

            await ProcessOperationAsync(
                () => _viewModel.ProcessImageWithValueAsync(value, Processing.ArithmeticOperations.Multiplication),
                "Error processing image."
            );
        }

        private async void DivisionButton_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.ValidateImages(_viewModel.MatrixA))
            {
                await DisplayAlert("Attention", "Select image A before dividing.", "OK");
                return;
            }

            if (!_viewModel.TryParseFloat(DivisionValue.Text, out float value))
            {
                await DisplayAlert("Error", "Enter a valid numeric value", "OK");
                return;
            }

            if (Math.Abs(value) < float.Epsilon)
            {
                await DisplayAlert("Error", "Cannot divide by zero", "OK");
                return;
            }

            await ProcessOperationAsync(
                () => _viewModel.ProcessImageWithValueAsync(value, Processing.ArithmeticOperations.Division),
                "Error processing image."
            );
        }

        private async void BlendingButton_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.ValidateImages(_viewModel.MatrixA, _viewModel.MatrixB))
            {
                await DisplayAlert("Attention", "Select both images (A and B) before blending.", "OK");
                return;
            }

            if (!_viewModel.TryParseFloat(BlendingRatio.Text, out float ratio))
            {
                await DisplayAlert("Error", "Enter a valid numeric value", "OK");
                return;
            }

            await ProcessOperationAsync(
                () => _viewModel.ProcessImageWithValueAsync(ratio, (a, v) => Processing.ArithmeticOperations.LinearBlending(a, _viewModel.MatrixB, v)),
                "Error processing image."
            );
        }

        // Image Transformations
        private async void ConvertToGrayScale_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ArithmeticOperations.ConvertToGrayScale),
                "Select image A before converting."
            );
        }

        private async void FlipLeftToRight_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ArithmeticOperations.FlipLeftToRight),
                "Select image A before converting."
            );
        }

        private async void FlipTopToBottom_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ArithmeticOperations.FlipTopToBottom),
                "Select image A before converting."
            );
        }

        private async void NegativeButton_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ArithmeticOperations.ImageNegative),
                "Select image A before converting."
            );
        }

        private async void ThresholdButton_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ArithmeticOperations.Thresholding),
                "Select image A before converting."
            );
        }

        private async void HistogramEqualizationButton_Clicked(object sender, EventArgs e)
        {
            LoadingIndicatorOperation.IsVisible = true;
            LoadingIndicatorOperation.IsRunning = true;

            try
            {
                bool success = await _viewModel.ProcessHistogramEqualizationAsync();
                if (success)
                {
                    ResultImage.Source = _viewModel.ResultImage;
                    ResultImage.IsVisible = true;
                    ResultLabel.IsVisible = true;
                    SaveResultButton.IsVisible = true;

                    HistogramChartBefore.Chart = _viewModel.HistogramBefore;
                    HistogramChartAfter.Chart = _viewModel.HistogramAfter;
                    HistogramContainer.IsVisible = true;
                }
                else
                {
                    await DisplayAlert("Attention", "Select image A before converting.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", string.Format(AppConstants.Messages.ProcessingError, ex.Message), "OK");
            }
            finally
            {
                LoadingIndicatorOperation.IsVisible = false;
                LoadingIndicatorOperation.IsRunning = false;
            }
        }

        private async void OnSaveResultClicked(object sender, EventArgs e)
        {
            var imageBytes = _viewModel.GetLastProcessedImageBytes();
            if (imageBytes == null)
            {
                await DisplayAlert("Attention", "No image to save.", "OK");
                return;
            }

            bool success = await _imageSaveService.SaveImageAsync(imageBytes, "result");
            if (success)
            {
                await DisplayAlert("Success", "Image saved successfully!", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Could not save the image.", "OK");
            }
        }
    }
}
