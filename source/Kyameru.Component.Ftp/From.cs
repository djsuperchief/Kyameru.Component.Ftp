using Kyameru.Core.Contracts;
using Kyameru.Core.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Kyameru.Component.Ftp
{
    /// <summary>
    /// FTP From Component
    /// </summary>
    public class From : IFromComponent
    {
        private FtpSettings ftpSettings;

        private FtpClient ftp;

        private Timer poller;

        private readonly AutoResetEvent autoEvent = new AutoResetEvent(false);

        /// <summary>
        /// Initializes a new instance of the <see cref="From"/> class.
        /// </summary>
        /// <param name="headers"></param>
        public From(Dictionary<string, string> headers)
        {
            this.ftpSettings = new FtpSettings(headers.ToFromConfig());
        }

        /// <summary>
        /// Event raised on action
        /// </summary>
        public event EventHandler<Routable> OnAction;

        /// <summary>
        /// Event raised when logging.
        /// </summary>
        public event EventHandler<Log> OnLog;

        /// <summary>
        /// Sets up the component.
        /// </summary>
        public void Setup()
        {
            this.ftp = new FtpClient(this.ftpSettings);
            this.poller = new Timer(this.ftp.Poll, this.autoEvent, 1000, this.ftpSettings.PollTime);
        }

        /// <summary>
        /// Starts the component.
        /// </summary>
        public void Start()
        {
            this.autoEvent.WaitOne();
        }

        /// <summary>
        /// Stops the component.
        /// </summary>
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}