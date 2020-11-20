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
        private readonly FtpWebRequest ftp;

        private readonly FtpSettings settings;

        public FtpClient(FtpSettings ftpSettings)
        {
            this.settings = ftpSettings;
            this.ftp = (FtpWebRequest)WebRequest.Create($"ftp://{this.settings.Host}:{this.settings.Port}");
            this.ftp.Method = WebRequestMethods.Ftp.ListDirectory;
            if (this.settings.Credentials != null)
            {
                this.ftp.Credentials = this.settings.Credentials;
            }
        }

        internal void Poll(object state)
        {
            using (FtpWebResponse response = (FtpWebResponse)this.ftp.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                string output = reader.ReadToEnd();
            }
        }
    }
}