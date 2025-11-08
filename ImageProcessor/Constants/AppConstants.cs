namespace ImageProcessor.Constants
{
    public static class AppConstants
    {
        public static class Messages
        {
            public const string SelectTwoImages = "Select both images (A and B) before executing the operation.";
            public const string SelectImageA = "Select image A before executing the operation.";
            public const string InvalidNumericValue = "Enter a valid numeric value";
            public const string DivisionByZero = "Cannot divide by zero";
            public const string NoImageToSave = "No image to save.";
            public const string ImageSavedSuccessfully = "Image saved successfully!";
            public const string SaveError = "Could not save the image.";
            public const string ProcessingError = "Error during processing: {0}";
            public const string ImageLoadError = "Could not load the image: {0}";
            public const string BinaryImageRequired = "Dilation operation requires a binary image (black and white only, values 0 or 255).";
        }

        public static class UI
        {
            public const string SelectImageA = "Select Image A";
            public const string SelectImageB = "Select Image B";
            public const string Processing = "Processing";
            public const string Resultado = "Result";
            public const string ArithmeticOperations = "Arithmetic Operations";
            public const string ImageOperations = "Image Operations";
            public const string ValueOperations = "Value Operations";
            public const string ImageTransformations = "Image Transformations";
        }

        public static class FileNames
        {
            public const string ResultPrefix = "result";
        }
    }
}
