namespace ImageProcessor.Constants
{
    public static class AppConstants
    {
        public static class Messages
        {
            public const string SelectTwoImages = "Selecione as duas imagens (A e B) antes de executar a operação.";
            public const string SelectImageA = "Selecione a imagem A antes de executar a operação.";
            public const string InvalidNumericValue = "Digite um valor numérico válido";
            public const string DivisionByZero = "Não é possível dividir por zero";
            public const string NoImageToSave = "Não há imagem para salvar.";
            public const string ImageSavedSuccessfully = "Imagem salva com sucesso!";
            public const string SaveError = "Não foi possível salvar a imagem.";
            public const string ProcessingError = "Erro durante o processamento: {0}";
            public const string ImageLoadError = "Não foi possível carregar a imagem: {0}";
        }

        public static class UI
        {
            public const string SelectImageA = "Select Image A";
            public const string SelectImageB = "Select Image B";
            public const string Processing = "Processing";
            public const string Resultado = "Resultado";
            public const string ArithmeticOperations = "Arithmetic Operations";
            public const string ImageOperations = "Image Operations";
            public const string ValueOperations = "Value Operations";
            public const string ImageTransformations = "Image Transformations";
        }

        public static class FileNames
        {
            public const string ResultPrefix = "resultado";
        }
    }
}
