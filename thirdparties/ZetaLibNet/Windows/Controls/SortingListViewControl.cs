namespace ZetaLib.Windows.Controls
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.ComponentModel;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Text;
	using System.Windows.Forms;
	using ZetaLib.Core.Common;
	using ZetaLib.Core.Logging;
	using ZetaLib.Windows.Common;
	using System.Collections;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// List view control that allows sorting and stores its sort state to 
	/// persistent storage.
	/// </summary>
	public partial class SortingListViewControl :
		ExtendedListViewControl
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public SortingListViewControl()
		{
			Sorting = SortOrder.None;
		}

		/// <summary>
		/// Apply the current sorting again.
		/// </summary>
		public void RefreshSort()
		{
			// Set to sort. This also immediately sorts.
			ListViewItemSorter = new Sorter(
				this,
				Math.Max( 0, lastSortColumnIndex ),
				sortAscending );

			// Enough sorted.
			ListViewItemSorter = null;

			ShowHeaderIcon(
				lastSortColumnIndex,
				sortAscending ? SortOrder.Ascending : SortOrder.Descending );
		}

		/// <summary>
		/// Manually persist the sort state. Also done automatically, but
		/// for more control, call it here manually.
		/// </summary>
		public void PersistSortState()
		{
			OnPersistingSortState( null );
		}

		/// <summary>
		/// Manually restore the state. Also done automatically, but
		/// for more control, call it here manually.
		/// </summary>
		public void RestoreSortState()
		{
			OnRestoringSortState( null );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public events and event arguments.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		public delegate void PersistingSortStateEventHandler(
			object sender,
			PersistingSortStateEventArgs args );

		/// <summary>
		/// 
		/// </summary>
		public delegate void RestoringSortStateEventHandler(
			object sender,
			RestoringSortStateEventArgs args );

		/// <summary>
		/// 
		/// </summary>
		public class PersistingSortStateEventArgs :
			EventArgs
		{
			#region Private variables.

			/// <summary>
			/// Default.
			/// </summary>
			private IPersistentPairStorage storage =
				FormHelper.Storage;

			#endregion

			#region Public properties.

			/// <summary>
			/// Gets or sets the storage.
			/// </summary>
			/// <value>The storage.</value>
			public IPersistentPairStorage Storage
			{
				get
				{
					return storage;
				}
				set
				{
					storage = value;
				}
			}

			#endregion
		}

		/// <summary>
		/// 
		/// </summary>
		public class RestoringSortStateEventArgs :
			EventArgs
		{
			#region Private variables.

			/// <summary>
			/// Default.
			/// </summary>
			private IPersistentPairStorage storage =
				FormHelper.Storage;

			#endregion

			#region Public properties.

			/// <summary>
			/// Gets or sets the storage.
			/// </summary>
			/// <value>The storage.</value>
			public IPersistentPairStorage Storage
			{
				get
				{
					return storage;
				}
				set
				{
					storage = value;
				}
			}

			#endregion
		}

		/// <summary>
		/// Called before persisting the sort state.
		/// </summary>
		[Description( @"Called before persisting the sort state." )]
		public event PersistingSortStateEventHandler PersistingSortState;

		/// <summary>
		/// Called before restoring the sort state.
		/// </summary>
		[Description( @"Called before restoring the sort state." )]
		public event RestoringSortStateEventHandler RestoringSortState;

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Get or set whether to automatically restore the state of
		/// the list view when creating.
		/// </summary>
		/// <value><c>true</c> if [auto restore state]; otherwise, <c>false</c>.</value>
		[
		Category( @"Behavior" ),
		DefaultValue( false ),
		Description( @"Get or set whether to automatically restore the state (column widths, sort order, selection info) of the list view when creating." )
		]
		public bool AutoRestoreState
		{
			get
			{
				return autoRestoreState;
			}
			set
			{
				autoRestoreState = value;
			}
		}

		/// <summary>
		/// Tell this object which columns this item has. Useful for sorting,
		/// but optionally, the sorting works without setting this, too.
		/// </summary>
		/// <value>The column types.</value>
		[Browsable( false )]
		public Type[] ColumnTypes
		{
			get
			{
				return columnTypes;
			}
			set
			{
				if ( value == null )
				{
					columnTypes = null;
				}
				else
				{
					Debug.Assert(
						(value == null && Columns == null) ||
						(value.Length == Columns.Count),
						string.Format(
						@"The number of column types ({0}) to set to the sorting " +
						@"list view control differs from the actual column count ({1}).",
						value == null ? 0 : value.Length,
						Columns == null ? 0 : Columns.Count ) );

					columnTypes = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether [auto sorting].
		/// </summary>
		/// <value><c>true</c> if [auto sorting]; otherwise, <c>false</c>.</value>
		[
		Category( @"Behavior" ),
		DefaultValue( false ),
		Description( @"Get or set whether the control should auto sort items." )
		]
		public bool AutoSorting
		{
			get
			{
				return autoSorting;
			}
			set
			{
				autoSorting = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private sorting helper class.
		// ------------------------------------------------------------------

		/// <summary>
		/// Helper class for sorting.
		/// </summary>
		private class Sorter :
			IComparer
		{
			#region Public methods.

			/// <summary>
			/// Initializes a new instance of the <see cref="Sorter"/> class.
			/// </summary>
			/// <param name="owner">The owner.</param>
			/// <param name="columnIndex">Index of the column.</param>
			/// <param name="sortAscending">if set to <c>true</c> [sort ascending].</param>
			public Sorter(
				SortingListViewControl owner,
				int columnIndex,
				bool sortAscending )
			{
				Owner = owner;
				ColumnIndex = columnIndex;
				SortAscending = sortAscending;
			}

			/// <summary>
			/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
			/// </summary>
			/// <param name="x">The first object to compare.</param>
			/// <param name="y">The second object to compare.</param>
			/// <returns>
			/// Value Condition Less than zero x is less than y. Zero x equals y. Greater than zero x is greater than y.
			/// </returns>
			/// <exception cref="T:System.ArgumentException">Neither x nor y implements the <see cref="T:System.IComparable"></see> interface.-or- x and y are of different types and neither one can handle comparisons with the other. </exception>
			public int Compare(
				object x,
				object y )
			{
				return Owner.OnSortInternal(
					ColumnIndex,
					SortAscending,
					(ListViewItem)x,
					(ListViewItem)y );
			}

			#endregion

			#region Private variables.

			private SortingListViewControl Owner;
			private int ColumnIndex;
			private bool SortAscending;

			#endregion
		}

		/// <summary>
		/// Called when sorting.
		/// </summary>
		/// <param name="columnIndex">Index of the column.</param>
		/// <param name="sortAscending">if set to <c>true</c> [sort ascending].</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		private int OnSortInternal(
			int columnIndex,
			bool sortAscending,
			ListViewItem x,
			ListViewItem y )
		{
			Type columnType =
				columnTypes == null || columnTypes.Length <= columnIndex ?
				typeof( string ) :
				columnTypes[columnIndex];

			int result = 0;

			if ( columnType == typeof( DateTime ) )
			{
				DateTime valueX = DateTime.MinValue;
				DateTime valueY = DateTime.MinValue;

				if ( ConvertHelper.IsDateTime(
					GetSubItemTagOrText( x.SubItems[columnIndex] ) ) )
				{
					valueX = Convert.ToDateTime(
						GetSubItemTagOrText( x.SubItems[columnIndex] ) );
				}
				if ( ConvertHelper.IsDateTime(
					GetSubItemTagOrText( y.SubItems[columnIndex] ) ) )
				{
					valueY = Convert.ToDateTime(
						GetSubItemTagOrText( y.SubItems[columnIndex] ) );
				}

				result = valueX.CompareTo( valueY );
			}
			else if ( columnType == typeof( bool ) )
			{
				bool valueX = false;
				bool valueY = false;

				if ( ConvertHelper.IsBoolean(
					GetSubItemTagOrText( x.SubItems[columnIndex] ) ) )
				{
					valueX = Convert.ToBoolean(
						GetSubItemTagOrText( x.SubItems[columnIndex] ) );
				}
				if ( ConvertHelper.IsBoolean(
					GetSubItemTagOrText( y.SubItems[columnIndex] ) ) )
				{
					valueY = Convert.ToBoolean(
						GetSubItemTagOrText( y.SubItems[columnIndex] ) );
				}

				result = valueX.CompareTo( valueY );
			}
			else if ( columnType == typeof( double ) )
			{
				double valueX = 0;
				double valueY = 0;

				if ( ConvertHelper.IsDouble(
					GetSubItemTagOrText( x.SubItems[columnIndex] ) ) )
				{
					valueX = Convert.ToDouble(
						GetSubItemTagOrText( x.SubItems[columnIndex] ) );
				}
				if ( ConvertHelper.IsDouble(
					GetSubItemTagOrText( y.SubItems[columnIndex] ) ) )
				{
					valueY = Convert.ToDouble(
						GetSubItemTagOrText( y.SubItems[columnIndex] ) );
				}

				result = valueX.CompareTo( valueY );
			}
			else if ( columnType == typeof( int ) )
			{
				int valueX = 0;
				int valueY = 0;

				if ( ConvertHelper.IsInt32(
					GetSubItemTagOrText( x.SubItems[columnIndex] ) ) )
				{
					valueX = Convert.ToInt32(
						GetSubItemTagOrText( x.SubItems[columnIndex] ) );
				}
				if ( ConvertHelper.IsInt32(
					GetSubItemTagOrText( y.SubItems[columnIndex] ) ) )
				{
					valueY = Convert.ToInt32(
						GetSubItemTagOrText( y.SubItems[columnIndex] ) );
				}

				result = valueX.CompareTo( valueY );
			}
			else
			{
				Debug.Assert( columnType == typeof( string ) );

				result = string.Compare(
					GetSubItemTagOrText( x.SubItems[columnIndex] ).ToString(),
					GetSubItemTagOrText( y.SubItems[columnIndex] ).ToString() );
			}

			if ( sortAscending )
			{
				result *= -1;
			}

			return result;
		}

		/// <summary>
		/// Gets the sub item tag or text.
		/// </summary>
		/// <param name="subItem">The sub item.</param>
		/// <returns></returns>
		private object GetSubItemTagOrText(
			ListViewItem.ListViewSubItem subItem )
		{
			if ( subItem.Tag != null )
			{
				return subItem.Tag;
			}
			else
			{
				return subItem.Text;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private overrides.
		// ------------------------------------------------------------------

		/// <summary>
		/// Called when [persisting sort state].
		/// </summary>
		/// <param name="storage">The storage.</param>
		protected virtual void OnPersistingSortState(
			IPersistentPairStorage storage )
		{
			if ( !insidePersisting )
			{
				insidePersisting = true;
				try
				{
					PersistingSortStateEventArgs args =
						new PersistingSortStateEventArgs();

					if ( storage != null )
					{
						args.Storage = storage;
					}

					if ( PersistingSortState != null )
					{
						PersistingSortState( this, args );
					}

					FormHelper.SaveState( args.Storage, this );

					FormHelper.SaveValue(
						args.Storage,
						string.Format(
						@"SortingListViewControl.{0}.LastSortColumnIndex",
						this.Name ),
						lastSortColumnIndex );
					FormHelper.SaveValue(
						args.Storage,
						string.Format(
						@"SortingListViewControl.{0}.SortAscending",
						this.Name ),
						sortAscending );
				}
				finally
				{
					insidePersisting = false;
				}
			}
		}

		/// <summary>
		/// Called when [restoring sort state].
		/// </summary>
		/// <param name="storage">The storage.</param>
		protected virtual void OnRestoringSortState(
			IPersistentPairStorage storage )
		{
			if ( !insideRestoring )
			{
				insideRestoring = true;
				try
				{
					RestoringSortStateEventArgs args =
						new RestoringSortStateEventArgs();

					if ( storage != null )
					{
						args.Storage = storage;
					}

					if ( RestoringSortState != null )
					{
						RestoringSortState( this, args );
					}

					FormHelper.RestoreState( args.Storage, this );

					lastSortColumnIndex = Convert.ToInt32( FormHelper.RestoreValue(
						args.Storage,
						string.Format(
						@"SortingListViewControl.{0}.LastSortColumnIndex",
						this.Name ),
						lastSortColumnIndex ) );
					sortAscending = Convert.ToBoolean( FormHelper.RestoreValue(
						args.Storage,
						string.Format(
						@"SortingListViewControl.{0}.SortAscending",
						this.Name ),
						sortAscending ) );
				}
				finally
				{
					insideRestoring = false;
				}
			}
		}

		/// <summary>
		/// The control is being created.
		/// </summary>
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
		}

		/// <summary>
		/// The control is being created.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnHandleCreated(
			EventArgs e )
		{
			base.OnHandleCreated( e );

			if ( autoRestoreState )
			{
				OnRestoringSortState( null );
			}

			// Initially sort.
			if ( autoSorting )
			{
				RefreshSort();
			}
		}

		/// <summary>
		/// The control is destroyed.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnHandleDestroyed(
			EventArgs e )
		{
			if ( autoRestoreState )
			{
				OnPersistingSortState( null );
			}

			base.OnHandleDestroyed( e );
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.ListView.ColumnClick"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ColumnClickEventArgs"></see> that contains the event data.</param>
		protected override void OnColumnClick(
			ColumnClickEventArgs e )
		{
			base.OnColumnClick( e );

			ApplySort( e.Column );
		}

		/// <summary>
		/// Sort by the given column.
		/// </summary>
		/// <param name="columnIndex">Index of the column.</param>
		private void ApplySort(
			int columnIndex )
		{
			BeginUpdate();
			try
			{
				if ( columnIndex == lastSortColumnIndex )
				{
					sortAscending = !sortAscending;
				}
				else
				{
					ShowHeaderIcon( lastSortColumnIndex, SortOrder.None );
					sortAscending = true;
				}

				lastSortColumnIndex = columnIndex;
				ShowHeaderIcon(
					lastSortColumnIndex,
					sortAscending ? SortOrder.Ascending : SortOrder.Descending );

				// Set to sort. This also immediately sorts.
				ListViewItemSorter = new Sorter(
					this,
					columnIndex,
					sortAscending );
			}
			finally
			{
				EndUpdate();
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private bool autoRestoreState = false;
		private int lastSortColumnIndex = -1;
		private bool sortAscending = true;

		private Type[] columnTypes = null;

		private bool autoSorting = false;

		private bool insidePersisting = false;
		private bool insideRestoring = false;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}