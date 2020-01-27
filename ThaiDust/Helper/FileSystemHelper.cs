using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using ThaiDust.Core.Helper;

namespace ThaiDust.Helper
{
    public class FilePicker : IFilePicker
    {
        public async Task<Stream> CreateFile(string filename = null)
        {
            var savePicker = new FileSavePicker {SuggestedStartLocation = PickerLocationId.DocumentsLibrary};
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Microsoft Excel Document", new List<string>() {".xlsx"});
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = filename ?? "Station";

            StorageFile file = await savePicker.PickSaveFileAsync();
            return await file?.OpenStreamForWriteAsync();
        }
    }
}