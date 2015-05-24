namespace ZetaLib.Windows.Controls
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Windows.Forms;
	using System.Runtime.InteropServices;
	using System.Diagnostics;
	using System.Drawing;
	using System.ComponentModel;
	using System.Drawing.Imaging;
	using System.Collections;
	using ZetaLib.Core.Common;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// An extended list view control that has tri-state check boxes.
	/// </summary>
	public partial class TriStateCheckBoxesListView :
		Vista_Api.ListView
	{
		#region Public tri-state related methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public TriStateCheckBoxesListView()
		{
			InitializeComponent();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public tri-state related properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets or sets whether to have checkboxes beside the items and
		/// whether they have tri state.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [use tri state check boxes]; otherwise, <c>false</c>.
		/// </value>
		[Category( @"Appearance" ),
		Description( @"Indicates whether tri-state check boxes are displayed beside list view items." )]
		public bool UseTriStateCheckBoxes
		{
			get
			{
				return StateImageList == CheckedTriStateImageList;
			}
			set
			{
				// Only if changed.
				if ( UseTriStateCheckBoxes != value )
				{
					if ( value )
					{
						CheckBoxes = false;
						StateImageList = CheckedTriStateImageList;

						foreach ( ListViewItem item in Items )
						{
							DoApplyStateImageIndexToItem(
								item,
								TriStateUncheckedStateImageIndex, false );
						}
					}
					else
					{
						StateImageList = null;

						foreach ( ListViewItem item in Items )
						{
							DoApplyStateImageIndexToItem(
								item,
								TriStateNoStateImageIndex,
								false );
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether check boxes are displayed
		/// next to the list view items in the list view control.
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// Provide new version of this property to ensure only one of the
		/// tri-state and normal checkboxes are shown.
		/// </remarks>
		public new bool CheckBoxes
		{
			get
			{
				return base.CheckBoxes;
			}
			set
			{
				if ( value != CheckBoxes )
				{
					UseTriStateCheckBoxes = false;
					base.CheckBoxes = value;
				}
			}
		}

		/// <summary>
		/// Gets and sets whether all selected list view items are
		/// automatically updated when one selected list view item is
		/// checked/unchecked by the user.
		/// </summary>
		/// <value><c>true</c> if [auto check selected]; otherwise, <c>false</c>.</value>
		[Category( @"Behavior" ),
		Description( @"Gets and sets whether all selected list view items are automatically updated when one selected list view item is checked/unchecked by the user." )]
		public bool AutoCheckSelected
		{
			get
			{
				return autoCheckSelected;
			}
			set
			{
				autoCheckSelected = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public tri-state related enums.
		// ------------------------------------------------------------------

		/// <summary>
		/// Apply more complex/recursive checks to a list view item.
		/// </summary>
		public enum ItemsCheckState
		{
			#region Enum members.

			/// <summary>
			/// Check all items.
			/// </summary>
			CheckAllItems,

			/// <summary>
			/// Uncheck all items.
			/// </summary>
			UncheckAllItems,

			/// <summary>
			/// Set all items to indeterminate.
			/// </summary>
			IndeterminateAllItems,

			/// <summary>
			/// Check all selected items.
			/// </summary>
			CheckAllSelectedItems,

			/// <summary>
			/// Uncheck all selected items.
			/// </summary>
			UncheckAllSelectedItems,

			/// <summary>
			/// Set all selected items to indeterminate.
			/// </summary>
			IndeterminateAllSelectedItems,

			#endregion
		}

		/// <summary>
		/// How the check state was set.
		/// </summary>
		public enum ListViewAction
		{
			#region Enum members.

			/// <summary>
			/// Unknown or by code.
			/// </summary>
			Unknown,

			/// <summary>
			/// By the keyboard.
			/// </summary>
			ByKeyboard,

			/// <summary>
			/// By the mouse.
			/// </summary>
			ByMouse

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public tri-state related methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Applies a new check state to the given list view item.
		/// The UseTriStateCheckBoxes property must be set to TRUE.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="checkState">State of the check.</param>
		public void SetItemCheckState(
			ListViewItem item,
			CheckState checkState )
		{
			Debug.Assert(
				UseTriStateCheckBoxes,
				@"The ExtendedListViewControl.UseTriStateCheckBoxes property must be set to TRUE before setting the item check state." );

			// Use ListViewAction.Unknown here, to indicate 
			// that it was called from outside.
			DoSetItemCheckState(
				item,
				ListViewAction.Unknown,
				checkState );
		}

		/// <summary>
		/// Get the check state of a given list view item.
		/// The UseTriStateCheckBoxes property must be set to TRUE.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public CheckState GetItemCheckState(
			ListViewItem item )
		{
			Debug.Assert(
				UseTriStateCheckBoxes,
				@"The ExtendedListViewControl.UseTriStateCheckBoxes property must be set to TRUE before setting the item check state." );

			return ConvertStateImageIndexToCheckState( item.StateImageIndex );
		}

		/// <summary>
		/// Apply more complex/recursive checks to a list view item.
		/// </summary>
		/// <param name="newStates">The new states.</param>
		public void ChangeItemsCheckStates(
			ItemsCheckState newStates )
		{
			switch ( newStates )
			{
				case ItemsCheckState.CheckAllItems:
					DeepSetCheckState(
						CheckState.Checked,
						Items );
					break;
				case ItemsCheckState.UncheckAllItems:
					DeepSetCheckState(
						CheckState.Unchecked,
						Items );
					break;
				case ItemsCheckState.IndeterminateAllItems:
					DeepSetCheckState(
						CheckState.Indeterminate,
						Items );
					break;
				case ItemsCheckState.CheckAllSelectedItems:
					DeepSetCheckState(
						CheckState.Checked,
						SelectedItems );
					break;
				case ItemsCheckState.UncheckAllSelectedItems:
					DeepSetCheckState(
						CheckState.Unchecked,
						SelectedItems );
					break;
				case ItemsCheckState.IndeterminateAllSelectedItems:
					DeepSetCheckState(
						CheckState.Indeterminate,
						SelectedItems );
					break;

				default:
					Debug.Assert(
						false,
						string.Format(
						@"Unknown ItemsCheckState enum value '{0}'.",
						newStates ) );
					break;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Events handler.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		public delegate void ItemCheckTriStateEventHandler(
			object sender,
			ItemCheckTriStateEventArgs args );

		/// <summary>
		/// 
		/// </summary>
		public delegate void ItemCheckedTriStateEventHandler(
			object sender,
			ItemCheckedTriStateEventArgs args );

		// ------------------------------------------------------------------
		#endregion

		#region Event arguments.
		// ------------------------------------------------------------------

		/// <summary>
		/// Specialized arguments class.
		/// </summary>
		public class ItemCheckTriStateEventArgs :
			ItemCheckEventArgs
		{
			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="item">The item.</param>
			/// <param name="action">The action.</param>
			/// <param name="newCheckValue">The new check value.</param>
			/// <param name="currentCheckValue">The current check value.</param>
			public ItemCheckTriStateEventArgs(
				ListViewItem item,
				ListViewAction action,
				CheckState newCheckValue,
				CheckState currentCheckValue )
				:
				base( item.Index, newCheckValue, currentCheckValue )
			{
				this.item = item;
				this.action = action;
			}

			/// <summary>
			/// Gets the action.
			/// </summary>
			/// <value>The action.</value>
			public ListViewAction Action
			{
				get
				{
					return action;
				}
			}

			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="ItemCheckTriStateEventArgs"/> is cancel.
			/// </summary>
			/// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
			public bool Cancel
			{
				get
				{
					return cancel;
				}
				set
				{
					cancel = value;
				}
			}

			/// <summary>
			/// Gets the item.
			/// </summary>
			/// <value>The item.</value>
			public ListViewItem Item
			{
				get
				{
					return item;
				}
			}

			/// <summary>
			/// The state image index of the related item.
			/// </summary>
			/// <value>The index of the current state image.</value>
			public int CurrentStateImageIndex
			{
				get
				{
					return ConvertCheckStateToImageIndex(
						base.CurrentValue );
				}
			}

			/// <summary>
			/// The state image index about to set to the related item.
			/// </summary>
			/// <value>The new index of the state image.</value>
			public int NewStateImageIndex
			{
				get
				{
					return ConvertCheckStateToImageIndex(
						base.NewValue );
				}
			}

			/// <summary>
			/// 
			/// </summary>
			private ListViewItem item = null;
			private bool cancel = false;
			private ListViewAction action;
		}

		/// <summary>
		/// Specialized arguments class.
		/// </summary>
		public class ItemCheckedTriStateEventArgs :
			ItemCheckedEventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="ItemCheckedTriStateEventArgs"/> class.
			/// </summary>
			/// <param name="item">The item.</param>
			/// <param name="action">The action.</param>
			/// <param name="stateImageIndex">Index of the state image.</param>
			public ItemCheckedTriStateEventArgs(
				ListViewItem item,
				ListViewAction action,
				int stateImageIndex )
				:
				base( item )
			{
				this.action = action;
				this.stateImageIndex = stateImageIndex;
			}

			/// <summary>
			/// Gets the action.
			/// </summary>
			/// <value>The action.</value>
			public ListViewAction Action
			{
				get
				{
					return action;
				}
			}

			/// <summary>
			/// The state image index of the related item.
			/// </summary>
			/// <value>The index of the state image.</value>
			public int StateImageIndex
			{
				get
				{
					return stateImageIndex;
				}
			}

			/// <summary>
			/// The check state of the related item.
			/// </summary>
			/// <value>The state of the check.</value>
			public CheckState CheckState
			{
				get
				{
					return ConvertStateImageIndexToCheckState(
						stateImageIndex );
				}
			}

			/// <summary>
			/// 
			/// </summary>
			private int stateImageIndex;
			private ListViewAction action;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Events.
		// ------------------------------------------------------------------

		/// <summary>
		/// Event that occurs when a check box on a list view item is about to be checked or unchecked.
		/// </summary>
		[Category( @"Behavior" ),
		Description( @"Occurs when a check box on a list view item is about to be checked or unchecked." )]
		public event ItemCheckTriStateEventHandler ItemCheckTriState;

		/// <summary>
		/// Event that occurs when a check box on a list view item has been checked or unchecked.
		/// </summary>
		[Category( @"Behavior" ),
		Description( @"Occurs when a check box on a list view item has been checked or unchecked." )]
		public event ItemCheckedTriStateEventHandler ItemCheckedTriState;

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// If configured, some auto-steps could be performed after a check.
		/// </summary>
		/// <param name="args">The <see cref="ZetaLib.Windows.Controls.TriStateCheckBoxesListView.ItemCheckedTriStateEventArgs"/> instance containing the event data.</param>
		private void CheckPerformAutoStepsTriStateChecked(
			ItemCheckedTriStateEventArgs args )
		{
			if ( args.Action == ListViewAction.ByKeyboard ||
				args.Action == ListViewAction.ByMouse )
			{
				if ( AutoCheckSelected )
				{
					if ( args.CheckState == CheckState.Checked )
					{
						ChangeItemsCheckStates(
							ItemsCheckState.CheckAllSelectedItems );
					}
					else if ( args.CheckState == CheckState.Unchecked )
					{
						ChangeItemsCheckStates(
							ItemsCheckState.UncheckAllSelectedItems );
					}
				}
			}
		}

		/// <summary>
		/// Call this internally.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="checkState">State of the check.</param>
		private void InternalSetItemCheckState(
			ListViewItem item,
			CheckState checkState )
		{
			DoSetItemCheckState(
				item,
				ListViewAction.Unknown,
				checkState );
		}

		/// <summary>
		/// Applies a new check state to the given list view item.
		/// The UseTriStateCheckBoxes property must be set to TRUE.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="action">The action.</param>
		/// <param name="checkState">State of the check.</param>
		private void DoSetItemCheckState(
			ListViewItem item,
			ListViewAction action,
			CheckState checkState )
		{
			// Use ListViewAction.ByKeyboard here, 
			// to indicate that it was called from outside.
			SetTriStateToItem(
				item,
				action,
				ConvertCheckStateToImageIndex( checkState ),
				true );
		}

		/// <summary>
		/// Deeps the state of the set check.
		/// </summary>
		/// <param name="checkState">State of the check.</param>
		/// <param name="items">The items.</param>
		private void DeepSetCheckState(
			CheckState checkState,
			IList items )
		{
			foreach ( ListViewItem cn in items )
			{
				InternalSetItemCheckState( cn, checkState );
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseClick"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseClick(
			MouseEventArgs e )
		{
			base.OnMouseClick( e );

			if ( UseTriStateCheckBoxes )
			{
				if ( e.Button == MouseButtons.Left )
				{
					ListViewHitTestInfo htInfo = HitTest( e.Location );

					if ( htInfo.Location == 
						ListViewHitTestLocations.StateImage )
					{
						ToggleTriStateCheck( 
							htInfo.Item, 
							ListViewAction.ByMouse );
					}
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"></see> that contains the event data.</param>
		protected override void OnKeyDown(
			KeyEventArgs e )
		{
			base.OnKeyDown( e );

			if ( UseTriStateCheckBoxes )
			{
				if ( e.KeyCode == Keys.Space )
				{
					ToggleTriStateCheck(
						FocusedItem,
						ListViewAction.ByKeyboard );
				}
			}
		}

		/// <summary>
		/// Toggles the tri state check.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="action">The action.</param>
		private void ToggleTriStateCheck(
			ListViewItem item,
			ListViewAction action )
		{
			if ( item.StateImageIndex == TriStateUncheckedStateImageIndex )
			{
				SetTriStateToItem(
					item,
					action,
					TriStateCheckedStateImageIndex,
					true );
			}
			else
			{
				SetTriStateToItem(
					item,
					action,
					TriStateUncheckedStateImageIndex,
					true );
			}
		}

		/// <summary>
		/// Map a state image index to a check state.
		/// </summary>
		/// <param name="imageIndex">Index of the image.</param>
		/// <returns></returns>
		private static CheckState ConvertStateImageIndexToCheckState(
			int imageIndex )
		{
			if ( imageIndex == TriStateCheckedStateImageIndex )
			{
				return CheckState.Checked;
			}
			else if ( imageIndex == TriStateUncheckedStateImageIndex )
			{
				return CheckState.Unchecked;
			}
			else if ( imageIndex == TriStateIndeterminateStateImageIndex )
			{
				return CheckState.Indeterminate;
			}
			else if ( imageIndex == TriStateNoStateImageIndex )
			{
				return CheckState.Unchecked;
			}
			else
			{
				Debug.Assert(
					false,
					string.Format(
					@"No check state for state image index {0} available.",
					imageIndex ) );
				return CheckState.Unchecked;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private bool autoCheckSelected = true;

		/// <summary>
		/// Map a check state to a state image index.
		/// </summary>
		/// <param name="checkState">State of the check.</param>
		/// <returns></returns>
		private static int ConvertCheckStateToImageIndex(
			CheckState checkState )
		{
			switch ( checkState )
			{
				case CheckState.Checked:
					return TriStateCheckedStateImageIndex;
				case CheckState.Unchecked:
					return TriStateUncheckedStateImageIndex;
				case CheckState.Indeterminate:
					return TriStateIndeterminateStateImageIndex;
				default:
					Debug.Assert(
						false,
						string.Format(
						@"No state image for check state '{0}' available.",
						checkState ) );
					return TriStateNoStateImageIndex;
			}
		}

		/// <summary>
		/// Applies the given state to the given item.
		/// Raises events, too.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="action">The action.</param>
		/// <param name="stateImageIndex">Index of the state image.</param>
		/// <param name="raiseEvents">if set to <c>true</c> [raise events].</param>
		private void SetTriStateToItem(
			ListViewItem item,
			ListViewAction action,
			int stateImageIndex,
			bool raiseEvents )
		{
			bool canSet = true;

			if ( raiseEvents )
			{
				ItemCheckEventArgs args =
					new ItemCheckEventArgs(
					item.Index,
					ConvertStateImageIndexToCheckState(
					stateImageIndex ),
					ConvertStateImageIndexToCheckState(
					item.StateImageIndex ) );

				OnItemCheck( args );

				ItemCheckTriStateEventArgs triArgs =
					new ItemCheckTriStateEventArgs(
					item,
					action,
					ConvertStateImageIndexToCheckState(
					stateImageIndex ),
					ConvertStateImageIndexToCheckState(
					item.StateImageIndex ) );

				OnItemCheckTriState( triArgs );

				canSet = !triArgs.Cancel;
			}

			if ( canSet )
			{
				// Actually set.
				// THIS IS THE ONLY PLACE to set.
				item.StateImageIndex = stateImageIndex;

				if ( raiseEvents )
				{
					ItemCheckedEventArgs args =
						new ItemCheckedEventArgs( item );

					OnItemChecked( args );

					ItemCheckedTriStateEventArgs triArgs =
						new ItemCheckedTriStateEventArgs(
						item,
						action,
						item.StateImageIndex );

					OnItemCheckedTriState( triArgs );
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="E:ItemCheckTriState"/> event.
		/// </summary>
		/// <param name="args">The <see cref="ZetaLib.Windows.Controls.TriStateCheckBoxesListView.ItemCheckTriStateEventArgs"/> instance containing the event data.</param>
		protected virtual void OnItemCheckTriState(
			ItemCheckTriStateEventArgs args )
		{
			if ( ItemCheckTriState != null )
			{
				ItemCheckTriState( this, args );
			}
		}

		/// <summary>
		/// Raises the <see cref="E:ItemCheckedTriState"/> event.
		/// </summary>
		/// <param name="args">The <see cref="ZetaLib.Windows.Controls.TriStateCheckBoxesListView.ItemCheckedTriStateEventArgs"/> instance containing the event data.</param>
		protected virtual void OnItemCheckedTriState(
			ItemCheckedTriStateEventArgs args )
		{
			if ( ItemCheckedTriState != null )
			{
				ItemCheckedTriState( this, args );
			}

			CheckPerformAutoStepsTriStateChecked( args );
		}

		/// <summary>
		/// Does the apply state image index to item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="stateImageIndex">Index of the state image.</param>
		/// <param name="raiseEvents">if set to <c>true</c> [raise events].</param>
		private void DoApplyStateImageIndexToItem(
			ListViewItem item,
			int stateImageIndex,
			bool raiseEvents )
		{
			SetTriStateToItem(
				item,
				ListViewAction.Unknown,
				stateImageIndex,
				raiseEvents );
		}

		/// <summary>
		/// Internal helper. Do not access directly but through the
		/// CheckedTriStateImageList property instead.
		/// </summary>
		private static ImageList _internalCheckedTriStateImageList = null;

		/// <summary>
		/// Access an image list with "CHECKED", "UNCHECKED" and
		/// "INDETERMINATE"´button states.
		/// The Tag properties of the images contain the respective value
		/// from the "System.Windows.Forms.CheckState" enumeration.
		/// </summary>
		/// <value>The checked tri state image list.</value>
		public static ImageList CheckedTriStateImageList
		{
			get
			{
				if ( _internalCheckedTriStateImageList == null )
				{
					_internalCheckedTriStateImageList =
						TriStateCheckBoxesTreeView.GenerateCheckedTriStateImageList();
				}

				return _internalCheckedTriStateImageList;
			}
		}

		/// <summary>
		/// The indexes into the state image list for the tri-state
		/// check boxes.
		/// </summary>
		private static readonly int TriStateNoStateImageIndex = -1;
		private static readonly int TriStateUncheckedStateImageIndex = 0;
		private static readonly int TriStateCheckedStateImageIndex = 1;
		private static readonly int TriStateIndeterminateStateImageIndex = 2;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}