using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using TAlex.Web;


namespace TAlex.Web.Tests
{
    [TestFixture]
    public class IPAddressRangeTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_AddressesFromDifferentsFamileis()
        {
            //arrange
            IPAddress lower = IPAddress.Parse("192.168.1.0");
            IPAddress upper = IPAddress.IPv6Any;

            //action
            IPAddressRange actual = new IPAddressRange(lower, upper);
        }

        [Test]
        public void Parse_SingleAddress()
        {
            //arrange
            IPAddress address = IPAddress.Parse("192.168.1.1");

            //action
            IPAddressRange actual = IPAddressRange.Parse(address.ToString());

            //assert
            Assert.AreEqual(address, actual.Lower);
            Assert.AreEqual(address, actual.Upper);
        }

        [Test]
        public void Parse_RangeAddressDashNotation()
        {
            //arrange
            IPAddress lowerAddress = IPAddress.Parse("192.168.1.1");
            IPAddress upperAddress = IPAddress.Parse("192.168.2.255");

            //action
            IPAddressRange actual = IPAddressRange.Parse(String.Format("{0}-{1}", lowerAddress, upperAddress));

            //assert
            Assert.AreEqual(lowerAddress, actual.Lower);
            Assert.AreEqual(upperAddress, actual.Upper);
        }

        [Test]
        public void Parse_RangeAddressSlashNotation()
        {
            //arrange
            IPAddress lowerAddress = IPAddress.Parse("192.168.1.1");
            IPAddress upperAddress = IPAddress.Parse("192.168.2.255");

            //action
            IPAddressRange actual = IPAddressRange.Parse(String.Format("{0}/{1}", lowerAddress, upperAddress));

            //assert
            Assert.AreEqual(lowerAddress, actual.Lower);
            Assert.AreEqual(upperAddress, actual.Upper);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Parse_ArgumentNullException()
        {
            //arrange
            string address = null;

            //action
            IPAddressRange actual = IPAddressRange.Parse(address);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Parse_InvalidFormatOfRangeAddresses()
        {
            //arrange
            IPAddress lowerAddress = IPAddress.Parse("192.168.1.1");
            IPAddress upperAddress = IPAddress.Parse("192.168.2.256");

            //action
            IPAddressRange actual = IPAddressRange.Parse(String.Format("{0}-{1}-{1}", lowerAddress, upperAddress));

            //assert
            Assert.AreEqual(lowerAddress, actual.Lower);
            Assert.AreEqual(upperAddress, actual.Upper);
        }

        [Test]
        public void TryParse_Success()
        {
            //arrange
            string addresses = "127.168.0.1-127.168.3.5";
            IPAddressRange actualRange;

            //action
            bool actula = IPAddressRange.TryParse(addresses, out actualRange);

            //assert
            Assert.IsTrue(actula);
            Assert.IsNotNull(actualRange);
        }

        [Test]
        public void TryParse_Fail()
        {
            //arrange
            string addresses = "127.168.0.1-127.168.3.5.46";
            IPAddressRange actualRange;

            //action
            bool actula = IPAddressRange.TryParse(addresses, out actualRange);

            //assert
            Assert.IsFalse(actula);
            Assert.IsNull(actualRange);
        }

        [Test]
        public void IsInRange_Success()
        {
            //arrange
            IPAddressRange range = new IPAddressRange(IPAddress.Parse("85.255.19.0"), IPAddress.Parse("85.255.19.255"));
            
            //action
            bool actual = range.IsInRange(IPAddress.Parse("85.255.19.3"));

            //assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsInRange_Fail()
        {
            //arrange
            IPAddressRange range = new IPAddressRange(IPAddress.Parse("85.255.19.0"), IPAddress.Parse("85.255.19.240"));

            //action
            bool actual = range.IsInRange(IPAddress.Parse("85.255.19.243"));

            //assert
            Assert.IsFalse(actual);
        }
    }
}
