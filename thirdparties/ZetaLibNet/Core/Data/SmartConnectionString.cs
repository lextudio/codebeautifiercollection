namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.IO;
	using System.Text.RegularExpressions;
	using ZetaLib.Core.Logging;
	using System.Collections.Generic;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// A class for providing a type-safe way to pass connection string
	/// (i.e. not as pure System.String anymore).
	/// </summary>
	public class SmartConnectionString :
		ICloneable,
		IComparable,
		IComparable<SmartConnectionString>,
		IEquatable<SmartConnectionString>
	{
		#region Public static methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Check whether a valid value is contained.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <returns>
		/// 	<c>true</c> if [is null or empty] [the specified connection string]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrEmpty(
			SmartConnectionString connectionString )
		{
			return
				connectionString == null ||
				connectionString.IsEmpty;
		}

		/// <summary>
		/// The connection string allows a tile ("~") as the first character
		/// in path specifications (e.g. inside a connection string for
		/// Microsoft Access MDB databases).
		/// </summary>
		/// <param name="connectionString">The connection string that
		/// (optionally) contains a tilde inside the path.</param>
		/// <returns>
		/// Returns the connection string with an expanded tilde,
		/// or the original connection string if no tilde was found at a
		/// suitable position.
		/// </returns>
		/// <remarks>
		/// This function tries to replace the tilde with path where the
		/// configuration file ("app.config" or "web.config") is located.
		/// </remarks>
		public static SmartConnectionString ReplaceConnectionStringTilde(
			SmartConnectionString connectionString )
		{
			if ( SmartConnectionString.IsNullOrEmpty( connectionString ) )
			{
				return connectionString;
			}
			else
			{
				SmartConnectionString clone = connectionString.Clone()
					as SmartConnectionString;

				clone.connectionString =
					DoReplaceConnectionStringTilde(
					clone.connectionString );

				return clone;
			}
		}

		/// <summary>
		/// The connection string allows a tile ("~") as the first character
		/// in path specifications (e.g. inside a connection string for
		/// Microsoft Access MDB databases).
		/// </summary>
		/// <param name="connectionString">The connection string that
		/// (optionally) contains a tilde inside the path.</param>
		/// <returns>
		/// Returns the connection string with an expanded tilde,
		/// or the original connection string if no tilde was found at a
		/// suitable position.
		/// </returns>
		/// <remarks>
		/// This function tries to replace the tilde with path where the
		/// configuration file ("app.config" or "web.config") is located.
		/// </remarks>
		public static string ReplaceConnectionStringTilde(
			string connectionString )
		{
			if ( string.IsNullOrEmpty( connectionString ) )
			{
				return connectionString;
			}
			else
			{
				return DoReplaceConnectionStringTilde(
					connectionString );
			}
		}

		/// <summary>
		/// Helper to actually replace.
		/// Caches to improve speed.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <returns></returns>
		private static string DoReplaceConnectionStringTilde(
			string connectionString )
		{
			if ( string.IsNullOrEmpty( connectionString ) )
			{
				return connectionString;
			}
			else
			{
				string result;
				if ( cacheFor_ReplacedConnectionStringTildes.TryGetValue(
					connectionString,
					out result ) )
				{
					return result;
				}
				else
				{
					string replaceWith;
					if ( string.IsNullOrEmpty(
						LogCentral.Current.ConfigurationFilePath ) )
					{
						replaceWith = string.Empty;
					}
					else
					{
						replaceWith =
							Path.GetDirectoryName(
							LogCentral.Current.ConfigurationFilePath ).
							TrimEnd( '\\' );
					}

					string replaced = Regex.Replace(
						connectionString,
						@"(^|[\s=])~([/\\])",
						string.Format( @"$1{0}$2", replaceWith ),
						RegexOptions.None );

					if ( replaced != connectionString )
					{
						LogCentral.Current.LogDebug(
							string.Format(
							@"Replacing tilde in connection string '{0}' " +
							@"with '{1}', resulting in '{2}'.",
							connectionString,
							replaceWith,
							replaced ) );
					}

					// Also, expand any possibly contained file path macros.
					replaced = LogCentral.ExpandFilePathMacros( replaced );

					cacheFor_ReplacedConnectionStringTildes[connectionString] =
						replaced;
					return replaced;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public SmartConnectionString()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public SmartConnectionString(
			string connectionString )
		{
			// Do NOT call DoReplaceConnectionStringTilde here, to avoid
			// recursion.

			this.connectionString = connectionString;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="other">The other.</param>
		public SmartConnectionString(
			SmartConnectionString other )
		{
			if ( !IsNullOrEmpty( other ) )
			{
				this.connectionString = other.connectionString;
			}
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the 
		/// current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current
		/// <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return connectionString;
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"></see> i
		/// s equal to the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"></see> to 
		/// compare with the current <see cref="T:System.Object"></see>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"></see> is equal 
		/// to the current <see cref="T:System.Object"></see>; otherwise, false.
		/// </returns>
		public override bool Equals(
			object obj )
		{
			return this.Equals( obj as SmartConnectionString );
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override int GetHashCode()
		{
			if ( string.IsNullOrEmpty( connectionString ) )
			{
				return base.GetHashCode();
			}
			else
			{
				return connectionString.GetHashCode();
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
		public bool IsEmpty
		{
			get
			{
				return string.IsNullOrEmpty( connectionString );
			}
		}

		/// <summary>
		/// The underlying connection string.
		/// </summary>
		/// <value>The connection string.</value>
		public string ConnectionString
		{
			get
			{
				if ( string.IsNullOrEmpty( connectionString ) )
				{
					return connectionString;
				}
				else
				{
					// Must do here, since in the constructor, sometimes
					// stack overflow can occur.
					return DoReplaceConnectionStringTilde(
						connectionString );
				}
			}
			set
			{
				connectionString = DoReplaceConnectionStringTilde( value );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private string connectionString;

		/// <summary>
		/// Cache for performance.
		/// </summary>
		private static Dictionary<string, string> cacheFor_ReplacedConnectionStringTildes =
			new Dictionary<string, string>();

		// ------------------------------------------------------------------
		#endregion

		#region ICloneable members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Creates a copy.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		public object Clone()
		{
			return new SmartConnectionString( this );
		}

		// ------------------------------------------------------------------
		#endregion

		#region IComparable members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Compares the current instance with another object of the same type.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the 
		/// objects being compared. The return value has these meanings: 
		/// Value Meaning Less than zero This instance is less than obj.
		/// Zero This instance is equal to obj.
		/// Greater than zero This instance is greater than obj.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">obj is not the same 
		/// type as this instance. </exception>
		public int CompareTo(
			object obj )
		{
			return CompareTo( this as SmartConnectionString );
		}

		// ------------------------------------------------------------------
		#endregion

		#region IComparable<SmartConnectionString> members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the 
		/// objects being compared. The return value has the following meanings: 
		/// Value Meaning Less than zero This object is less than the other parameter.
		/// Zero This object is equal to other. 
		/// Greater than zero This object is greater than other.
		/// </returns>
		public int CompareTo(
			SmartConnectionString other )
		{
			return this.connectionString.CompareTo(
				other.connectionString );
		}

		// ------------------------------------------------------------------
		#endregion

		#region IEquatable<SmartConnectionString> members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the other parameter; otherwise, false.
		/// </returns>
		public bool Equals(
			SmartConnectionString other )
		{
			return this.CompareTo( other ) == 0;
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}