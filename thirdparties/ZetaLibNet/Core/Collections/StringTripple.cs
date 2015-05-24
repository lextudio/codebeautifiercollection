namespace ZetaLib.Core.Collections
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
	using ICSharpCode.SharpZipLib;
	using ICSharpCode.SharpZipLib.Checksums;
	using ICSharpCode.SharpZipLib.Zip;
	using System.Diagnostics;
	using System.Runtime.InteropServices;

	// ------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// A simple pair of strings, used for e.g. name-value pairs inside
	/// combobox values.
	/// </summary>
	[Serializable]
	public class StringTripple
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Initializes a new instance of the <see cref="StringTripple"/> class.
		/// </summary>
		public StringTripple()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StringTripple"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public StringTripple(
			string name )
		{
			Name = name;
			Value = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StringTripple"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		public StringTripple(
			string name,
			string val )
		{
			Name = name;
			Value = val;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StringTripple"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <param name="val2">The val2.</param>
		public StringTripple(
			string name,
			string val,
			string val2 )
		{
			Name = name;
			Value = val;
			Value2 = val2;
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
			return Name;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		/// <summary>
		/// Gets or sets the value2.
		/// </summary>
		/// <value>The value2.</value>
		public string Value2
		{
			get
			{
				return this.value2;
			}
			set
			{
				this.value2 = value;
			}
		}

		/// <summary>
		/// Alias.
		/// </summary>
		/// <value>The first.</value>
		public string First
		{
			get
			{
				return Name;
			}
			set
			{
				Name = value;
			}
		}

		/// <summary>
		/// Alias.
		/// </summary>
		/// <value>The second.</value>
		public string Second
		{
			get
			{
				return Value;
			}
			set
			{
				Value = value;
			}
		}

		/// <summary>
		/// Alias.
		/// </summary>
		/// <value>The third.</value>
		public string Third
		{
			get
			{
				return Value2;
			}
			set
			{
				Value2 = value;
			}
		}

		/// <summary>
		/// Alias.
		/// </summary>
		/// <value>The one.</value>
		public string One
		{
			get
			{
				return Name;
			}
			set
			{
				Name = value;
			}
		}

		/// <summary>
		/// Alias.
		/// </summary>
		/// <value>The two.</value>
		public string Two
		{
			get
			{
				return Value;
			}
			set
			{
				Value = value;
			}
		}

		/// <summary>
		/// Alias.
		/// </summary>
		/// <value>The three.</value>
		public string Three
		{
			get
			{
				return Value2;
			}
			set
			{
				Value2 = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private string name;
		private string value;
		private string value2;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}