using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace Project.IO.Utilities
{
    internal class FileUtil
    {
        public async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            var uploadDirectory = Path.Combine(AppContext.BaseDirectory, "ProjectFiles");

            // Ensure directory exists
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            foreach (var file in e.GetMultipleFiles())
            {
                var path = Path.Combine(uploadDirectory, file.Name);

                // Save the file
                using var stream = file.OpenReadStream();
                using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(fileStream);

                Debug.WriteLine($"File saved to {path}");
            }
        }
    }
}
