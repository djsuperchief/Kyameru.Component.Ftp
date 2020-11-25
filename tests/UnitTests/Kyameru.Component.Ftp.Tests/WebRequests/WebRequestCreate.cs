using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Kyameru.Component.Ftp.Tests.WebRequests
{
    public class WebRequestCreate : IWebRequestCreate
    {
        private static WebRequest nextRequest;
        private static object lockObject = new object();

        private static WebRequest Next
        {
            get
            {
                return nextRequest;
            }
            set
            {
                lock (lockObject)
                {
                    nextRequest = value;
                }
            }
        }

        public WebRequest Create(Uri uri)
        {
            return nextRequest;
        }

        public static MockWebRequest CreateWebRequest(string expectedResponse)
        {
            MockWebRequest request = new MockWebRequest(expectedResponse);
            Next = request;
            return request;
        }
    }
}