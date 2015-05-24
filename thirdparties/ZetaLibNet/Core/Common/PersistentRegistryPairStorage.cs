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
	/// Implementation of the IPersistentPairStorage interface that stores
	/// into the registry.
	/// </summary>
	public class PersistentRegistryPairStorage :
		PersistentPairStorageBase
	{
		#region IPersistentPairStorage members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Persist a value.
		/// </summary>
		/// <param name="name">The name of the value to persist.</param>
		/// <param name="value">The value to persist.</param>
		public override void PersistValue(
			string name,
			object value )
		{
			RegistryKey key =
				Registry.CurrentUser.OpenSubKey(
				stateSubKeyName,
				true );

			if ( key == null )
			{
				key = Registry.CurrentUser.CreateSubKey( stateSubKeyName );
			}

			key.SetValue( name, value );
		}

		/// <summary>
		/// Retrieve a value.
		/// </summary>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <returns>
		/// Returns the retrieved value or NULL if not available.
		/// </returns>
		public override object RetrieveValue(
			string name )
		{
			return RetrieveValue( name, null );
		}

		/// <summary>
		/// Retrieve a value.
		/// </summary>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <param name="fallbackValue">The value to return when the retrieved
		/// value is not available.</param>
		/// <returns>
		/// Returns the retrieved value or 'fallbackValue' if not available.
		/// </returns>
		public override object RetrieveValue(
			string name,
			object fallbackValue )
		{
			RegistryKey key =
				Registry.CurrentUser.OpenSubKey(
				stateSubKeyName,
				true );

			if ( key == null )
			{
				return fallbackValue;
			}
			else
			{
				object value = key.GetValue( name );

				if ( value == null )
				{
					return fallbackValue;
				}
				else
				{
					return value;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets or sets the application key name.
		/// Use e.g. "MyApplication".
		/// </summary>
		/// <value>The name of the application key.</value>
		public virtual string ApplicationKeyName
		{
			get
			{
				return applicationKeyName;
			}
			set
			{
				applicationKeyName = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private helper.
		// ------------------------------------------------------------------

		/// <summary>
		/// Delay access to the property.
		/// </summary>
		private string _applicationKeyName = null;

		/// <summary>
		/// Gets or sets the name of the application key.
		/// </summary>
		/// <value>The name of the application key.</value>
		private string applicationKeyName
		{
			get
			{
				if ( _applicationKeyName == null )
				{
					_applicationKeyName =
						LibraryConfiguration.Current.ApplicationRegistryKeyName;
				}

				return _applicationKeyName;
			}
			set
			{
				_applicationKeyName = value;
				loggedKey = false;
			}
		}

		/// <summary>
		/// The name of the sub key for storing.
		/// Use relative paths, like "Software\\MyApplication\\MyKey".
		/// </summary>
		/// <value>The name of the state sub key.</value>
		protected virtual string stateSubKeyName
		{
			get
			{
				if ( string.IsNullOrEmpty( applicationKeyName ) )
				{
					throw new ArgumentException(
						Resources.Str_ZetaLib_Core_Common_Storage_01,
						@"ApplicationKeyName" );
				}
				else
				{
					string result =string.Format(
						@"Software\{0}\Settings",
						applicationKeyName );

					if ( !loggedKey )
					{
						loggedKey = true;

						LogCentral.Current.LogDebug(
							string.Format(
							@"Using registry key storage with sub key '{0}'.",
							result ) );
					}

					return result;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private bool loggedKey = false;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}