namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Data;
	using System.Data.SqlClient;
	using System.Data.OleDb;
	using System.Data.Odbc;
	using System.Data.OracleClient;
	using System.Diagnostics;
	using System.Text;
	using System.Text.RegularExpressions;
	using ZetaLib.Core.Common;
	using System.Data.Common;
	using System.Collections.Generic;
	using ZetaLib.Core.Logging;
	using ZetaLib.Core.Properties;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Base class for parameter collection classes.
	/// </summary>
	public class AdoNetBaseParamCollection<P> :
		ICollection<P>,
		IEnumerable<P>,
		IList<P>,
		ICollection,
		IEnumerable,
		IList
		where P : DbParameter, new()
	{
		#region Static methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="dataType">Type of the data.</param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			DbType dataType )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = value;
			p.DbType = dataType;
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="dataType">Type of the data.</param>
		/// <param name="nullBehaviour">The null behaviour.</param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			DbType dataType,
			NullBehaviour nullBehaviour )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = ApplyNullBehaviourToValue( value, nullBehaviour );
			p.DbType = dataType;
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = value;
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="nullBehaviour">The null behaviour.</param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			NullBehaviour nullBehaviour )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = ApplyNullBehaviourToValue( value, nullBehaviour );
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="dataType">Type of the data.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			DbType dataType,
			int size )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = value;
			p.DbType = dataType;
			p.Size = size;
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="dataType">Type of the data.</param>
		/// <param name="size">The size.</param>
		/// <param name="nullBehaviour">The null behaviour.</param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			DbType dataType,
			int size,
			NullBehaviour nullBehaviour )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = ApplyNullBehaviourToValue( value, nullBehaviour );
			p.DbType = dataType;
			p.Size = size;
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			int size )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = value;
			p.Size = size;
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="size">The size.</param>
		/// <param name="nullBehaviour">The null behaviour.</param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			int size,
			NullBehaviour nullBehaviour )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = ApplyNullBehaviourToValue( value, nullBehaviour );
			p.Size = size;
			return p;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public AdoNetBaseParamCollection()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		public AdoNetBaseParamCollection(
			params P[] parameters )
		{
			if ( parameters != null )
			{
				foreach ( P parameter in parameters )
				{
					Add( parameter );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region ICollection member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe).
		/// </summary>
		/// <value></value>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe); otherwise, false.</returns>
		public bool IsSynchronized
		{
			get
			{
				return (parameterList as ICollection).IsSynchronized;
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		public int Count
		{
			get
			{
				return parameterList.Count;
			}
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">array is null. </exception>
		/// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"></see> cannot be cast automatically to the type of the destination array. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
		public void CopyTo(
			Array array,
			int index )
		{
			(parameterList as ICollection).CopyTo( array, index );
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.</returns>
		public object SyncRoot
		{
			get
			{
				return (parameterList as ICollection).SyncRoot;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region IEnumerable member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator GetEnumerator()
		{
			return parameterList.GetEnumerator();
		}

		// ------------------------------------------------------------------
		#endregion

		#region IList member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get
			{
				return (parameterList as IList).IsReadOnly;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Object"/> at the specified index.
		/// </summary>
		/// <value></value>
		public object this[int index]
		{
			get
			{
				return parameterList[index];
			}
			set
			{
				parameterList[index] = value as P;
			}
		}

		/// <summary>
		/// Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		public void RemoveAt( int index )
		{
			parameterList.RemoveAt( index );
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.IList"></see> at the specified index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value">The <see cref="T:System.Object"></see> to insert into the <see cref="T:System.Collections.IList"></see>.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
		/// <exception cref="T:System.NullReferenceException">value is null reference in the <see cref="T:System.Collections.IList"></see>.</exception>
		public void Insert(
			int index,
			object value )
		{
			(parameterList as IList).Insert( index, value );
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"></see>.
		/// </summary>
		/// <param name="value">The <see cref="T:System.Object"></see> to remove from the <see cref="T:System.Collections.IList"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
		public void Remove( 
			object value )
		{
			(parameterList as IList).Remove( value );
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.IList"></see> contains a specific value.
		/// </summary>
		/// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
		/// <returns>
		/// true if the <see cref="T:System.Object"></see> is found in the <see cref="T:System.Collections.IList"></see>; otherwise, false.
		/// </returns>
		public bool Contains(
			object value )
		{
			return (parameterList as IList).Contains( value );
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
		public void Clear()
		{
			parameterList.Clear();
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.IList"></see>.
		/// </summary>
		/// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
		/// <returns>
		/// The index of value if found in the list; otherwise, -1.
		/// </returns>
		public int IndexOf(
			object value )
		{
			return (parameterList as IList).IndexOf( value );
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.IList"></see>.
		/// </summary>
		/// <param name="value">The <see cref="T:System.Object"></see> to add to the <see cref="T:System.Collections.IList"></see>.</param>
		/// <returns>
		/// The position into which the new element was inserted.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
		public int Add(
			object value )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( @"value" );
			}
			else if ( !(value is P) )
			{
				throw new ArgumentException(
					Resources.Str_ZetaLib_Core_Data_AdoNetBaseHelper_01,
					@"value" );
			}
			else
			{
				return (parameterList as IList).Add( value );
			}
		}

		/// <summary>
		/// Adds the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public int Add(
			P value )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( @"value" );
			}
			else
			{
				return (parameterList as IList).Add( value );
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.IList"></see> has a fixed size.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.IList"></see> has a fixed size; otherwise, false.</returns>
		public bool IsFixedSize
		{
			get
			{
				return (parameterList as IList).IsFixedSize;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region ICollection<P> members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		void ICollection<P>.Add( 
			P item )
		{
			parameterList.Add( item );
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>
		/// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
		/// </returns>
		public bool Contains(
			P item )
		{
			return parameterList.Contains( item );
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">array is null.</exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
		public void CopyTo( 
			P[] array, 
			int arrayIndex )
		{
			parameterList.CopyTo( array, arrayIndex );
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>
		/// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public bool Remove(
			P item )
		{
			return parameterList.Remove( item );
		}

		// ------------------------------------------------------------------
		#endregion

		#region IEnumerable<P> members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		IEnumerator<P> IEnumerable<P>.GetEnumerator()
		{
			return parameterList.GetEnumerator();
		}

		// ------------------------------------------------------------------
		#endregion

		#region IList<P> members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		/// <returns>
		/// The index of item if found in the list; otherwise, -1.
		/// </returns>
		/// 
		public int IndexOf( P item )
		{
			return parameterList.IndexOf( item );
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		public void Insert( 
			int index, 
			P item )
		{
			Insert( index, item );
		}

		/// <summary>
		/// Gets or sets the <see cref="P"/> at the specified index.
		/// </summary>
		/// <value></value>
		P IList<P>.this[int index]
		{
			get
			{
				return parameterList[index];
			}
			set
			{
				parameterList[index] = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns a modified version of the given value depending on
		/// the NullBehaviour.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="nullBehaviour">The null behaviour.</param>
		/// <returns></returns>
		public static object ApplyNullBehaviourToValue(
			object value,
			NullBehaviour nullBehaviour )
		{
			if ( value == null )
			{
				if ( nullBehaviour == NullBehaviour.ConvertNullToDBNull )
				{
					return DBNull.Value;
				}
				else if ( nullBehaviour == NullBehaviour.ConvertNullToEmptyString )
				{
					return string.Empty;
				}
				else
				{
					return value;
				}
			}
			else if ( (value is string) && value.ToString().Length <= 0 )
			{
				if ( nullBehaviour == NullBehaviour.ConvertEmptyStringToDBNull )
				{
					return DBNull.Value;
				}
				else
				{
					return value;
				}
			}
			else if ( ((value is Int16) || (value is Int32) || (value is Int64)) &&
			   Convert.ToInt32( value ) == 0 )
			{
				if ( nullBehaviour == NullBehaviour.ConvertZeroIntToDBNull )
				{
					return DBNull.Value;
				}
				else
				{
					return value;
				}
			}
			else
			{
				return value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private List<P> parameterList = new List<P>();

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}