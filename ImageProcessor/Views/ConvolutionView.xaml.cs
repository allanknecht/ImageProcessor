using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SkiaSharp;
using ImageProcessor.Services;
using ImageProcessor.ViewModels;
using ImageProcessor.Constants;
using ImageProcessor.Processing;

namespace ImageProcessor.Views
{
    public partial class ConvolutionView : ContentPage
    {
        private readonly ConvolutionViewModel _viewModel;
        private readonly IImageSelectionService _imageSelectionService;
        private readonly IImageSaveService _imageSaveService;

        // Default constructor for XAML instantiation
        public ConvolutionView()
        {
            InitializeComponent();
            // Note: This constructor is required for XAML but won't work properly
            // The proper constructor with DI should be used instead
        }

        public ConvolutionView(ConvolutionViewModel viewModel, IImageSelectionService imageSelectionService, IImageSaveService imageSaveService)
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
            SelectedImage.Source = null;
            SelectedImage.IsVisible = false;
            ResultImage.Source = null;
            ResultImage.IsVisible = false;
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            LoadingIndicatorOperation.IsVisible = false;
            LoadingIndicatorOperation.IsRunning = false;
            ButtonSelectImage.Text = "Select Image";
            SaveResultButton.IsVisible = false;
            ResultLabel.IsVisible = false;
        }

        private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            try
            {
                var result = await _imageSelectionService.PickImageAsync();
                if (result != null)
                {
                    _viewModel.Matrix = result.Matrix;
                    SelectedImage.Source = _imageSelectionService.CreateImageSource(result.Bytes);
                    SelectedImage.IsVisible = true;
                    ButtonSelectImage.Text = result.FileName;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", string.Format(AppConstants.Messages.ImageLoadError, ex.Message), "OK");
            }
            finally
            {
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
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

        private async void OnSaveResultClicked(object sender, EventArgs e)
        {
            var imageBytes = _viewModel.GetLastProcessedImageBytes();
            if (imageBytes == null)
            {
                await DisplayAlert("Attention", "No image to save.", "OK");
                return;
            }

            bool success = await _imageSaveService.SaveImageAsync(imageBytes, "convolution_result");
            if (success)
            {
                await DisplayAlert("Success", "Image saved successfully!", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Could not save the image.", "OK");
            }
        }

        private async void Mean_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.MeanFilter),
                "Select an image before applying the mean filter."
            );
        }

        private async void Min_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.MinFilter),
                "Select an image before applying the min filter."
            );
        }


        private async void Max_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.MaxFilter),
                "Select an image before applying the max filter."
            );
        }


        private async void Median_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.MedianFilter),
                "Select an image before applying the median filter."
            );
        }

        private async void Order_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.TryParseInt(OrderEntry.Text, out var order) || order < 0 || order > 8)
            {
                await DisplayAlert("Error", "Enter a valid integer (>= 0 and <= 8) for the order.", "OK");
                return;
            }

            await ProcessOperationAsync(
                () => _viewModel.ProcessImageWithParameterAsync(order, Processing.ConvolutionOperations.OrderFilter),
                "Select an image before applying the order filter."
            );
        }

        private async void ConservativeSmoothingButton_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
            () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.ConservativeSmoothing),
            "Select an image before applying the Conservative Smoothing filter."
        );
        }

        private async void GaussianBlur_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.TryParseDouble(SigmaEntry.Text, out var sigma) || sigma <= 0.0)
            {
                await DisplayAlert("Error", "Enter a valid sigma value (> 0).", "OK");
                return;
            }

            await ProcessOperationAsync(
                () => _viewModel.ProcessImageWithDoubleParameterAsync(
                    sigma,
                    ConvolutionOperations.GaussianBlur
                ),
                "Select an image before applying the Gaussian Blur."
            );
        }

        private async void Prewit_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
            () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.Prewit),
            "Select an image before applying the Conservative Smoothing filter."
            );
        }

        private async void Sobel_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
            () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.Sobel),
            "Select an image before applying the Conservative Smoothing filter."
            );
        }

        private async void Laplacian_Clicked(object sender, EventArgs e)
        {
            await ProcessOperationAsync(
            () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.Laplacian),
            "Select an image before applying the Conservative Smoothing filter."
            );
        }

        private async void Dilation_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.ValidateImage(_viewModel.Matrix))
            {
                await DisplayAlert("Attention", "Select an image before applying the Dilation operation.", "OK");
                return;
            }

            // if (!_viewModel.ValidateBinaryImage(_viewModel.Matrix))
            // {
            //     await DisplayAlert("Error", AppConstants.Messages.BinaryImageRequired, "OK");
            //     return;
            // }

            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.Dilation),
                "Select an image before applying the Dilation operation."
            );
        }

        private async void Erosion_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.ValidateImage(_viewModel.Matrix))
            {
                await DisplayAlert("Attention", "Select an image before applying the Dilation operation.", "OK");
                return;
            }

            // if (!_viewModel.ValidateBinaryImage(_viewModel.Matrix))
            // {
            //     await DisplayAlert("Error", AppConstants.Messages.BinaryImageRequired, "OK");
            //     return;
            // }

            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.Erosion),
                "Select an image before applying the Dilation operation."
            );
        }

        private async void Opening_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.ValidateImage(_viewModel.Matrix))
            {
                await DisplayAlert("Attention", "Select an image before applying the Opening operation.", "OK");
                return;
            }

            // if (!_viewModel.ValidateBinaryImage(_viewModel.Matrix))
            // {
            //     await DisplayAlert("Error", AppConstants.Messages.BinaryImageRequired, "OK");
            //     return;
            // }

            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.Opening),
                "Select an image before applying the Opening operation."
            );
        }

        private async void Closing_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.ValidateImage(_viewModel.Matrix))
            {
                await DisplayAlert("Attention", "Select an image before applying the Closing operation.", "OK");
                return;
            }

            //if (!_viewModel.ValidateBinaryImage(_viewModel.Matrix))
            //{
            //    await DisplayAlert("Error", AppConstants.Messages.BinaryImageRequired, "OK");
            //    return;
            //}

            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.Closing),
                "Select an image before applying the Closing operation."
            );
        }

        private async void Contour_Clicked(object sender, EventArgs e)
        {
            if (!_viewModel.ValidateImage(_viewModel.Matrix))
            {
                await DisplayAlert("Attention", "Select an image before applying the Contour operation.", "OK");
                return;
            }

            // if (!_viewModel.ValidateBinaryImage(_viewModel.Matrix))
            // {
            //     await DisplayAlert("Error", AppConstants.Messages.BinaryImageRequired, "OK");
            //     return;
            // }

            await ProcessOperationAsync(
                () => _viewModel.ProcessImageAsync(Processing.ConvolutionOperations.Contour),
                "Select an image before applying the Contour operation."
            );
        }

    }
}
