using Kyameru.Core.Entities;
using NUnit.Framework;
using System;

namespace Kyameru.Component.Ftp.Tests
{
    public class Tests
    {
        [Test]
        public void Nope()
        {
            Kyameru.Core.Entities.RouteAttributes attributes = new Core.Entities.RouteAttributes("ftp://giles@10.211.55.3/in/&Archive=../archive/out");
            UriBuilder uriBuilder = new UriBuilder("ftp://giles@10.211.55.3/in/&Archive=../archive/out");
            Inflator inflator = new Inflator();
            var to = inflator.CreateToComponent(attributes.Headers);
            Routable routable = new Routable(new System.Collections.Generic.Dictionary<string, string>()
            {
                { "FullSource", "C:/temp/test.txt" },
                { "SourceFile", "test.txt" }
            }, System.Text.Encoding.UTF8.GetBytes("Hello world"));
            to.Process(routable);
            //var from = inflator.CreateFromComponent(attributes.Headers);
            //from.Setup();
            //from.Start();
        }
    }
}