namespace ZetaLib.Core.Caching
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Diagnostics;
	using System.Text;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Better than a string, but internally converted to a string.
	/// Use for building cache keys from more complex items.
	/// </summary>
	[DebuggerDisplay( @"Key = {key}" )]
	public class SmartCacheKey :
		IEquatable<SmartCacheKey>
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Build from multiple keys.
		/// </summary>
		/// <param name="keys">The keys.</param>
		/// <returns></returns>
		public static SmartCacheKey Build(
			params object[] keys )
		{
			StringBuilder k = new StringBuilder();

			foreach ( object key in keys )
			{
				if ( k.Length > 0 )
				{
					k.Append( @", " );
				}

				if ( key == null )
				{
					k.Append( @"(null)" );
				}
				else
				{
					k.Append( key.ToString() );
				}
			}

			SmartCacheKey result = new SmartCacheKey( k.ToString() );
			return result;
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
			return key;
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"></see> 
		/// is equal to the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"></see> to compare 
		/// with the current <see cref="T:System.Object"></see>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"></see> is equal 
		/// to the current <see cref="T:System.Object"></see>; otherwise, false.
		/// </returns>
		public override bool Equals(
			object obj )
		{
			return Equals( obj as SmartCacheKey );
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override int GetHashCode()
		{
			return key.GetHashCode();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <value>The key.</value>
		public string Key
		{
			get
			{
				return key;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private string key;

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="key">The key.</param>
		private SmartCacheKey(
			string key )
		{
			this.key = key;
		}

		// ------------------------------------------------------------------
		#endregion

		#region IEquatable<SmartCacheKey> members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Indicates whether the current object is equal to another 
		/// object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the other parameter;
		/// otherwise, false.
		/// </returns>
		public bool Equals(
			SmartCacheKey other )
		{
			return this.key == other.key;
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}