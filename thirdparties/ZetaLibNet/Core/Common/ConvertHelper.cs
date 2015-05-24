namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Threading;
	using System.Globalization;
	using ZetaLib.Core.Logging;
	using System.Text;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Helper routines for converting integral types.
	/// </summary>
	public sealed class ConvertHelper
	{
		#region Miscellaneous converting routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Toes the culture info.
		/// </summary>
		/// <param name="languageCode">The language code.</param>
		/// <returns></returns>
		public static CultureInfo ToCultureInfo(
			string languageCode )
		{
			return ToCultureInfo(
				languageCode,
				CultureInfo.InvariantCulture );
		}

		/// <summary>
		/// Toes the culture info.
		/// </summary>
		/// <param name="languageCode">The language code.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns></returns>
		public static CultureInfo ToCultureInfo(
			string languageCode,
			CultureInfo fallbackTo )
		{
			if ( string.IsNullOrEmpty( languageCode ) ||
				languageCode.Trim().Length < 2 )
			{
				return fallbackTo;
			}
			else
			{
				string c4;
				string c2;

				if ( languageCode.Length == 2 )
				{
					c2 = languageCode;
					c4 = c2 + @"-" + c2;
				}
				else if ( languageCode.Length == 4 )
				{
					c2 = languageCode.Substring( 0, 2 );
					c4 = languageCode;
				}
				else
				{
					c2 = languageCode.Substring( 0, 2 );
					c4 = c2 + @"-" + c2;
				}

				try
				{
					CultureInfo info = new CultureInfo(
						c4 );
					return info;
				}
				catch ( ArgumentException )
				{
					try
					{
						// if languageCode 4 failed, try languageCode 2.
						CultureInfo info = new CultureInfo(
							c2 );
						return info;
					}
					catch ( ArgumentException y )
					{
						LogCentral.Current.LogWarn(
							string.Format(
							@"No suitable culture for language '{0}' found.",
							languageCode ),
							y );

						return fallbackTo;
					}
				}
			}
		}

		/// <summary>
		/// Use this to convert a SQL-timestamp field to a printable
		/// string (e.g. for debugging purposes).
		/// </summary>
		/// <param name="buffer">The buffer to convert.</param>
		/// <returns>
		/// Returns the textual representation of the buffer
		/// or a NULL string if the buffer is NULL or a NULL
		/// if the buffer is empty.
		/// </returns>
		public static string ToString(
			byte[] buffer )
		{
			if ( buffer == null )
			{
				return null;
			}
			else if ( buffer.Length <= 0 )
			{
				return null;
			}
			else
			{
				StringBuilder s = new StringBuilder();

				foreach ( byte b in buffer )
				{
					if ( s.Length > 0 )
					{
						s.Append( @"-" );
					}

					s.Append( Convert.ToString( b ) );
				}

				return s.ToString();
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Converting routines with default fallbacks.
		// ------------------------------------------------------------------

		/// <summary>
		/// Toes the T.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public static T ToT<T>(
			object o )
		{
			return ToT<T>( o, default( T ) );
		}

		/// <summary>
		/// Convert to a string.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public static string ToString(
			object o )
		{
			return ToString( o, (string)null );
		}

		/// <summary>
		/// Convert to a string.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static string ToString(
			object o,
			IFormatProvider provider )
		{
			return ToString( o, null, provider );
		}

		/// <summary>
		/// Convert a string to a double, returns 0.0 if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public static double ToDouble(
			object o )
		{
			return ToDouble( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert a string to a double, returns 0.0 if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static double ToDouble(
			object o,
			IFormatProvider provider )
		{
			return ToDouble( o, 0.0, provider );
		}

		/// <summary>
		/// Convert a string to an integer, returns 0 if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public static int ToInt32(
			object o )
		{
			return ToInt32( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert a string to an integer, returns 0 if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static int ToInt32(
			object o,
			IFormatProvider provider )
		{
			return ToInt32( o, 0, provider );
		}

		/// <summary>
		/// Convert a string to an integer, returns 0 if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public static long ToInt64(
			object o )
		{
			return ToInt64( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert a string to an integer, returns 0 if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static long ToInt64(
			object o,
			IFormatProvider provider )
		{
			return ToInt64( o, 0, provider );
		}

		/// <summary>
		/// Convert a string to a decimal, returns zero if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public static decimal ToDecimal(
			object o )
		{
			return ToDecimal( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert a string to a decimal, returns zero if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static decimal ToDecimal(
			object o,
			IFormatProvider provider )
		{
			return ToDecimal( o, decimal.Zero, provider );
		}

		/// <summary>
		/// Convert a string to a date time, returns DateTime.MinValue if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public static DateTime ToDateTime(
			object o )
		{
			return ToDateTime( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert a string to a date time, returns DateTime.MinValue if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static DateTime ToDateTime(
			object o,
			IFormatProvider provider )
		{
			return ToDateTime( o, DateTime.MinValue, provider );
		}

		/// <summary>
		/// Convert a string to a boolean, returns FALSE if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public static bool ToBoolean(
			object o )
		{
			return ToBoolean( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert a string to a boolean, returns FALSE if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static bool ToBoolean(
			object o,
			IFormatProvider provider )
		{
			return ToBoolean( o, false, provider );
		}

		/// <summary>
		/// Toes the GUID.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public static Guid ToGuid(
			object o )
		{
			return ToGuid( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Toes the GUID.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static Guid ToGuid(
			object o,
			IFormatProvider provider )
		{
			return ToGuid( o, Guid.Empty, provider );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Converting routines with user-defined fallbacks.
		// ------------------------------------------------------------------

		/// <summary>
		/// Toes the T.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns></returns>
		public static T ToT<T>(
			object o,
			T fallbackTo )
		{
			if ( o == null )
			{
				return fallbackTo;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( T ) )
			{
				return (T)o;
			}
			else if ( typeof( T ).IsEnum )
			{
				if ( Enum.IsDefined( typeof( T ), o ) )
				{
					return (T)Enum.Parse( typeof( T ), o.ToString(), true );
				}
				else
				{
					return fallbackTo;
				}
			}
			else
			{
				return fallbackTo;
			}
		}

		/// <summary>
		/// Convert to a string.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns></returns>
		public static string ToString(
			object o,
			string fallbackTo )
		{
			return ToString( o, fallbackTo, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert to a string.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static string ToString(
			object o,
			string fallbackTo,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return fallbackTo;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( string ) )
			{
				return (string)o;
			}
			else
			{
				return Convert.ToString( o, provider );
			}
		}

		/// <summary>
		/// Convert a string to a double, returns 0.0 if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns></returns>
		public static double ToDouble(
			object o,
			double fallbackTo )
		{
			return ToDouble( o, fallbackTo, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert a string to a double, returns 0.0 if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static double ToDouble(
			object o,
			double fallbackTo,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return fallbackTo;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( double ) )
			{
				return (double)o;
			}
			else if ( IsFloat( o, provider ) )
			{
				return Convert.ToDouble( o, provider );
			}
			else
			{
				return fallbackTo;
			}
		}

		/// <summary>
		/// Convert a string to an integer, returns 0 if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns></returns>
		public static int ToInt32(
			object o,
			int fallbackTo )
		{
			return ToInt32( o, fallbackTo, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert a string to an integer, returns 0 if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static int ToInt32(
			object o,
			int fallbackTo,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return fallbackTo;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( int ) )
			{
				return (int)o;
			}
			else if ( IsInteger( o, provider ) )
			{
				return Convert.ToInt32( o, provider );
			}
			else if ( o is Enum )
			{
				return (int)o;
			}
			else
			{
				return fallbackTo;
			}
		}

		/// <summary>
		/// Convert a string to an integer, returns 0 if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns></returns>
		public static long ToInt64(
			object o,
			long fallbackTo )
		{
			return ToInt64( o, fallbackTo, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert a string to an integer, returns 0 if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static long ToInt64(
			object o,
			long fallbackTo,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return fallbackTo;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( long ) )
			{
				return (long)o;
			}
			else if ( IsInt64( o, provider ) )
			{
				return Convert.ToInt64( o, provider );
			}
			else
			{
				return fallbackTo;
			}
		}

		/// <summary>
		/// Convert a string to a decimal, returns zero if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns></returns>
		public static decimal ToDecimal(
			object o,
			decimal fallbackTo )
		{
			return ToDecimal( o, fallbackTo, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert a string to a decimal, returns zero if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static decimal ToDecimal(
			object o,
			decimal fallbackTo,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return fallbackTo;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( decimal ) )
			{
				return (decimal)o;
			}
			else if ( IsDecimal( o, provider ) )
			{
				return Convert.ToDecimal( o, provider );
			}
			else
			{
				return fallbackTo;
			}
		}

		/// <summary>
		/// Convert a string to a date time,
		/// returns DateTime.MinValue if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns></returns>
		public static DateTime ToDateTime(
			object o,
			DateTime fallbackTo )
		{
			return ToDateTime( o, fallbackTo, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert a string to a date time,
		/// returns DateTime.MinValue if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static DateTime ToDateTime(
			object o,
			DateTime fallbackTo,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return fallbackTo;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( DateTime ) )
			{
				return (DateTime)o;
			}
			else if ( IsDateTime( o, provider ) )
			{
				return Convert.ToDateTime( o, provider );
			}
			else
			{
				return fallbackTo;
			}
		}

		/// <summary>
		/// Convert a string to a boolean, returns FALSE if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">if set to <c>true</c> [fallback to].</param>
		/// <returns></returns>
		public static bool ToBoolean(
			object o,
			bool fallbackTo )
		{
			return ToBoolean( o, fallbackTo, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Convert a string to a boolean, returns FALSE if fails.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">if set to <c>true</c> [fallback to].</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static bool ToBoolean(
			object o,
			bool fallbackTo,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return fallbackTo;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( bool ) )
			{
				return (bool)o;
			}
			else if ( IsBoolean( o, provider ) )
			{
				try
				{
					string s =
						Convert.ToString( o, provider ).Trim().ToLowerInvariant();

					if ( s.Length <= 0 )
					{
						return fallbackTo;
					}
					else if (
						string.Compare( s, bool.TrueString, true ) == 0 ||
						s == @"1" ||
						s == @"-1" )
					{
						return true;
					}
					else if (
						string.Compare( s, bool.FalseString, true ) == 0 ||
					   s == @"0" )
					{
						return false;
					}
					else
					{
						return bool.Parse( Convert.ToString( o, provider ) );
					}
				}
				catch ( FormatException )
				{
					return fallbackTo;
				}
			}
			else
			{
				return fallbackTo;
			}
		}

		/// <summary>
		/// Toes the GUID.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <returns></returns>
		public static Guid ToGuid(
			object o,
			Guid fallbackTo )
		{
			return ToGuid( o, fallbackTo, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Toes the GUID.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="fallbackTo">The fallback to.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static Guid ToGuid(
			object o,
			Guid fallbackTo,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return fallbackTo;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( Guid ) )
			{
				return (Guid)o;
			}
			else if ( IsGuid( o, provider ) )
			{
				if ( o is byte[] )
				{
					return new Guid( o as byte[] );
				}
				else
				{
					return new Guid( Convert.ToString( o, provider ) );
				}
			}
			else
			{
				return fallbackTo;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Conversion checkings.
		// ------------------------------------------------------------------

		/// <summary>
		/// Checks whether a string contains a valid boolean.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <returns>
		/// Returns TRUE if is a boolean, FALSE if not.
		/// </returns>
		public static bool IsBoolean(
			object o )
		{
			return IsBoolean( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Checks whether a string contains a valid boolean.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <param name="provider">The provider.</param>
		/// <returns>
		/// Returns TRUE if is a boolean, FALSE if not.
		/// </returns>
		public static bool IsBoolean(
			object o,
			IFormatProvider provider )
		{
			try
			{
				if ( o == null )
				{
					return false;
				}
				// This is the fastest way, see
				// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
				else if ( o.GetType() == typeof( bool ) )
				{
					return true;
				}
				else
				{
					string s =
						Convert.ToString( o, provider ).Trim().ToLowerInvariant();

					if ( s.Length <= 0 )
					{
						return false;
					}
					else if ( o is bool )
					{
						return true;
					}
					else if (
						string.Compare( s, bool.TrueString, true ) == 0 ||
						s == @"1" ||
						s == @"-1" )
					{
						return true;
					}
					else if (
					   string.Compare( s, bool.FalseString, true ) == 0 ||
					   s == @"0" )
					{
						return true;
					}
					else
					{
						bool.Parse( Convert.ToString( o, provider ) );
					}
				}
			}
			catch ( FormatException )
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Checks whether a string contains a valid date and/or time.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <returns>
		/// Returns TRUE if is date/time, FALSE if not.
		/// </returns>
		public static bool IsDateTime(
			object o )
		{
			return IsDateTime( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Checks whether a string contains a valid date and/or time.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <param name="provider">The provider.</param>
		/// <returns>
		/// Returns TRUE if is date/time, FALSE if not.
		/// </returns>
		public static bool IsDateTime(
			object o,
			IFormatProvider provider )
		{
			if ( o == null ||
				Convert.ToString( o, provider ).Trim().Length <= 0 )
			{
				return false;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( DateTime ) )
			{
				return true;
			}
			else
			{
				DateTime r;

				return DateTime.TryParse(
					Convert.ToString( o, provider ),
					provider,
					DateTimeStyles.None,
					out r );
			}
		}

		/// <summary>
		/// Checks whether a string contains a valid float.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <returns>
		/// Returns TRUE if the string contains a float,
		/// FALSE if not.
		/// </returns>
		public static bool IsNumeric(
			object o )
		{
			return IsNumeric( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Checks whether a string contains a valid float.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <param name="provider">The provider.</param>
		/// <returns>
		/// Returns TRUE if the string contains a float,
		/// FALSE if not.
		/// </returns>
		public static bool IsNumeric(
			object o,
			IFormatProvider provider )
		{
			return DoIsNumeric( o,
				floatNumberStyle,
				provider );
		}

		/// <summary>
		/// Checks whether a string contains a valid decimal.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <returns>
		/// Returns TRUE if the string contains a decimal,
		/// FALSE if not.
		/// </returns>
		public static bool IsDecimal(
			object o )
		{
			return IsDecimal( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Checks whether a string contains a valid decimal.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <param name="provider">The provider.</param>
		/// <returns>
		/// Returns TRUE if the string contains a decimal,
		/// FALSE if not.
		/// </returns>
		public static bool IsDecimal(
			object o,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return false;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( decimal ) )
			{
				return true;
			}
			else
			{
				return DoIsNumeric(
					o,
					floatNumberStyle,
					provider );
			}
		}

		/// <summary>
		/// Checks whether a string contains a valid float.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <returns>
		/// Returns TRUE if the string contains a float,
		/// FALSE if not.
		/// </returns>
		public static bool IsFloat(
			object o )
		{
			return IsFloat( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Checks whether a string contains a valid float.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <param name="provider">The provider.</param>
		/// <returns>
		/// Returns TRUE if the string contains a float,
		/// FALSE if not.
		/// </returns>
		public static bool IsFloat(
			object o,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return false;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( float ) )
			{
				return true;
			}
			else
			{
				return DoIsNumeric( o,
					floatNumberStyle,
					provider );
			}
		}

		/// <summary>
		/// Checks whether a string contains a valid double.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <returns>
		/// Returns TRUE if the string contains a double,
		/// FALSE if not.
		/// </returns>
		public static bool IsDouble(
			object o )
		{
			return IsDouble( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Checks whether a string contains a valid double.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <param name="provider">The provider.</param>
		/// <returns>
		/// Returns TRUE if the string contains a double,
		/// FALSE if not.
		/// </returns>
		public static bool IsDouble(
			object o,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return false;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( double ) )
			{
				return true;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( float ) )
			{
				return true;
			}
			else
			{
				return DoIsNumeric( o,
					floatNumberStyle,
					provider );
			}
		}

		/// <summary>
		/// Checks whether a string contains a valid integer.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <returns>
		/// Returns TRUE if the string contains an integer,
		/// FALSE if not.
		/// </returns>
		public static bool IsInteger(
			object o )
		{
			return IsInteger( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Checks whether a string contains a valid integer.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <param name="provider">The provider.</param>
		/// <returns>
		/// Returns TRUE if the string contains an integer,
		/// FALSE if not.
		/// </returns>
		public static bool IsInteger(
			object o,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return false;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( int ) )
			{
				return true;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( long ) )
			{
				return true;
			}
			else if ( o is Enum )
			{
				return true;
			}
			else
			{
				return DoIsNumeric( o, NumberStyles.Integer, provider );
			}
		}

		/// <summary>
		/// Checks whether a string contains a valid integer.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <returns>
		/// Returns TRUE if the string contains an integer,
		/// FALSE if not.
		/// </returns>
		public static bool IsInt32(
			object o )
		{
			return IsInt32( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Checks whether a string contains a valid integer.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <param name="provider">The provider.</param>
		/// <returns>
		/// Returns TRUE if the string contains an integer,
		/// FALSE if not.
		/// </returns>
		public static bool IsInt32(
			object o,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return false;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( Int32 ) )
			{
				return true;
			}
			else
			{
				return DoIsNumeric( o, NumberStyles.Integer, provider );
			}
		}

		/// <summary>
		/// Checks whether a string contains a valid integer.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <returns>
		/// Returns TRUE if the string contains an integer,
		/// FALSE if not.
		/// </returns>
		public static bool IsInt64(
			object o )
		{
			return IsInt64( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Checks whether a string contains a valid integer.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <param name="provider">The provider.</param>
		/// <returns>
		/// Returns TRUE if the string contains an integer,
		/// FALSE if not.
		/// </returns>
		public static bool IsInt64(
			object o,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return false;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( Int64 ) )
			{
				return true;
			}
			else
			{
				return DoIsNumeric( o, NumberStyles.Integer, provider );
			}
		}

		/// <summary>
		/// Checks whether a string contains a valid currency number.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <returns>
		/// Returns TRUE if the string contains a currency,
		/// FALSE if not.
		/// </returns>
		public static bool IsCurrency(
			object o )
		{
			return IsCurrency( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Checks whether a string contains a valid currency number.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <param name="provider">The provider.</param>
		/// <returns>
		/// Returns TRUE if the string contains a currency,
		/// FALSE if not.
		/// </returns>
		public static bool IsCurrency(
			object o,
			IFormatProvider provider )
		{
			return DoIsNumeric( o, NumberStyles.Currency, provider );
		}

		/// <summary>
		/// Does the is numeric.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="styles">The styles.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		private static bool DoIsNumeric(
			object o,
			NumberStyles styles,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return false;
			}
			else if ( Convert.ToString( o, provider ).Length <= 0 )
			{
				return false;
			}
			else
			{
				double result;
				return double.TryParse(
					o.ToString(),
					styles,
					provider,
					out result );
			}
		}

		/// <summary>
		/// Checks whether a string contains a valid float.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <returns>
		/// Returns TRUE if the string contains a float, FALSE if not.
		/// </returns>
		public static bool IsGuid(
			object o )
		{
			return IsGuid( o, CultureInfo.CurrentCulture );
		}

		/// <summary>
		/// Checks whether a string contains a valid float.
		/// </summary>
		/// <param name="o">The string to check.</param>
		/// <param name="provider">The provider.</param>
		/// <returns>
		/// Returns TRUE if the string contains a float, FALSE if not.
		/// </returns>
		public static bool IsGuid(
			object o,
			IFormatProvider provider )
		{
			if ( o == null )
			{
				return false;
			}
			// This is the fastest way, see
			// http://www.google.de/url?sa=t&ct=res&cd=4&url=http%3A%2F%2Fblogs.msdn.com%2Fvancem%2Farchive%2F2006%2F10%2F01%2F779503.aspx&ei=nOuTRY7TAoXe2QLi7qX3Dg&usg=__GUu0brYrkgjJl63ZZ3JBOzJCVH8=&sig2=1wvt78Kof6Bw7Drs3LL_ng
			else if ( o.GetType() == typeof( Guid ) )
			{
				return true;
			}
			else if ( o is byte[] )
			{
				try
				{
					Guid ignore = new Guid( o as byte[] );
					return true;
				}
				catch ( ArgumentException )
				{
					return false;
				}
			}
			else
			{
				try
				{
					Guid ignore = new Guid( o.ToString() );
					return true;
				}
				catch ( ArgumentException )
				{
					return false;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Formatting routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Formats WITH currency symbol, default precision.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static string FormatCurrency(
			decimal val )
		{
			// "C": With currency symbol.
			return FormatCurrency( val, Thread.CurrentThread.CurrentCulture );
		}

		/// <summary>
		/// Formats WITH currency symbol, default precision.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static string FormatCurrency(
			decimal val,
			IFormatProvider provider )
		{
			// "C": With currency symbol.
			return val.ToString( @"C", provider );
		}

		/// <summary>
		/// Formats WITH currency symbol.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="precision">Gives the number of decimals digits
		/// after the point.</param>
		/// <returns></returns>
		public static string FormatCurrency(
			decimal val,
			int precision )
		{
			return FormatCurrency(
				val,
				precision,
				Thread.CurrentThread.CurrentCulture );
		}

		/// <summary>
		/// Formats WITH currency symbol.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="precision">Gives the number of decimals digits
		/// after the point.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static string FormatCurrency(
			decimal val,
			int precision,
			IFormatProvider provider )
		{
			NumberFormatInfo nfi =
				(provider.GetFormat( typeof( NumberFormatInfo ) ) as
				NumberFormatInfo).Clone() as NumberFormatInfo;
			nfi.CurrencyDecimalDigits = precision;

			// "C": With currency symbol.
			return val.ToString( @"C", nfi );
		}

		/// <summary>
		/// WITH or WITHOUT currency symbol, default precision.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="addCurrencySymbol">if set to <c>true</c> [add currency symbol].</param>
		/// <returns></returns>
		public static string FormatCurrency(
			decimal val,
			bool addCurrencySymbol )
		{
			return FormatCurrency(
				val,
				addCurrencySymbol,
				Thread.CurrentThread.CurrentCulture );
		}

		/// <summary>
		/// WITH or WITHOUT currency symbol, default precision.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="addCurrencySymbol">if set to <c>true</c> [add currency symbol].</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static string FormatCurrency(
			decimal val,
			bool addCurrencySymbol,
			IFormatProvider provider )
		{
			if ( addCurrencySymbol )
			{
				return FormatCurrency( val );
			}
			else
			{
				return val.ToString( @"n", provider );
			}
		}

		/// <summary>
		/// WITH or WITHOUT currency symbol, user-defined precision.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="precision">The precision.</param>
		/// <param name="addCurrencySymbol">if set to <c>true</c> [add currency symbol].</param>
		/// <returns></returns>
		public static string FormatCurrency(
			decimal val,
			int precision,
			bool addCurrencySymbol )
		{
			return FormatCurrency(
				val,
				precision,
				addCurrencySymbol,
				Thread.CurrentThread.CurrentCulture );
		}

		/// <summary>
		/// WITH or WITHOUT currency symbol, user-defined precision.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="precision">The precision.</param>
		/// <param name="addCurrencySymbol">if set to <c>true</c> [add currency symbol].</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static string FormatCurrency(
			decimal val,
			int precision,
			bool addCurrencySymbol,
			IFormatProvider provider )
		{
			if ( addCurrencySymbol )
			{
				return FormatCurrency( val, precision, provider );
			}
			else
			{
				NumberFormatInfo nfi =
					(provider.GetFormat( typeof( NumberFormatInfo ) ) as
					NumberFormatInfo).Clone() as NumberFormatInfo;
				nfi.NumberDecimalDigits = precision;

				return val.ToString( @"n", nfi );
			}
		}

		/// <summary>
		/// Formats the decimal.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static string FormatDecimal(
			decimal val )
		{
			return FormatDecimal( val, Thread.CurrentThread.CurrentCulture );
		}

		/// <summary>
		/// Formats the decimal.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static string FormatDecimal(
			decimal val,
			IFormatProvider provider )
		{
			return val.ToString( @"D", provider );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private helper.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private static readonly NumberStyles floatNumberStyle =
			NumberStyles.Float |
			NumberStyles.Number |
			NumberStyles.AllowThousands |
			NumberStyles.AllowDecimalPoint |
			NumberStyles.AllowLeadingSign |
			NumberStyles.AllowLeadingWhite |
			NumberStyles.AllowTrailingWhite;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}