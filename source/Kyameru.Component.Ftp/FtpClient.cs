using Kyameru.Component.Ftp.Contracts;
using Kyameru.Component.Ftp.Enums;
using Kyameru.Component.Ftp.Extensions;
using Kyameru.Component.Ftp.Settings;
using Kyameru.Core.Entities;
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
        private readonly IWebRequestUtility webRequestUtility;
        private const string TMPDIR = "ftp_temp";

        public FtpClient(FtpSettings ftpSettings, IWebRequestUtility webRequestUtility)
        {
            this.settings = ftpSettings;
            this.webRequestUtility = webRequestUtility;
        }

        public event EventHandler<string> OnLog;

        public event EventHandler<Exception> OnError;

        public event EventHandler<Kyameru.Core.Entities.Routable> OnDownloadFile;

        internal void Poll()
        {
            List<string> files = this.GetDirectoryContents();
            if (files.Count > 0)
            {
                DownloadFiles(files);
                DeleteFiles(files);
            }
        }

        internal void UploadFile(string fileSource)
        {
            this.UploadFile(System.IO.File.ReadAllBytes(fileSource), System.IO.Path.GetFileName(fileSource));
        }

        internal void UploadFile(byte[] file, string name)
        {
            try
            {
                this.RaiseLog(string.Format(Resources.INFO_UPLOADING, name));
                this.webRequestUtility.UploadFile(file, this.settings, name);
            }
            catch (Exception ex)
            {
                this.RaiseError(ex);
            }
        }

        private void DeleteFiles(List<string> files)
        {
            if (this.settings.Delete)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    bool closeConnection = i == files.Count - 1;
                    try
                    {
                        this.RaiseLog(string.Format(Resources.INFO_DELETINGFILE, files[i]));
                        this.webRequestUtility.DeleteFile(settings, files[i], closeConnection);
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
            for (int i = 0; i < files.Count; i++)
            {
                try
                {
                    string transfer = $"{TMPDIR}/{files[i]}";
                    if (!Directory.Exists(TMPDIR))
                    {
                        Directory.CreateDirectory(TMPDIR);
                    }
                    byte[] file = this.webRequestUtility.DownloadFile(files[i], this.settings);
                    this.CreateAndRoute(transfer, file);
                }
                catch (Exception ex)
                {
                    this.RaiseError(ex);
                }
            }
        }

        private void CreateAndRoute(string sourceFile, byte[] file)
        {
            FileInfo info = new FileInfo(sourceFile);
            sourceFile = sourceFile.Replace("\\", "/");
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("SourceDirectory", System.IO.Path.GetDirectoryName(sourceFile));
            headers.Add("SourceFile", System.IO.Path.GetFileName(sourceFile));
            headers.Add("FullSource", sourceFile);
            headers.Add("DateCreated", info.CreationTimeUtc.ToLongDateString());
            headers.Add("Readonly", info.IsReadOnly.ToString());
            headers.Add("DataType", "byte");
            headers.Add("FtpSource", this.ConstructFtpUri(this.settings.Path, System.IO.Path.GetFileName(sourceFile)));
            Routable dataItem = new Routable(headers, file);
            this.RaiseOnDownload(dataItem);
        }

        private List<string> GetDirectoryContents()
        {
            List<string> response = null;
            try
            {
                response = this.webRequestUtility.GetDirectoryContents(this.settings);
            }
            catch (Exception ex)
            {
                this.RaiseError(ex);
            }

            return response;
        }

        private string ConstructFtpUri(string path, string file)
        {
            StringBuilder response = new StringBuilder($"ftp://{this.settings.Host}:{this.settings.Port}/");
            if (!path.IsNullOrEmptyPath())
            {
                response.Append($"{path}/");
            }

            if (!string.IsNullOrWhiteSpace(file))
            {
                response.Append($"{file}");
            }

            return response.ToString();
        }

        private void RaiseLog(string message)
        {
            this.OnLog?.Invoke(this, message);
        }

        private void RaiseError(Exception ex)
        {
            this.OnError?.Invoke(this, ex);
        }

        private void RaiseOnDownload(Routable routable)
        {
            this.OnDownloadFile?.Invoke(this, routable);
        }
    }
}