using Microsoft.Maui.Storage;

namespace ImageProcessor.Services
{
    public interface IImageSaveService
    {
        Task<bool> SaveImageAsync(byte[] imageBytes, string fileName);
        Task<bool> RequestStoragePermissionsAsync();
    }

    public class ImageSaveService : IImageSaveService
    {
        public async Task<bool> SaveImageAsync(byte[] imageBytes, string fileName)
        {
            try
            {
                bool hasPermission = await RequestStoragePermissionsAsync();
                if (!hasPermission)
                    return false;

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string finalFileName = $"{fileName}_{timestamp}.png";

#if ANDROID
                return await SaveImageAndroidAsync(imageBytes, finalFileName);
#else
                return await SaveImageWindowsAsync(imageBytes, finalFileName);
#endif
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RequestStoragePermissionsAsync()
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
                return status == PermissionStatus.Granted;
            }
            catch (Exception)
            {
                return false;
            }
        }

#if ANDROID
        private async Task<bool> SaveImageAndroidAsync(byte[] imageBytes, string fileName)
        {
            try
            {
                var picturesPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
                var appFolder = new Java.IO.File(picturesPath, "ImageProcessor");

                if (!appFolder.Exists())
                    appFolder.Mkdirs();

                var file = new Java.IO.File(appFolder, fileName);
                var filePath = file.AbsolutePath;

                await File.WriteAllBytesAsync(filePath, imageBytes);

                var context = Platform.CurrentActivity ?? Android.App.Application.Context;
                Android.Media.MediaScannerConnection.ScanFile(context, new[] { filePath }, new[] { "image/png" }, null);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
#endif

        private async Task<bool> SaveImageWindowsAsync(byte[] imageBytes, string fileName)
        {
            try
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var filePath = Path.Combine(documentsPath, fileName);
                await File.WriteAllBytesAsync(filePath, imageBytes);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
