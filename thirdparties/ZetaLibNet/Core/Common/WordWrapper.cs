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
	/// Class for breaking a text into multiple lines at a specific column
	/// width. Behaves like Notepad in word-wrap mode.
	/// </summary>
	public class WordWrapper :
		IDisposable
	{
		#region Public routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="columnWidth">Width of the column.</param>
		public WordWrapper(
			string text,
			int columnWidth )
		{
			Text = text;
			ColumnWidth = columnWidth;
		}

		/// <summary>
		/// Does the actual wrapping.
		/// </summary>
		/// <returns>Returns the wrapped string.</returns>
		public string Wrap()
		{
			if ( Text == null || Text.Length < ColumnWidth )
			{
				return Text;
			}
			else
			{
				CurrentColumnPos = 0;

				CurrentPos = 0;
				while ( CurrentPos < Text.Length )
				{
					// Reached right margin.
					if ( CurrentColumnPos >= ColumnWidth )
					{
						// Go back until a word is reached.
						// Break at that word.
						int wordStartPos =
							GetWordStartLeftFromPos( CurrentPos );
						int wordEndPos =
							GetWordEndRightFromPos( CurrentPos );

						if ( wordEndPos - wordStartPos < ColumnWidth ||
							!IsAtLineStartPos( wordStartPos ) )
						{
							// Does fit into a single line, break before word.
							// Also a word that is longer than a complete line,
							// break if the word is not at the first column
							// position of a line.

							Text = Text.Insert( wordStartPos, "\n" );
							CurrentColumnPos = 0;
							CurrentPos = wordStartPos + 1;

							continue;
						}
						else
						{
							// The word itself is longer than a complete line,
							// break inside word.

							Text = Text.Insert( CurrentPos, "\n" );
							CurrentColumnPos = 0;
							CurrentPos++;

							continue;
						}
					}

					// --

					++CurrentPos;
					++CurrentColumnPos;
				}

				return Text;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Searches for the word start in the current line,
		/// left to the given position.
		/// </summary>
		/// <param name="pos">The pos.</param>
		/// <returns></returns>
		private int GetWordStartLeftFromPos(
			int pos )
		{
			if ( pos == 0 )
			{
				return pos;
			}
			else
			{
				while ( pos > Text.Length - 1 && pos > 0 )
				{
					--pos;
				}

				if ( IsAtLineStartPos( pos ) ||
					IsWordStartAtPos( pos ) )
				{
					return pos;
				}
				else
				{
					while ( pos > 0 )
					{
						if ( IsAtLineStartPos( pos ) ||
							IsWordStartAtPos( pos ) )
						{
							return pos;
						}
						else
						{
							--pos;
						}
					}

					return pos;
				}
			}
		}

		/// <summary>
		/// Searches for the word end,
		/// right to the given position.
		/// </summary>
		/// <param name="pos">The pos.</param>
		/// <returns></returns>
		private int GetWordEndRightFromPos(
			int pos )
		{
			if ( pos > Text.Length - 1 )
			{
				return Text.Length - 1;
			}
			else
			{
				while ( pos < Text.Length - 1 )
				{
					if ( IsWordEndAtPos( pos ) )
					{
						return pos;
					}
					else
					{
						++pos;
					}
				}

				return pos;
			}
		}

		/// <summary>
		/// Check whether a line STARTS at the given position.
		/// </summary>
		/// <param name="pos">The pos.</param>
		/// <returns>
		/// 	<c>true</c> if [is at line start pos] [the specified pos]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsAtLineStartPos(
			int pos )
		{
			if ( pos == 0 )
			{
				return true;
			}
			else if ( pos > Text.Length - 1 )
			{
				return false;
			}
			else
			{
				if ( Text[pos - 1] == '\n' )
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Check whether a word STARTS at the given position.
		/// </summary>
		/// <param name="pos">The pos.</param>
		/// <returns>
		/// 	<c>true</c> if [is word start at pos] [the specified pos]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsWordStartAtPos(
			int pos )
		{
			if ( pos == 0 )
			{
				return true;
			}
			else if ( pos > Text.Length - 1 )
			{
				return false;
			}
			else
			{
				string text = Text.Substring( pos - 1, 2 );

				Match m = Regex.Match(
					text,
					@"\b",
					RegexOptions.IgnoreCase );

				if ( m.Success )
				{
					if ( m.Index == 1 )
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Check whether a word ENDS at the given position.
		/// </summary>
		/// <param name="pos">The pos.</param>
		/// <returns>
		/// 	<c>true</c> if [is word end at pos] [the specified pos]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsWordEndAtPos(
			int pos )
		{
			if ( pos >= Text.Length - 1 )
			{
				return true;
			}
			else
			{
				string text = Text.Substring( pos, 2 );

				Match m = Regex.Match(
					text,
					@"\b",
					RegexOptions.IgnoreCase );

				if ( m.Success )
				{
					if ( m.Index == 1 )
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// The position of the current colum in the source text.
		/// </summary>
		private int CurrentColumnPos;

		/// <summary>
		/// The absolute position within the string.
		/// </summary>
		private int CurrentPos;

		private string Text;
		private int ColumnWidth;

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
			Text = null;
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}