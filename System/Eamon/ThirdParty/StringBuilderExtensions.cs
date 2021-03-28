#region License
// The MIT License (MIT)

// Copyright (c) 2014 Andrii Chebukin as XperiAndri

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion License

// 20151102		MP		Commented out the Contracts stuff

//using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;

namespace System.Text
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Removes all occurences of specified characters from <see cref="System.Text.StringBuilder"/>.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to remove from.</param>
        /// <param name="removeChars">A Unicode characters to remove.</param>
        /// <returns>
        /// Returns <see cref="System.Text.StringBuilder"/> without specified Unicode characters.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="removeChars"/> is null.</exception>
        public static StringBuilder Remove(this StringBuilder sb, params char[] removeChars)
        {
            //Contract.Requires<ArgumentNullException>(removeChars != null);
            //Contract.Ensures(sb.Length <= Contract.OldValue<int>(sb.Length));

            for (int i = 0; i < sb.Length; )
            {
                if (removeChars.Any(ch => ch == sb[i]))
                    sb.Remove(i, 1);
                else
                    i++;
            }
            return sb;
        }

        /// <summary>
        /// Removes the range of characters from the specified index to the end of <see cref="System.Text.StringBuilder"/>.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to remove from.</param>
        /// <param name="startIndex">The zero-based position to begin deleting characters.</param>
        /// <returns>A reference to this instance after the excise operation has completed.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If <paramref name="startIndex"/> is less than zero, or <paramref name="startIndex"/> is greater
        /// than the length - 1 of this instance.
        /// </exception>
        public static StringBuilder Remove(this StringBuilder sb, int startIndex)
        {
            //Contract.Requires<ArgumentOutOfRangeException>(startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(startIndex < sb.Length);
            //Contract.Ensures(sb.Length <= Contract.OldValue<int>(sb.Length));

            return sb.Remove(startIndex, sb.Length - startIndex);
        }

        ///// <summary>
        ///// Removes all characters from the <see cref="System.Text.StringBuilder"/>.
        ///// </summary>
        ///// <param name="sb"></param>
        //public static void Clear(this StringBuilder sb)
        //{
        //    Contract.Ensures(sb.Length == 0);
        //    sb.Length = 0;
        //}

        private static bool IsBOMWhitespace(char c)
        {
            return false;
        }

        private static StringBuilder TrimHelper(this StringBuilder sb, int trimType)
        {
            int end = sb.Length - 1;
            int start = 0;
            if (trimType != 1)
            {
                start = 0;
                while (start < sb.Length)
                {
                    if (!char.IsWhiteSpace(sb[start]) && !IsBOMWhitespace(sb[start]))
                    {
                        break;
                    }
                    start++;
                }
            }
            if (trimType != 0)
            {
                end = sb.Length - 1;
                while (end >= start)
                {
                    if (!char.IsWhiteSpace(sb[end]) && !IsBOMWhitespace(sb[start]))
                    {
                        break;
                    }
                    end--;
                }
            }
            return sb.CreateTrimmedString(start, end);
        }

        private static StringBuilder CreateTrimmedString(this StringBuilder sb, int start, int end)
        {
            int length = (end - start) + 1;
            if (length == sb.Length)
            {
                return sb;
            }
            if (length == 0)
            {
                sb.Length = 0;
                return sb;
            }
            return sb.InternalSubString(start, end);
        }

        private static StringBuilder InternalSubString(this StringBuilder sb, int startIndex, int end)
        {
            sb.Length = end + 1;
            sb.Remove(0, startIndex);
            return sb;
        }

        private static StringBuilder TrimHelper(this StringBuilder sb, char[] trimChars, int trimType)
        {
            int end = sb.Length - 1;
            int start = 0;
            if (trimType != 1)
            {
                start = 0;
                while (start < sb.Length)
                {
                    int index = 0;
                    char ch = sb[start];
                    while (index < trimChars.Length)
                    {
                        if (trimChars[index] == ch)
                        {
                            break;
                        }
                        index++;
                    }
                    if (index == trimChars.Length)
                    {
                        break;
                    }
                    start++;
                }
            }
            if (trimType != 0)
            {
                end = sb.Length - 1;
                while (end >= start)
                {
                    int num4 = 0;
                    char ch2 = sb[end];
                    while (num4 < trimChars.Length)
                    {
                        if (trimChars[num4] == ch2)
                        {
                            break;
                        }
                        num4++;
                    }
                    if (num4 == trimChars.Length)
                    {
                        break;
                    }
                    end--;
                }
            }
            return sb.CreateTrimmedString(start, end);
        }

        /// <summary>
        /// Removes all leading occurrences of a set of characters specified in an array 
        /// from the current <see cref="System.Text.StringBuilder"/> object.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to remove from.</param>
        /// <param name="trimChars">An array of Unicode characters to remove, or null.</param>
        /// <returns>
        /// The <see cref="System.Text.StringBuilder"/> object that contains a list of characters 
        /// that remains after all occurrences of the characters in the <paramref name="trimChars"/> parameter 
        /// are removed from the end of the current string. If <paramref name="trimChars"/> is null or an empty array, 
        /// Unicode white-space characters are removed instead.
        /// </returns>
        public static StringBuilder TrimStart(this StringBuilder sb, params char[] trimChars)
        {
            //Contract.Ensures(Contract.Result<StringBuilder>() != null);
            //if (sb.Length != 0)
            //{
            //    int length = 0;
            //    int num2 = sb.Length;
            //    while ((sb[length] == ' ') && (length < num2))
            //    {
            //        length++;
            //    }
            //    if (length > 0)
            //    {
            //        sb.Remove(0, length);
            //    }
            //}
            //return sb;
            if ((trimChars != null) && (trimChars.Length != 0))
                return sb.TrimHelper(trimChars, 0);
            else
                return sb.TrimHelper(0);
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of characters specified in an array 
        /// from the current <see cref="System.Text.StringBuilder"/> object.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to remove from.</param>
        /// <param name="trimChars">An array of Unicode characters to remove, or null.</param>
        /// <returns>
        /// The <see cref="System.Text.StringBuilder"/> object that contains a list of characters that remains 
        /// after all occurrences of the characters in the <paramref name="trimChars"/> parameter are removed 
        /// from the end of the current string. If <paramref name="trimChars"/> is null or an empty array, 
        /// Unicode white-space characters are removed instead.
        /// </returns>
        public static StringBuilder TrimEnd(this StringBuilder sb, params char[] trimChars)
        {
            //Contract.Ensures(Contract.Result<StringBuilder>() != null);
            //if (sb.Length != 0)
            //{
            //    int length = sb.Length;
            //    int num2 = length - 1;
            //    while ((sb[num2] == ' ') && (num2 > -1))
            //    {
            //        num2--;
            //    }
            //    if (num2 < (length - 1))
            //    {
            //        sb.Remove(num2 + 1, (length - num2) - 1);
            //    }
            //}
            //return sb;
            if ((trimChars != null) && (trimChars.Length != 0))
                return sb.TrimHelper(trimChars, 1);
            else
                return sb.TrimHelper(1);
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the current <see cref="System.Text.StringBuilder"/> object.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to remove from.</param>
        /// <returns>
        /// The <see cref="System.Text.StringBuilder"/> object that contains a list of characters 
        /// that remains after all white-space characters are removed 
        /// from the start and end of the current StringBuilder.
        /// </returns>
        public static StringBuilder Trim(this StringBuilder sb)
        {
            //Contract.Ensures(Contract.Result<StringBuilder>() != null);
            //if (sb.Length != 0)
            //{
            //    int length = 0;
            //    int num2 = sb.Length;
            //    while ((sb[length] == ' ') && (length < num2))
            //    {
            //        length++;
            //    }
            //    if (length > 0)
            //    {
            //        sb.Remove(0, length);
            //        num2 = sb.Length;
            //    }
            //    length = num2 - 1;
            //    while ((sb[length] == ' ') && (length > -1))
            //    {
            //        length--;
            //    }
            //    if (length < (num2 - 1))
            //    {
            //        sb.Remove(length + 1, (num2 - length) - 1);
            //    }
            //}
            //return sb;
            return sb.TrimHelper(2);
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of characters specified in an array
        /// from the current <see cref="System.Text.StringBuilder"/> object.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to </param>
        /// <param name="trimChars">An array of Unicode characters to remove, or null.</param>
        /// <returns>
        /// The <see cref="System.Text.StringBuilder"/> object that contains a list of characters that remains 
        /// after all occurrences of the characters in the <paramref name="trimChars"/> parameter are removed 
        /// from the end of the current StringBuilder. If <paramref name="trimChars"/> is null or an empty array, 
        /// Unicode white-space characters are removed instead.
        /// </returns>
        public static StringBuilder Trim(this StringBuilder sb, params char[] trimChars)
        {
            //Contract.Ensures(Contract.Result<StringBuilder>() != null);

            if ((trimChars != null) && (trimChars.Length != 0))
                return sb.TrimHelper(trimChars, 2);
            else
                return sb.TrimHelper(2);
        }

        /// <summary>
        /// Reports the zero-based index position of the first occurrence of the specified Unicode
        /// character within this instance.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to </param>
        /// <param name="value">A Unicode character to seek.</param>
        /// <returns>
        /// The zero-based index position of <paramref name="value"/> if that character is found, or -1
        /// if it is not.
        /// </returns>
        public static int IndexOf(this StringBuilder sb, char value)
        {
            //Contract.Ensures(Contract.Result<int>() >= -1);
            //Contract.Ensures(Contract.Result<int>() < sb.Length);
            return IndexOf(sb, value, 0, sb.Length);
        }

        /// <summary>
        /// Reports the zero-based index position of the first occurrence of the specified Unicode
        /// character within this intance. The search starts at a specified character position.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="value">A Unicode character to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <returns>
        /// The zero-based index position of <paramref name="value"/> if that character is found, or -1
        /// if it is not.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The current instance <see cref="System.Text.StringBuilder.Length"/> does not equal 0, and <paramref name="startIndex"/> 
        /// is less than 0 (zero) or greater than the length of the <see cref="System.Text.StringBuilder"/>.
        /// </exception>
        public static int IndexOf(this StringBuilder sb, char value, int startIndex)
        {
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex < sb.Length);
            //Contract.Ensures(Contract.Result<int>() == -1 || Contract.Result<int>() >= startIndex);
            //Contract.Ensures(Contract.Result<int>() < sb.Length);

            return sb.IndexOf(value, startIndex, sb.Length - startIndex);
        }

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified Unicode
        /// character in this <see cref="System.Text.StringBuilder"/>. The search starts 
        /// at a specified character position and examines a specified number of character positions.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="value">A Unicode character to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>
        /// The zero-based index position of <paramref name="value"/> if that character is found, or -1 
        /// if it is not.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The current instance <see cref="System.Text.StringBuilder.Length"/> does not equal 0, and <paramref name="count"/> 
        /// or <paramref name="startIndex"/> is negative.-or- <paramref name="startIndex"/> is greater than 
        /// the length of this <see cref="System.Text.StringBuilder"/>.
        /// -or-The current instance <see cref="System.Text.StringBuilder.Length"/> does not equal 0, and <paramref name="count"/> 
        /// is greater than the length of this string minus <paramref name="startIndex"/>. 
        /// </exception>
        public static int IndexOf(this StringBuilder sb, char value, int startIndex, int count)
        {
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || count >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex + count <= sb.Length);
            //Contract.Ensures(Contract.Result<int>() >= -1);
            //Contract.Ensures(Contract.Result<int>() < sb.Length);
            //Contract.Ensures(Contract.Result<int>() == -1 ||
            //              (Contract.Result<int>() >= startIndex && Contract.Result<int>() < startIndex + count));
            //Contract.Ensures((count == 0 && Contract.Result<int>() == -1) || true);

            if (sb.Length == 0 || count == 0)
                return -1;

            for (int i = startIndex; i < startIndex + count; i++)
            {
                if (sb[i] == value)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string in the current <see cref="System.Text.StringBuilder"/> object.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>
        /// The zero-based index position of the <paramref name="value"/> parameter if that string is found, 
        /// or -1 if it is not. If <paramref name="value"/> is <see cref="System.String.Empty"/>, the return value 
        /// is 0.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null.</exception>
        public static int IndexOf(this StringBuilder sb, string value, bool ignoreCase = false)
        {
            //Contract.Requires<ArgumentNullException>(value != null);
            //Contract.Ensures(Contract.Result<int>() >= -1);
            //Contract.Ensures(sb.Length == 0 || Contract.Result<int>() <= Math.Max(sb.Length - value.Length, -1));
            //Contract.Ensures(value != string.Empty || Contract.Result<int>() == 0);
            if (value == string.Empty)
                return 0;

            return IndexOfInternal(sb, value, 0, sb.Length, ignoreCase);
        }

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string in the current <see cref="System.Text.StringBuilder"/> object. 
        /// Parameter specifies the starting search position in the current <see cref="System.Text.StringBuilder"/>.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="value">The string to seek. </param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>
        /// The zero-based index position of the <paramref name="value"/> parameter if that string is found, 
        /// or -1 if it is not. If <paramref name="value"/> is <see cref="System.String.Empty"/>, the return value 
        /// is <paramref name="startIndex"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="startIndex"/> is less than 0 (zero) or greater than the length of this <see cref="System.Text.StringBuilder"/>.
        /// </exception>
        public static int IndexOf(this StringBuilder sb, string value, int startIndex, bool ignoreCase = false)
        {
            //Contract.Requires<ArgumentNullException>(value != null);
            //Contract.Requires<ArgumentOutOfRangeException>(startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>((sb.Length == 0 && startIndex == 0) || startIndex < sb.Length);
            //Contract.Ensures(Contract.Result<int>() == -1 || Contract.Result<int>() >= startIndex);
            //Contract.Ensures(value == string.Empty || Contract.Result<int>() < sb.Length);
            //Contract.Ensures(value != string.Empty || Contract.Result<int>() == startIndex);

            return IndexOfInternal(sb, value, startIndex, sb.Length - startIndex, ignoreCase);
        }
                
        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string in the current <see cref="System.Text.StringBuilder"/> object. 
        /// The search starts at a specified character position and examines a specified number of character positions.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>
        /// The zero-based index position of the <paramref name="value"/> parameter if that string is found, 
        /// or -1 if it is not. If <paramref name="value"/> is <see cref="System.String.Empty"/>, 
        /// the return value is <paramref name="startIndex"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">value is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="count"/> or <paramref name="startIndex"/> is negative.-or- <paramref name="startIndex"/> is 
        /// greater than the length of this instance.-or-<paramref name="count"/> is greater than the length of 
        /// this <see cref="System.Text.StringBuilder"/> minus <paramref name="startIndex"/>.
        /// </exception>
        public static int IndexOf(this StringBuilder sb, string value, int startIndex, int count, bool ignoreCase = false)
        {
            //Contract.Requires<ArgumentNullException>(value != null);
            //Contract.Requires<ArgumentOutOfRangeException>(startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(startIndex + count <= sb.Length);
            //Contract.Ensures(value == string.Empty || Contract.Result<int>() < Math.Max(startIndex + 1 + count - value.Length, 0));
            //Contract.Ensures(Contract.Result<int>() == -1 || Contract.Result<int>() >= startIndex);
            //Contract.Ensures(value != string.Empty || Contract.Result<int>() == startIndex);

            return IndexOfInternal(sb, value, startIndex, count, ignoreCase);
        }

        private static int IndexOfInternal(StringBuilder sb, string value, int startIndex, int count, bool ignoreCase)
        {
            if (value == string.Empty)
                return startIndex;
            if (sb.Length == 0 || count == 0 || startIndex + 1 + value.Length > sb.Length)
                return -1;

            int num3;
            int length = value.Length;
            int num2 = startIndex + count - value.Length;
            if (ignoreCase == false)
            {
                for (int i = startIndex; i <= num2; i++)
                {
                    if (sb[i] == value[0])
                    {
                        num3 = 1;
                        while ((num3 < length) && (sb[i + num3] == value[num3]))
                        {
                            num3++;
                        }
                        if (num3 == length)
                        {
                            return i;
                        }
                    }
                }
            }
            else
            {
                for (int j = startIndex; j <= num2; j++)
                {
                    if (char.ToLower(sb[j]) == char.ToLower(value[0]))
                    {
                        num3 = 1;
                        while ((num3 < length) && (char.ToLower(sb[j + num3]) == char.ToLower(value[num3])))
                        {
                            num3++;
                        }
                        if (num3 == length)
                        {
                            return j;
                        }
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Reports the zero-based index of the first occurrence in this instance 
        /// of any character in a specified array of Unicode characters.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <returns>
        /// The zero-based index position of the first occurrence in this instance
        /// where any character in <paramref name="anyOf"/> was found; -1 if no character in <paramref name="anyOf"/> was found.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">anyOf is null.</exception>
        public static int IndexOfAny(this StringBuilder sb, char[] anyOf)
        {
            //Contract.Requires<ArgumentNullException>(anyOf != null);
            //Contract.Ensures(Contract.Result<int>() >= -1);
            //Contract.Ensures(Contract.Result<int>() < sb.Length);

            return sb.IndexOfAny(anyOf, 0, sb.Length);
        }

        /// <summary>
        /// Reports the zero-based index of the first occurrence in this instance of any character 
        /// in a specified array of Unicode characters. The search starts at a specified character position.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <returns>
        /// The zero-based index position of the first occurrence in this instance
        /// where any character in <paramref name="anyOf"/> was found; -1 if no character in <paramref name="anyOf"/> was found.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="anyOf"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="startIndex"/> is negative.-or-<paramref name="startIndex"/> is greater 
        /// than the number of characters in this instance.
        /// </exception>
        public static int IndexOfAny(this StringBuilder sb, char[] anyOf, int startIndex)
        {
            //Contract.Requires<ArgumentNullException>(anyOf != null);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex < sb.Length);
            //Contract.Ensures(Contract.Result<int>() < sb.Length);
            //Contract.Ensures(Contract.Result<int>() == -1 ||
            //                (Contract.Result<int>() >= startIndex && Contract.Result<int>() < sb.Length));

            return sb.IndexOfAny(anyOf, startIndex, sb.Length - startIndex);
        }

        /// <summary>
        /// Reports the zero-based index of the first occurrence in this instance of any character 
        /// in a specified array of Unicode characters. The search starts at a specified character position and examines a specified number of character positions.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>
        /// The zero-based index position of the first occurrence in this instance
        /// where any character in <paramref name="anyOf"/> was found; -1 if no character in <paramref name="anyOf"/> was found.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="anyOf"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="count"/> or <paramref name="startIndex"/> is negative.
        /// -or-<paramref name="count"/> + <paramref name="startIndex"/> is greater than the number of characters in this instance.
        /// </exception>
        public static int IndexOfAny(this StringBuilder sb, char[] anyOf, int startIndex, int count)
        {
            //Contract.Requires<ArgumentNullException>(anyOf != null);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex < sb.Length);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || count >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex + count <= sb.Length);
            //Contract.Ensures(Contract.Result<int>() < sb.Length);
            //Contract.Ensures(Contract.Result<int>() == -1 ||
            //                (Contract.Result<int>() >= startIndex && Contract.Result<int>() < startIndex + count));
            //Contract.Ensures((count != 0 && Contract.Result<int>() == -1) || true);

            if (sb.Length == 0 || count == 0)
                return -1;

            for (int i = startIndex; i < startIndex + count; i++)
            {
                if (anyOf.Any(ch => ch == sb[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Reports the zero-based index of the last occurrence of the specified Unicode
        /// character within this instance.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to </param>
        /// <param name="value">A Unicode character to seek.</param>
        /// <returns>
        /// The zero-based index position of <paramref name="value"/> if that character is found, or -1
        /// if it is not.
        /// </returns>
        public static int LastIndexOf(this StringBuilder sb, char value)
        {
            //Contract.Ensures(Contract.Result<int>() >= -1);
            //Contract.Ensures(Contract.Result<int>() < sb.Length);
            return LastIndexOf(sb, value, sb.Length - 1, sb.Length);
        }

        /// <summary>
        /// Reports the zero-based index position of the last occurrence of the specified Unicode character 
        /// in a substring within this instance. The search starts at a specified character position and 
        /// proceeds backward toward the beginning of the <see cref="System.Text.StringBuilder"/> 
        /// for a specified number of character positions.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="value">A Unicode character to seek.</param>
        /// <param name="startIndex">
        /// The starting position of the search. The search proceeds from <paramref name="startIndex"/> toward the beginning 
        /// of this instance.
        /// </param>
        /// <returns>
        /// The zero-based index position of <paramref name="value"/> if that character is found, or -1
        /// if it is not.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The current instance <see cref="System.Text.StringBuilder.Length"/> does not equal 0, 
        /// and <paramref name="startIndex"/> is less than zero or greater than or equal to the length of this instance.
        /// </exception>
        public static int LastIndexOf(this StringBuilder sb, char value, int startIndex)
        {
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex < sb.Length);
            //Contract.Ensures(Contract.Result<int>() == -1 || Contract.Result<int>() <= startIndex);
            //Contract.Ensures(Contract.Result<int>() < sb.Length);
            return sb.LastIndexOf(value, startIndex, startIndex + 1);
        }

        /// <summary>
        /// Reports the zero-based index of the last occurrence of the specified Unicode
        /// character in this <see cref="System.Text.StringBuilder"/>. The search starts 
        /// at a specified character position and examines a specified number of character positions.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="value">A Unicode character to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>
        /// The zero-based index position of <paramref name="value"/> if that character is found, or -1 
        /// if it is not.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The current instance <see cref="System.Text.StringBuilder.Length"/> does not equal 0, 
        /// and <paramref name="startIndex"/> is less than zero or greater than or equal to the length of this instance.
        /// -or-The current instance <see cref="System.Text.StringBuilder.Length"/> 
        /// does not equal 0, and <paramref name="startIndex"/> - <paramref name="count"/> + 1 is less than zero.
        /// </exception>
        public static int LastIndexOf(this StringBuilder sb, char value, int startIndex, int count)
        {
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex < sb.Length);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || count >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex - count + 1 >= 0);
            //Contract.Ensures(Contract.Result<int>() >= -1);
            //Contract.Ensures(Contract.Result<int>() <= startIndex);
            //Contract.Ensures(Contract.Result<int>() == -1 || Contract.Result<int>() >= startIndex - count + 1);
            //Contract.Ensures((count == 0 && Contract.Result<int>() == -1) || true);

            if (sb.Length == 0 || count == 0)
                return -1;

            for (int i = startIndex; i > startIndex - count; i--)
            {
                if (sb[i] == value)
                {
                    return i;
                }
            }
            return -1;
        }


        /// <summary>
        /// Reports the zero-based index of the last occurrence of the specified string 
        /// in the current <see cref="System.Text.StringBuilder"/> object.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to </param>
        /// <param name="value">The string to seek.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>
        /// The zero-based index position of the value parameter if that string is found, 
        /// or -1 if it is not. If value is <see cref="System.String.Empty"/>, the return value is startIndex.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">value is null.</exception>
        public static int LastIndexOf(this StringBuilder sb, string value, bool ignoreCase = false)
        {
            //Contract.Requires<ArgumentNullException>(value != null);
            //Contract.Ensures(Contract.Result<int>() >= -1);
            //Contract.Ensures(Contract.Result<int>() <= Math.Max(sb.Length - value.Length, -1));
            //Contract.Ensures(value != string.Empty || (Contract.Result<int>() == 0 && sb.Length == 0) || (Contract.Result<int>() == sb.Length - 1));
            if (value == string.Empty)
            {
                if (sb.Length == 0)
                    return 0;
                else
                    return sb.Length - 1;
            }
            if (sb.Length == 0)
                return -1;

            return LastIndexOfInternal(sb, value, sb.Length - 1, sb.Length, ignoreCase);
        }

        /// <summary>
        /// Reports the zero-based index of the last occurrence of the specified string in the current <see cref="System.Text.StringBuilder"/> object. 
        /// Parameter specifies the starting search position in the current <see cref="System.Text.StringBuilder"/>.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="value">The string to seek. </param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>
        /// The zero-based index position of the value parameter if that string is found, 
        /// or -1 if it is not. If value is <see cref="System.String.Empty"/>, the return value is startIndex.
        /// </returns>
        public static int LastIndexOf(this StringBuilder sb, string value, int startIndex, bool ignoreCase = false)
        {
            //Contract.Requires<ArgumentNullException>(value != null);
            //Contract.Requires<ArgumentOutOfRangeException>(startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>((sb.Length == 0 && startIndex == 0) || startIndex < sb.Length);
            //Contract.Ensures(Contract.Result<int>() >= -1);
            //Contract.Ensures(value == string.Empty || Contract.Result<int>() <= Math.Max(startIndex + 1 - value.Length, -1));
            //Contract.Ensures(value != string.Empty || Contract.Result<int>() == startIndex);

            return LastIndexOfInternal(sb, value, startIndex, startIndex + 1, ignoreCase);
        }

        /// <summary>
        /// Reports the zero-based index of the last occurrence of the specified string in the current <see cref="System.Text.StringBuilder"/> object. 
        /// The search starts at a specified character position and examines a specified number of character positions.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>
        /// The zero-based index position of the value parameter if that string is found, 
        /// or -1 if it is not. If value is <see cref="System.String.Empty"/>, the return value is startIndex.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">value is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// startIndex is less than 0 (zero) or greater than the length of this string.
        /// </exception>
        public static int LastIndexOf(this StringBuilder sb, string value, int startIndex, int count, bool ignoreCase = false)
        {
            //Contract.Requires<ArgumentNullException>(value != null);
            //Contract.Requires<ArgumentOutOfRangeException>(startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>((sb.Length == 0 && startIndex == 0) || startIndex < sb.Length);
            //Contract.Requires<ArgumentOutOfRangeException>((sb.Length == 0 && startIndex == 0) || startIndex - count + 1 >= 0);
            //Contract.Ensures(Contract.Result<int>() >= -1);
            //Contract.Ensures(value == string.Empty || Contract.Result<int>() <= Math.Max(startIndex + 1 - value.Length, -1));
            //Contract.Ensures(Contract.Result<int>() == -1 || Contract.Result<int>() >= startIndex + 1 - count);
            //Contract.Ensures(value != string.Empty || Contract.Result<int>() == startIndex);

            return LastIndexOfInternal(sb, value, startIndex, count, ignoreCase);
        }

        private static int LastIndexOfInternal(StringBuilder sb, string value, int startIndex, int count, bool ignoreCase)
        {
            if (value == string.Empty)
                return startIndex;
            if (sb.Length == 0 || count == 0 || startIndex + 1 - count + value.Length > sb.Length)
                return -1;

            int num3;
            int length = value.Length;
            int maxValueIndex = length - 1;
            int num2 = startIndex - count + value.Length;
            if (ignoreCase == false)
            {
                for (int i = startIndex; i >= num2; i--)
                {
                    if (sb[i] == value[maxValueIndex])
                    {
                        num3 = 1;
                        while ((num3 < length) && (sb[i - num3] == value[maxValueIndex - num3]))
                        {
                            num3++;
                        }
                        if (num3 == length)
                        {
                            return i - num3 + 1;
                        }
                    }
                }
            }
            else
            {
                for (int j = startIndex; j >= num2; j--)
                {
                    if (char.ToLower(sb[j]) == char.ToLower(value[maxValueIndex]))
                    {
                        num3 = 1;
                        while ((num3 < length) && (char.ToLower(sb[j - num3]) == char.ToLower(value[maxValueIndex - num3])))
                        {
                            num3++;
                        }
                        if (num3 == length)
                        {
                            return j - num3 + 1;
                        }
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// Reports the zero-based index of the last occurrence in this instance 
        /// of any character in a specified array of Unicode characters.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <returns>
        /// The zero-based index position of the last occurrence in this instance
        /// where any character in <paramref name="anyOf"/> was found; -1 if no character in <paramref name="anyOf"/> was found.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="anyOf"/> is null.</exception>
        public static int LastIndexOfAny(this StringBuilder sb, char[] anyOf)
        {
            //Contract.Requires<ArgumentNullException>(anyOf != null);
            //Contract.Ensures(Contract.Result<int>() >= -1);
            //Contract.Ensures(Contract.Result<int>() < sb.Length);
            return sb.LastIndexOfAny(anyOf, sb.Length - 1, sb.Length);
        }

        /// <summary>
        /// Reports the zero-based index of the last occurrence in this instance of any character 
        /// in a specified array of Unicode characters. The search starts at a specified character position.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <returns>
        /// The zero-based index position of the last occurrence in this instance
        /// where any character in <paramref name="anyOf"/> was found; -1 if no character in <paramref name="anyOf"/> was found.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="anyOf"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="startIndex"/> is negative.-or- <paramref name="startIndex"/> is greater 
        /// than the number of characters in this instance.
        /// </exception>
        public static int LastIndexOfAny(this StringBuilder sb, char[] anyOf, int startIndex)
        {
            //Contract.Requires<ArgumentNullException>(anyOf != null);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex < sb.Length);
            //Contract.Ensures(Contract.Result<int>() < sb.Length);
            //Contract.Ensures(Contract.Result<int>() >= -1 && Contract.Result<int>() <= startIndex);
            return sb.LastIndexOfAny(anyOf, startIndex, startIndex + 1);
        }

        /// <summary>
        /// Reports the zero-based index of the last occurrence in this instance of any character 
        /// in a specified array of Unicode characters. The search starts at a specified character position and examines a specified number of character positions.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to search.</param>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>
        /// The zero-based index position of the last occurrence in this instance
        /// where any character in <paramref name="anyOf"/> was found; -1 if no character in <paramref name="anyOf"/> was found.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="anyOf"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="count"/> or <paramref name="startIndex"/> is negative.
        /// -or-<paramref name="count"/> + <paramref name="startIndex"/> is greater than the number of characters in this instance.
        /// </exception>
        public static int LastIndexOfAny(this StringBuilder sb, char[] anyOf, int startIndex, int count)
        {
            //Contract.Requires<ArgumentNullException>(anyOf != null);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex < sb.Length);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || count >= 0);
            //Contract.Requires<ArgumentOutOfRangeException>(sb.Length == 0 || startIndex - count + 1 >= 0);
            //Contract.Ensures(Contract.Result<int>() < sb.Length);
            //Contract.Ensures(Contract.Result<int>() == -1 ||
            //                (Contract.Result<int>() <= startIndex && Contract.Result<int>() > startIndex - count));
            //Contract.Ensures((count == 0 && Contract.Result<int>() == -1) || true);

            if (sb.Length == 0 || count == 0)
                return -1;

            for (int i = startIndex; i > startIndex - count; i--)
            {
                if (anyOf.Any(ch => ch == sb[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Determines whether this instance of <see cref="System.Text.StringBuilder"/> starts with the specified string.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to compare.</param>
        /// <param name="value">The string to compare.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>
        /// true if the <paramref name="value"/> parameter matches the beginning of this string; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null.</exception>
        public static bool StartsWith(this StringBuilder sb, string value, bool ignoreCase = false)
        {
            //Contract.Requires<ArgumentNullException>(value != null);
            //Contract.Ensures(!Contract.Result<bool>() || value.Length <= sb.Length);

            int length = value.Length;
            if (length > sb.Length)
                return false;

            if (ignoreCase == false)
            {
                for (int i = 0; i < length; i++)
                {
                    if (sb[i] != value[i])
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int j = 0; j < length; j++)
                {
                    if (char.ToLower(sb[j]) != char.ToLower(value[j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether this instance of <see cref="System.Text.StringBuilder"/> ends with the specified string.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to compare.</param>
        /// <param name="value">The string to compare to the substring at the end of this instance.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>
        /// true if the <paramref name="value"/> parameter matches the beginning of this string; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null.</exception>
        public static bool EndsWith(this StringBuilder sb, string value, bool ignoreCase = false)
        {
            //Contract.Requires<ArgumentNullException>(value != null);
            //Contract.Ensures(!Contract.Result<bool>() || value.Length <= sb.Length);

            int length = value.Length;
            int maxSBIndex = sb.Length - 1;
            int maxValueIndex = length - 1;
            if (length > sb.Length)
                return false;

            if (ignoreCase == false)
            {
                for (int i = 0; i < length; i++)
                {
                    if (sb[maxSBIndex - i] != value[maxValueIndex - i])
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int j = length - 1; j >= 0; j--)
                {
                    if (char.ToLower(sb[maxSBIndex - j]) != char.ToLower(value[maxValueIndex - j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Returns a <see cref="System.Text.StringBuilder"/> converted to lowercase.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to convert to lowercase.</param>
        /// <returns>The <see cref="System.Text.StringBuilder"/> converted to lowercase.</returns>
        public static StringBuilder ToLower(this StringBuilder sb)
        {
            //Contract.Ensures(Contract.Result<StringBuilder>() != null);
            //Contract.Ensures(Contract.Result<StringBuilder>().Length == sb.Length);

            for (int i = 0; i < sb.Length; i++)
            {
                sb[i] = char.ToLower(sb[i]);
            }

            return sb;
        }

        /// <summary>
        /// Returns a <see cref="System.Text.StringBuilder"/> converted to lowercase, using the casing rules of the specified culture.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to convert to lowercase.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>The <see cref="System.Text.StringBuilder"/> converted to lowercase using specified culture.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="culture"/>is null.</exception>
        public static StringBuilder ToLower(this StringBuilder sb, CultureInfo culture)
        {
            //Contract.Requires<ArgumentNullException>(culture != null);
            //Contract.Ensures(Contract.Result<StringBuilder>() != null);
            //Contract.Ensures(Contract.Result<StringBuilder>().Length == sb.Length);

            for (int i = 0; i < sb.Length; i++)
            {
                sb[i] = char.ToLower(sb[i], culture);
            }

            return sb;
        }

        /// <summary>
        /// Returns a <see cref="System.Text.StringBuilder"/> converted to lowercase, using the casing rules of the invarian culture.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to convert to lowercase.</param>
        /// <returns>The <see cref="System.Text.StringBuilder"/> converted to lowercase using invariant culture.</returns>
        public static StringBuilder ToLowerInvariant(this StringBuilder sb)
        {
            //Contract.Ensures(Contract.Result<StringBuilder>() != null);
            //Contract.Ensures(Contract.Result<StringBuilder>().Length == sb.Length);

            return sb.ToLower(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a <see cref="System.Text.StringBuilder"/> converted to uppercase.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to convert to uppercase.</param>
        /// <returns>The <see cref="System.Text.StringBuilder"/> converted to uppercase.</returns>
        public static StringBuilder ToUpper(this StringBuilder sb)
        {
            //Contract.Ensures(Contract.Result<StringBuilder>() != null);
            //Contract.Ensures(Contract.Result<StringBuilder>().Length == sb.Length);

            for (int i = 0; i < sb.Length; i++)
            {
                sb[i] = char.ToUpper(sb[i]);
            }

            return sb;
        }

        /// <summary>
        /// Returns a <see cref="System.Text.StringBuilder"/> converted to uppercase, using the casing rules of the specified culture.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to convert to uppercase.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>The <see cref="System.Text.StringBuilder"/> converted to uppercase using specified culture.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="culture"/>is null.</exception>
        public static StringBuilder ToUpper(this StringBuilder sb, CultureInfo culture)
        {
            //Contract.Requires<ArgumentNullException>(culture != null);
            //Contract.Ensures(Contract.Result<StringBuilder>() != null);
            //Contract.Ensures(Contract.Result<StringBuilder>().Length == sb.Length);

            for (int i = 0; i < sb.Length; i++)
            {
                sb[i] = char.ToUpper(sb[i], culture);
            }

            return sb;
        }

        /// <summary>
        /// Returns a <see cref="System.Text.StringBuilder"/> converted to uppercase, using the casing rules of the invariant culture.
        /// </summary>
        /// <param name="sb">A <see cref="System.Text.StringBuilder"/> to convert to uppercase.</param>
        /// <returns>The <see cref="System.Text.StringBuilder"/> converted to uppercase using invariant culture.</returns>
        public static StringBuilder ToUpperInvariant(this StringBuilder sb)
        {
            //Contract.Ensures(Contract.Result<StringBuilder>() != null);
            //Contract.Ensures(Contract.Result<StringBuilder>().Length == sb.Length);

            return sb.ToUpper(CultureInfo.InvariantCulture);
        }
    }
}
