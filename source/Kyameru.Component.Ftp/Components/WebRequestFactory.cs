using Kyameru.Component.Ftp.Contracts;
using Kyameru.Component.Ftp.Enums;
using System.Collections.Generic;
using System.Net;

namespace Kyameru.Component.Ftp.Components
{
    internal class WebRequestFactory : IWebRequestFactory
    {
        private readonly Dictionary<FtpOperation, string> ftpClientOperation = new Dictionary<FtpOperation, string>()
        {
            { FtpOperation.List, WebRequestMethods.Ftp.ListDirectory},
            { FtpOperation.Delete, WebRequestMethods.Ftp.DeleteFile },
            { FtpOperation.Download, WebRequestMethods.Ftp.DownloadFile },
            { FtpOperation.Upload, WebRequestMethods.Ftp.UploadFile }
        };

        public FtpWebRequest GetFtpWebRequest(string path, FtpOperation method, Settings.FtpSettings settings, bool closeConnection = false)
        {
            FtpWebRequest response = (FtpWebRequest)WebRequest.Create($"ftp://{settings.Host}:{settings.Port}/{path}");
            response.Method = this.ftpClientOperation[method];
            response.UseBinary = true;
            response.KeepAlive = !closeConnection;
            if (settings.Credentials != null)
            {
                response.Credentials = settings.Credentials;
            }

            return response;
        }
    }
}