namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Text.RegularExpressions;
	using System.Collections.Generic;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Class for formatting and replacing text in columns.
	/// The placeholders have the format "{Xxx}" or "{Xxx:nn}" or "{Xxx:nn,align}", 
	/// with ":nn" being an optional integer value defining the column 
	/// width. If too small, the remaining columns are filled with blanks, 
	/// if too wide, the text is word-wrapped.
	/// ",align" is an optional alignment which can be "l" or "r". "l" aligns
	/// left (the default if omitted), "r" aligns right.
	/// </summary>
	/// <remarks>Be sure to let the formatter expand the variables from
	/// LEFT to RIGHT, otherwise you would get some gaps upon word wrapping.</remarks>
	public class TextFormatter :
		IDisposable
	{
		#region Public routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="searchIn">The text to search in.</param>
		public TextFormatter(
			string searchIn )
		{
			if ( searchIn != null && searchIn.Length > 0 )
			{
				lines = MakeLines( searchIn );
			}
		}

		/// <summary>
		/// Replacing placeholders with and without column number
		/// information.
		/// </summary>
		/// <param name="searchFor">The text to search for.</param>
		/// <param name="replaceWith">The text to insert.</param>
		public void ReplaceText(
			string searchFor,
			string replaceWith )
		{
			if ( lines != null )
			{
				bool isNormalPlaceholder;
				string realPlaceholder;
				Alignment alignment;
				int columnWidth;
				int lineNumber;
				int columnNumber;

				while ( FindPlaceholder(
					searchFor,
					out isNormalPlaceholder,
					out alignment,
					out realPlaceholder,
					out columnWidth,
					out lineNumber,
					out columnNumber ) )
				{
					if ( isNormalPlaceholder )
					{
						// Normal placeholder, direct replace.

						Line line = lines[lineNumber] as Line;

						line.Content = line.Content.Replace(
							realPlaceholder,
							replaceWith );

						lines[lineNumber] = line;
					}
					else
					{
						// Special placeholder.

						WordWrapper wr = new WordWrapper(
							replaceWith,
							columnWidth );

						string insertString = wr.Wrap();
						List<Line> inserts = MakeLines( insertString );

						// Fill up with spaces if to small.
						foreach ( Line insert in inserts )
						{
							insert.Content = insert.Content.Trim();

							while ( insert.Content.Length < columnWidth )
							{
								if ( alignment == Alignment.Left )
								{
									insert.Content += ' ';
								}
								else
								{
									insert.Content = ' ' + insert.Content;
								}
							}
						}

						// Ensure that the lines are present.
						CheckInsertDynamicLines( lineNumber, inserts.Count - 1 );

						// --

						// Delete the placeholder itself.
						Line startLine = lines[lineNumber] as Line;
						startLine.Content = startLine.Content.Remove(
							columnNumber,
							realPlaceholder.Length );

						// Fill at the deleted placeholder, to ensure
						// that the correct width. The spaces get deleted later.
						for ( int i = columnNumber; i < columnNumber + columnWidth; ++i )
						{
							startLine.Content = startLine.Content.Insert(
								columnNumber,
								@" " );
						}

						// 

						// Insert into line.
						for ( int i = lineNumber; i < lineNumber + inserts.Count; ++i )
						{
							Line line = lines[i] as Line;
							Line insert = inserts[i - lineNumber] as Line;

							// Fill up left columns.
							while ( line.Content.Length < columnNumber + columnWidth )
							{
								line.Content += ' ';
							}

							// Cut out and insert.
							line.Content = line.Content.Remove(
								columnNumber,
								columnWidth );

							line.Content = line.Content.Insert(
								columnNumber,
								insert.Content );
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns the string with the replaced texts.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get
			{
				return MakeStringFromLines();
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Splits the text into lines.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		private static List<Line> MakeLines(
			string text )
		{
			text = text.Replace( "\r\n", "\n" );
			text = text.Replace( "\r", "\n" );

			string[] lines = text.Split( '\n' );
			List<Line> result = new List<Line>();

			if ( lines != null && lines.Length > 0 )
			{
				int index = 0;
				foreach ( string line in lines )
				{
					Line l = new Line();

					l.Content = line;
					l.Type = Line.LineType.OriginalLine;

					result.Add( l );
					index++;
				}
			}

			return result;
		}

		/// <summary>
		/// Convert the current lines to a single, multiline string.
		/// </summary>
		/// <returns></returns>
		private string MakeStringFromLines()
		{
			if ( lines == null || lines.Count <= 0 )
			{
				return null;
			}
			else
			{
				string result = string.Empty;

				foreach ( Line line in lines )
				{
					if ( result.Length > 0 )
					{
						result += "\r\n";
					}

					result += line.Content;
				}

				return result;
			}
		}

		/// <summary>
		/// Searches for a placeholder and returns information
		/// about that placeholder.
		/// </summary>
		/// <param name="corePlaceholder">The core placeholder.</param>
		/// <param name="isNormalPlaceholder">if set to <c>true</c> [is normal placeholder].</param>
		/// <param name="alignment">The alignment.</param>
		/// <param name="realPlaceholder">The real placeholder.</param>
		/// <param name="columnWidth">Width of the column.</param>
		/// <param name="lineNumber">The line number.</param>
		/// <param name="columnNumber">The column number.</param>
		/// <returns></returns>
		private bool FindPlaceholder(
			string corePlaceholder,
			out bool isNormalPlaceholder,
			out Alignment alignment,
			out string realPlaceholder,
			out int columnWidth,
			out int lineNumber,
			out int columnNumber )
		{
			corePlaceholder = corePlaceholder.Trim();
			corePlaceholder = corePlaceholder.Trim( '{', '}' );
			corePlaceholder = corePlaceholder.Trim();

			int index = 0;
			foreach ( Line line in lines )
			{
				int pos = line.Content.IndexOf( @"{" + corePlaceholder + @"}" );
				if ( pos >= 0 )
				{
					isNormalPlaceholder = true;
					realPlaceholder = @"{" + corePlaceholder + @"}";
					alignment = Alignment.Left;
					columnWidth = -1;
					lineNumber = index;
					columnNumber = pos;

					return true;
				}
				else
				{
					string cp = EscapeRxCharacters( corePlaceholder );

					Match m = Regex.Match(
						line.Content,
						@"\{" + cp + @":([0-9]+)(,(?:l|r))?\}",
						RegexOptions.IgnoreCase );

					if ( m.Success )
					{
						isNormalPlaceholder = false;
						realPlaceholder = m.Value;
						columnWidth = Convert.ToInt32( m.Groups[1].Value );
						lineNumber = index;
						columnNumber = m.Index;

						if ( m.Groups.Count > 2 )
						{
							string s = m.Groups[2].Value.Trim().Trim( ',' ).ToLower();
							if ( s == @"r" )
							{
								alignment = Alignment.Right;
							}
							else
							{
								alignment = Alignment.Left;
							}
						}
						else
						{
							alignment = Alignment.Left;
						}

						return true;
					}
				}

				index++;
			}

			// --
			// Not found.

			isNormalPlaceholder = true;
			realPlaceholder = null;
			alignment = Alignment.Left;
			columnWidth = -1;
			lineNumber = -1;
			columnNumber = -1;

			return false;
		}

		/// <summary>
		/// Ensures that the specified number of blank lines
		/// are present below the given line.
		/// </summary>
		/// <param name="insertAfterLineNumber">The insert after line number.</param>
		/// <param name="linesToInsertCount">The lines to insert count.</param>
		private void CheckInsertDynamicLines(
			int insertAfterLineNumber,
			int linesToInsertCount )
		{
			// Insert to have the end match.
			while ( insertAfterLineNumber > lines.Count - 1 )
			{
				Line line = new Line();

				line.Content = string.Empty;
				line.Type = Line.LineType.NewlyInsertedLine;

				lines.Add( line );
			}

			// --

			// Insert at the end.
			if ( insertAfterLineNumber == lines.Count - 1 )
			{
				for ( int i = 0; i < linesToInsertCount; ++i )
				{
					Line line = new Line();

					line.Content = string.Empty;
					line.Type = Line.LineType.NewlyInsertedLine;

					lines.Add( line );
				}
			}
			else
			{
				// --
				// Determine the actual number of lines to insert.

				int numberOfLinesToInsert = 0;

				int lower = insertAfterLineNumber + 1;
				int upper = insertAfterLineNumber + linesToInsertCount;

				for ( int i = lower; i <= upper; ++i )
				{
					if ( i >= lines.Count )
					{
						numberOfLinesToInsert = upper - i + 1;
						break;
					}
					else
					{
						Line line = lines[i] as Line;

						if ( line.Type == Line.LineType.OriginalLine )
						{
							numberOfLinesToInsert = upper - i + 1;
							break;
						}
					}
				}

				// --
				// Actually insert.

				for ( int i = 0; i < numberOfLinesToInsert; ++i )
				{
					Line line = new Line();

					line.Content = string.Empty;
					line.Type = Line.LineType.NewlyInsertedLine;

					lines.Insert( insertAfterLineNumber + 1 + i, line );
				}
			}
		}

		/// <summary>
		/// Escape special RX characters.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		private string EscapeRxCharacters(
			string text )
		{
			// As first!
			text = text.Replace( @"\", @"\\" );

			text = text.Replace( @"+", @"\+" );
			text = text.Replace( @"?", @"\?" );
			text = text.Replace( @".", @"\." );
			text = text.Replace( @"*", @"\*" );
			text = text.Replace( @"^", @"\^" );
			text = text.Replace( @"$", @"\$" );
			text = text.Replace( @"(", @"\(" );
			text = text.Replace( @")", @"\)" );
			text = text.Replace( @"[", @"\[" );
			text = text.Replace( @"]", @"\]" );
			text = text.Replace( @"{", @"\{" );
			text = text.Replace( @"}", @"\}" );
			text = text.Replace( @"|", @"\|" );

			return text;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private members.
		// ------------------------------------------------------------------

		/// <summary>
		/// A collection of Line objects.
		/// </summary>
		private List<Line> lines;

		/// <summary>
		/// How text should be aligned.
		/// </summary>
		private enum Alignment
		{
			#region Enum members.

			/// <summary>
			/// Align left, the default.
			/// </summary>
			Left,

			/// <summary>
			/// Align text right.
			/// </summary>
			Right

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region A line class.
		// ------------------------------------------------------------------

		/// <summary>
		/// A single line of text.
		/// </summary>
		private class Line
		{
			#region Public variables.

			/// <summary>
			/// The content of the line.
			/// </summary>
			public string Content;

			/// <summary>
			/// what type is this line.
			/// </summary>
			public LineType Type;

			#endregion

			/// <summary>
			/// what type is this line.
			/// </summary>
			public enum LineType
			{
				#region Enum members.

				/// <summary>
				/// A line, presented in the original text.
				/// </summary>
				OriginalLine,

				/// <summary>
				/// A dynamically inserted line.
				/// </summary>
				NewlyInsertedLine

				#endregion
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region IDisposable member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Performs application-defined tasks associated with freeing, 
		/// releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			lines = null;
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}