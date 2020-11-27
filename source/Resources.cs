using System;
using System.Collections.Generic;
using System.Text;

namespace Kyameru.Component.Ftp
{
    internal static class Resources
    {
        public const string INFO_GETTINGDIRECTORY = "Getting directory contents...";
        public const string INFO_GETTINGFILE = "Downloading file {0}";
        public const string INFO_DELETINGFILE = "Deleting file {0}";
        public const string INFO_UPLOADING = "Uploading file {0}";

        public const string ERROR_FTPPROCESSING = "Error processing FTP request, see inner exception.";
        public const string ERROR_UPLOADING = "Error uploading file {0}. See inner exception.";
    }
}