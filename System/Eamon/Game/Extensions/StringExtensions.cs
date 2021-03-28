
// StringExtensions.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;

namespace Eamon.Game.Extensions
{
	public static class StringExtensions
	{
		public static string FirstCharToLower(this string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				return Char.ToLower(str[0]) + str.Substring(1);
			}
			else
			{
				return str;
			}
		}

		public static string FirstCharToUpper(this string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				return Char.ToUpper(str[0]) + str.Substring(1);
			}
			else
			{
				return str;
			}
		}

		/// <summary>
		/// Indicates whether this <see cref="string">string</see> contains the supplied string (using <see cref="String.IndexOf(string, StringComparison)">IndexOf</see>).
		/// </summary>
		/// <param name="source">The string to search.</param>
		/// <param name="toCheck">The string to look for.</param>
		/// <param name="comp">The type of string comparison to use.</param>
		/// <returns><c>True</c> if the supplied string is found; <c>false</c> otherwise.</returns>
		public static bool Contains(this string source, string toCheck, StringComparison comp)
		{
			if (source != null && toCheck != null)
			{
				return source.IndexOf(toCheck, comp) >= 0;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Indicates whether this <see cref="string">string</see> contains any of the strings in the supplied array (using <see cref="String.IndexOf(string, StringComparison)">IndexOf</see>).
		/// </summary>
		/// <param name="source">The string to search.</param>
		/// <param name="toCheckArray">The array of strings to look for.</param>
		/// <param name="comp">The type of string comparison to use.</param>
		/// <returns><c>True</c> if any of the strings are found; <c>false</c> otherwise.</returns>
		public static bool ContainsAny(this string source, string[] toCheckArray, StringComparison comp)
		{
			if (source != null && toCheckArray != null)
			{
				foreach (var toCheck in toCheckArray)
				{
					if (source.IndexOf(toCheck, comp) >= 0)
					{
						return true;
					}
				}
			}

			return false;
		}

		public static string PadTLeft(this string text, int totalWidth, char paddingChar)
		{
			Debug.Assert(text != null);

			string arg = text.Length > totalWidth ? text.Substring(0, totalWidth) : text;

			return arg.PadLeft(totalWidth, paddingChar);
		}

		public static string PadTRight(this string text, int totalWidth, char paddingChar)
		{
			Debug.Assert(text != null);

			string arg = text.Length > totalWidth ? text.Substring(0, totalWidth) : text;

			return arg.PadRight(totalWidth, paddingChar);
		}

		public static string Truncate(this string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value)) return value;
			return value.Length <= maxLength ? value : value.Substring(0, maxLength);
		}
	}
}
