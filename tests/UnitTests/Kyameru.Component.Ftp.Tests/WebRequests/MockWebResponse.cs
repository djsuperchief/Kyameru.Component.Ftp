using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Kyameru.Component.Ftp.Tests.WebRequests
{
    public class MockWebResponse : WebResponse
    {
        private readonly Stream responseStream;

        public MockWebResponse(Stream responseStream)
        {
            this.responseStream = responseStream;
        }

        public override Stream GetResponseStream()
        {
            return this.responseStream;
        }
    }
}