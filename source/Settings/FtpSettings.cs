using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Kyameru.Component.Ftp.Settings
{
    /// <summary>
    /// Ftp settings entity
    /// </summary>
    public class FtpSettings
    {
        public string Host { get; private set; }

        public string Path { get; private set; }

        public int Port { get; private set; }

        public int PollTime { get; private set; }

        public string Filter { get; private set; }

        public NetworkCredential Credentials { get; private set; }

        public bool Recursive { get; private set; }

        public bool Delete { get; private set; }

        public FtpSettings(Dictionary<string, string> headers)
        {
            this.Host = headers["Host"];
            this.Path = headers["Target"];
            this.Port = int.Parse(headers["Port"]);
            this.PollTime = int.Parse(headers["PollTime"]);
            this.Filter = headers["Filter"];
            this.Credentials = this.GetCredentials(headers.GetKeyValue("UserName"), headers.GetKeyValue("Password"));
            this.Recursive = bool.Parse(headers["Recursive"]);
            this.Delete = bool.Parse(headers["Delete"]);
        }

        private NetworkCredential GetCredentials(string username, string password)
        {
            NetworkCredential response = null;
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                response = new NetworkCredential(username, password);
            }
            else if (!string.IsNullOrWhiteSpace(username))
            {
                response = new NetworkCredential("anonymous", username);
            }

            return response;
        }
    }
}