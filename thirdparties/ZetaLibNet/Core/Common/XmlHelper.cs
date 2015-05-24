namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Globalization;
	using System.Xml;
	using System.IO;
	using System.Text;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Helper routines for reading and manipulating XML documents.
	/// </summary>
	public sealed class XmlHelper
	{
		#region Miscellaneous routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Creates a new document with the appropriate processing
		/// instructions.
		/// </summary>
		/// <returns></returns>
		public static XmlDocument CreateDocument()
		{
			return CreateDocument( @"unicode" );
		}

		/// <summary>
		/// Creates a new document with the appropriate processing
		/// instructions.
		/// </summary>
		/// <param name="encoding">The encoding.</param>
		/// <returns></returns>
		public static XmlDocument CreateDocument(
			string encoding )
		{
			XmlDocument doc = new XmlDocument();

			doc.AppendChild(
				doc.CreateProcessingInstruction(
				@"xml",
				string.Format(
				@"version='1.0' encoding='{0}'",
				encoding ) ) );

			return doc;
		}

		/// <summary>
		/// Creates a new document with the appropriate processing
		/// instructions.
		/// </summary>
		/// <param name="encoding">The encoding.</param>
		/// <returns></returns>
		public static XmlDocument CreateDocument(
			Encoding encoding )
		{
			return CreateDocument( encoding.BodyName );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Reading attributes.
		// ------------------------------------------------------------------

		/// <summary>
		/// Read a string.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		public static void ReadAttribute(
			out string result,
			XmlAttribute attribute )
		{
			ReadAttribute(
				out result,
				attribute,
				null );
		}

		/// <summary>
		/// Read a string.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		/// <param name="defaultResult">The default result.</param>
		public static void ReadAttribute(
			out string result,
			XmlAttribute attribute,
			string defaultResult )
		{
			if ( attribute == null )
			{
				result = defaultResult;
			}
			else
			{
				result = attribute.Value;
			}
		}

		/// <summary>
		/// Read a DirectoryInfo.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		public static void ReadAttribute(
			out DirectoryInfo result,
			XmlAttribute attribute )
		{
			ReadAttribute(
				out result,
				attribute,
				null );
		}

		/// <summary>
		/// Read a DirectoryInfo.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		/// <param name="defaultResult">The default result.</param>
		public static void ReadAttribute(
			out DirectoryInfo result,
			XmlAttribute attribute,
			DirectoryInfo defaultResult )
		{
			if ( attribute == null )
			{
				result = defaultResult;
			}
			else
			{
				result = new DirectoryInfo( attribute.Value );
			}
		}

		/// <summary>
		/// Read a T.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		public static void ReadAttribute<T>(
			out T result,
			XmlAttribute attribute )
		{
			ReadAttribute<T>(
				out result,
				attribute,
				default( T ) );
		}

		/// <summary>
		/// Read a T.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		/// <param name="defaultResult">The default result.</param>
		public static void ReadAttribute<T>(
			out T result,
			XmlAttribute attribute,
			T defaultResult )
		{
			if ( attribute == null )
			{
				result = defaultResult;
			}
			else
			{
				result = ConvertHelper.ToT<T>( attribute.Value, defaultResult );
			}
		}

		/// <summary>
		/// Read a FileInfo.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		public static void ReadAttribute(
			out FileInfo result,
			XmlAttribute attribute )
		{
			ReadAttribute(
				out result,
				attribute,
				null );
		}

		/// <summary>
		/// Read a FileInfo.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		/// <param name="defaultResult">The default result.</param>
		public static void ReadAttribute(
			out FileInfo result,
			XmlAttribute attribute,
			FileInfo defaultResult )
		{
			if ( attribute == null )
			{
				result = defaultResult;
			}
			else
			{
				result = new FileInfo( attribute.Value );
			}
		}

		/// <summary>
		/// Read a GUID.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		public static void ReadAttribute(
			out Guid result,
			XmlAttribute attribute )
		{
			ReadAttribute(
				out result,
				attribute,
				Guid.Empty );
		}

		/// <summary>
		/// Read a Guid.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		/// <param name="defaultResult">The default result.</param>
		public static void ReadAttribute(
			out Guid result,
			XmlAttribute attribute,
			Guid defaultResult )
		{
			if ( attribute == null )
			{
				result = defaultResult;
			}
			else
			{
				result = new Guid( attribute.Value );
			}
		}

		/// <summary>
		/// Read an double.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		public static void ReadAttribute(
			out double result,
			XmlAttribute attribute )
		{
			ReadAttribute(
				out result,
				attribute,
				0 );
		}

		/// <summary>
		/// Read an double.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		/// <param name="defaultResult">The default result.</param>
		public static void ReadAttribute(
			out double result,
			XmlAttribute attribute,
			double defaultResult )
		{
			if ( attribute == null || attribute.Value == null )
			{
				result = defaultResult;
			}
			else
			{
				try
				{
					result = double.Parse( attribute.Value );
				}
				catch ( FormatException )
				{
					result = defaultResult;
				}
				catch ( OverflowException )
				{
					result = defaultResult;
				}
			}
		}

		/// <summary>
		/// Read an integer.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		public static void ReadAttribute(
			out int result,
			XmlAttribute attribute )
		{
			ReadAttribute(
				out result,
				attribute,
				0 );
		}

		/// <summary>
		/// Read an integer.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		/// <param name="defaultResult">The default result.</param>
		public static void ReadAttribute(
			out int result,
			XmlAttribute attribute,
			int defaultResult )
		{
			if ( attribute == null || attribute.Value == null )
			{
				result = defaultResult;
			}
			else
			{
				try
				{
					result = int.Parse( attribute.Value );
				}
				catch ( FormatException )
				{
					result = defaultResult;
				}
				catch ( OverflowException )
				{
					result = defaultResult;
				}
			}
		}

		/// <summary>
		/// Read an DateTime.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		public static void ReadAttribute(
			out DateTime result,
			XmlAttribute attribute )
		{
			ReadAttribute(
				out result,
				attribute,
				DateTime.MinValue );
		}

		/// <summary>
		/// Read an DateTime.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		/// <param name="defaultResult">The default result.</param>
		public static void ReadAttribute(
			out DateTime result,
			XmlAttribute attribute,
			DateTime defaultResult )
		{
			if ( attribute == null || attribute.Value == null )
			{
				result = defaultResult;
			}
			else
			{
				try
				{
					result = DateTime.Parse( attribute.Value );
				}
				catch ( FormatException )
				{
					result = defaultResult;
				}
				catch ( OverflowException )
				{
					result = defaultResult;
				}
			}
		}

		/// <summary>
		/// Read a boolean.
		/// </summary>
		/// <param name="result">if set to <c>true</c> [result].</param>
		/// <param name="attribute">The attribute.</param>
		public static void ReadAttribute(
			out bool result,
			XmlAttribute attribute )
		{
			ReadAttribute(
				out result,
				attribute,
				false );
		}

		/// <summary>
		/// Read a boolean.
		/// </summary>
		/// <param name="result">if set to <c>true</c> [result].</param>
		/// <param name="attribute">The attribute.</param>
		/// <param name="defaultResult">if set to <c>true</c> [default result].</param>
		public static void ReadAttribute(
			out bool result,
			XmlAttribute attribute,
			bool defaultResult )
		{
			if ( attribute == null || attribute.Value == null )
			{
				result = defaultResult;
			}
			else
			{
				if ( ConvertHelper.IsBoolean( attribute.Value ) )
				{
					result = ConvertHelper.ToBoolean(
						attribute.Value,
						defaultResult );
				}
				else
				{
					if ( attribute.Value == null )
					{
						result = defaultResult;
					}
					else
					{
						if ( attribute.Value == @"0" )
						{
							result = false;
						}
						else if ( attribute.Value == @"-1" || 
							attribute.Value == @"1" )
						{
							result = true;
						}
						else
						{
							result = defaultResult;
						}
					}
				}
			}
		}

		/// <summary>
		/// Read a decimal.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		public static void ReadAttribute(
			out decimal result,
			XmlAttribute attribute )
		{
			ReadAttribute(
				out result,
				attribute,
				decimal.Zero );
		}

		/// <summary>
		/// Read a decimal.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="attribute">The attribute.</param>
		/// <param name="defaultResult">The default result.</param>
		public static void ReadAttribute(
			out decimal result,
			XmlAttribute attribute,
			decimal defaultResult )
		{
			if ( attribute == null || attribute.Value == null )
			{
				result = defaultResult;
			}
			else
			{
				try
				{
					result = decimal.Parse(
						attribute.Value,
						CultureInfo.InvariantCulture );
				}
				catch ( FormatException )
				{
					result = defaultResult;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Reading nodes.
		// ------------------------------------------------------------------

		/// <summary>
		/// Read a string.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="node">The node.</param>
		public static void ReadNode(
			out string result,
			XmlNode node )
		{
			ReadNode(
				out result,
				node,
				null );
		}

		/// <summary>
		/// Read a string.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="node">The node.</param>
		/// <param name="defaultResult">The default result.</param>
		public static void ReadNode(
			out string result,
			XmlNode node,
			string defaultResult )
		{
			if ( node == null )
			{
				result = defaultResult;
			}
			else
			{
				result = node.InnerText;
			}
		}

		/// <summary>
		/// Read an integer.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="node">The node.</param>
		public static void ReadNode(
			out int result,
			XmlNode node )
		{
			ReadNode(
				out result,
				node,
				0 );
		}

		/// <summary>
		/// Read an integer.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="node">The node.</param>
		/// <param name="defaultResult">The default result.</param>
		public static void ReadNode(
			out int result,
			XmlNode node,
			int defaultResult )
		{
			if ( node == null || node.InnerText == null )
			{
				result = defaultResult;
			}
			else
			{
				try
				{
					result = int.Parse( node.InnerText );
				}
				catch ( FormatException )
				{
					result = defaultResult;
				}
				catch ( OverflowException )
				{
					result = defaultResult;
				}
			}
		}

		/// <summary>
		/// Read a boolean.
		/// </summary>
		/// <param name="result">if set to <c>true</c> [result].</param>
		/// <param name="node">The node.</param>
		public static void ReadNode(
			out bool result,
			XmlNode node )
		{
			ReadNode(
				out result,
				node,
				false );
		}

		/// <summary>
		/// Read a boolean.
		/// </summary>
		/// <param name="result">if set to <c>true</c> [result].</param>
		/// <param name="node">The node.</param>
		/// <param name="defaultResult">if set to <c>true</c> [default result].</param>
		public static void ReadNode(
			out bool result,
			XmlNode node,
			bool defaultResult )
		{
			if ( node == null || node.InnerText == null )
			{
				result = defaultResult;
			}
			else
			{
				try
				{
					result = bool.Parse( node.InnerText );
				}
				catch ( FormatException )
				{
					if ( node.InnerText == @"0" )
					{
						result = false;
					}
					else if ( node.InnerText == @"-1" || 
						node.InnerText == @"1" )
					{
						result = true;
					}
					else
					{
						result = defaultResult;
					}
				}
			}
		}

		/// <summary>
		/// Read a decimal.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="node">The node.</param>
		public static void ReadNode(
			out decimal result,
			XmlNode node )
		{
			ReadNode(
				out result,
				node,
				decimal.Zero );
		}

		/// <summary>
		/// Read a decimal.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="node">The node.</param>
		/// <param name="defaultResult">The default result.</param>
		public static void ReadNode(
			out decimal result,
			XmlNode node,
			decimal defaultResult )
		{
			if ( node == null || node.InnerText == null )
			{
				result = defaultResult;
			}
			else
			{
				try
				{
					result = decimal.Parse(
						node.InnerText,
						CultureInfo.InvariantCulture );
				}
				catch ( FormatException )
				{
					result = defaultResult;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Never instantiate this class.
		/// </summary>
		public XmlHelper()
		{
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}