using NUnit.Framework;
using System;

namespace Kyameru.Component.Ftp.Tests
{
    public class Tests
    {
        [Test]
        public void Nope()
        {
            Kyameru.Core.Entities.RouteAttributes attributes = new Core.Entities.RouteAttributes("ftp://giles@10.211.55.3/&Delete=true&test=plop");
            UriBuilder uriBuilder = new UriBuilder("ftp://giles@10.211.55.3/&Delete=true&test=plop");
            Inflator inflator = new Inflator();
            var from = inflator.CreateFromComponent(attributes.Headers);
            from.Setup();
            from.Start();
        }
    }
}