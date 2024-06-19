using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Project.IO.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Components.Pages
{
    partial class DocumentOverviewPage
    {

        private FileUtil fileUtil = new FileUtil();

        private async Task FileUploading(InputFileChangeEventArgs e)
        {
            await fileUtil.HandleFileSelected(e);
        }

    }
}
