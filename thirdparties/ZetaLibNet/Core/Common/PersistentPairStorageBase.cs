namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Data;
	using System.IO;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Text;
	using System.Xml;
	using Microsoft.Win32;
	using ZetaLib.Core.Properties;
	using ZetaLib.Core.Logging;

	// ------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Base class which provide some more methods to the 
	/// IPersistentPairStorage interface.
	/// </summary>
	public abstract class PersistentPairStorageBase :
		IPersistentPairStorage
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Serialize a value and persist it.
		/// The opposite to "DeserializeValue".
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		public void SerializeValue(
			string name,
			object val )
		{
			if ( val == null )
			{
				PersistValue( name, null );
			}
			else
			{
				PersistValue( name, StringHelper.SerializeToString( val ) );
			}
		}

		/// <summary>
		/// Deserializes a value from persistent storage.
		/// The opposite to "SerializeValue".
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>Returns NULL if not found.</returns>
		public object DeserializeValue(
			string name )
		{
			object o = RetrieveValue( name );

			if ( o == null || !(o is string) )
			{
				return null;
			}
			else
			{
				return StringHelper.DeserializeFromString( o.ToString() );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region IPersistentPairStorage members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Persist a value.
		/// </summary>
		/// <param name="name">The name of the value to persist.</param>
		/// <param name="value">The value to persist.</param>
		public abstract void PersistValue(
			string name,
			object value );

		/// <summary>
		/// Retrieve a value.
		/// </summary>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <returns>
		/// Returns the retrieved value or NULL if not available.
		/// </returns>
		public abstract object RetrieveValue(
			string name );

		/// <summary>
		/// Retrieve a value.
		/// </summary>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <param name="fallbackValue">The value to return when the retrieved
		/// value is not available.</param>
		/// <returns>
		/// Returns the retrieved value or 'fallbackValue' if not available.
		/// </returns>
		public abstract object RetrieveValue(
			string name,
			object fallbackValue );

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}