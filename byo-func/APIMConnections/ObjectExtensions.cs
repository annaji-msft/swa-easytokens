using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MSHA.ApiConnections
{
    /// <summary>
    /// Extensions for action values retrieved in a microservice action body.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Substrings a string safely.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        /// <returns>The safely substringed string.</returns>
        public static string SafeSubstring(this string str, int start, int length)
        {
            return str.Substring(start, Math.Min(str.Length, length));
        }

        /// <summary>
        /// Checks an argument for null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="parameterName">The parameter name.</param>
        public static void CheckArgumentForNull(this object argument, string parameterName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Checks an argument for null or empty.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="parameterName">The parameter name.</param>
        public static void CheckArgumentForNullOrEmpty(this string argument, string parameterName)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Checks an argument for null or empty.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="parameterName">The parameter name.</param>
        public static void CheckArgumentForNullOrWhiteSpace(this string argument, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Checks an argument for null or empty list.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="parameterName">The parameter name.</param>
        public static void CheckArgumentForNullOrEmptyList<T>(this IEnumerable<T> argument, string parameterName)
        {
            if (argument == null || !argument.Any())
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Checks an integer argument within a range.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="parameterName">The parameter name.</param>
        public static void CheckIntegerArgumentRange(this int argument, int min, int max, string parameterName)
        {
            if (argument < min || argument > max)
            {
                if (min == int.MinValue)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} must be equal or less than {1}.", parameterName, max), parameterName);
                }
                else if (max == int.MaxValue)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} must be equal or greater than {1}.", parameterName, min), parameterName);
                }
                else
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} must be between {1} and {2} (inclusive).", parameterName, min, max), parameterName);
                }
            }
        }

        /// <summary>
        /// Casts the specified object to type T.
        /// </summary>
        /// <typeparam name="T">The type to cast to</typeparam>
        /// <param name="value">The input object</param>
        public static T Cast<T>(this object value)
        {
            return value.CastObject<T>();
        }

        /// <summary>
        /// Casts the specified object to type T.
        /// </summary>
        /// <typeparam name="T">The type to cast to</typeparam>
        /// <param name="value">The input object</param>
        public static T CastObject<T>(this object value)
        {
            return (T)value;
        }
    }
}
