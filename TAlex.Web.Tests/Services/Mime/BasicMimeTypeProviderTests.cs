using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAlex.Web.Services.Mime;
using FluentAssertions;


namespace LicenseDataProviders.Test.Services.Mime
{
    [TestFixture]
    public class BasicMimeTypeProviderTests
    {
        private IMimeTypeProvider _mimeTypeProvider;

        [SetUp]
        public void SetUp()
        {
            _mimeTypeProvider = new BasicMimeTypeProvider();
        }


        [TestCase("", BasicMimeTypeProvider.DefaultMimeType)]
        [TestCase("asdf", BasicMimeTypeProvider.DefaultMimeType)]
        [TestCase(".jpg", "image/jpeg")]
        [TestCase("jpg", "image/jpeg")]
        [TestCase("jpeg", "image/jpeg")]
        [TestCase(".exe", "application/octet-stream")]
        public void GetMimeTypeTest(string extension, string expected)
        {
            //action
            string actual = _mimeTypeProvider.GetMimeType(extension);

            //assert
            actual.Should().Be(expected);
        }
    }
}
