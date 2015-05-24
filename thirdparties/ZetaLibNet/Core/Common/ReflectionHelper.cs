namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Threading;
	using System.Globalization;
	using System.Reflection;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Helper methods for dealing with reflection.
	/// </summary>
	public sealed class ReflectionHelper
	{
		#region Access attributes.
		// ------------------------------------------------------------------

		/// <summary>
		/// Check whether a certain attribute is associated with an object.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns>Returns TRUE if present, FALSE if not.</returns>
		/// <typeparam name="T">The attribute to check.</typeparam>
		public static bool HasAttribute<T>(
			object o ) where T : Attribute
		{
			return GetAttribute<T>( o, null ) != null;
		}

		/// <summary>
		/// Read a certain attribute that is associated with an object.
		/// </summary>
		/// <param name="o">The object with the possible connected
		/// attribute.</param>
		/// <returns>
		/// Returns the attribute or NULL if not found.
		/// </returns>
		/// <typeparam name="T">The attribute to read.</typeparam>
		public static T GetAttribute<T>(
			object o ) where T : Attribute
		{
			return GetAttribute<T>( o, null );
		}

		/// <summary>
		/// Read a certain attribute that is associated with an object.
		/// </summary>
		/// <param name="o">The object with the possible connected
		/// attribute.</param>
		/// <param name="fallbackValue">The value to use when not
		/// found.</param>
		/// <returns>
		/// Returns the attribute or the fallback value if not
		/// found.
		/// </returns>
		/// <typeparam name="T">The attribute to read.</typeparam>
		public static T GetAttribute<T>(
			object o,
			T fallbackValue ) where T : Attribute
		{
			if ( o == null )
			{
				return fallbackValue;
			}
			else
			{
				if ( o is Enum )
				{
					FieldInfo fi =
						((Enum)o).GetType().GetField( o.ToString() );

					T[] attributes =
						(T[])fi.GetCustomAttributes(
						typeof( T ),
						false );

					if ( attributes.Length > 0 )
					{
						return attributes[0];
					}
					else
					{
						return fallbackValue;
					}
				}
				else
				{
					Attribute[] attributes =
						Attribute.GetCustomAttributes( o.GetType() );

					if ( attributes == null || attributes.Length <= 0 )
					{
						return fallbackValue;
					}
					else
					{
						foreach ( Attribute attribute in attributes )
						{
							if ( attribute is T )
							{
								return attribute as T;
							}
						}

						return fallbackValue;
					}
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}