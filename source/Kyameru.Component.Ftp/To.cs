using Kyameru.Core.Contracts;
using Kyameru.Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kyameru.Component.Ftp
{
    public class To : IToComponent
    {
        private readonly FtpSettings ftpSettings;
        private readonly FtpClient ftpClient;
        private readonly string archivePath;
        private readonly string source;

        public To(Dictionary<string, string> headers)
        {
            Dictionary<string, string> config = headers.ToToConfig();
            this.archivePath = config.GetKeyValue("Archive");
            this.archivePath = config.GetKeyValue("Source");
            this.ftpSettings = new FtpSettings(config);
            this.ftpClient = new FtpClient(this.ftpSettings);
        }

        /// <summary>
        /// Logging event.
        /// </summary>
        public event EventHandler<Log> OnLog;

        /// <summary>
        /// Process Message.
        /// </summary>
        /// <param name="item">Message to process.</param>
        public void Process(Routable item)
        {
            try
            {
                this.ftpClient.UploadFile(this.GetSource(item), item.Headers.GetKeyValue("SourceFile"));
            }
            catch (Exception ex)
            {
                item.SetInError(this.GetError("Upload", string.Format(Resources.ERROR_UPLOADING, item.Headers.GetKeyValue("FileName"))));
                this.RaiseLog(string.Format(Resources.ERROR_UPLOADING, item.Headers.GetKeyValue("FileName")), LogLevel.Error, ex);
            }
        }

        private byte[] GetSource(Routable item)
        {
            byte[] response = null;
            if (this.source == "File" || string.IsNullOrWhiteSpace(this.source))
            {
                response = System.IO.File.ReadAllBytes(item.Headers.GetKeyValue("FullSource"));
            }
            else
            {
                response = (byte[])item.Body;
            }

            return response;
        }

        private Log RaiseLog(string message, LogLevel logLevel, Exception ex = null)
        {
            return new Log(logLevel, message, ex);
        }

        private Error GetError(string action, string message)
        {
            return new Error("ToFtp", action, message);
        }
    }
}