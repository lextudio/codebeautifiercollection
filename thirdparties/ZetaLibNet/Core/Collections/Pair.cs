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
	/// A simple pair, used for e.g. name-value pairs inside
	/// combobox values.
	/// </summary>
	[Serializable]
	[DebuggerDisplay( @"Name = {name}, Value = {value}" )]
	[ComVisible( false )]
	public class Pair<K, V>
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public Pair()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name.</param>
		public Pair(
			K name )
		{
			Name = name;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		public Pair(
			K name,
			V val )
		{
			Name = name;
			Value = val;
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
			if ( Name == null )
			{
				return null;
			}
			else
			{
				return Name.ToString();
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Alias.
		/// </summary>
		/// <value>The one.</value>
		public K One
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
		public V Two
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
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public K Name
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
		public V Value
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
		/// Gets or sets the first.
		/// </summary>
		/// <value>The first.</value>
		public K First
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
		/// Gets or sets the second.
		/// </summary>
		/// <value>The second.</value>
		public V Second
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

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Does the compare.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		private int DoCompare(
			object a,
			object b )
		{
			IComparable x = a as IComparable;
			IComparable y = b as IComparable;

			if ( x == null && y == null )
			{
				return 0;
			}
			else if ( x != null )
			{
				return x.CompareTo( y );
			}
			else if ( y != null )
			{
				return -y.CompareTo( x );
			}
			else
			{
				return 0;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private K name;
		private V value;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// A simple pair, used for e.g. name-value pairs inside
	/// combobox values.
	/// </summary>
	[Serializable]
	[ComVisible( false )]
	public class Pair :
		Pair<object, object>
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Initializes a new instance of the <see cref="Pair"/> class.
		/// </summary>
		public Pair()
			:
			base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Pair"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public Pair(
			object name )
			:
			base( name )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Pair"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		public Pair(
			object name,
			object val )
			:
			base( name, val )
		{
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}