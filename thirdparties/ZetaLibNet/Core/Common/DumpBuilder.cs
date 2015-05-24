namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Configuration;
	using System.Collections;
	using System.Diagnostics;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using ZetaLib.Core.Common;
	using System.Data;
	using ZetaLib.Core.Data;
	using System.Data.OleDb;
	using ZetaLib.Core.Properties;
	using System.Reflection;
	using System.ComponentModel;
	using System.Collections.Generic;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Class that helps in dumping.
	/// </summary>
	public sealed class DumpBuilder
	{
		#region Public constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public DumpBuilder()
			:
			this( 0, false, null )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="indentLevel">The indent level.</param>
		public DumpBuilder(
			int indentLevel )
			:
			this( indentLevel, false, null )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="indentLevel">The indent level.</param>
		/// <param name="deep">if set to <c>true</c> [deep].</param>
		public DumpBuilder(
			int indentLevel,
			bool deep )
			:
			this( indentLevel, deep, null )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="indentLevel">The indent level.</param>
		/// <param name="deep">if set to <c>true</c> [deep].</param>
		/// <param name="typeToDump">The type to dump.</param>
		public DumpBuilder(
			int indentLevel,
			bool deep,
			Type typeToDump )
		{
			this.indentLevel = indentLevel;
			this.deep = deep;

			if ( typeToDump != null )
			{
				string s = string.Format(
					@"Dumping for '{0}':",
					typeToDump.FullName );

				lines.Add( s );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Add a line to dump.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void AddLine(
			string name,
			object value )
		{
			lines.Add( MakeStringToAdd( name, value ) );
		}

		/// <summary>
		/// Add a line to dump.
		/// </summary>
		/// <param name="text">The text.</param>
		public void AddLine(
			string text )
		{
			lines.Add( text );
		}

		/// <summary>
		/// Insert a line to dump.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void InsertLine(
			int index,
			string name,
			object value )
		{
			lines.Insert( index, MakeStringToAdd( name, value ) );
		}

		/// <summary>
		/// Insert a line to dump.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="text">The text.</param>
		public void InsertLine(
			int index,
			string text )
		{
			lines.Insert( index, text );
		}

		/// <summary>
		/// Get the dumped content.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			foreach ( string line in lines )
			{
				if ( !string.IsNullOrEmpty( line ) )
				{
					sb.AppendLine( DoIndent( line.TrimEnd() ) );
				}
			}

			return sb.ToString().TrimEnd();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets a value indicating whether this instance is deep.
		/// </summary>
		/// <value><c>true</c> if this instance is deep; otherwise, <c>false</c>.</value>
		public bool IsDeep
		{
			get
			{
				return deep;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Static methods for dumping predefined items.
		// ------------------------------------------------------------------

		/// <summary>
		/// Dumps a data row.
		/// </summary>
		/// <param name="row">The row to dump.</param>
		/// <returns>Returns the string with the dump.</returns>
		public static string Dump(
			DataRow row )
		{
			if ( row == null )
			{
				return @"(null)";
			}
			else
			{
				return AdoNetBaseHelper<
					OleDbCommand,
					OleDbCommandBuilder,
					OleDbConnection,
					OleDbDataAdapter,
					OleDbParameter,
					AdoNetOleDBParamCollection>.
					DumpDataRow( row );
			}
		}

		/// <summary>
		/// Dumps a data row.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns>Returns the string with the dump.</returns>
		public static string Dump(
			DataTable table )
		{
			if ( table == null )
			{
				return @"(null)";
			}
			else
			{
				return AdoNetBaseHelper<
					OleDbCommand,
					OleDbCommandBuilder,
					OleDbConnection,
					OleDbDataAdapter,
					OleDbParameter,
					AdoNetOleDBParamCollection>.
					DumpDataTable( table );
			}
		}

		/// <summary>
		/// Dumps an exception.
		/// </summary>
		/// <param name="x">The exception to dump.</param>
		/// <returns>Returns the string with the dump.</returns>
		/// <remarks>Thanks to J. Dunlap for the code.</remarks>
		public static string Dump(
			Exception x )
		{
			StringBuilder sb = new StringBuilder();
			Reflect( sb, x );

			return sb.ToString();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Reflection output.
		// ------------------------------------------------------------------

		/// <summary>
		/// Reflects the specified sb.
		/// </summary>
		/// <param name="sb">The sb.</param>
		/// <param name="obj">The obj.</param>
		private static void Reflect(
			StringBuilder sb,
			object obj )
		{
			Reflect( sb, new GraphRef( null, obj, null ), 0 );
		}

		/// <summary>
		/// Reflects the specified sb.
		/// </summary>
		/// <param name="sb">The sb.</param>
		/// <param name="obj">The obj.</param>
		/// <param name="indent">The indent.</param>
		private static void Reflect(
			StringBuilder sb,
			GraphRef obj,
			int indent )
		{
			const int maxDepth = 3;

			//Ensure that we are not following a circular reference path
			if ( !(obj.Value is ValueType) )
			{
				GraphRef parentRef = obj.Parent;
				while ( parentRef != null )
				{
					if ( parentRef.Value == obj.Value )
						return;
					parentRef = parentRef.Parent;
				}
			}

			sb.Append( '\t', indent );

			//Output property name if applicable
			if ( !String.IsNullOrEmpty( obj.PropName ) )
			{
				sb.Append( obj.PropName );
				sb.Append( "=" );
			}

			int childIndent = indent + 1;

			//If value is null, output "null"
			if ( obj.Value == null )
			{
				sb.Append( "null" );
			}
			//If value is a string, output value with quotes around it
			else if ( obj.Value is string )
			{
				sb.Append( "\"" + Escape( (string)obj.Value ) + "\"" );
			}
			//If value is a char, output value with single quotes around it
			else if ( obj.Value is char )
			{
				sb.Append( "\'" + Escape( new String( (char)obj.Value, 1 ) ) + "\'" );
			}
			//If value is an array, output each array element
			else if ( obj.Value is Array )
			{
				Array arr = (Array)obj.Value;
				sb.Append( "\r\n" );
				sb.Append( '\t', indent );
				sb.Append( "[\r\n" );
				for ( int i = 0; i < arr.Length; i++ )
				{
					Reflect( sb, new GraphRef( obj, arr.GetValue( i ), null ), childIndent );
					if ( i < arr.Length - 1 )
						sb.Append( ',' );
					sb.Append( "\r\n" );
				}
				sb.Append( '\t', indent );
				sb.Append( "]\r\n" );
			}
			//If it's a Type object, we don't want to endlessly follow long trains of 
			//interconnected type info objects
			else if ( obj.Value is Type )
			{
				sb.Append( "Type: " );
				sb.Append( ((Type)obj.Value).FullName );
			}
			//...and similarly for MemberInfo objects
			else if ( obj.Value is MemberInfo )
			{
				sb.Append( obj.Value.GetType().Name );
				sb.Append( ": " );
				sb.Append( ((MemberInfo)obj.Value).Name );
			}
			//If value is not of a basic datatype
			else if ( Convert.GetTypeCode( obj.Value ) == TypeCode.Object )
			{

				Type type = obj.Value.GetType();
				sb.Append( type.Name ); //might want to use type.FullName instead
				if ( indent <= maxDepth )
				{
					sb.Append( "\r\n" );
					sb.Append( '\t', indent );
					sb.Append( "{\r\n" );
					//Get all the properties in the object's type
					PropertyInfo[] props = type.GetProperties( BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy );
					//Enumerate all the properties and output their values
					for ( int i = 0; i < props.Length; i++ )
					{
						PropertyInfo pi = props[i];
						if ( pi.GetIndexParameters().Length == 0 ) //Ignore indexers
						{
							try
							{
								Reflect( sb, new GraphRef( obj, pi.GetValue( obj.Value, null ), pi.Name ), childIndent );
							}
							catch ( Exception e )
							{
								sb.Append( "<Error getting property value (" );
								sb.Append( e.GetType().Name );
								sb.Append( ")>" );
							}
							if ( i < props.Length - 1 )
								sb.Append( ',' );
							sb.Append( "\r\n" );
						}
					}
					//If IList, output all the values in the list
					if ( obj is IList )
					{
						IList list = (IList)obj.Value;
						sb.Append( "\r\n" );
						sb.Append( '\t', indent );
						sb.Append( "[\r\n" );
						for ( int i = 0; i < list.Count; i++ )
						{
							Reflect( sb, new GraphRef( obj, list[i], null ), childIndent );
							if ( i < list.Count - 1 )
								sb.Append( ',' );
							sb.Append( "\r\n" );
						}
						sb.Append( '\t', indent );
						sb.Append( "]\r\n" );
					}
					sb.Append( '\t', indent );
					sb.Append( "}" );
				}
			}
			//If value is of a basic datatype
			else
			{
				sb.Append( obj.Value.ToString() );
			}

		}

		// ------------------------------------------------------------------
		#endregion

		#region Escaping.
		// ------------------------------------------------------------------

		static string[] escapeChars = new string[] { "\r", "\n", "\t", "\"", "\'", "\\" };
		static string[] escapeCharReplacements = new string[]
		{
			"\\r","\\n","\\t","\\\"","\\\'","\\\\"
		};

		/// <summary>
		/// Escapes characters in a string using the escaping system used 
		/// in C# string literals
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		static string Escape( string input )
		{
			StringBuilder sb = new StringBuilder( input );
			for ( int i = 0; i < escapeChars.Length; i++ )
				sb.Replace( escapeChars[i], escapeCharReplacements[i] );
			return sb.ToString();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private helper class for reflecting.
		// ------------------------------------------------------------------

		/// <summary>
		/// Represents a reference within an object graph.
		/// </summary>
		private class GraphRef
		{
			#region Public methods.

			/// <summary>
			/// Initializes a new instance of the <see cref="GraphRef"/> class.
			/// </summary>
			/// <param name="parent">The parent.</param>
			/// <param name="obj">The obj.</param>
			/// <param name="propName">Name of the prop.</param>
			public GraphRef(
				GraphRef parent,
				object obj, 
				string propName )
			{
				this.parent = parent;
				this.value = obj;
				this.propName = propName;
			}

			#endregion

			#region Public properties.

			/// <summary>
			/// Gets the value.
			/// </summary>
			/// <value>The value.</value>
			public object Value
			{
				get
				{
					return value;
				}
			}

			/// <summary>
			/// Gets the parent.
			/// </summary>
			/// <value>The parent.</value>
			public GraphRef Parent
			{
				get
				{
					return parent;
				}
			}

			/// <summary>
			/// Gets the name of the prop.
			/// </summary>
			/// <value>The name of the prop.</value>
			public string PropName
			{
				get
				{
					return propName;
				}
			}

			#endregion

			#region Private variables.

			private object value;
			private GraphRef parent;
			private string propName;

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Makes the string to add.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		private static string MakeStringToAdd(
			string name,
			object value )
		{
			if ( name == null )
			{
				throw new ArgumentNullException(
					Resources.Str_ZetaLib_Core_Common_Dump_01,
					@"name" );
			}
			else if ( name.Length <= 0 )
			{
				throw new ArgumentException(
					Resources.Str_ZetaLib_Core_Common_Dump_02,
					@"name" );
			}
			else
			{
				string result = string.Empty;

				result += string.Format(
					@"{0}: '{1}'",
					name,
					value == null ? @"(null)" : value.ToString() );

				return result;
			}
		}

		/// <summary>
		/// Does the indent.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		private string DoIndent(
			string text )
		{
			StringBuilder result = new StringBuilder();

			result.Append( '\t', indentLevel );
			result.Append( text.TrimEnd() );

			return result.ToString();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private List<string> lines = new List<string>();

		private readonly int indentLevel = 0;
		private readonly bool deep = false;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}