using SmartHouse.Lib;
using SmartHouse.UWPLib.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;
using SmartHouse.UWPLib.Service;

namespace SmartHouse.UWPClient.ViewModels
{
    public class ImageViewerViewModel : BaseViewModel
    {
        private bool _disableUpload;

        private DelegateCommand _uploadCommand;
        public DelegateCommand UploadCommand => _uploadCommand ?? (_uploadCommand = new DelegateCommand(UploadImage,() => !_disableUpload));

        public string FileName
        {
            get => Get<string>();
            set => Set(value);
        }

        public string Message
        {
            get => Get<string>();
            set => Set(value);
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            FileName = parameter.ToString();
            return Task.CompletedTask;
        }

        private async void UploadImage()
        {
            try
            {
                Message = "Starting to upload";
                _disableUpload = true;
                _uploadCommand.RaiseCanExecuteChanged();

                var service = new SmartHouseService();
                var file = await StorageFile.GetFileFromPathAsync(FileName);

                using (var stream = await file.OpenStreamForReadAsync())
                using (var binaryReader = new BinaryReader(stream))
                {
                    var bytes = binaryReader.ReadBytes((int)stream.Length);
                    var model = GetModelData(bytes);

                    await service.UploadContent(model);
                }

                Message = "Upload complete";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _disableUpload = false;
                _uploadCommand.RaiseCanExecuteChanged();
            }
        }

        private ContentUploadModel GetModelData(byte[] bytes)
        {
            return new ContentUploadModel()
            {
                Application = "Smart House",
                ContentCategoryEnum = ContentCategoryEnum.Image,
                ContentBase64 = Convert.ToBase64String(bytes)
            };
        }

        public async Task SaveFile()
        {
            var imageFile = await StorageFile.GetFileFromPathAsync(FileName);

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Images", new List<string>() { ".jpg" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = imageFile.DisplayName;

            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                await imageFile.CopyAndReplaceAsync(file);
                var status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    Message = "File " + file.Name + " was saved.";
                }
                else
                {
                    Message = "File " + file.Name + " couldn't be saved.";
                }
            }
            else
            {
                Message = "Operation cancelled.";
            }
        }

    }
}
