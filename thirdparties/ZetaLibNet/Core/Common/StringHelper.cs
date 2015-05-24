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
	using System.Threading;
	using System.Reflection;
	using System.ComponentModel;
	using System.Collections.Generic;
	using ZetaLib.Core.Logging;
	using ZetaLib.Core.Collections;
	using ZetaLib.Core.Properties;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Miscellaneous helper functions for strings.
	/// </summary>
	public sealed class StringHelper
	{
		#region Special conversions from and to strings.
		// ------------------------------------------------------------------

		/// <summary>
		/// Serializes an object to a string.
		/// </summary>
		/// <param name="o">The object to serialize.</param>
		/// <returns>
		/// Returns the base64-encoded string
		/// that represents the serialized object.
		/// </returns>
		public static string SerializeToString(
			object o )
		{
			if ( o == null )
			{
				return null;
			}
			else
			{
				StringDictionary dic = o as StringDictionary;

				// Special case for string dictionary.
				if ( dic != null )
				{
					Hashtable ht = new Hashtable();

					foreach ( string key in dic.Keys )
					{
						ht[key] = dic[key];
					}

					// Remember.
					ht[stringDictionaryKey] = true;

					o = ht;
				}

				IFormatter f = new BinaryFormatter();
				using ( MemoryStream stream = new MemoryStream() )
				{
					f.Serialize( stream, o );

					byte[] buf = stream.ToArray();
					return Convert.ToBase64String( buf );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private const string stringDictionaryKey =
			@"__is_StringDictionary_wrapper__";

		/// <summary>
		/// Deserializes an object from a string.
		/// </summary>
		/// <param name="s">The base64-encoded string
		/// that represents the serialized object.</param>
		/// <returns>Returns the deserialized object.</returns>
		public static object DeserializeFromString(
			string s )
		{
			return DeserializeFromString(
				s,
				true );
		}

		/// <summary>
		/// Deserializes an object from a string.
		/// </summary>
		/// <param name="s">The base64-encoded string
		/// that represents the serialized object.</param>
		/// <param name="ignoreSerializationExceptions">if set to <c>true</c> [ignore serialization exceptions].</param>
		/// <returns>Returns the deserialized object.</returns>
		public static object DeserializeFromString(
			string s,
			bool ignoreSerializationExceptions )
		{
			if ( s == null || s.Length <= 0 )
			{
				return null;
			}
			else
			{
				try
				{
					IFormatter f = new BinaryFormatter();

					using ( MemoryStream stream = new MemoryStream(
						Convert.FromBase64String( s ) ) )
					{
						object o = f.Deserialize( stream );

						// Special case for string dictionary.
						if ( o is Hashtable )
						{
							Hashtable ht = o as Hashtable;

							if ( ht.ContainsKey( stringDictionaryKey ) )
							{
								ht.Remove( stringDictionaryKey );

								StringDictionary dic =
									new StringDictionary();

								foreach ( string key in ht.Keys )
								{
									object ob = ht[key];

									if ( ob == null )
									{
										dic[key] = null;
									}
									else
									{
										dic[key] = ob.ToString();
									}
								}

								return dic;
							}
							else
							{
								return o;
							}
						}
						else
						{
							return o;
						}
					}
				}
				catch ( FormatException x )
				{
					if ( ignoreSerializationExceptions )
					{
						// Can happen due to legacy issues.
						// Ignore, but log for statistics.
						LogCentral.Current.LogWarn(
							string.Format(
							@"FormatException while deserializing from " +
							@"string with {0} characters length ('{1}'). " +
							@"Returning NULL.",
							s.Length,
							s ), x );

						return null;
					}
					else
					{
						throw;
					}
				}
				catch ( SerializationException x )
				{
					if ( ignoreSerializationExceptions )
					{
						// Can happen due to legacy issues.
						// Ignore, but log for statistics.
						LogCentral.Current.LogWarn(
							string.Format(
							@"SerializationException while deserializing " +
							@"from string with {0} characters length ('{1}'). " +
							@"Returning NULL.",
							s.Length,
							s ), x );

						return null;
					}
					else
					{
						throw;
					}
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Encryption/decryption.
		// ------------------------------------------------------------------

		/// <summary>
		/// Encrypt a string.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		/// <see cref="http://www.codeproject.com/dotnet/encryption_decryption.asp"/>
		/// <see cref="http://dobon.net/vb/dotnet/string/encryptstring.html"/>
		public static string EncryptString(
			string source )
		{
			if ( string.IsNullOrEmpty( source ) )
			{
				return source;
			}
			else
			{
				byte[] bytIn = Encoding.UTF8.GetBytes( source );

				// create a MemoryStream so that the process can be done without I/O files
				using ( MemoryStream ms = new MemoryStream() )
				{
					using ( DESCryptoServiceProvider mobjCryptoService =
						 new DESCryptoServiceProvider() )
					{
						byte[] bytKey = cryptKey;

						// set the private key
						mobjCryptoService.Key = bytKey;
						mobjCryptoService.IV = bytKey;

						// create an Encryptor from the Provider Service instance
						using ( ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor() )
						{
							// create Crypto Stream that transforms a stream using the encryption
							using ( CryptoStream cs = new CryptoStream(
								ms,
								encrypto,
								CryptoStreamMode.Write ) )
							{
								// write out encrypted content into MemoryStream
								cs.Write( bytIn, 0, bytIn.Length );
								cs.FlushFinalBlock();

								// http://www.codeproject.com/dotnet/encryption_decryption.asp?msg=749582#xx749582xx
								byte[] byteOut = ms.GetBuffer();
								return System.Convert.ToBase64String(
									byteOut,
									0,
									(int)ms.Length );
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Decrypt a previously encrypted string.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		/// <see cref="http://www.codeproject.com/dotnet/encryption_decryption.asp"/>
		/// <see cref="http://dobon.net/vb/dotnet/string/encryptstring.html"/>
		public static string DecryptString(
			string source )
		{
			if ( string.IsNullOrEmpty( source ) )
			{
				return source;
			}
			else
			{
				// convert from Base64 to binary
				byte[] bytIn = Convert.FromBase64String( source );
				// create a MemoryStream with the input
				using ( MemoryStream ms = new MemoryStream(
					bytIn,
					0,
					bytIn.Length ) )
				{
					byte[] bytKey = cryptKey;

					// set the private key.
					using ( DESCryptoServiceProvider mobjCryptoService =
						new DESCryptoServiceProvider() )
					{
						mobjCryptoService.Key = bytKey;
						mobjCryptoService.IV = bytKey;

						// create a Decryptor from the Provider Service instance
						using ( ICryptoTransform encrypto =
							mobjCryptoService.CreateDecryptor() )
						{
							// create Crypto Stream that transforms a 
							// stream using the decryption
							using ( CryptoStream cs = new CryptoStream(
								ms,
								encrypto,
								CryptoStreamMode.Read ) )
							{
								// read out the result from the Crypto Stream
								using ( StreamReader sr = new StreamReader(
									cs,
									Encoding.UTF8 ) )
								{
									return sr.ReadToEnd();
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets the crypt key.
		/// </summary>
		/// <value>The crypt key.</value>
		private static byte[] cryptKey
		{
			get
			{
				return new byte[]
				{
					0x0E,
					0x41,
					0x6A,
					0x29,
					0x94,
					0x12,
					0xEB,
					0x63
				};
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Miscellaneous routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Reads the description value from an enumeration.
		/// </summary>
		/// <param name="value">The description of the enum value to
		/// read.</param>
		/// <returns>
		/// Returns the description string or the enum value
		/// as a string, if no description was found.
		/// </returns>
		/// <seealso cref="http://www.codeproject.com/csharp/EnumDescConverter.asp."/>
		public static string GetEnumDescription(
			Enum value )
		{
			string result;
			if ( recentEnumDescriptions.TryGetValue( value, out result ) )
			{
				return result;
			}
			else
			{
				FieldInfo fi = value.GetType().GetField( value.ToString() );

				DescriptionAttribute[] attributes =
					(DescriptionAttribute[])fi.GetCustomAttributes(
					typeof( DescriptionAttribute ),
					false );

				if ( attributes != null &&
					attributes.Length > 0 )
				{
					result = attributes[0].Description;
				}
				else
				{
					result = value.ToString();
				}

				recentEnumDescriptions[value] = result;
				return result;
			}
		}

		/// <summary>
		/// Generate a MD5 hash from a given string.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static string GenerateHash(
			string input )
		{
			if ( string.IsNullOrEmpty( input ) )
			{
				return input;
			}
			else
			{
				return input.GetHashCode().ToString(
					@"X",
					CultureInfo.InvariantCulture );
			}
		}

		/// <summary>
		/// Converts a numeric value into a string that represents the number
		/// expressed as a size value in bytes, kilobytes, megabytes, or
		/// gigabytes, depending on the size.
		/// </summary>
		/// <param name="fileSize">Size of the file.</param>
		/// <returns></returns>
		public static string FormatFileSize(
			int fileSize )
		{
			return FormatFileSize( (long)fileSize );
		}

		/// <summary>
		/// Converts a numeric value into a string that represents the number
		/// expressed as a size value in bytes, kilobytes, megabytes, or
		/// gigabytes, depending on the size.
		/// </summary>
		/// <param name="fileSize">Size of the file.</param>
		/// <returns></returns>
		public static string FormatFileSize(
			long fileSize )
		{
			const long fileSize1KB = 1024;
			const long fileSize100KB = 102400;
			const long fileSize1MB = 1048576;
			const long fileSize1GB = 1073741824;
			const long fileSize1TB = 1099511627776;

			if ( fileSize < fileSize1KB )
			{
				return string.Format(
					Thread.CurrentThread.CurrentCulture,
					@"{0} {1}",
					fileSize,
					Resources.Str_FormatFileSize_Bytes );
			}
			else if ( fileSize < fileSize100KB )
			{
				return string.Format(
					Thread.CurrentThread.CurrentCulture,
					@"{0:F1} {1}",
					(double)fileSize / (double)fileSize1KB,
					Resources.Str_FormatFileSize_KB );
			}
			else if ( fileSize < fileSize1MB )
			{
				return string.Format(
					Thread.CurrentThread.CurrentCulture,
					@"{0} {1}",
					fileSize / fileSize1KB,
					Resources.Str_FormatFileSize_KB );
			}
			else if ( fileSize < fileSize1GB )
			{
				return string.Format(
					Thread.CurrentThread.CurrentCulture,
					@"{0:F1} {1}",
					(double)fileSize / (double)fileSize1MB,
					Resources.Str_FormatFileSize_MB );
			}
			else if ( fileSize < fileSize1TB )
			{
				return string.Format(
					Thread.CurrentThread.CurrentCulture,
					@"{0:F2} {1}",
					(double)fileSize / (double)fileSize1GB,
					Resources.Str_FormatFileSize_GB );
			}
			else
			{
				return string.Format(
					Thread.CurrentThread.CurrentCulture,
					@"{0:F2} {1}",
					(double)fileSize / (double)fileSize1TB,
					Resources.Str_FormatFileSize_TB );
			}
		}

		/// <summary>
		/// Escape special RX characters.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="ignoreChars">The ignore chars.</param>
		/// <returns></returns>
		public static string EscapeRXCharacters(
			string text,
			params char[] ignoreChars )
		{
			Set<char> ignores = new Set<char>( ignoreChars );

			// As first!
			if ( !ignores.Contains( '\\' ) )
			{
				text = text.Replace( @"\", @"\\" );
			}

			if ( !ignores.Contains( '+' ) )
			{
				text = text.Replace( @"+", @"\+" );
			}
			if ( !ignores.Contains( '+' ) )
			{
				text = text.Replace( @"?", @"\?" );
			}
			if ( !ignores.Contains( '.' ) )
			{
				text = text.Replace( @".", @"\." );
			}
			if ( !ignores.Contains( '*' ) )
			{
				text = text.Replace( @"*", @"\*" );
			}
			if ( !ignores.Contains( '^' ) )
			{
				text = text.Replace( @"^", @"\^" );
			}
			if ( !ignores.Contains( '$' ) )
			{
				text = text.Replace( @"$", @"\$" );
			}
			if ( !ignores.Contains( '(' ) )
			{
				text = text.Replace( @"(", @"\(" );
			}
			if ( !ignores.Contains( ')' ) )
			{
				text = text.Replace( @")", @"\)" );
			}
			if ( !ignores.Contains( '[' ) )
			{
				text = text.Replace( @"[", @"\[" );
			}
			if ( !ignores.Contains( ']' ) )
			{
				text = text.Replace( @"]", @"\]" );
			}
			if ( !ignores.Contains( '{' ) )
			{
				text = text.Replace( @"{", @"\{" );
			}
			if ( !ignores.Contains( '}' ) )
			{
				text = text.Replace( @"}", @"\}" );
			}
			if ( !ignores.Contains( '|' ) )
			{
				text = text.Replace( @"|", @"\|" );
			}

			return text;
		}

		/// <summary>
		/// Generates a matchCode, based on 'name', if 'matchCode' is
		/// currently NOT a valid matchCode.
		/// otherwise just returns 'matchCode'.
		/// </summary>
		/// <param name="matchCode">Matchcode to check.</param>
		/// <param name="name">Base for possible matchCode to generate.</param>
		/// <returns>Returns the ready-to-use matchCode.</returns>
		public static string GenerateMatchCode(
			string matchCode,
			string name )
		{
			if ( IsValidMatchCode( matchCode ) )
			{
				return matchCode;
			}
			else
			{
				matchCode = name.ToLower();
				matchCode = matchCode.Trim();

				matchCode = RXReplace( matchCode, @"^[^a-z_]", @"_", @"gs" );			// first char.
				matchCode = RXReplace( matchCode, @"[^a-z0-9_]", @"_", @"gs" );		// second+ chars.

				return matchCode;
			}
		}

		/// <summary>
		/// Generates the match code.
		/// </summary>
		/// <param name="matchCode">The match code.</param>
		/// <returns></returns>
		public static string GenerateMatchCode(
			string matchCode )
		{
			return GenerateMatchCode( matchCode, matchCode );
		}

		/// <summary>
		/// Splits the extended.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <param name="separator">The separator.</param>
		/// <returns></returns>
		public static string[] SplitExtended(
			string s,
			string separator )
		{
			if ( string.IsNullOrEmpty( s ) )
			{
				return null;
			}
			else
			{
				List<string> list = new List<string>();

				int lastPos = 0;
				int pos = 0;
				while ( (pos = s.IndexOf( separator, lastPos )) >= 0 )
				{
					list.Add( s.Substring( lastPos, pos - lastPos + 1 ) );

					lastPos = pos + separator.Length;
				}

				if ( lastPos < s.Length - 1 )
				{
					list.Add( s.Substring( lastPos ) );
				}

				return list.ToArray();
			}
		}

		/// <summary>
		/// Splits the extended.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <param name="separator">The separator.</param>
		/// <returns></returns>
		public static string[] SplitExtended(
			string s,
			params char[] separator )
		{
			string[] t = s.Split( separator );

			// remove empties.
			List<string> list = new List<string>();
			foreach ( string u in t )
			{
				if ( u.Length > 0 )
					list.Add( u );
			}

			return list.ToArray();
		}

		/// <summary>
		/// Check whether a given matchCode is valid.
		/// </summary>
		/// <param name="matchCode">The matchCode to check.</param>
		/// <returns>Returns TRUE if valid, FALSE otherwise.</returns>
		public static bool IsValidMatchCode(
			string matchCode )
		{
			return RXTest( matchCode, @"^[a-z_][a-z0-9_]*$", @"si" );
		}

		/// <summary>
		/// Appends zeros ('0') at the head of the passed string,
		/// until a certain number of total characters is reached.
		/// </summary>
		/// <param name="text">The string to append the zeros.</param>
		/// <param name="length">The required number of characters.</param>
		/// <returns>
		/// Returns the string with the appended number of
		/// characters.
		/// </returns>
		public static string AddZerosPrefix(
			object text,
			int length )
		{
			string s = Convert.ToString( text );

			if ( !string.IsNullOrEmpty( s ) && s.Length < length )
			{
				StringBuilder sb = new StringBuilder( s );
				sb.Append( '0', length - s.Length );

				return sb.ToString();
			}
			else
			{
				return s;
			}
		}

		/// <summary>
		/// Mimics the CString.Left() function.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public static string Left(
			string s,
			int count )
		{
			if ( string.IsNullOrEmpty( s ) )
			{
				return s;
			}
			else
			{
				return s.Substring( 0, Math.Min( count, s.Length ) );
			}
		}

		/// <summary>
		/// Mimics the CString.Right() function.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public static string Right(
			string s,
			int count )
		{
			if ( string.IsNullOrEmpty( s ) )
			{
				return s;
			}
			else
			{
				int length = s.Length;

				if ( s.Length <= count )
				{
					return s;
				}
				else
				{
					return s.Substring( length - count );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Conversion checkings.
		// ------------------------------------------------------------------

		/// <summary>
		/// Does some (weak) validations whether the given strings
		/// are valid e-mail addresses.
		/// It only returns TRUE if all strings are valid.
		/// </summary>
		/// <param name="emailAddresses">The strings to check.</param>
		/// <returns>
		/// Returns TRUE if the e-mail addresses seems to be valid,
		/// FALSE if not.
		/// </returns>
		public static bool IsValidEMailAddress(
			string[] emailAddresses )
		{
			if ( emailAddresses == null || emailAddresses.Length <= 0 )
			{
				return false;
			}
			else
			{
				return IsValidEMailAddress(
					string.Join( @";", emailAddresses ) );
			}
		}

		/// <summary>
		/// Does some (weak) validations whether the given string
		/// is a valid e-mail address.
		/// The string can contain multiple e-mail-addresses, separated by
		/// semicolon (";"). It then only returns TRUE if all are valid.
		/// </summary>
		/// <param name="emailAddress">The string to check.</param>
		/// <returns>
		/// Returns TRUE if the e-mail address seems to be valid,
		/// FALSE if not.
		/// </returns>
		public static bool IsValidEMailAddress(
			string emailAddress )
		{
			if ( emailAddress == null || emailAddress.Trim().Length <= 0 )
			{
				return false;
			}
			else
			{
				string[] ss = emailAddress.Split( ';' );

				if ( ss.Length <= 0 )
				{
					return false;
				}
				else
				{
					foreach ( string s_ in ss )
					{
						string s = s_.Trim();

						if ( s.Length <= 0 )
						{
							return false;
						}
						else
						{
							// Location of "@".
							int atPos = s.IndexOf( @"@" );

							if ( atPos < 0 ||
								s.LastIndexOf( @"." ) < atPos + 1 )
							{
								// "@" must exist, and last "." 
								// in string must follow the "@".
								return false;
							}
							else if ( s.IndexOf( @"@", atPos + 1 ) > atPos )
							{
								// String can't have more than one "@".
								return false;
							}
							else if ( s.Substring( atPos + 1, 1 ) == @"." )
							{
								// String can't have "." 
								// immediately following "@".
								return false;
							}
							else if ( s.Substring(
								s.Length - 2 ).IndexOf( @"." ) > 0 )
							{
								// String must have at least 
								// a two-character top-level domain.
								return false;
							}
						}
					}

					// All valid.
					return true;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Regular expression routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Performs a regular expression replace like ( )~ s/ / / in Perl.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="pattern">The pattern.</param>
		/// <param name="replacement">The replacement.</param>
		/// <param name="flags">"i":ignore case and "g":global,
		/// "m":multiline.</param>
		/// <returns></returns>
		public static string RXReplace(
			string text,
			string pattern,
			string replacement,
			string flags )
		{
			RegexOptions options =
				ConvertRXOptionsFromString( flags );

			Regex rx = new Regex( pattern, options );
			if ( flags.Contains( @"g" ) )
			{
				return rx.Replace( text, replacement );
			}
			else
			{
				return rx.Replace( text, replacement, 1 );
			}
		}

		/// <summary>
		/// Performs a regular expression test like ( )~ m/ / in Perl.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="pattern">The pattern.</param>
		/// <param name="flags">"i":ignore case and "m":multiline.</param>
		/// <returns></returns>
		public static bool RXTest(
			string text,
			string pattern,
			string flags )
		{
			if ( string.IsNullOrEmpty( text ) )
			{
				return false;
			}
			else
			{
				RegexOptions options =
					ConvertRXOptionsFromString( flags );

				Regex rx = new Regex(
					pattern,
					options );
				return rx.IsMatch( text );
			}
		}

		/// <summary>
		/// Performs a regular expression test like ( )~ m/ / in Perl.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="pattern">The pattern.</param>
		/// <param name="flags">"i":ignore case and "m":multiline.</param>
		/// <returns>Returns the number of matches.</returns>
		public static int RXTestCount(
			string text,
			string pattern,
			string flags )
		{
			if ( string.IsNullOrEmpty( text ) )
			{
				return 0;
			}
			else
			{
				RegexOptions options =
					ConvertRXOptionsFromString( flags );

				Regex rx = new Regex(
					pattern,
					options );
				return rx.Matches( text ).Count;
			}
		}

		/// <summary>
		/// Converts between Perl-style RX flags and .NET RX options.
		/// </summary>
		/// <param name="flags">The Perl-style RX flags.</param>
		/// <returns>Returns the .NET RX options.</returns>
		public static RegexOptions ConvertRXOptionsFromString(
			string flags )
		{
			RegexOptions options;

			if ( recentRegexOptions.TryGetValue( flags, out options ) )
			{
				return options;
			}
			else
			{
				options = RegexOptions.None;

				if ( flags.Contains( @"i" ) )
				{
					options |= RegexOptions.IgnoreCase;
				}
				if ( flags.Contains( @"x" ) )
				{
					options |= RegexOptions.IgnorePatternWhitespace;
				}
				if ( flags.Contains( @"m" ) )
				{
					options |= RegexOptions.Multiline;
				}
				if ( flags.Contains( @"s" ) )
				{
					options |= RegexOptions.Singleline;
				}

				recentRegexOptions[flags] = options;
				return options;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private helper.
		// ------------------------------------------------------------------

		/// <summary>
		/// Much faster access by storing previously read values.
		/// </summary>
		private static Dictionary<Enum, string> recentEnumDescriptions =
			new Dictionary<Enum, string>();

		/// <summary>
		/// Much faster access by storing previously read values.
		/// </summary>
		private static Dictionary<string, RegexOptions> recentRegexOptions =
			new Dictionary<string, RegexOptions>();

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}