namespace ZetaLib.Core.Collections
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Collections;
	using System.Diagnostics;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// A generic Set class.
	/// </summary>
	/// <remarks>
	/// See source from http://www.codeproject.com/csharp/genericset.asp.
	/// Enhanced by Uwe Keim.
	/// </remarks>
	/// <typeparam name="T">The type of the set.</typeparam>
	[Serializable]
	[DebuggerDisplay( @"Count = {Count}" )]
	public class Set<T> :
		ICollection<T>,
		IEnumerable<T>,
		ICollection,
		IEnumerable
	{
		#region Private helper dummy class.
		// ------------------------------------------------------------------

		/// <summary>
		/// Dummy structure serving as the value in the underlying
		/// dictionary object.
		/// </summary>
		[DebuggerDisplay( @"" )]
		private struct Dummy
		{
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private static Dummy dummy = new Dummy();

		/// <summary>
		/// The actual data storage.
		/// </summary>
		private Dictionary<T, Dummy> data;

		// ------------------------------------------------------------------
		#endregion

		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Initializes a new instance of the <see cref="Set&lt;T&gt;"/> class.
		/// </summary>
		public Set()
		{
			data = new Dictionary<T, Dummy>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Set&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public Set(
			int capacity )
		{
			data = new Dictionary<T, Dummy>( capacity );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Set&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="original">The original.</param>
		public Set(
			Set<T> original )
		{
			if ( original == null )
			{
				data = new Dictionary<T, Dummy>();
			}
			else
			{
				data = new Dictionary<T, Dummy>( original.data );
			}
		}

		/// <summary>
		/// Added 2006-05-25, Uwe Keim.
		/// </summary>
		public void Sort()
		{
			List<T> temp = new List<T>( this );

			temp.Sort();

			this.Clear();
			this.AddRange( temp );
		}

		/// <summary>
		/// Added 2006-05-25, Uwe Keim.
		/// </summary>
		/// <param name="comparison">The comparison.</param>
		public void Sort(
			Comparison<T> comparison )
		{
			List<T> temp = new List<T>( this );

			temp.Sort( comparison );

			this.Clear();
			this.AddRange( temp );
		}

		/// <summary>
		/// Added 2006-05-25, Uwe Keim.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		public void Sort(
			IComparer<T> comparer )
		{
			List<T> temp = new List<T>( this );

			temp.Sort( comparer );

			this.Clear();
			this.AddRange( temp );
		}

		/// <summary>
		/// Added 2006-05-25, Uwe Keim.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		/// <param name="comparer">The comparer.</param>
		public void Sort(
			int index,
			int count,
			IComparer<T> comparer )
		{
			List<T> temp = new List<T>( this );

			temp.Sort( index, count, comparer );

			this.Clear();
			this.AddRange( temp );
		}

		/// <summary>
		/// Added 2006-05-25, Uwe Keim.
		/// </summary>
		/// <returns></returns>
		public T[] ToArray()
		{
			T[] result = new T[data.Keys.Count];

			data.Keys.CopyTo( result, 0 );

			return result;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Set&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="original">The original.</param>
		public Set(
			IEnumerable<T> original )
		{
			data = new Dictionary<T, Dummy>();

			if ( original != null )
			{
				AddRange( original );
			}
		}

		/// <summary>
		/// Adds the specified a.
		/// </summary>
		/// <param name="a">A.</param>
		public void Add(
			T a )
		{
			data[a] = dummy;
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="range">The range.</param>
		public void AddRange(
			IEnumerable<T> range )
		{
			if ( range != null )
			{
				foreach ( T a in range )
				{
					Add( a );
				}
			}
		}

		/// <summary>
		/// Removes the range.
		/// </summary>
		/// <param name="range">The range.</param>
		public void RemoveRange(
			IEnumerable<T> range )
		{
			if ( range != null )
			{
				foreach ( T a in range )
				{
					Remove( a );
				}
			}
		}

		/// <summary>
		/// Converts all.
		/// </summary>
		/// <param name="converter">The converter.</param>
		/// <returns></returns>
		public Set<U> ConvertAll<U>(
			Converter<T, U> converter )
		{
			Set<U> result = new Set<U>( this.Count );
			foreach ( T element in this )
			{
				result.Add( converter( element ) );
			}

			return result;
		}

		/// <summary>
		/// Trues for all.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns></returns>
		public bool TrueForAll(
			Predicate<T> predicate )
		{
			foreach ( T element in this )
			{
				if ( !predicate( element ) )
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Finds all.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns></returns>
		public Set<T> FindAll(
			Predicate<T> predicate )
		{
			Set<T> result = new Set<T>();

			foreach ( T element in this )
			{
				if ( predicate( element ) )
				{
					result.Add( element );
				}
			}
			return result;
		}

		/// <summary>
		/// Searches with the given predicate delegate for the first element
		/// that matches the predicate.
		/// </summary>
		/// <param name="predicate">The predicate to search with.</param>
		/// <returns>
		/// Returns the found element or default(T) if not found.
		/// </returns>
		public T Find(
			Predicate<T> predicate )
		{
			foreach ( T element in this )
			{
				if ( predicate( element ) )
				{
					return element;
				}
			}

			return default( T );
		}

		/// <summary>
		/// Searches with the given predicate delegate for the first element
		/// that matches the predicate.
		/// </summary>
		/// <param name="foundElement">The found element if found,
		/// default(T) otherwise.</param>
		/// <param name="predicate">The predicate to search with.</param>
		/// <returns>
		/// Returns TRUE if found, FALSe if not found.
		/// </returns>
		public bool Find(
			out T foundElement,
			Predicate<T> predicate )
		{
			foreach ( T element in this )
			{
				if ( predicate( element ) )
				{
					foundElement = element;
					return true;
				}
			}

			foundElement = default( T );
			return false;
		}

		/// <summary>
		/// Fors the each.
		/// </summary>
		/// <param name="action">The action.</param>
		public void ForEach(
			Action<T> action )
		{
			foreach ( T element in this )
			{
				action( element );
			}
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The 
		/// <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
		public void Clear()
		{
			data.Clear();
		}

		/// <summary>
		/// Determines whether [contains] [the specified a].
		/// </summary>
		/// <param name="a">A.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified a]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(
			T a )
		{
			return data.ContainsKey( a );
		}

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="index">The index.</param>
		public void CopyTo(
			T[] array,
			int index )
		{
			data.Keys.CopyTo( array, index );
		}

		/// <summary>
		/// Removes the specified a.
		/// </summary>
		/// <param name="a">A.</param>
		/// <returns></returns>
		public bool Remove(
			T a )
		{
			return data.Remove( a );
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return data.Keys.GetEnumerator();
		}

		/// <summary>
		/// Operator |s the specified a.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public static Set<T> operator |(
			Set<T> a,
			Set<T> b )
		{
			if ( a == null )
			{
				return b;
			}
			else if ( b == null )
			{
				return a;
			}
			else
			{
				Set<T> result = new Set<T>( a );
				result.AddRange( b );
				return result;
			}
		}

		/// <summary>
		/// Unions the specified b.
		/// </summary>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public Set<T> Union(
			IEnumerable<T> b )
		{
			if ( b == null )
			{
				return this;
			}
			else
			{
				return this | new Set<T>( b );
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
		/// </returns>
		public override bool Equals(
			object obj )
		{
			Set<T> a = this;
			Set<T> b = obj as Set<T>;

			if ( b == null )
			{
				return false;
			}
			else
			{
				return a == b;
			}
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override int GetHashCode()
		{
			int hashcode = 0;
			foreach ( T element in this )
			{
				hashcode ^= element.GetHashCode();
			}

			return hashcode;
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
		void ICollection.CopyTo(
			Array array,
			int index )
		{
			((ICollection)data.Keys).CopyTo( array, index );
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)data.Keys).GetEnumerator();
		}

		/// <summary>
		/// Intersections the specified b.
		/// </summary>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public Set<T> Intersection(
			IEnumerable<T> b )
		{
			if ( b == null )
			{
				return null;
			}
			else
			{
				return this & new Set<T>( b );
			}
		}

		/// <summary>
		/// Differences the specified b.
		/// </summary>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public Set<T> Difference(
			IEnumerable<T> b )
		{
			if ( b == null )
			{
				return this;
			}
			else
			{
				return this - new Set<T>( b );
			}
		}

		/// <summary>
		/// Symmetrics the difference.
		/// </summary>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public Set<T> SymmetricDifference(
			IEnumerable<T> b )
		{
			if ( b == null )
			{
				return this;
			}
			else
			{
				return this ^ new Set<T>( b );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets the number of elements contained in the
		/// <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the
		/// <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		public int Count
		{
			get
			{
				return data.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
		public bool IsEmpty
		{
			get
			{
				return data == null || data.Count <= 0;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the empty.
		/// </summary>
		/// <value>The empty.</value>
		public static Set<T> Empty
		{
			get
			{
				return new Set<T>( 0 );
			}
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.</returns>
		object ICollection.SyncRoot
		{
			get
			{
				return ((ICollection)data.Keys).SyncRoot;
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe).
		/// </summary>
		/// <value></value>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe); otherwise, false.</returns>
		bool ICollection.IsSynchronized
		{
			get
			{
				return ((ICollection)data.Keys).IsSynchronized;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public operators.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		public static Set<T> operator &(
			Set<T> a,
			Set<T> b )
		{
			Set<T> result = new Set<T>();

			if ( a != null )
			{
				foreach ( T element in a )
				{
					if ( b != null && b.Contains( element ) )
					{
						result.Add( element );
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Operator -s the specified a.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public static Set<T> operator -(
			Set<T> a,
			Set<T> b )
		{
			Set<T> result = new Set<T>();

			if ( a != null )
			{
				foreach ( T element in a )
				{
					if ( b == null || !b.Contains( element ) )
					{
						result.Add( element );
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Operator ^s the specified a.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public static Set<T> operator ^(
			Set<T> a,
			Set<T> b )
		{
			Set<T> result = new Set<T>();

			if ( a != null )
			{
				foreach ( T element in a )
				{
					if ( b == null || !b.Contains( element ) )
					{
						result.Add( element );
					}
				}
			}

			if ( a != null )
			{
				foreach ( T element in b )
				{
					if ( a == null || !a.Contains( element ) )
					{
						result.Add( element );
					}
				}
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		public static bool operator <=(
			Set<T> a,
			Set<T> b )
		{
			if ( a == null && b == null )
			{
				return false;
			}
			else if ( a == null )
			{
				return true;
			}
			else
			{
				foreach ( T element in a )
				{
					if ( b == null || !b.Contains( element ) )
					{
						return false;
					}
				}

				return true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static bool operator <(
			Set<T> a,
			Set<T> b )
		{
			if ( a == null && b == null )
			{
				return false;
			}
			else if ( a == null )
			{
				return true;
			}
			else if ( b == null )
			{
				return false;
			}
			else
			{
				return (a.Count < b.Count) && (a <= b);
			}
		}

		/// <summary>
		/// Operator ==s the specified a.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public static bool operator ==(
			Set<T> a,
			Set<T> b )
		{
			if ( (a as object) == null && (b as object) == null )
			{
				return true;
			}
			else if ( (a as object) == null )
			{
				return false;
			}
			else if ( (b as object) == null )
			{
				return false;
			}
			else
			{
				return (a.Count == b.Count) && (a <= b);
			}
		}

		/// <summary>
		/// Operator &gt;s the specified a.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public static bool operator >( Set<T> a, Set<T> b )
		{
			return b < a;
		}

		/// <summary>
		/// Operator &gt;=s the specified a.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public static bool operator >=( Set<T> a, Set<T> b )
		{
			return (b <= a);
		}

		/// <summary>
		/// Operator !=s the specified a.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public static bool operator !=( Set<T> a, Set<T> b )
		{
			return !(a == b);
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}