using Kyameru.Component.Ftp.Settings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Kyameru.Component.Ftp.Tests.Generic
{
    [TestFixture]
    internal class FtpClientTests
    {
        [Test]
        public void CanDownloadFile()
        {
            string response = "download file";
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            bool worked = WebRequest.RegisterPrefix("ftp://127.0.0.1", new WebRequests.WebRequestCreate());
            WebRequests.MockWebRequest request = WebRequests.WebRequestCreate.CreateWebRequest(response);
            Kyameru.Core.Entities.RouteAttributes routeAttributes = new Core.Entities.RouteAttributes("ftp://test@127.0.0.1/download&Delete=false");
            FtpSettings ftpSettings = new FtpSettings(routeAttributes.Headers.ToFromConfig());
            FtpClient client = new FtpClient(ftpSettings);
            client.Poll(null);
        }
    }
}