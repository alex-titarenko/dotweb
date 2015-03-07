using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace TAlex.Web
{
    /// <summary>
    /// Provides an Internet Protocol (IP) address range.
    /// </summary>
    public class IPAddressRange
    {
        #region Fields

        internal static readonly string[] IPRangeDelimeters = new string[] { "-", "/" };

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the lower bound of ip addresses range.
        /// </summary>
        public IPAddress Lower { get; set; }

        /// <summary>
        /// Gets or sets the upper bound of ip addresses range.
        /// </summary>
        public IPAddress Upper { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.Common.Net.IPAddressRange"/> class by using
        /// the lower and upper bounds of ip addresses range.
        /// </summary>
        /// <param name="lower">A lower bound of ip addresses range.</param>
        /// <param name="upper">A upper bound of ip addresses range.</param>
        public IPAddressRange(IPAddress lower, IPAddress upper)
        {
            Lower = lower;
            Upper = upper;

            EnsureAddressFamilyEquals();
        }


        /// <summary>
        /// Returns value that indicates the specified address is in range of ip addresses.
        /// </summary>
        /// <param name="address">An <see cref="System.Net.IPAddress"/> for check.</param>
        /// <returns>true if address is in range; otherwise false;</returns>
        public virtual bool IsInRange(IPAddress address)
        {
            EnsureAddressFamilyEquals();
            if (address.AddressFamily != Lower.AddressFamily)
            {
                return false;
            }

            byte[] addressBytes = address.GetAddressBytes();
            byte[] lowerBytes = Lower.GetAddressBytes();
            byte[] upperBytes = Upper.GetAddressBytes();

            bool lowerBoundary = true, upperBoundary = true;

            for (int i = 0; i < lowerBytes.Length &&
                (lowerBoundary || upperBoundary); i++)
            {
                if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                    (upperBoundary && addressBytes[i] > upperBytes[i]))
                {
                    return false;
                }

                lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
                upperBoundary &= (addressBytes[i] == upperBytes[i]);
            }

            return true;
        }

        /// <summary>
        /// Converts an IP addresses range string to an <see cref="TAlex.Common.Net.IPAddressRange"/> instance.
        /// </summary>
        /// <param name="s">
        /// A string that contains an IP addresses range in dotted-quad notation for IPv4 and
        /// in colon-hexadecimal notation for IPv6.
        /// </param>
        /// <returns>An <see cref="TAlex.Common.Net.IPAddressRange"/> instance.</returns>
        /// <exception cref="System.ArgumentException">s contains IP addresses from different families.</exception>
        /// <exception cref="System.ArgumentNullException">s is null.</exception>
        /// <exception cref="System.FormatException">s is not a valid IP addresses range.</exception>
        public static IPAddressRange Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            IPAddress lower;
            IPAddress upper;

            if (IsRange(s))
                HandleRangeAddress(s, out lower, out upper);
            else
                lower = upper = IPAddress.Parse(s);

            return new IPAddressRange(lower, upper);
        }

        /// <summary>
        /// Determines whether a string is a valid IP addresses range.
        /// </summary>
        /// <param name="s">The string to validate.</param>
        /// <param name="result">The <see cref="TAlex.Common.Net.IPAddressRange"/> version of the string.</param>
        /// <returns>true if s is a valid IP addresses range; otherwise, false.</returns>
        public static bool TryParse(string s, out IPAddressRange result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }

        private void EnsureAddressFamilyEquals()
        {
            if (Lower.AddressFamily != Upper.AddressFamily)
            {
                throw new ArgumentException();
            }
        }

        private static bool IsRange(string s)
        {
            foreach (string delimeter in IPRangeDelimeters)
            {
                if (s.Contains(delimeter))
                    return true;
            }
            return false;
        }

        private static IPAddress HandleSignleAddress(string s)
        {
            return IPAddress.Parse(s);
        }

        private static void HandleRangeAddress(string s, out IPAddress lower, out IPAddress upper)
        {
            string[] parts = s.Split(IPRangeDelimeters, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                throw new FormatException();
            }
            lower = HandleSignleAddress(parts[0]);
            upper = HandleSignleAddress(parts[1]);
        }
    }
}
