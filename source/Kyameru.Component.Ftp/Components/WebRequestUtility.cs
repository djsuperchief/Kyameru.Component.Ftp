using Kyameru.Component.Ftp.Contracts;
using Kyameru.Component.Ftp.Enums;
using Kyameru.Component.Ftp.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Kyameru.Component.Ftp.Components
{
    internal class WebRequestUtility : IWebRequestUtility
    {
        public event EventHandler<string> OnLog;

        private readonly Dictionary<FtpOperation, string> ftpClientOperation = new Dictionary<FtpOperation, string>()
        {
            { FtpOperation.List, WebRequestMethods.Ftp.ListDirectory},
            { FtpOperation.Delete, WebRequestMethods.Ftp.DeleteFile },
            { FtpOperation.Download, WebRequestMethods.Ftp.DownloadFile },
            { FtpOperation.Upload, WebRequestMethods.Ftp.UploadFile }
        };

        public void DeleteFile(FtpSettings settings, string fileName, bool closeConnection = true)
        {
            FtpWebRequest request = this.GetFtpWebRequest($"{settings.Path}/{fileName}", FtpOperation.Delete, settings, closeConnection);
            request.GetResponse();
        }

        public byte[] DownloadFile(string fileName, FtpSettings settings)
        {
            byte[] file = null;
            FtpWebRequest request = this.GetFtpWebRequest($"{settings.Path}/{fileName}", FtpOperation.Download, settings);
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (MemoryStream responseStream = new MemoryStream())
            {
                response.GetResponseStream().CopyTo(responseStream);
                file = responseStream.ToArray();
            }

            return file;
        }

        public List<string> GetDirectoryContents(FtpSettings settings)
        {
            List<string> response = new List<string>();
            FtpWebRequest ftp = this.GetFtpWebRequest(settings.Path, FtpOperation.List, settings, false);
            using (FtpWebResponse ftpResponse = (FtpWebResponse)ftp.GetResponse())
            using (Stream responseStream = ftpResponse.GetResponseStream())
            {
                this.RaiseLog(Resources.INFO_GETTINGDIRECTORY);
                StreamReader reader = new StreamReader(responseStream);
                while (!reader.EndOfStream)
                {
                    string file = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Path.GetExtension(file)))
                    {
                        response.Add(file);
                    }
                }
            }

            return response;
        }

        public void UploadFile(byte[] file, FtpSettings settings, string fileName)
        {
            FtpWebRequest request = this.GetFtpWebRequest($"{settings.Path}/{fileName}", FtpOperation.Upload, settings, true);
            request.ContentLength = file.Length;
            using (Stream ftpStream = request.GetRequestStream())
            {
                ftpStream.Write(file, 0, file.Length);
            }
        }

        private FtpWebRequest GetFtpWebRequest(string path, FtpOperation method, Settings.FtpSettings settings, bool closeConnection = false)
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

        private void RaiseLog(string message)
        {
            this.OnLog?.Invoke(this, message);
        }
    }
}