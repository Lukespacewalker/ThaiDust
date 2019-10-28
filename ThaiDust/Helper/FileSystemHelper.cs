using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;

namespace ThaiDust.Helper
{
    public static class FileSystemHelper
    {
        public static async Task<StorageFile> CreateFile(string filename = null)
        {
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Microsoft Excel Document", new List<string>() { ".xlsx" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = filename ?? "Station";

            StorageFile file = await savePicker.PickSaveFileAsync();
            return file;
            //if (file != null)
            //{
            //    // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
            //    CachedFileManager.DeferUpdates(file);
            //    // write to file
            //    await FileIO.WriteTextAsync(file, file.Name);
            //    // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
            //    // Completing updates may require Windows to ask for user input.
            //    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            //    if (status == FileUpdateStatus.Complete)
            //    {
            //        OutputTextBlock.Text = "File " + file.Name + " was saved.";
            //    }
            //    else
            //    {
            //        OutputTextBlock.Text = "File " + file.Name + " couldn't be saved.";
            //    }
            //}
        }
    }
}