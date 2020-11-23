using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Kyameru.Component.Ftp
{
    /// <summary>
    /// Ftp Client.
    /// </summary>
    internal class FtpClient
    {
        private readonly FtpSettings settings;

        private const string TMPDIR = "ftp_temp";

        private enum ftpOperation
        {
            List,
            Delete,
            Download,
            Upload
        }

        private readonly Dictionary<ftpOperation, string> ftpClientOperation = new Dictionary<ftpOperation, string>()
        {
            { ftpOperation.List, WebRequestMethods.Ftp.ListDirectory},
            { ftpOperation.Delete, WebRequestMethods.Ftp.DeleteFile },
            { ftpOperation.Download, WebRequestMethods.Ftp.DownloadFile },
            { ftpOperation.Upload, WebRequestMethods.Ftp.UploadFile }
        };

        public FtpClient(FtpSettings ftpSettings)
        {
            this.settings = ftpSettings;
        }

        public event EventHandler<string> OnLog;

        public event EventHandler<Exception> OnError;

        internal void Poll(object state)
        {
            List<string> files = this.GetDirectoryContents();
            if (files.Count > 0)
            {
                DownloadFiles(files);
                DeleteFiles(files);
            }
        }

        private void DeleteFiles(List<string> files)
        {
            if (this.settings.Delete)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    FtpWebRequest ftp = this.GetFtpRequest($"{this.settings.Path}/{files[i]}", ftpOperation.Delete, false);
                    try
                    {
                        using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
                        {
                            this.RaiseLog(string.Format(Resources.INFO_DELETINGFILE, files[i]));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.RaiseError(ex);
                    }
                }
            }
        }

        private void DownloadFiles(List<string> files)
        {
            using (WebClient ftpClient = new WebClient())
            {
                if (this.settings.Credentials != null)
                {
                    ftpClient.Credentials = this.settings.Credentials;
                }

                for (int i = 0; i < files.Count; i++)
                {
                    try
                    {
                        string path = $"ftp://{this.settings.Host}:{this.settings.Port}/{this.settings.Path}/{files[i]}";
                        string transfer = $"{TMPDIR}/{files[i]}";
                        if (!Directory.Exists(TMPDIR))
                        {
                            Directory.CreateDirectory(TMPDIR);
                        }

                        ftpClient.DownloadFile(path, transfer);
                    }
                    catch (Exception ex)
                    {
                        this.RaiseError(ex);
                    }
                }
            }
        }

        private List<string> GetDirectoryContents()
        {
            List<string> response = new List<string>();
            FtpWebRequest ftp = this.GetFtpRequest(this.settings.Path, ftpOperation.List);
            try
            {
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
            }
            catch (Exception ex)
            {
                this.RaiseError(ex);
            }

            return response;
        }

        private FtpWebRequest GetFtpRequest(string path, ftpOperation method, bool keepOpen = true)
        {
            FtpWebRequest response = (FtpWebRequest)WebRequest.Create($"ftp://{this.settings.Host}:{this.settings.Port}/{path}");
            response.Method = this.ftpClientOperation[method];
            response.UseBinary = true;
            response.KeepAlive = keepOpen;
            if (this.settings.Credentials != null)
            {
                response.Credentials = this.settings.Credentials;
            }

            return response;
        }

        private void RaiseLog(string message)
        {
            this.OnLog?.Invoke(this, message);
        }

        private void RaiseError(Exception ex)
        {
            this.OnError?.Invoke(this, ex);
        }
    }
}