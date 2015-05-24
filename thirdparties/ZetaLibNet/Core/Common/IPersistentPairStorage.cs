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
	using System.Runtime.InteropServices;

	// ------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Interface that is implemented by a class that can persist
	/// name-value pairs.
	/// </summary>
	[ComVisible( false )]
	public interface IPersistentPairStorage
	{
		#region Interface members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Persist a value.
		/// </summary>
		/// <param name="name">The name of the value to persist.</param>
		/// <param name="value">The value to persist.</param>
		void PersistValue(
			string name,
			object value );

		/// <summary>
		/// Retrieve a value.
		/// </summary>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <returns>Returns the retrieved value or NULL if not available.</returns>
		object RetrieveValue(
			string name );

		/// <summary>
		/// Retrieve a value.
		/// </summary>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <param name="fallbackValue">The value to return when the retrieved
		/// value is not available.</param>
		/// <returns>Returns the retrieved value or 'fallbackValue' if not available.</returns>
		object RetrieveValue(
			string name,
			object fallbackValue );

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}