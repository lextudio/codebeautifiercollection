// this is the string helper class. 
// Copyright (C) 2006  Lex Y. Li
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace Lextm
{
	/// <summary>
	/// String helper.
	/// </summary>
	public sealed class StringHelper
	{
		private StringHelper()
		{
		}
		/// <summary>
		/// Verifies a string is white.
		/// </summary>
		/// <param name="str">String</param>
		/// <returns></returns>
		public static bool IsWhite(string str) {
			return (str.Trim().Length == 0);
		}
		/// <summary>
		/// Verifies a string is null or empty.
		/// </summary>
		/// <param name="str">String</param>
		/// <returns></returns>
		public static bool IsHttpUri(string str) {
			return str.StartsWith("http://", StringComparison.OrdinalIgnoreCase) 
				|| str.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
				|| str.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase);
		}
        /// <summary>
        /// Gets line value.
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="lineNumber">Line number</param>
        /// <returns>String.Empty if wrong.</returns>
        public static string GetLineValue(string str, int lineNumber)
        {
            if (lineNumber < 1)
            {
                return String.Empty;
            }

            StringReader _Reader = new StringReader(str);

            string _Line = null;

            for (int i = 0; i < lineNumber; i++)
            {
                _Line = _Reader.ReadLine();

                if (_Line == null)
                {
                    return String.Empty;
                }
            }

            return _Line;
        }
        /// <summary>
        /// Verifies is string case ins.
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="pos">Position</param>
        /// <param name="subStr">Substring</param>
        /// <returns></returns>
        public static bool IsStrCaseIns(string source, int pos, string subStr)
        {
            if (pos + subStr.Length - 1 <= source.Length)
            {
                return subStr.ToUpperInvariant() == source.Substring(pos, subStr.Length).ToUpperInvariant();
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Quotes a string.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>String.Empty if wrong.</returns>
        public static string QuoteString(string str)
        {
            if (str == null)
            {
                return String.Empty;
            }

            return String.Format(CultureInfo.InvariantCulture, "\"{0}\"", str);
        }
        /// <summary>
        /// Gets lines from string.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>new string array if wrong.</returns>
        public static IList<string> GetLinesFromString(string str)
        {
            List<string> result = new List<string>();
            if (string.IsNullOrEmpty(str))
            {
                return result;
            }
            StringReader _Reader = new StringReader(str);
            string _Line = _Reader.ReadLine();
            while (_Line != null)
            {
                result.Add(_Line);
                _Line = _Reader.ReadLine();
            }
            return result;
        }
        /// <summary>
        /// Searches text.
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="source">Source</param>
        /// <param name="currentPos">Current position</param>
        /// <param name="prev">Previous</param>
        /// <param name="foundPos">Found position</param>
        /// <returns>true if found, false if not.</returns>
        public static bool SearchText(string text,
                                      string source,
                                      int currentPos,
                                      bool prev,
                                      out int foundPos)
        {
            int _StartPos = currentPos;
            foundPos = -1;

            if (text.Length == 0)
            {
                return false;
            }

            text = text.ToUpperInvariant();

            while (GoNext(source, ref _StartPos, prev))
            {
                //if (Char.ToUpper(aSource[_StartPos]) == aText[0]) {
                if (IsStrCaseIns(source, _StartPos, text))
                {
                    foundPos = _StartPos;
                    return true;
                }
                //}
            }

            return false;
        }
        /// <summary>
        /// Goes to next.
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="startPos">Start position</param>
        /// <param name="prev">Previous</param>
        /// <returns>true if right, false if out of bound.</returns>
        public static bool GoNext(string source, ref int startPos, bool prev)
        {
            if (prev)
            {
                startPos--;
            }
            else
            {
                startPos++;
            }
            return (startPos >= 0) && (startPos < source.Length);
        }
	}
}
