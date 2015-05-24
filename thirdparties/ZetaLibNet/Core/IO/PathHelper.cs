namespace ZetaLib.Core.IO
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Globalization;
	using System.IO;
	using System.Text;
	using System.Runtime.InteropServices;
	using System.Diagnostics;
	using System.Reflection;
	using System.Collections.Generic;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Miscellaneous helper functions for manipulating filenames and
	/// folder names.
	/// </summary>
	/// <remarks>Parts copied from "HttpUtility.cs" from MONO.</remarks>
	public sealed class PathHelper
	{
		#region HTML- and URL-encoding/-decoding.
		// ------------------------------------------------------------------

		/// <summary>
		/// What to encode.
		/// </summary>
		public enum UrlEncoding
		{
			#region Enum members.

			/// <summary>
			/// Escape all unsafe characters.
			/// </summary>
			XAlphas,

			/// <summary>
			/// As XAlphas but allows '+'.
			/// </summary>
			XPAlphas,

			/// <summary>
			/// As XPAlphas but allows '/'.
			/// </summary>
			Path,

			/// <summary>
			/// As Path but allows ':'.
			/// </summary>
			DosFile

			#endregion
		}

		/// <summary>
		/// URL-encode a text with the XAlphas encoding.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static string UrlEncode(
			string text )
		{
			return UrlEncode(
				text,
				UrlEncoding.XAlphas,
				Encoding.UTF8 );
		}

		/// <summary>
		/// URL-encode a text with the given encoding.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <param name="urlEncoding">The URL encoding.</param>
		/// <returns></returns>
		public static string UrlEncode(
			string s,
			UrlEncoding urlEncoding )
		{
			return UrlEncode(
				s,
				urlEncoding,
				Encoding.UTF8 );
		}

		/// <summary>
		/// URL-encode a text with the given encoding.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <param name="urlEncoding">The URL encoding.</param>
		/// <param name="e">The e.</param>
		/// <returns></returns>
		public static string UrlEncode(
			string s,
			UrlEncoding urlEncoding,
			Encoding e )
		{
			if ( s == null )
			{
				return null;
			}
			else
			{
				if ( s.Length <= 0 )
				{
					return string.Empty;
				}
				else
				{
					byte[] bytes = e.GetBytes( s );
					return Encoding.ASCII.GetString(
						UrlEncodeToBytes(
						bytes,
						0, bytes.Length,
						urlEncoding ) );
				}
			}
		}

		/// <summary>
		/// URL-decode a text.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns></returns>
		public static string UrlDecode(
			string s )
		{
			return UrlDecode( s, Encoding.UTF8 );
		}

		/// <summary>
		/// URL-decode a text.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <param name="e">The e.</param>
		/// <returns></returns>
		public static string UrlDecode(
			string s,
			Encoding e )
		{
			if ( null == s )
			{
				return null;
			}
			else if ( s.IndexOf( '%' ) == -1 && s.IndexOf( '+' ) == -1 )
			{
				return s;
			}
			else
			{
				if ( e == null )
				{
					e = Encoding.UTF8;
				}

				StringBuilder output = new StringBuilder();
				long len = s.Length;
				NumberStyles hexa = NumberStyles.HexNumber;
				MemoryStream bytes = new MemoryStream();

				for ( int i = 0; i < len; i++ )
				{
					if ( s[i] == '%' && i + 2 < len )
					{
						if ( s[i + 1] == 'u' && i + 5 < len )
						{
							if ( bytes.Length > 0 )
							{
								output.Append( GetChars( bytes, e ) );
								bytes.SetLength( 0 );
							}
							output.Append( (char)Int32.Parse( s.Substring( i + 2, 4 ), hexa ) );
							i += 5;
						}
						else
						{
							bytes.WriteByte( (byte)Int32.Parse( s.Substring( i + 1, 2 ), hexa ) );
							i += 2;
						}
						continue;
					}

					if ( bytes.Length > 0 )
					{
						output.Append( GetChars( bytes, e ) );
						bytes.SetLength( 0 );
					}

					if ( s[i] == '+' )
					{
						output.Append( ' ' );
					}
					else
					{
						output.Append( s[i] );
					}
				}

				if ( bytes.Length > 0 )
				{
					output.Append( GetChars( bytes, e ) );
				}

				bytes = null;
				return output.ToString();
			}
		}

		/// <summary>
		/// HTML-encode a text.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns></returns>
		public static string HtmlEncode(
			string s )
		{
			if ( string.IsNullOrEmpty( s ) )
			{
				return null;
			}
			else
			{
				StringBuilder output = new StringBuilder();

				foreach ( char c in s )
				{
					switch ( c )
					{
						case '&':
							output.Append( @"&amp;" );
							break;
						case '>':
							output.Append( @"&gt;" );
							break;
						case '<':
							output.Append( @"&lt;" );
							break;
						case '"':
							output.Append( @"&quot;" );
							break;
						default:
							if ( (int)c > 128 )
							{
								output.Append( @"&#" );
								output.Append( ((int)c).ToString() );
								output.Append( @";" );
							}
							else
								output.Append( c );
							break;
					}
				}
				return output.ToString();
			}
		}

		/// <summary>
		/// HTML-decode a text.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns></returns>
		public static string HtmlDecode(
			string s )
		{
			if ( string.IsNullOrEmpty( s ) )
			{
				throw new ArgumentNullException( @"s" );
			}
			else if ( s.IndexOf( '&' ) == -1 )
			{
				return s;
			}
			else
			{
				bool insideEntity = false; // used to indicate that we are in a potential entity
				string entity = String.Empty;
				StringBuilder output = new StringBuilder();
				int len = s.Length;

				for ( int i = 0; i < len; i++ )
				{
					char c = s[i];
					switch ( c )
					{
						case '&':
							output.Append( entity );
							entity = @"&";
							insideEntity = true;
							break;
						case ';':
							if ( !insideEntity )
							{
								output.Append( c );
								break;
							}

							entity += c;
							int length = entity.Length;
							if ( length >= 2 && entity[1] == '#' && entity[2] != ';' )
							{
								entity = ((char)Int32.Parse( entity.Substring( 2, entity.Length - 3 ) )).ToString();
							}
							else if ( length > 1 && Entities.ContainsKey( entity.Substring( 1, entity.Length - 2 ) ) )
							{
								entity = Entities[entity.Substring( 1, entity.Length - 2 )].ToString();
							}
							output.Append( entity );
							entity = String.Empty;
							insideEntity = false;
							break;
						default:
							if ( insideEntity )
							{
								entity += c;
							}
							else
							{
								output.Append( c );
							}
							break;
					}
				}
				output.Append( entity );
				return output.ToString();
			}
		}

		/// <summary>
		/// Converts a windows file path
		/// (with drive letter or UNC) to a "file://"-URL.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns></returns>
		public static string ConvertFilePathToFileUrl(
			string filePath )
		{
			string fileUrl = filePath;
			fileUrl = fileUrl.Replace( @"\", @"/" );

			fileUrl = UrlEncode(
				fileUrl,
				UrlEncoding.DosFile );

			fileUrl = fileUrl.TrimStart( '/' );

			if ( PathHelper.IsUncPath( filePath ) )
			{
				fileUrl = @"file://" + fileUrl;
			}
			else if ( PathHelper.IsDriveLetterPath( filePath ) )
			{
				fileUrl = @"file:///" + fileUrl;
			}
			else
			{
				fileUrl = @"file:///" + fileUrl;
			}

			return fileUrl;
		}

		/// <summary>
		/// The other way around.
		/// </summary>
		/// <param name="fileUrl">The file URL.</param>
		/// <returns></returns>
		public static string ConvertFileUrlToFilePath(
			string fileUrl )
		{
			string prefixA = @"file:///";
			string prefixB = @"file://";

			string filePath;

			if ( fileUrl.IndexOf( prefixA ) == 0 )
			{
				filePath = UrlDecode( fileUrl.Substring( prefixA.Length ) );
			}
			else if ( fileUrl.IndexOf( prefixB ) == 0 )
			{
				filePath = UrlDecode( fileUrl.Substring( prefixB.Length ) );
				filePath = @"\\" + filePath;
			}
			else
			{
				filePath = UrlDecode( fileUrl );
			}

			filePath = filePath.Replace( @"/", @"\" );

			return filePath;
		}

		/// <summary>
		/// Determines whether [is drive letter path] [the specified file path].
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns>
		/// 	<c>true</c> if [is drive letter path] [the specified file path]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsDriveLetterPath(
			string filePath )
		{
			if ( string.IsNullOrEmpty( filePath ) )
			{
				return false;
			}
			else
			{
				return filePath.IndexOf( ':' ) == 1;
			}
		}

		/// <summary>
		/// Determines whether [is unc path] [the specified file path].
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns>
		/// 	<c>true</c> if [is unc path] [the specified file path]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsUncPath(
			string filePath )
		{
			if ( string.IsNullOrEmpty( filePath ) )
			{
				return false;
			}
			else
			{
				return
					ConvertForwardSlashsToBackSlashs(
					filePath ).StartsWith( @"\\" ) &&
					!string.IsNullOrEmpty( GetShare( filePath ) );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Miscellaneous function.
		// ------------------------------------------------------------------

		/// <summary>
		/// Sets the back slash end.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="setSlash">if set to <c>true</c> [set slash].</param>
		/// <returns></returns>
		public static string SetBackSlashEnd(
			string path,
			bool setSlash )
		{
			return SetSlashEnd( path, setSlash, '\\' );
		}

		/// <summary>
		/// Sets the forward slash end.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="setSlash">if set to <c>true</c> [set slash].</param>
		/// <returns></returns>
		public static string SetForwardSlashEnd(
			string path,
			bool setSlash )
		{
			return SetSlashEnd( path, setSlash, '/' );
		}

		/// <summary>
		/// Sets the back slash begin.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="setSlash">if set to <c>true</c> [set slash].</param>
		/// <returns></returns>
		public static string SetBackSlashBegin(
			string path,
			bool setSlash )
		{
			return SetSlashBegin( path, setSlash, '\\' );
		}

		/// <summary>
		/// Sets the forward slash begin.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="setSlash">if set to <c>true</c> [set slash].</param>
		/// <returns></returns>
		public static string SetForwardSlashBegin(
			string path,
			bool setSlash )
		{
			return SetSlashBegin( path, setSlash, '/' );
		}

		/// <summary>
		/// Get the parent path, if possible.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static string GetParentPath(
			string text )
		{
			if ( string.IsNullOrEmpty( text ) )
			{
				return text;
			}
			else
			{
				return Path.GetFullPath( Path.Combine( text, @".." ) );
			}
		}

		/// <summary>
		/// Get a temporary file path with file extension ".tmp".
		/// The file will NOT be created.
		/// </summary>
		/// <returns></returns>
		public static FileInfo GetTempFileName()
		{
			return GetTempFileName( @"tmp" );
		}

		/// <summary>
		/// Get a temporary file path with the given file extension.
		/// The file will NOT be created.
		/// </summary>
		/// <param name="extension">The extension.</param>
		/// <returns></returns>
		public static FileInfo GetTempFileName(
			string extension )
		{
			if ( string.IsNullOrEmpty( extension ) )
			{
				extension = @"tmp";
			}

			extension = extension.Trim( '.' );

			string tempFolderPath = Path.GetTempPath();
			string tempFileName = Guid.NewGuid().ToString( @"N" );

			return new FileInfo( PathHelper.Combine(
				tempFolderPath,
				tempFileName + @"." + extension ) );
		}

		/// <summary>
		/// Convert all forward slashs to backslashs.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static string ConvertBackSlashsToForwardSlashs(
			string text )
		{
			if ( string.IsNullOrEmpty( text ) )
			{
				return text;
			}
			else
			{
				return text.Replace( '\\', '/' );
			}
		}

		/// <summary>
		/// Convert all backslashs to forwardslashs.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static string ConvertForwardSlashsToBackSlashs(
			string text )
		{
			if ( string.IsNullOrEmpty( text ) )
			{
				return text;
			}
			else
			{
				return text.Replace( '/', '\\' );
			}
		}

		/// <summary>
		/// Check whether a given path contains an absolute or relative path.
		/// No disk-access is performed, only the syntax of the given string
		/// is checked.
		/// </summary>
		/// <param name="path">The path to check.</param>
		/// <returns>
		/// Returns TRUE if the given path is an absolute path,
		/// returns FALSE if the given path is a relative path.
		/// </returns>
		public static bool IsAbsolutePath(
			string path )
		{
			path = path.Replace( '/', '\\' );

			if ( path.Length < 2 )
			{
				return false;
			}
			else if ( path.Substring( 0, 2 ) == @"\\" )
			{
				// UNC.
				return IsUncPath( path );
			}
			else if ( path.Substring( 1, 1 ) == @":" )
			{
				// "C:"
				return IsDriveLetterPath( path );
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Makes 'path' an absolute path, based on 'basePath'.
		/// If the given path is already an absolute path, the path
		/// is returned unmodified.
		/// </summary>
		/// <param name="pathToMakeAbsolute">The path to make absolute.</param>
		/// <param name="basePathToWhichToMakeAbsoluteTo">The base path to use when making an
		/// absolute path.</param>
		/// <returns>Returns the absolute path.</returns>
		public static string GetAbsolutePath(
			string pathToMakeAbsolute,
			string basePathToWhichToMakeAbsoluteTo )
		{
			if ( IsAbsolutePath( pathToMakeAbsolute ) )
			{
				return pathToMakeAbsolute;
			}
			else
			{
				return Path.GetFullPath(
					Path.Combine(
					basePathToWhichToMakeAbsoluteTo,
					pathToMakeAbsolute ) );
			}
		}

		/// <summary>
		/// Makes a path relative to another.
		/// (i.e. what to type in a "cd" command to get from
		/// the PATH1 folder to PATH2). works like e.g. developer studio,
		/// when you add a file to a project: there, only the relative
		/// path of the file to the project is stored, too.
		/// e.g.:
		/// path1  = "c:\folder1\folder2\folder4\"
		/// path2  = "c:\folder1\folder2\folder3\file1.txt"
		/// result = "..\folder3\file1.txt"
		/// </summary>
		/// <param name="pathToWhichToMakeRelativeTo">The path to which to make relative to.</param>
		/// <param name="pathToMakeRelative">The path to make relative.</param>
		/// <returns>
		/// Returns the relative path, IF POSSIBLE.
		/// If not possible (i.e. no same parts in PATH2 and the PATH1),
		/// returns the complete PATH2.
		/// </returns>
		public static string GetRelativePath(
			string pathToWhichToMakeRelativeTo,
			string pathToMakeRelative )
		{
			if ( string.IsNullOrEmpty( pathToWhichToMakeRelativeTo ) ||
				string.IsNullOrEmpty( pathToMakeRelative ) )
			{
				return pathToMakeRelative;
			}
			else
			{
				string o = pathToWhichToMakeRelativeTo.ToLower().Replace( '/', '\\' ).TrimEnd( '\\' );
				string t = pathToMakeRelative.ToLower().Replace( '/', '\\' );

				// --
				// Handle special cases for Driveletters and UNC shares.

				string td = GetDriveOrShare( t );
				string od = GetDriveOrShare( o );

				td = td.Trim();
				td = td.Trim( '\\', '/' );

				od = od.Trim();
				od = od.Trim( '\\', '/' );

				// Different drive or share, i.e. nothing common, skip.
				if ( td != od )
				{
					return pathToMakeRelative;
				}
				else
				{
					int ol = o.Length;
					int tl = t.Length;

					// compare each one, until different.
					int pos = 0;
					while ( pos < ol && pos < tl && o[pos] == t[pos] )
					{
						pos++;
					}
					if ( pos < ol )
					{
						pos--;
					}

					// after comparison, make normal (i.e. NOT lowercase) again.
					t = pathToMakeRelative;

					// --

					// noting in common.
					if ( pos <= 0 )
					{
						return t;
					}
					else
					{
						// If not matching at a slash-boundary, navigate back until slash.
						if ( !(pos == ol || o[pos] == '\\' || o[pos] == '/') )
						{
							while ( pos > 0 && (o[pos] != '\\' && o[pos] != '/') )
							{
								pos--;
							}
						}

						// noting in common.
						if ( pos <= 0 )
						{
							return t;
						}
						else
						{
							// --
							// grab and split the reminders.

							string oRemaining = o.Substring( pos );
							oRemaining = oRemaining.Trim( '\\', '/' );

							// Count how many folders are following in 'path1'.
							// Count by splitting.
							string[] oRemainingParts = oRemaining.Split( '\\' );

							string tRemaining = t.Substring( pos );
							tRemaining = tRemaining.Trim( '\\', '/' );

							// --

							StringBuilder result = new StringBuilder();

							// Path from path1 to common root.
							foreach ( string oRemainingPart in oRemainingParts )
							{
								if ( !string.IsNullOrEmpty( oRemainingPart ) )
								{
									result.Append( @"..\" );
								}
							}

							// And up to 'path2'.
							result.Append( tRemaining );

							// --

							return result.ToString();
						}
					}
				}
			}
		}

		/// <summary>
		/// A "less intelligent" Combine (in contrast to to Path.Combine).
		/// </summary>
		/// <param name="path1">The path1.</param>
		/// <param name="path2">The path2.</param>
		/// <returns></returns>
		public static string Combine(
			string path1,
			string path2 )
		{
			if ( string.IsNullOrEmpty( path1 ) )
			{
				return path2;
			}
			else if ( string.IsNullOrEmpty( path2 ) )
			{
				return path1;
			}
			else
			{
				path1 = path1.TrimEnd( '\\', '/' ).Replace( '/', '\\' );
				path2 = path2.TrimStart( '\\', '/' ).Replace( '/', '\\' );

				return path1 + @"\" + path2;
			}
		}

		/// <summary>
		/// A "less intelligent" Combine (in contrast to to Path.Combine).
		/// </summary>
		/// <param name="path1">The path1.</param>
		/// <param name="path2">The path2.</param>
		/// <param name="path3">The path3.</param>
		/// <param name="paths">The paths.</param>
		/// <returns></returns>
		public static string Combine(
			string path1,
			string path2,
			string path3,
			params string[] paths )
		{
			string resultPath = Combine( path1, path2 );
			resultPath = Combine( resultPath, path3 );

			if ( paths != null )
			{
				foreach ( string path in paths )
				{
					resultPath = Combine( resultPath, path );
				}
			}

			return resultPath;
		}

		/// <summary>
		/// A "less intelligent" Combine (in contrast to to Path.Combine).
		/// For paths with forward slash.
		/// </summary>
		/// <param name="path1">The path1.</param>
		/// <param name="path2">The path2.</param>
		/// <returns></returns>
		public static string CombineVirtual(
			string path1,
			string path2 )
		{
			if ( string.IsNullOrEmpty( path1 ) )
			{
				return path2;
			}
			else if ( string.IsNullOrEmpty( path2 ) )
			{
				return path1;
			}
			else
			{
				// Avoid removing too much "/", so that "file://" still
				// stays "file://" and does not become "file:/".
				// (The same applies for other protocols.

				path1 = path1.Replace( '\\', '/' );
				if ( path1[path1.Length - 1] != '/' )
				{
					path1 += @"/";
				}

				path2 = path2.Replace( '\\', '/' );

				// Do allow "file://" + "/C:/..." to really form "file:///C:/...",
				// with three slashes.
				if ( path2.Length >= 3 )
				{
					if ( path2[0] == '/' && path2[2] == ':' && char.IsLetter( path2[1] ) )
					{
						// Is OK to have a leading slash.
					}
					else
					{
						path2 = path2.TrimStart( '/', '\\' );
					}
				}
				else
				{
					path2 = path2.TrimStart( '/', '\\' );
				}

				return path1 + path2;
			}
		}

		/// <summary>
		/// A "less intelligent" Combine (in contrast to to Path.Combine).
		/// For paths with forward slash.
		/// </summary>
		/// <param name="path1">The path1.</param>
		/// <param name="path2">The path2.</param>
		/// <param name="path3">The path3.</param>
		/// <param name="paths">The paths.</param>
		/// <returns></returns>
		public static string CombineVirtual(
			string path1,
			string path2,
			string path3,
			params string[] paths )
		{
			string resultPath = CombineVirtual( path1, path2 );
			resultPath = CombineVirtual( resultPath, path3 );

			if ( paths != null )
			{
				foreach ( string path in paths )
				{
					resultPath = CombineVirtual( resultPath, path );
				}
			}

			return resultPath;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Splitting a path into different parts.
		// ------------------------------------------------------------------

		/// <summary>
		/// Checks for the drive part in a given string.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		/// <remarks>
		/// Example:  "C:\Team\Text\Test.Txt" would return "C:".
		/// </remarks>
		public static string GetDrive(
			string path )
		{
			if ( string.IsNullOrEmpty( path ) )
			{
				return path;
			}
			else
			{
				path = ConvertForwardSlashsToBackSlashs( path );

				int colonPos = path.IndexOf( ':' );
				int slashPos = path.IndexOf( '\\' );

				if ( colonPos <= 0 )
				{
					return string.Empty;
				}
				else
				{
					if ( slashPos < 0 || slashPos > colonPos )
					{
						return path.Substring( 0, colonPos + 1 );
					}
					else
					{
						return string.Empty;
					}
				}
			}
		}

		/// <summary>
		/// Retrieves the share in a given string.
		/// </summary>
		/// <param name="path">The path to retrieve the share from.</param>
		/// <returns>
		/// Returns the share or an empty string if not found.
		/// </returns>
		/// <remarks>
		/// Example: "\\Server\C\Team\Text\Test.Txt" would return "\\Server\C".
		/// -
		/// Please note: Searches until the last backslash (including).
		/// If none is present, the share will not be detected. The schema of
		/// a share looks like: "\\Server\Share\Dir1\Dir2\Dir3". The backslash
		/// after "Share" MUST be present to be detected successfully as a share.
		/// </remarks>
		public static string GetShare(
			string path )
		{
			if ( string.IsNullOrEmpty( path ) )
			{
				return path;
			}
			else
			{
				string str = path;
				string ret;

				// Nach Doppel-Slash suchen.
				// Kann z.B. "\\server\share\" sein,
				// aber auch "http:\\www.xyz.com\".
				string dblslsh = @"\\";
				int n = str.IndexOf( dblslsh );
				if ( n < 0 )
				{
					return string.Empty;
				}
				else
				{
					// Übernehme links von Doppel-Slash alles in Rückgabe
					// (inkl. Doppel-Slash selbst).
					ret = str.Substring( 0, n + dblslsh.Length );
					str = str.Remove( 0, n + dblslsh.Length );

					// Jetzt nach Slash nach Server-Name suchen.
					// Dieser Slash darf nicht unmittelbar nach den 2 Anfangsslash stehen.
					n = str.IndexOf( '\\' );
					if ( n <= 0 )
					{
						return string.Empty;
					}
					else
					{
						// Wiederum übernehmen in Rückgabestring.
						ret += str.Substring( 0, n + 1 );
						str = str.Remove( 0, n + 1 );

						// Jetzt nach Slash nach Share-Name suchen.
						// Dieser Slash darf ebenfalls nicht unmittelbar 
						// nach dem jetzigen Slash stehen.
						n = str.IndexOf( '\\' );
						if ( n < 0 )
						{
							n = str.Length;
						}
						else if ( n == 0 )
						{
							return string.Empty;
						}

						// Wiederum übernehmen in Rückgabestring, 
						// aber ohne letzten Slash.
						ret += str.Substring( 0, n );
						str += str.Remove( 0, n );

						// The last item must not be a slash.
						if ( ret[ret.Length - 1] == '\\' )
						{
							return string.Empty;
						}
						else
						{
							return ret;
						}
					}
				}
			}
		}

		/// <summary>
		/// Searches for drive or share.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public static string GetDriveOrShare(
			string path )
		{
			if ( string.IsNullOrEmpty( path ) )
			{
				return path;
			}
			else
			{
				if ( !string.IsNullOrEmpty( GetDrive( path ) ) )
				{
					return GetDrive( path );
				}
				else if ( !string.IsNullOrEmpty( GetShare( path ) ) )
				{
					return GetShare( path );
				}
				else
				{
					return string.Empty;
				}
			}
		}

		/// <summary>
		/// Retrieves the path part in a given string (without the drive or share).
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		/// <remarks>
		/// Example: "C:\Team\Text\Test.Txt" would return "\Test\Text\".
		/// -
		/// Please note: Searches until the last backslash (including).
		/// If not present, the path is not treated as a directory.
		/// (E.g.. "C:\Test\MyDir" would return "\Test" only as the directory).
		/// </remarks>
		public static string GetDirectory(
			string path )
		{
			if ( string.IsNullOrEmpty( path ) )
			{
				return path;
			}
			else
			{
				string driveOrShare = GetDriveOrShare( path );

				string dir = Path.GetDirectoryName( path );

				Debug.Assert(
					string.IsNullOrEmpty( driveOrShare ) ||
					dir.StartsWith( driveOrShare ),

					string.Format(
					@"Variable 'dir' ('{0}') must start with drive or share '{1}'.",
					dir,
					driveOrShare ) );

				if ( !string.IsNullOrEmpty( driveOrShare ) &&
					dir.StartsWith( driveOrShare ) )
				{
					return dir.Substring( driveOrShare.Length );
				}
				else
				{
					return dir;
				}
			}
		}

		/// <summary>
		/// Retrieves the file name without the extension in a given string.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		/// <remarks>
		/// Examples:
		/// "C:\Team\Text\Test.Txt" would return "Test".
		/// "C:\Team\Text\Test" would also return "Test".
		/// </remarks>
		public static string GetNameWithoutExtension(
			string path )
		{
			if ( string.IsNullOrEmpty( path ) )
			{
				return path;
			}
			else
			{
				return Path.GetFileNameWithoutExtension( path );
			}
		}

		/// <summary>
		/// Retrieves the file name with the extension in a given string.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		/// <remarks>
		/// Examples:
		/// "C:\Team\Text\Test.Txt" would return "Test.Txt".
		/// "C:\Team\Text\Test" would return "Test".
		/// </remarks>
		public static string GetNameWithExtension(
			FileInfo path )
		{
			return GetNameWithExtension( path.FullName );
		}

		/// <summary>
		/// Retrieves the file name with the extension in a given string.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		/// <remarks>
		/// Examples:
		/// "C:\Team\Text\Test.Txt" would return "Test.Txt".
		/// "C:\Team\Text\Test" would return "Test".
		/// </remarks>
		public static string GetNameWithExtension(
			string path )
		{
			if ( string.IsNullOrEmpty( path ) )
			{
				return path;
			}
			else
			{
				return Path.GetFileName( path );
			}
		}

		/// <summary>
		/// Retrieves the file extension in a given string. Including the dot.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		/// <remarks>
		/// Examples:
		/// "C:\Team\Text\Test.Txt" would return ".Txt".
		/// "C:\Team\Text\Test." would return ".".
		/// "C:\Team\Text\Test" would return "".
		/// </remarks>
		public static string GetExtension(
			string path )
		{
			if ( string.IsNullOrEmpty( path ) )
			{
				return path;
			}
			else
			{
				return Path.GetExtension( path );
			}
		}

		/// <summary>
		/// Splits a path into its parts.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public static SplittedPath SplitPath(
			string path )
		{
			return new FileOrDirectoryInfo( path ).SplittedPath;
		}

		/// <summary>
		/// Splits a path into its parts.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public static SplittedPath SplitPath(
			FileInfo path )
		{
			return new FileOrDirectoryInfo( path ).SplittedPath;
		}

		/// <summary>
		/// Splits a path into its parts.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public static SplittedPath SplitPath(
			DirectoryInfo path )
		{
			return new FileOrDirectoryInfo( path ).SplittedPath;
		}

		/// <summary>
		/// Splits a path into its parts.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public static SplittedPath SplitPath(
			FileOrDirectoryInfo path )
		{
			return path.SplittedPath;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private helper for HTML- and URL-encoding/-decoding.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets the chars.
		/// </summary>
		/// <param name="b">The b.</param>
		/// <param name="e">The e.</param>
		/// <returns></returns>
		private static char[] GetChars(
			MemoryStream b,
			Encoding e )
		{
			return e.GetChars( b.GetBuffer(), 0, (int)b.Length );
		}

		/// <summary>
		/// 
		/// </summary>
		private static Dictionary<string, char> entities;

		/// <summary>
		/// 
		/// </summary>
		private static object typeLock = new object();

		/// <summary>
		/// Gets the entities.
		/// </summary>
		/// <value>The entities.</value>
		private static Dictionary<string, char> Entities
		{
			get
			{
				if ( entities == null )
				{
					lock ( typeLock )
					{
						InitEntities();
					}
				}

				return entities;
			}
		}

		/// <summary>
		/// Inits the entities.
		/// </summary>
		private static void InitEntities()
		{
			// Build the hash table of HTML entity references.  
			// This list comes from the HTML 4.01 W3C recommendation.
			entities = new Dictionary<string, char>();
			entities.Add( @"nbsp", '\u00A0' );
			entities.Add( @"iexcl", '\u00A1' );
			entities.Add( @"cent", '\u00A2' );
			entities.Add( @"pound", '\u00A3' );
			entities.Add( @"curren", '\u00A4' );
			entities.Add( @"yen", '\u00A5' );
			entities.Add( @"brvbar", '\u00A6' );
			entities.Add( @"sect", '\u00A7' );
			entities.Add( @"uml", '\u00A8' );
			entities.Add( @"copy", '\u00A9' );
			entities.Add( @"ordf", '\u00AA' );
			entities.Add( @"laquo", '\u00AB' );
			entities.Add( @"not", '\u00AC' );
			entities.Add( @"shy", '\u00AD' );
			entities.Add( @"reg", '\u00AE' );
			entities.Add( @"macr", '\u00AF' );
			entities.Add( @"deg", '\u00B0' );
			entities.Add( @"plusmn", '\u00B1' );
			entities.Add( @"sup2", '\u00B2' );
			entities.Add( @"sup3", '\u00B3' );
			entities.Add( @"acute", '\u00B4' );
			entities.Add( @"micro", '\u00B5' );
			entities.Add( @"para", '\u00B6' );
			entities.Add( @"middot", '\u00B7' );
			entities.Add( @"cedil", '\u00B8' );
			entities.Add( @"sup1", '\u00B9' );
			entities.Add( @"ordm", '\u00BA' );
			entities.Add( @"raquo", '\u00BB' );
			entities.Add( @"frac14", '\u00BC' );
			entities.Add( @"frac12", '\u00BD' );
			entities.Add( @"frac34", '\u00BE' );
			entities.Add( @"iquest", '\u00BF' );
			entities.Add( @"Agrave", '\u00C0' );
			entities.Add( @"Aacute", '\u00C1' );
			entities.Add( @"Acirc", '\u00C2' );
			entities.Add( @"Atilde", '\u00C3' );
			entities.Add( @"Auml", '\u00C4' );
			entities.Add( @"Aring", '\u00C5' );
			entities.Add( @"AElig", '\u00C6' );
			entities.Add( @"Ccedil", '\u00C7' );
			entities.Add( @"Egrave", '\u00C8' );
			entities.Add( @"Eacute", '\u00C9' );
			entities.Add( @"Ecirc", '\u00CA' );
			entities.Add( @"Euml", '\u00CB' );
			entities.Add( @"Igrave", '\u00CC' );
			entities.Add( @"Iacute", '\u00CD' );
			entities.Add( @"Icirc", '\u00CE' );
			entities.Add( @"Iuml", '\u00CF' );
			entities.Add( @"ETH", '\u00D0' );
			entities.Add( @"Ntilde", '\u00D1' );
			entities.Add( @"Ograve", '\u00D2' );
			entities.Add( @"Oacute", '\u00D3' );
			entities.Add( @"Ocirc", '\u00D4' );
			entities.Add( @"Otilde", '\u00D5' );
			entities.Add( @"Ouml", '\u00D6' );
			entities.Add( @"times", '\u00D7' );
			entities.Add( @"Oslash", '\u00D8' );
			entities.Add( @"Ugrave", '\u00D9' );
			entities.Add( @"Uacute", '\u00DA' );
			entities.Add( @"Ucirc", '\u00DB' );
			entities.Add( @"Uuml", '\u00DC' );
			entities.Add( @"Yacute", '\u00DD' );
			entities.Add( @"THORN", '\u00DE' );
			entities.Add( @"szlig", '\u00DF' );
			entities.Add( @"agrave", '\u00E0' );
			entities.Add( @"aacute", '\u00E1' );
			entities.Add( @"acirc", '\u00E2' );
			entities.Add( @"atilde", '\u00E3' );
			entities.Add( @"auml", '\u00E4' );
			entities.Add( @"aring", '\u00E5' );
			entities.Add( @"aelig", '\u00E6' );
			entities.Add( @"ccedil", '\u00E7' );
			entities.Add( @"egrave", '\u00E8' );
			entities.Add( @"eacute", '\u00E9' );
			entities.Add( @"ecirc", '\u00EA' );
			entities.Add( @"euml", '\u00EB' );
			entities.Add( @"igrave", '\u00EC' );
			entities.Add( @"iacute", '\u00ED' );
			entities.Add( @"icirc", '\u00EE' );
			entities.Add( @"iuml", '\u00EF' );
			entities.Add( @"eth", '\u00F0' );
			entities.Add( @"ntilde", '\u00F1' );
			entities.Add( @"ograve", '\u00F2' );
			entities.Add( @"oacute", '\u00F3' );
			entities.Add( @"ocirc", '\u00F4' );
			entities.Add( @"otilde", '\u00F5' );
			entities.Add( @"ouml", '\u00F6' );
			entities.Add( @"divide", '\u00F7' );
			entities.Add( @"oslash", '\u00F8' );
			entities.Add( @"ugrave", '\u00F9' );
			entities.Add( @"uacute", '\u00FA' );
			entities.Add( @"ucirc", '\u00FB' );
			entities.Add( @"uuml", '\u00FC' );
			entities.Add( @"yacute", '\u00FD' );
			entities.Add( @"thorn", '\u00FE' );
			entities.Add( @"yuml", '\u00FF' );
			entities.Add( @"fnof", '\u0192' );
			entities.Add( @"Alpha", '\u0391' );
			entities.Add( @"Beta", '\u0392' );
			entities.Add( @"Gamma", '\u0393' );
			entities.Add( @"Delta", '\u0394' );
			entities.Add( @"Epsilon", '\u0395' );
			entities.Add( @"Zeta", '\u0396' );
			entities.Add( @"Eta", '\u0397' );
			entities.Add( @"Theta", '\u0398' );
			entities.Add( @"Iota", '\u0399' );
			entities.Add( @"Kappa", '\u039A' );
			entities.Add( @"Lambda", '\u039B' );
			entities.Add( @"Mu", '\u039C' );
			entities.Add( @"Nu", '\u039D' );
			entities.Add( @"Xi", '\u039E' );
			entities.Add( @"Omicron", '\u039F' );
			entities.Add( @"Pi", '\u03A0' );
			entities.Add( @"Rho", '\u03A1' );
			entities.Add( @"Sigma", '\u03A3' );
			entities.Add( @"Tau", '\u03A4' );
			entities.Add( @"Upsilon", '\u03A5' );
			entities.Add( @"Phi", '\u03A6' );
			entities.Add( @"Chi", '\u03A7' );
			entities.Add( @"Psi", '\u03A8' );
			entities.Add( @"Omega", '\u03A9' );
			entities.Add( @"alpha", '\u03B1' );
			entities.Add( @"beta", '\u03B2' );
			entities.Add( @"gamma", '\u03B3' );
			entities.Add( @"delta", '\u03B4' );
			entities.Add( @"epsilon", '\u03B5' );
			entities.Add( @"zeta", '\u03B6' );
			entities.Add( @"eta", '\u03B7' );
			entities.Add( @"theta", '\u03B8' );
			entities.Add( @"iota", '\u03B9' );
			entities.Add( @"kappa", '\u03BA' );
			entities.Add( @"lambda", '\u03BB' );
			entities.Add( @"mu", '\u03BC' );
			entities.Add( @"nu", '\u03BD' );
			entities.Add( @"xi", '\u03BE' );
			entities.Add( @"omicron", '\u03BF' );
			entities.Add( @"pi", '\u03C0' );
			entities.Add( @"rho", '\u03C1' );
			entities.Add( @"sigmaf", '\u03C2' );
			entities.Add( @"sigma", '\u03C3' );
			entities.Add( @"tau", '\u03C4' );
			entities.Add( @"upsilon", '\u03C5' );
			entities.Add( @"phi", '\u03C6' );
			entities.Add( @"chi", '\u03C7' );
			entities.Add( @"psi", '\u03C8' );
			entities.Add( @"omega", '\u03C9' );
			entities.Add( @"thetasym", '\u03D1' );
			entities.Add( @"upsih", '\u03D2' );
			entities.Add( @"piv", '\u03D6' );
			entities.Add( @"bull", '\u2022' );
			entities.Add( @"hellip", '\u2026' );
			entities.Add( @"prime", '\u2032' );
			entities.Add( @"Prime", '\u2033' );
			entities.Add( @"oline", '\u203E' );
			entities.Add( @"frasl", '\u2044' );
			entities.Add( @"weierp", '\u2118' );
			entities.Add( @"image", '\u2111' );
			entities.Add( @"real", '\u211C' );
			entities.Add( @"trade", '\u2122' );
			entities.Add( @"alefsym", '\u2135' );
			entities.Add( @"larr", '\u2190' );
			entities.Add( @"uarr", '\u2191' );
			entities.Add( @"rarr", '\u2192' );
			entities.Add( @"darr", '\u2193' );
			entities.Add( @"harr", '\u2194' );
			entities.Add( @"crarr", '\u21B5' );
			entities.Add( @"lArr", '\u21D0' );
			entities.Add( @"uArr", '\u21D1' );
			entities.Add( @"rArr", '\u21D2' );
			entities.Add( @"dArr", '\u21D3' );
			entities.Add( @"hArr", '\u21D4' );
			entities.Add( @"forall", '\u2200' );
			entities.Add( @"part", '\u2202' );
			entities.Add( @"exist", '\u2203' );
			entities.Add( @"empty", '\u2205' );
			entities.Add( @"nabla", '\u2207' );
			entities.Add( @"isin", '\u2208' );
			entities.Add( @"notin", '\u2209' );
			entities.Add( @"ni", '\u220B' );
			entities.Add( @"prod", '\u220F' );
			entities.Add( @"sum", '\u2211' );
			entities.Add( @"minus", '\u2212' );
			entities.Add( @"lowast", '\u2217' );
			entities.Add( @"radic", '\u221A' );
			entities.Add( @"prop", '\u221D' );
			entities.Add( @"infin", '\u221E' );
			entities.Add( @"ang", '\u2220' );
			entities.Add( @"and", '\u2227' );
			entities.Add( @"or", '\u2228' );
			entities.Add( @"cap", '\u2229' );
			entities.Add( @"cup", '\u222A' );
			entities.Add( @"int", '\u222B' );
			entities.Add( @"there4", '\u2234' );
			entities.Add( @"sim", '\u223C' );
			entities.Add( @"cong", '\u2245' );
			entities.Add( @"asymp", '\u2248' );
			entities.Add( @"ne", '\u2260' );
			entities.Add( @"equiv", '\u2261' );
			entities.Add( @"le", '\u2264' );
			entities.Add( @"ge", '\u2265' );
			entities.Add( @"sub", '\u2282' );
			entities.Add( @"sup", '\u2283' );
			entities.Add( @"nsub", '\u2284' );
			entities.Add( @"sube", '\u2286' );
			entities.Add( @"supe", '\u2287' );
			entities.Add( @"oplus", '\u2295' );
			entities.Add( @"otimes", '\u2297' );
			entities.Add( @"perp", '\u22A5' );
			entities.Add( @"sdot", '\u22C5' );
			entities.Add( @"lceil", '\u2308' );
			entities.Add( @"rceil", '\u2309' );
			entities.Add( @"lfloor", '\u230A' );
			entities.Add( @"rfloor", '\u230B' );
			entities.Add( @"lang", '\u2329' );
			entities.Add( @"rang", '\u232A' );
			entities.Add( @"loz", '\u25CA' );
			entities.Add( @"spades", '\u2660' );
			entities.Add( @"clubs", '\u2663' );
			entities.Add( @"hearts", '\u2665' );
			entities.Add( @"diams", '\u2666' );
			entities.Add( @"quot", '\u0022' );
			entities.Add( @"amp", '\u0026' );
			entities.Add( @"lt", '\u003C' );
			entities.Add( @"gt", '\u003E' );
			entities.Add( @"OElig", '\u0152' );
			entities.Add( @"oelig", '\u0153' );
			entities.Add( @"Scaron", '\u0160' );
			entities.Add( @"scaron", '\u0161' );
			entities.Add( @"Yuml", '\u0178' );
			entities.Add( @"circ", '\u02C6' );
			entities.Add( @"tilde", '\u02DC' );
			entities.Add( @"ensp", '\u2002' );
			entities.Add( @"emsp", '\u2003' );
			entities.Add( @"thinsp", '\u2009' );
			entities.Add( @"zwnj", '\u200C' );
			entities.Add( @"zwj", '\u200D' );
			entities.Add( @"lrm", '\u200E' );
			entities.Add( @"rlm", '\u200F' );
			entities.Add( @"ndash", '\u2013' );
			entities.Add( @"mdash", '\u2014' );
			entities.Add( @"lsquo", '\u2018' );
			entities.Add( @"rsquo", '\u2019' );
			entities.Add( @"sbquo", '\u201A' );
			entities.Add( @"ldquo", '\u201C' );
			entities.Add( @"rdquo", '\u201D' );
			entities.Add( @"bdquo", '\u201E' );
			entities.Add( @"dagger", '\u2020' );
			entities.Add( @"Dagger", '\u2021' );
			entities.Add( @"permil", '\u2030' );
			entities.Add( @"lsaquo", '\u2039' );
			entities.Add( @"rsaquo", '\u203A' );
			entities.Add( @"euro", '\u20AC' );
		}

		/// <summary>
		/// 
		/// </summary>
		private static char[] hexChars = @"0123456789abcdef".ToCharArray();

		/// <summary>
		/// URLs the encode to bytes.
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="count">The count.</param>
		/// <param name="urlEncoding">The URL encoding.</param>
		/// <returns></returns>
		private static byte[] UrlEncodeToBytes(
			byte[] bytes,
			int offset,
			int count,
			UrlEncoding urlEncoding )
		{
			if ( bytes == null )
			{
				return null;
			}
			else
			{
				int len = bytes.Length;
				if ( len == 0 )
				{
					return new byte[0];
				}
				if ( offset < 0 || offset >= len )
				{
					throw new ArgumentOutOfRangeException( @"offset" );
				}
				if ( count < 0 || count > len - offset )
				{
					throw new ArgumentOutOfRangeException( @"count" );
				}

				// --

				string additionalSafeChars;
				if ( urlEncoding == UrlEncoding.XAlphas )
				{
					additionalSafeChars = @"+";
				}
				else if ( urlEncoding == UrlEncoding.XPAlphas )
				{
					additionalSafeChars = @"+/";
				}
				else if ( urlEncoding == UrlEncoding.DosFile )
				{
					additionalSafeChars = @"+/:(){}[]$";
				}
				else
				{
					additionalSafeChars = string.Empty;
				}

				// --

				using ( MemoryStream result = new MemoryStream() )
				{
					int end = offset + count;
					for ( int i = offset; i < end; i++ )
					{
						char c = (char)bytes[i];

						bool isUnsafe =
							(c == ' ') ||
							(c < '0' && c != '-' && c != '.' && c != '!') ||
							(c < 'A' && c > '9') ||
							(c > 'Z' && c < 'a' && c != '_') ||
							(c > 'z');

						if ( isUnsafe &&
							additionalSafeChars.IndexOf( c ) >= 0 )
						{
							isUnsafe = false;
						}

						if ( isUnsafe )
						{
							// An unsafe character, must escape.
							result.WriteByte( (byte)'%' );
							int idx = ((int)c) >> 4;
							result.WriteByte( (byte)hexChars[idx] );
							idx = ((int)c) & 0x0F;
							result.WriteByte( (byte)hexChars[idx] );
						}
						else
						{
							// A safe character just write.
							result.WriteByte( (byte)c );
						}
					}

					return result.ToArray();
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Miscellaneous private helper.
		// ------------------------------------------------------------------

		/// <summary>
		/// Sets the slash begin.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="setSlash">if set to <c>true</c> [set slash].</param>
		/// <param name="directorySeparatorChar">The directory separator char.</param>
		/// <returns></returns>
		private static string SetSlashBegin(
			string path,
			bool setSlash,
			char directorySeparatorChar )
		{
			if ( setSlash )
			{
				if ( string.IsNullOrEmpty( path ) )
				{
					return directorySeparatorChar.ToString();
				}
				else
				{
					if ( path[0] == directorySeparatorChar )
					{
						return path;
					}
					else
					{
						return directorySeparatorChar + path;
					}
				}
			}
			else
			{
				if ( string.IsNullOrEmpty( path ) )
				{
					return path;
				}
				else
				{
					if ( path[0] == directorySeparatorChar )
					{
						return path.Substring( 1 );
					}
					else
					{
						return path;
					}
				}
			}
		}

		/// <summary>
		/// Sets the slash end.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="setSlash">if set to <c>true</c> [set slash].</param>
		/// <param name="directorySeparatorChar">The directory separator char.</param>
		/// <returns></returns>
		private static string SetSlashEnd(
			string path,
			bool setSlash,
			char directorySeparatorChar )
		{
			if ( setSlash )
			{
				if ( string.IsNullOrEmpty( path ) )
				{
					return directorySeparatorChar.ToString();
				}
				else
				{
					if ( path[path.Length - 1] == directorySeparatorChar )
					{
						return path;
					}
					else
					{
						return path + directorySeparatorChar;
					}
				}
			}
			else
			{
				if ( string.IsNullOrEmpty( path ) )
				{
					return path;
				}
				else
				{
					if ( path[path.Length - 1] == directorySeparatorChar )
					{
						return path.Substring( 0, path.Length - 1 );
					}
					else
					{
						return path;
					}
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}