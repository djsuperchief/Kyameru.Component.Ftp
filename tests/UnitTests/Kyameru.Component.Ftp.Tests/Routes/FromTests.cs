using Kyameru.Component.Ftp.Contracts;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kyameru.Component.Ftp.Tests.Routes
{
    [TestFixture(Category = "Routes")]
    public class FromTests
    {
        private readonly Mock<IWebRequestFactory> webRequestFactory = new Mock<IWebRequestFactory>();

        [Test]
        public void CanConstructFrom()
        {
        }

        [SetUp]
        public void Init()
        {
        }
    }
}