namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Globalization;
	using System.IO;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Security.Cryptography;
	using System.Text;
	using System.Text.RegularExpressions;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Miscellaneous helper functions for HTML and related.
	/// </summary>
	public sealed class HtmlHelper
	{
		#region Public routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Like Path.Combine() but for virtual paths with forwardslash.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public static string VirtualPathCombine(
			string x,
			string y )
		{
			if ( x == null && y == null )
			{
				return null;
			}
			else
			{
				if ( x == null )
				{
					return y;
				}
				else if ( y == null )
				{
					return x;
				}
				else
				{
					return Path.Combine(
						x.Replace( @"/", @"\" ),
						y.Replace( @"/", @"\" ) ).
						Replace( @"\", @"/" );
				}
			}
		}

		/// <summary>
		/// Formats a multiline (ML) string. replaces '\n' with '&lt;br /&gt;'.
		/// </summary>
		/// <param name="textToFormat">The text to format.</param>
		/// <returns></returns>
		public static string FormatMultiLine(
			string textToFormat )
		{
			if ( textToFormat == null )
			{
				return string.Empty;
			}
			else
			{
				string result = textToFormat;
				if ( result.Length > 0 )
				{
					result = result.Replace( "\r\n", "\n" );
					result = result.Replace( "\r", "\n" );
					result = result.Replace( "\n", @"<br>" );
					return result;
				}
				else
				{
					return textToFormat;
				}
			}
		}

		/// <summary>
		/// Escapes special characters used in JavaScript strings.
		/// </summary>
		/// <param name="textToEscape">The unescaped textToEscape to escape.</param>
		/// <returns>Returns the escaped textToEscape.</returns>
		public static string EscapeJavaScript(
			string textToEscape )
		{
			if ( textToEscape == null )
			{
				return textToEscape;
			}
			else
			{
				textToEscape = textToEscape.Replace( "'", "\\'" );
				textToEscape = textToEscape.Replace( "\"", "\\\"" );

				return textToEscape;
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}