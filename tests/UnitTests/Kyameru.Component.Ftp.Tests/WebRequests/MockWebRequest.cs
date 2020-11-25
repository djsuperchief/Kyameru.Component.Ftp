using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Kyameru.Component.Ftp.Tests.WebRequests
{
    /// <summary>
    /// Fake web request for FTP
    /// </summary>
    public class MockWebRequest : WebRequest
    {
        private readonly MemoryStream responseStream;
        private readonly MemoryStream requestStream = new MemoryStream();

        public override string Method { get; set; }
        public override string ContentType { get; set; }
        public override long ContentLength { get; set; }

        public MockWebRequest(string expectedResponse)
        {
            this.responseStream = new MemoryStream(Encoding.UTF8.GetBytes(expectedResponse));
        }

        public string ContentAsString()
        {
            return System.Text.Encoding.UTF8.GetString(requestStream.ToArray());
        }

        public override Stream GetRequestStream()
        {
            return requestStream;
        }

        public override WebResponse GetResponse()
        {
            return new MockWebResponse(responseStream);
        }
    }
}