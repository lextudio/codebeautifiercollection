namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Globalization;
	using ZetaLib.Core.Common;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Miscellaneous functions for helping with databases.
	/// </summary>
	public sealed class DBHelper
	{
		#region Miscellaneous routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// When doing SQL-string-replacements of TRUE/1 or FALSE/0, 
		/// this is the placeholder for true.
		/// </summary>
		public const string TruePlaceholder = @"$(true)";

		/// <summary>
		/// When doing SQL-string-replacements of TRUE/1 or FALSE/0, 
		/// this is the placeholder for false.
		/// </summary>
		public const string FalsePlaceholder = @"$(false)";

		/// <summary>
		/// Private constructor.
		/// </summary>
		private DBHelper()
		{
		}

		/// <summary>
		/// Formats the date portion of a DateTime value so that it can
		/// be used directly inside a SQL query.
		/// The format is 'YYYYMMDD' (including the quotes).
		/// </summary>
		/// <param name="dateToFormat">The date to format.</param>
		/// <returns>The ready-to-use SQL string.</returns>
		public static string FormatSqlDateOnly(
			DateTime dateToFormat )
		{
			return string.Format(
				CultureInfo.CurrentCulture,
				@"'{0}{1:00}{2:00}'",
				dateToFormat.Year,
				dateToFormat.Month,
				dateToFormat.Day );
		}

		/// <summary>
		/// Formats a date time value so that it fits the JET specification
		/// for date formats to be usable from within a SQL query.
		/// The format is #MM/DD/YYYY HH:MM:SS# (including the hashs).
		/// </summary>
		/// <param name="dateToFormat">The date and time to format</param>
		/// <returns>Returns the ready-to-use SQL string.</returns>
		/// <see cref="http://msdn.microsoft.com/archive/en-us/dnaraccgen/html/msdn_datetime.asp"/>
		public static string FormatJetDateAndTime(
			DateTime dateToFormat )
		{
			return string.Format(
				CultureInfo.CurrentCulture,
				@"#{0:00}/{1:00}/{2} {3:00}:{4:00}:{5:00}#",
				dateToFormat.Month,
				dateToFormat.Day,
				dateToFormat.Year,
				dateToFormat.Hour,
				dateToFormat.Minute,
				dateToFormat.Second );
		}

		/// <summary>
		/// Formats the date portion of a date time value so that it fits into
		/// the JET specification for date formats to be usable from within a SQL query.
		/// The format is #MM/DD/YYYY# (including the hashs).
		/// </summary>
		/// <param name="dateToFormat">The date and time to format</param>
		/// <returns>Returns the ready-to-use SQL string.</returns>
		/// <see cref="http://msdn.microsoft.com/archive/en-us/dnaraccgen/html/msdn_datetime.asp"/>
		public static string FormatJetDateOnly(
			DateTime dateToFormat )
		{
			return string.Format(
				CultureInfo.CurrentCulture,
				@"#{0:00}/{1:00}/{2}#",
				dateToFormat.Month,
				dateToFormat.Day,
				dateToFormat.Year );
		}

		/// <summary>
		/// Formats the time portion of a date time value so that it fits into
		/// the JET specification for date formats to be usable from within a SQL query.
		/// The format is #HH:MM:SS# (including the hashs).
		/// </summary>
		/// <param name="dateToFormat">The date and time to format</param>
		/// <returns>Returns the ready-to-use SQL string.</returns>
		/// <see cref="http://msdn.microsoft.com/archive/en-us/dnaraccgen/html/msdn_datetime.asp"/>
		public static string FormatJetTimeOnly(
			DateTime dateToFormat )
		{
			return string.Format(
				CultureInfo.CurrentCulture,
				@"#{0:00}:{1:00}:{2:00}#",
				dateToFormat.Hour,
				dateToFormat.Minute,
				dateToFormat.Second );
		}

		/// <summary>
		/// Escapes special SQL characters, so that they can be safely passed to a
		/// SQL query.
		/// </summary>
		/// <param name="sql">The unescaped text to escape.</param>
		/// <returns>Returns the escaped text.</returns>
		public static string EscapeSql(
			string sql )
		{
			if ( sql == null )
			{
				return sql;
			}
			else
			{
				sql = sql.Replace( @"'", @"''" );

				return sql;
			}
		}

		/// <summary>
		/// Tries to remove all malicious SQL code constructs from a search term
		/// that usually the user enteres through a web form fieldValue.
		/// Use this function before passing any search keywords to a SQL query.
		/// </summary>
		/// <param name="keyword">The keyword(s) that the user entered.</param>
		/// <returns>
		/// Returns the ready-to-use, safe, keyword(s).
		/// </returns>
		/// <remarks>See: http://www.codeproject.com/aspnet/search.asp </remarks>
		public static string DemalifySearchTerm(
			string keyword )
		{
			if ( keyword == null )
			{
				return null;
			}
			else
			{
				// Remove all semi-colons, which should stop
				// attempt to run malicious SQL code.
				keyword = keyword.Replace( @";", string.Empty );

				return keyword;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Reading from DB fields.
		// ------------------------------------------------------------------

		/// <summary>
		/// Read a value from the database.
		/// Use this overload for SQL-'timestamp' fields.
		/// </summary>
		/// <param name="readValue">The value read and to return.
		/// Returns a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.</param>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		public static void ReadField(
			out byte[] readValue,
			object fieldValue )
		{
			readValue = ReadFieldByteArray( fieldValue );
		}

		/// <summary>
		/// Read a value from the database.
		/// </summary>
		/// <param name="readValue">The value read and to return.
		/// Returns a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.</param>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		public static void ReadField(
			out Guid readValue,
			object fieldValue )
		{
			readValue = ReadFieldGuid( fieldValue );
		}

		/// <summary>
		/// Read a value from the database.
		/// </summary>
		/// <param name="readValue">The value read and to return.
		/// Returns a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.</param>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		public static void ReadField(
			out int readValue,
			object fieldValue )
		{
			readValue = ReadFieldInteger( fieldValue );
		}

		/// <summary>
		/// Read a value from the database.
		/// </summary>
		/// <param name="readValue">The value read and to return. 
		/// Returns a default value if not readable, 
		/// e.g. if NULL or <code>DBNull.Value</code>.</param>
		/// <param name="fieldValue">The object to read from. 
		/// Usually something like <code>row["MyField"]</code>.</param>
		public static void ReadField(
			out long readValue,
			object fieldValue )
		{
			readValue = ReadFieldLong( fieldValue );
		}

		/// <summary>
		/// Read a value from the database.
		/// </summary>
		/// <param name="readValue">The value read and to return.
		/// Returns a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.</param>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		public static void ReadField(
			out double readValue,
			object fieldValue )
		{
			readValue = ReadFieldDouble( fieldValue );
		}

		/// <summary>
		/// Read a value from the database.
		/// </summary>
		/// <param name="readValue">The value read and to return. 
		/// Returns a default value if not readable, 
		/// e.g. if NULL or <code>DBNull.Value</code>.</param>
		/// <param name="fieldValue">The object to read from. 
		/// Usually something like <code>row["MyField"]</code>.</param>
		public static void ReadField(
			out string readValue,
			object fieldValue )
		{
			readValue = ReadFieldString( fieldValue );
		}

		/// <summary>
		/// Read a value from the database.
		/// </summary>
		/// <param name="readValue">The value read and to return. 
		/// Returns a default value if not readable, 
		/// e.g. if NULL or <code>DBNull.Value</code>.</param>
		/// <param name="fieldValue">The object to read from. 
		/// Usually something like <code>row["MyField"]</code>.</param>
		public static void ReadField(
			out decimal readValue,
			object fieldValue )
		{
			readValue = ReadFieldDecimal( fieldValue );
		}

		/// <summary>
		/// Read a value from the database.
		/// </summary>
		/// <param name="readValue">The value read and to return.
		/// Returns a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.</param>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		public static void ReadField(
			out object readValue,
			object fieldValue )
		{
			readValue = ReadFieldObject( fieldValue );
		}

		/// <summary>
		/// Read a value from the database.
		/// </summary>
		/// <param name="readValue">The value read and to return. 
		/// Returns a default value if not readable, 
		/// e.g. if NULL or <code>DBNull.Value</code>.</param>
		/// <param name="fieldValue">The object to read from. 
		/// Usually something like <code>row["MyField"]</code>.</param>
		public static void ReadField(
			out DateTime readValue,
			object fieldValue )
		{
			readValue = ReadFieldDateTime( fieldValue );
		}

		/// <summary>
		/// Read a value from the database.
		/// </summary>
		/// <param name="readValue">The value read and to return.
		/// Returns a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.</param>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		public static void ReadField(
			out bool readValue,
			object fieldValue )
		{
			readValue = ReadFieldBoolean( fieldValue );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Reading from DB fields, with default fallback.
		// ------------------------------------------------------------------

		/// <summary>
		/// Read a value from the database. Provide a default value if not
		/// readable, e.g. if NULL or <code>DBNull.Value</code>.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The value to return if the field
		/// to read from is not readable or is NULL or <code>DBNull.value</code>.</param>
		/// <returns>
		/// Returns the read value or the default value if not readable.
		/// </returns>
		public static int ReadField(
			object fieldValue,
			int fallbackTo )
		{
			return fieldValue == DBNull.Value ? fallbackTo : Convert.ToInt32(
				fieldValue,
				CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Read a value from the database. Provide a default value if not
		/// readable, e.g. if NULL or <code>DBNull.Value</code>.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The value to return if the field
		/// to read from is not readable or is NULL or <code>DBNull.value</code>.</param>
		/// <returns>
		/// Returns the read value or the default value if not readable.
		/// </returns>
		public static long ReadField(
			object fieldValue,
			long fallbackTo )
		{
			return fieldValue == DBNull.Value ? fallbackTo : Convert.ToInt64(
				fieldValue,
				CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Read a value from the database. Provide a default value if not
		/// readable, e.g. if NULL or <code>DBNull.Value</code>.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The value to return if the field
		/// to read from is not readable or is NULL or <code>DBNull.value</code>.</param>
		/// <returns>
		/// Returns the read value or the default value if not readable.
		/// </returns>
		public static string ReadField(
			object fieldValue,
			string fallbackTo )
		{
			return fieldValue == DBNull.Value ? fallbackTo : Convert.ToString(
				fieldValue,
				CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Read a value from the database. Provide a default value if not
		/// readable, e.g. if NULL or <code>DBNull.Value</code>.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The value to return if the field
		/// to read from is not readable or is NULL or <code>DBNull.value</code>.</param>
		/// <returns>
		/// Returns the read value or the default value if not readable.
		/// </returns>
		public static bool ReadField(
			object fieldValue,
			bool fallbackTo )
		{
			return fieldValue == DBNull.Value ? fallbackTo : Convert.ToBoolean(
				fieldValue,
				CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Read a value from the database. Provide a default value if not
		/// readable, e.g. if NULL or <code>DBNull.Value</code>.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The value to return if the field
		/// to read from is not readable or is NULL or <code>DBNull.value</code>.</param>
		/// <returns>
		/// Returns the read value or the default value if not readable.
		/// </returns>
		public static DateTime ReadField(
			object fieldValue,
			DateTime fallbackTo )
		{
			return fieldValue == DBNull.Value ? fallbackTo : Convert.ToDateTime(
				fieldValue,
				CultureInfo.CurrentCulture );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Reading from DB fields by specifing type explicitely.
		// ------------------------------------------------------------------

		/// <summary>
		/// Read a GUID value from the database.
		/// Use this for SQL-'timestamp' fields.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static byte[] ReadFieldByteArray(
			object fieldValue )
		{
			if ( fieldValue == null || fieldValue == DBNull.Value )
			{
				return null;
			}
			else
			{
				return (byte[])fieldValue;
			}
		}

		/// <summary>
		/// Read a GUID value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static Guid ReadFieldGuid(
			object fieldValue )
		{
			if ( fieldValue == null )
			{
				return Guid.Empty;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( fieldValue.GetType() == typeof( Guid ) )
			{
				return (Guid)fieldValue;
			}
			else if ( fieldValue == DBNull.Value )
			{
				return Guid.Empty;
			}
			else if ( fieldValue.ToString().Length <= 0 )
			{
				return Guid.Empty;
			}
			else
			{
				return new Guid( fieldValue.ToString() );
			}
		}

		/// <summary>
		/// Read an integer value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static int ReadFieldInteger(
			object fieldValue )
		{
			if ( fieldValue == null )
			{
				return 0;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( fieldValue.GetType() == typeof( int ) )
			{
				return (int)fieldValue;
			}
			else if ( fieldValue == DBNull.Value )
			{
				return 0;
			}
			else if ( !ConvertHelper.IsNumeric( fieldValue.ToString() ) )
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(
					fieldValue,
					CultureInfo.CurrentCulture );
			}
		}

		/// <summary>
		/// Read a long value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static long ReadFieldLong(
			object fieldValue )
		{
			if ( fieldValue == null )
			{
				return 0;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( fieldValue.GetType() == typeof( long ) )
			{
				return (long)fieldValue;
			}
			else if ( fieldValue.GetType() == typeof( int ) )
			{
				return (int)fieldValue;
			}
			else if ( fieldValue == DBNull.Value )
			{
				return 0;
			}
			else if ( !ConvertHelper.IsNumeric( fieldValue.ToString() ) )
			{
				return 0;
			}
			else
			{
				return Convert.ToInt64(
					fieldValue,
					CultureInfo.CurrentCulture );
			}
		}

		/// <summary>
		/// Read an double value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static double ReadFieldDouble(
			object fieldValue )
		{
			if ( fieldValue == null )
			{
				return 0;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( fieldValue.GetType() == typeof( double ) )
			{
				return (double)fieldValue;
			}
			else if ( fieldValue == DBNull.Value )
			{
				return 0;
			}
			else if ( !ConvertHelper.IsFloat( fieldValue.ToString() ) )
			{
				return 0;
			}
			else
			{
				return Convert.ToDouble(
					fieldValue,
					CultureInfo.CurrentCulture );
			}
		}

		/// <summary>
		/// Read a string value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static string ReadFieldString(
			object fieldValue )
		{
			if ( fieldValue == null )
			{
				return string.Empty;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( fieldValue.GetType() == typeof( string ) )
			{
				return (string)fieldValue;
			}
			else if ( fieldValue == DBNull.Value )
			{
				return string.Empty;
			}
			else
			{
				return Convert.ToString(
					fieldValue,
					CultureInfo.CurrentCulture );
			}
		}

		/// <summary>
		/// Read a decimal value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static decimal ReadFieldDecimal(
			object fieldValue )
		{
			if ( fieldValue == null )
			{
				return decimal.Zero;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( fieldValue.GetType() == typeof( decimal ) )
			{
				return (decimal)fieldValue;
			}
			else if ( fieldValue == DBNull.Value )
			{
				return decimal.Zero;
			}
			else
			{
				return Convert.ToDecimal(
					fieldValue,
					CultureInfo.CurrentCulture );
			}
		}

		/// <summary>
		/// Read an object value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static object ReadFieldObject(
			object fieldValue )
		{
			if ( fieldValue == null || fieldValue == DBNull.Value )
			{
				return null;
			}
			else
			{
				return fieldValue;
			}
		}

		/// <summary>
		/// Read a DateTime value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static DateTime ReadFieldDateTime(
			object fieldValue )
		{
			if ( fieldValue == null )
			{
				return DateTime.MinValue;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( fieldValue.GetType() == typeof( DateTime ) )
			{
				return (DateTime)fieldValue;
			}
			else if ( fieldValue == DBNull.Value )
			{
				return DateTime.MinValue;
			}
			else
			{
				return Convert.ToDateTime(
					fieldValue,
					CultureInfo.CurrentCulture );
			}
		}

		/// <summary>
		/// Read a boolean value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static bool ReadFieldBoolean(
			object fieldValue )
		{
			if ( fieldValue == null )
			{
				return false;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( fieldValue.GetType() == typeof( bool ) )
			{
				return (bool)fieldValue;
			}
			else if ( fieldValue == DBNull.Value )
			{
				return false;
			}
			else
			{
				return Convert.ToBoolean(
					fieldValue,
					CultureInfo.CurrentCulture );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Reading from DB fields by specifing type explicitely, with default fallback.
		// ------------------------------------------------------------------

		/// <summary>
		/// Read a GUID value from the database.
		/// Use this for SQL-'timestamp' fields.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static byte[] ReadFieldByteArray(
			object fieldValue,
			byte[] fallbackTo )
		{
			if ( fieldValue == null ||
				fieldValue == DBNull.Value )
			{
				return fallbackTo;
			}
			else
			{
				return (byte[])fieldValue;
			}
		}

		/// <summary>
		/// Read a GUID value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static Guid ReadFieldGuid(
			object fieldValue,
			Guid fallbackTo )
		{
			if ( fieldValue == null ||
				fieldValue == DBNull.Value ||
				fieldValue.ToString().Length <= 0 )
			{
				return fallbackTo;
			}
			else
			{
				return new Guid( fieldValue.ToString() );
			}
		}

		/// <summary>
		/// Read an integer value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static int ReadFieldInteger(
			object fieldValue,
			int fallbackTo )
		{
			if ( fieldValue == null ||
				fieldValue == DBNull.Value ||
				!ConvertHelper.IsNumeric( fieldValue.ToString() ) )
			{
				return fallbackTo;
			}
			else
			{
				return Convert.ToInt32(
					fieldValue,
					CultureInfo.CurrentCulture );
			}
		}

		/// <summary>
		/// Read a long value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static long ReadFieldLong(
			object fieldValue,
			long fallbackTo )
		{
			if ( fieldValue == null ||
				fieldValue == DBNull.Value ||
				!ConvertHelper.IsNumeric( fieldValue.ToString() ) )
			{
				return fallbackTo;
			}
			else
			{
				return Convert.ToInt64(
					fieldValue,
					CultureInfo.CurrentCulture );
			}
		}

		/// <summary>
		/// Read an double value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static double ReadFieldDouble(
			object fieldValue,
			double fallbackTo )
		{
			if ( fieldValue == null ||
				fieldValue == DBNull.Value ||
				!ConvertHelper.IsFloat( fieldValue.ToString() ) )
			{
				return fallbackTo;
			}
			else
			{
				return Convert.ToDouble(
					fieldValue,
					CultureInfo.CurrentCulture );
			}
		}

		/// <summary>
		/// Read a string value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static string ReadFieldString(
			object fieldValue,
			string fallbackTo )
		{
			return fieldValue == null ||
				fieldValue == DBNull.Value ? fallbackTo : Convert.ToString(
				fieldValue,
				CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Read a decimal value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static decimal ReadFieldDecimal(
			object fieldValue,
			decimal fallbackTo )
		{
			return fieldValue == null ||
				fieldValue == DBNull.Value ? fallbackTo : Convert.ToDecimal(
				fieldValue,
				CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Read an object value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static object ReadFieldObject(
			object fieldValue,
			object fallbackTo )
		{
			return fieldValue == null ||
				fieldValue == DBNull.Value ? fallbackTo : fieldValue;
		}

		/// <summary>
		/// Read a DateTime value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static DateTime ReadFieldDateTime(
			object fieldValue,
			DateTime fallbackTo )
		{
			return fieldValue == null ||
				fieldValue == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(
				fieldValue,
				CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Read a boolean value from the database.
		/// </summary>
		/// <param name="fieldValue">The object to read from.
		/// Usually something like <code>row["MyField"]</code>.</param>
		/// <param name="fallbackTo">if set to <c>true</c> [fallback to].</param>
		/// <returns>
		/// Returns the read value or a default value if not readable,
		/// e.g. if NULL or <code>DBNull.Value</code>.
		/// </returns>
		public static bool ReadFieldBoolean(
			object fieldValue,
			bool fallbackTo )
		{
			return fieldValue == null ||
				fieldValue == DBNull.Value ? fallbackTo : Convert.ToBoolean(
				fieldValue,
				CultureInfo.CurrentCulture );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Writing to DB fields.
		// ------------------------------------------------------------------

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteField(
			Guid fieldValue )
		{
			return WriteFieldGuid( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteField(
			int fieldValue )
		{
			return WriteFieldInteger( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteField(
			long fieldValue )
		{
			return WriteFieldLong( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteField(
			string fieldValue )
		{
			return WriteFieldString( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteField(
			decimal fieldValue )
		{
			return WriteFieldDecimal( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteField(
			double fieldValue )
		{
			return WriteFieldDouble( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteField(
			DateTime fieldValue )
		{
			return WriteFieldDateTime( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">if set to <c>true</c> [field value].</param>
		/// <returns></returns>
		public static object WriteField(
			bool fieldValue )
		{
			return WriteFieldBoolean( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteField(
			object fieldValue )
		{
			return WriteFieldObject( fieldValue );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Writing to DB fields with definable NULL behaviour.
		// ------------------------------------------------------------------

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="nullBehaviour">The null behaviour.</param>
		/// <returns></returns>
		public static object WriteField(
			int fieldValue,
			NullBehaviour nullBehaviour )
		{
			return WriteFieldObject(
				AdoNetOleDBParamCollection.ApplyNullBehaviourToValue(
				fieldValue,
				nullBehaviour ) );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="nullBehaviour">The null behaviour.</param>
		/// <returns></returns>
		public static object WriteField(
			long fieldValue,
			NullBehaviour nullBehaviour )
		{
			return WriteFieldObject(
				AdoNetOleDBParamCollection.ApplyNullBehaviourToValue(
				fieldValue,
				nullBehaviour ) );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="nullBehaviour">The null behaviour.</param>
		/// <returns></returns>
		public static object WriteField(
			string fieldValue,
			NullBehaviour nullBehaviour )
		{
			return WriteFieldObject(
				AdoNetOleDBParamCollection.ApplyNullBehaviourToValue(
				fieldValue,
				nullBehaviour ) );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="nullBehaviour">The null behaviour.</param>
		/// <returns></returns>
		public static object WriteField(
			object fieldValue,
			NullBehaviour nullBehaviour )
		{
			return WriteFieldObject(
				AdoNetOleDBParamCollection.ApplyNullBehaviourToValue(
				fieldValue,
				nullBehaviour ) );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Writing to DB fields, alternative signature.
		// ------------------------------------------------------------------

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="readValue">The read value.</param>
		public static void WriteField(
			byte[] fieldValue,
			out object readValue )
		{
			readValue = WriteFieldByteArray( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="readValue">The read value.</param>
		public static void WriteField(
			Guid fieldValue,
			out object readValue )
		{
			readValue = WriteFieldGuid( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="readValue">The read value.</param>
		public static void WriteField(
			int fieldValue,
			out object readValue )
		{
			readValue = WriteFieldInteger( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="readValue">The read value.</param>
		public static void WriteField(
			long fieldValue,
			out object readValue )
		{
			readValue = WriteFieldLong( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="readValue">The read value.</param>
		public static void WriteField(
			string fieldValue,
			out object readValue )
		{
			readValue = WriteFieldString( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="readValue">The read value.</param>
		public static void WriteField(
			decimal fieldValue,
			out object readValue )
		{
			readValue = WriteFieldDecimal( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="readValue">The read value.</param>
		public static void WriteField(
			double fieldValue,
			out object readValue )
		{
			readValue = WriteFieldDouble( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="readValue">The read value.</param>
		public static void WriteField(
			DateTime fieldValue,
			out object readValue )
		{
			readValue = WriteFieldDateTime( fieldValue );
		}

		/// <summary>
		/// Writes the field.
		/// </summary>
		/// <param name="fieldValue">if set to <c>true</c> [field value].</param>
		/// <param name="readValue">The read value.</param>
		public static void WriteField(
			bool fieldValue,
			out object readValue )
		{
			readValue = WriteFieldBoolean( fieldValue );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Writing to DB fields by specifying type explicitely.
		// ------------------------------------------------------------------

		/// <summary>
		/// Writes the field byte array.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteFieldByteArray(
			byte[] fieldValue )
		{
			if ( fieldValue == null || fieldValue.Length <= 0 )
			{
				return DBNull.Value;
			}
			else
			{
				return fieldValue;
			}
		}

		/// <summary>
		/// Writes the field GUID.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteFieldGuid(
			Guid fieldValue )
		{
			if ( fieldValue == Guid.Empty )
			{
				return DBNull.Value;
			}
			else
			{
				return fieldValue;
			}
		}

		/// <summary>
		/// Writes the field integer.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteFieldInteger(
			int fieldValue )
		{
			return fieldValue;
		}

		/// <summary>
		/// Writes the field long.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteFieldLong(
			long fieldValue )
		{
			return fieldValue;
		}

		/// <summary>
		/// Writes the field integer.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteFieldInteger(
			string fieldValue )
		{
			return ConvertHelper.ToInt32( fieldValue );
		}

		/// <summary>
		/// Writes the field U long.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteFieldULong(
			int fieldValue )
		{
			return fieldValue;
		}

		/// <summary>
		/// Writes the field double.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteFieldDouble(
			double fieldValue )
		{
			return fieldValue;
		}

		/// <summary>
		/// Writes the field GUID.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="ifThis">If this.</param>
		/// <param name="thenThis">The then this.</param>
		/// <returns></returns>
		public static object WriteFieldGuid(
			Guid fieldValue,
			Guid ifThis,
			object thenThis )
		{
			if ( fieldValue == ifThis )
			{
				return thenThis;
			}
			else
			{
				return fieldValue;
			}
		}

		/// <summary>
		/// Writes the field integer.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="ifThis">If this.</param>
		/// <param name="thenThis">The then this.</param>
		/// <returns></returns>
		public static object WriteFieldInteger(
			int fieldValue,
			int ifThis,
			object thenThis )
		{
			if ( fieldValue == ifThis )
			{
				return thenThis;
			}
			else
			{
				return fieldValue;
			}
		}

		/// <summary>
		/// Writes the field long.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="ifThis">If this.</param>
		/// <param name="thenThis">The then this.</param>
		/// <returns></returns>
		public static object WriteFieldLong(
			long fieldValue,
			long ifThis,
			object thenThis )
		{
			if ( fieldValue == ifThis )
			{
				return thenThis;
			}
			else
			{
				return fieldValue;
			}
		}

		/// <summary>
		/// Writes the field string.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteFieldString(
			string fieldValue )
		{
			if ( string.IsNullOrEmpty( fieldValue ) )
			{
				return DBNull.Value;
			}
			else
			{
				return fieldValue;
			}
		}

		/// <summary>
		/// Writes the field decimal.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteFieldDecimal(
			decimal fieldValue )
		{
			return fieldValue;
		}

		/// <summary>
		/// Writes the field object.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteFieldObject(
			object fieldValue )
		{
			if ( fieldValue == null )
			{
				return DBNull.Value;
			}
			else
			{
				return fieldValue;
			}
		}

		/// <summary>
		/// Writes the field date time.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		public static object WriteFieldDateTime(
			DateTime fieldValue )
		{
			if ( fieldValue == DateTime.MinValue )
			{
				return DBNull.Value;
			}
			else
			{
				return fieldValue;
			}
		}

		/// <summary>
		/// Writes the field boolean.
		/// </summary>
		/// <param name="fieldValue">if set to <c>true</c> [field value].</param>
		/// <returns></returns>
		public static object WriteFieldBoolean(
			bool fieldValue )
		{
			return fieldValue;
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}