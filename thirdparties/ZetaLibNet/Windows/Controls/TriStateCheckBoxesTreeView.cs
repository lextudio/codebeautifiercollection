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
	using System.Runtime.InteropServices;
	using System.Drawing;
	using System.Drawing.Imaging;
	using ZetaLib.Core.Common;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// An extended tree view control that has tri-state check boxes.
	/// </summary>
	public partial class TriStateCheckBoxesTreeView :
		Vista_Api.TreeView
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public TriStateCheckBoxesTreeView()
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
		Description( @"Indicates whether tri-state check boxes are displayed beside nodes." )]
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

						foreach ( TreeNode node in Nodes )
						{
							DoApplyStateImageIndexToAllNodes(
								node,
								TriStateUncheckedStateImageIndex,
								false );
						}
					}
					else
					{
						StateImageList = null;

						foreach ( TreeNode node in Nodes )
						{
							DoApplyStateImageIndexToAllNodes(
								node,
								TriStateNoStateImageIndex,
								false );
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether check boxes are displayed
		/// next to the tree nodes in the tree view control.
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
		/// Gets and sets whether child node check states are
		/// automatically updated when a parent node is checked/unchecked
		/// by the user.
		/// </summary>
		/// <value><c>true</c> if [auto check childs]; otherwise, <c>false</c>.</value>
		[Category( @"Behavior" ),
		Description( @"Gets and sets whether child node check states are automatically updated when a parent node is checked/unchecked by the user." )]
		public bool AutoCheckChilds
		{
			get
			{
				return autoCheckChilds;
			}
			set
			{
				autoCheckChilds = value;
			}
		}

		/// <summary>
		/// Gets and sets whether parent node check states are
		/// automatically updated when a child node is checked/unchecked
		/// by the user.
		/// </summary>
		/// <value><c>true</c> if [auto check parents]; otherwise, <c>false</c>.</value>
		[Category( @"Behavior" ),
		Description( @"Gets and sets whether parent node check states are automatically updated when a child node is checked/unchecked by the user." )]
		public bool AutoCheckParents
		{
			get
			{
				return autoCheckParents;
			}
			set
			{
				autoCheckParents = value;
			}
		}

		/// <summary>
		/// http://www.codeproject.com/useritems/ZetaLibNet.asp?msg=1734376#xx1734376xx.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [indeterminate to checked]; otherwise, <c>false</c>.
		/// </value>
		[Category( @"Behavior" ),
		Description( @"" )]
		public bool IndeterminateToChecked
		{
			get
			{
				return indeterminateToChecked;
			}
			set
			{
				indeterminateToChecked = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public tri-state related enums.
		// ------------------------------------------------------------------

		/// <summary>
		/// Apply more complex/recursive checks to a tree node.
		/// </summary>
		public enum NodesCheckState
		{
			#region Enum members.

			/// <summary>
			/// Deep-check all childs, including the node itself, too.
			/// </summary>
			CheckAllChilds,

			/// <summary>
			/// Deep-uncheck all childs, including the node itself, too.
			/// </summary>
			UncheckAllChilds,

			/// <summary>
			/// Updates the check state of the given node.
			/// - setting the node to unchecked if none of the deep-childs 
			///   are unchecked,
			/// - setting the node to checked if all of the deep-childs 
			///   are checked,
			/// - setting the node to tri-state if some of the deep-childs 
			///   are checked.
			/// </summary>
			UpdateStateFromChilds,

			/// <summary>
			/// Updates the state of all parent nodes of the given node,
			/// based on the child states.
			/// </summary>
			UpdateParentStates

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public tri-state related methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Applies a new check state to the given tree node.
		/// The UseTriStateCheckBoxes property must be set to TRUE.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="checkState">State of the check.</param>
		public void SetNodeCheckState(
			TreeNode node,
			CheckState checkState )
		{
			Debug.Assert(
				UseTriStateCheckBoxes,
				@"The ExtendedTreeViewControl.UseTriStateCheckBoxes property must be set to TRUE before setting the node check state." );

			// Use TreeViewAction.Unknown here, to indicate that it 
			// was called from outside.
			DoSetNodeCheckState(
				node,
				TreeViewAction.Unknown,
				checkState );
		}

		/// <summary>
		/// Get the check state of a given tree node.
		/// The UseTriStateCheckBoxes property must be set to TRUE.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns></returns>
		public CheckState GetNodeCheckState(
			TreeNode node )
		{
			Debug.Assert(
				UseTriStateCheckBoxes,
				@"The ExtendedTreeViewControl.UseTriStateCheckBoxes property must be set to TRUE before setting the node check state." );

			return ConvertStateImageIndexToCheckState( node.StateImageIndex );
		}

		/// <summary>
		/// Apply more complex/recursive checks to a tree node.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="newStates">The new states.</param>
		public void ChangeNodesCheckStates(
			TreeNode node,
			NodesCheckState newStates )
		{
			if ( node == null )
			{
				throw new ArgumentNullException( @"node" );
			}
			else
			{
				switch ( newStates )
				{
					case NodesCheckState.CheckAllChilds:
						DeepSetCheckState( node, CheckState.Checked );
						break;
					case NodesCheckState.UncheckAllChilds:
						DeepSetCheckState( node, CheckState.Unchecked );
						break;
					case NodesCheckState.UpdateStateFromChilds:
						int checkedChildCount =
							DeepCountChildsWithState(
							node,
							CheckState.Checked );
						int uncheckedChildCount =
							DeepCountChildsWithState(
							node,
							CheckState.Unchecked );
						int indeterminateChildCount =
							DeepCountChildsWithState(
							node,
							CheckState.Indeterminate );

						if ( indeterminateChildCount > 0 )
						{
							InternalSetNodeCheckState(
								node,
								CheckState.Indeterminate );
						}
						else
						{
							if ( checkedChildCount > 0 )
							{
								if ( uncheckedChildCount > 0 )
								{
									InternalSetNodeCheckState(
										node,
										CheckState.Indeterminate );
								}
								else
								{
									InternalSetNodeCheckState(
										node,
										CheckState.Checked );
								}
							}
							else
							{
								InternalSetNodeCheckState(
									node,
									CheckState.Unchecked );
							}
						}
						break;

					case NodesCheckState.UpdateParentStates:
						node = node.Parent;
						while ( node != null )
						{
							ChangeNodesCheckStates(
								node,
								NodesCheckState.UpdateStateFromChilds );

							node = node.Parent;
						}
						break;

					default:
						Debug.Assert(
							false,
							string.Format(
							@"Unknown NodesCheckState enum value.",
							newStates ) );
						break;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Events handler.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		public delegate void BeforeTriStateCheckEventHandler(
			object sender,
			TreeViewCancelTriStateEventArgs args );

		/// <summary>
		/// 
		/// </summary>
		public delegate void AfterTriStateCheckEventHandler(
			object sender,
			TreeViewTriStateEventArgs args );

		// ------------------------------------------------------------------
		#endregion

		#region Event arguments.
		// ------------------------------------------------------------------

		/// <summary>
		/// Specialized arguments class.
		/// </summary>
		public class TreeViewCancelTriStateEventArgs :
			TreeViewCancelEventArgs
		{
			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="node">The node.</param>
			/// <param name="cancel">if set to <c>true</c> [cancel].</param>
			/// <param name="action">The action.</param>
			/// <param name="currentStateImageIndex">Index of the current state image.</param>
			/// <param name="newStateImageIndex">New index of the state image.</param>
			public TreeViewCancelTriStateEventArgs(
					TreeNode node,
					bool cancel,
					TreeViewAction action,
					int currentStateImageIndex,
					int newStateImageIndex )
				:
				base( node, cancel, action )
			{
				this.currentStateImageIndex = currentStateImageIndex;
				this.newStateImageIndex = newStateImageIndex;
			}

			/// <summary>
			/// The state image index of the related node.
			/// </summary>
			/// <value>The index of the current state image.</value>
			public int CurrentStateImageIndex
			{
				get
				{
					return currentStateImageIndex;
				}
			}

			/// <summary>
			/// The check state of the related node.
			/// </summary>
			/// <value>The state of the current check.</value>
			public CheckState CurrentCheckState
			{
				get
				{
					return ConvertStateImageIndexToCheckState(
						currentStateImageIndex );
				}
			}

			/// <summary>
			/// The state image index about to set to the related node.
			/// </summary>
			/// <value>The new index of the state image.</value>
			public int NewStateImageIndex
			{
				get
				{
					return newStateImageIndex;
				}
			}

			/// <summary>
			/// The check state about to set to the related node.
			/// </summary>
			/// <value>The new state of the check.</value>
			public CheckState NewCheckState
			{
				get
				{
					return ConvertStateImageIndexToCheckState(
						newStateImageIndex );
				}
			}

			/// <summary>
			/// 
			/// </summary>
			private int currentStateImageIndex;
			private int newStateImageIndex;
		}

		/// <summary>
		/// Specialized arguments class.
		/// </summary>
		public class TreeViewTriStateEventArgs :
			TreeViewEventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="TreeViewTriStateEventArgs"/> class.
			/// </summary>
			/// <param name="node">The node.</param>
			/// <param name="action">The action.</param>
			/// <param name="stateImageIndex">Index of the state image.</param>
			public TreeViewTriStateEventArgs(
				TreeNode node,
				TreeViewAction action,
				int stateImageIndex )
				:
				base( node, action )
			{
				this.stateImageIndex = stateImageIndex;
			}

			/// <summary>
			/// The state image index of the related node.
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
			/// The check state of the related node.
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
		}

		// ------------------------------------------------------------------
		#endregion

		#region Events.
		// ------------------------------------------------------------------

		/// <summary>
		/// Event that occurs when a check box on a tree node is about to be checked or unchecked.
		/// </summary>
		[Category( @"Behavior" ),
		Description( @"Occurs when a check box on a tree node is about to be checked or unchecked." )]
		public event BeforeTriStateCheckEventHandler BeforeTriStateCheck;

		/// <summary>
		/// Event that occurs when a check box on a tree node has been checked or unchecked.
		/// </summary>
		[Category( @"Behavior" ),
		Description( @"Occurs when a check box on a tree node has been checked or unchecked." )]
		public event AfterTriStateCheckEventHandler AfterTriStateCheck;

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Call this internally.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="checkState">State of the check.</param>
		private void InternalSetNodeCheckState(
			TreeNode node,
			CheckState checkState )
		{
			DoSetNodeCheckState(
				node,
				TreeViewAction.Unknown,
				checkState );
		}

		/// <summary>
		/// Applies a new check state to the given tree node.
		/// The UseTriStateCheckBoxes property must be set to TRUE.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="action">The action.</param>
		/// <param name="checkState">State of the check.</param>
		private void DoSetNodeCheckState(
			TreeNode node,
			TreeViewAction action,
			CheckState checkState )
		{
			// Use TreeViewAction.ByKeyboard here, 
			// to indicate that it was called from outside.
			SetTriStateToNode(
				node,
				action,
				ConvertCheckStateToImageIndex( checkState ),
				true );
		}

		/// <summary>
		/// Counts the number of childs with a given state.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="checkState">State of the check.</param>
		/// <returns></returns>
		private int DeepCountChildsWithState(
			TreeNode node,
			CheckState checkState )
		{
			int result = 0;

			foreach ( TreeNode cn in node.Nodes )
			{
				result += DoGetDeepCountChildState( cn, checkState );
			}

			return result;
		}

		/// <summary>
		/// Helper.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="checkState">State of the check.</param>
		/// <returns></returns>
		private int DoGetDeepCountChildState(
			TreeNode node,
			CheckState checkState )
		{
			int result = 0;

			if ( GetNodeCheckState( node ) == checkState )
			{
				result++;
			}

			foreach ( TreeNode cn in node.Nodes )
			{
				result += DoGetDeepCountChildState( cn, checkState );
			}

			return result;
		}

		/// <summary>
		/// Deeps the state of the set check.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="checkState">State of the check.</param>
		private void DeepSetCheckState(
			TreeNode node,
			CheckState checkState )
		{
			InternalSetNodeCheckState( node, checkState );

			foreach ( TreeNode cn in node.Nodes )
			{
				DeepSetCheckState( cn, checkState );
			}
		}

		/// <summary>
		/// Does the apply state image index to all nodes.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="stateImageIndex">Index of the state image.</param>
		/// <param name="raiseEvents">if set to <c>true</c> [raise events].</param>
		private void DoApplyStateImageIndexToAllNodes(
			TreeNode node,
			int stateImageIndex,
			bool raiseEvents )
		{
			SetTriStateToNode(
				node,
				TreeViewAction.Unknown,
				stateImageIndex,
				raiseEvents );

			foreach ( TreeNode cn in node.Nodes )
			{
				DoApplyStateImageIndexToAllNodes(
					cn,
					stateImageIndex,
					raiseEvents );
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.TreeView.NodeMouseClick"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeNodeMouseClickEventArgs"></see> that contains the event data.</param>
		protected override void OnNodeMouseClick(
			TreeNodeMouseClickEventArgs e )
		{
			base.OnNodeMouseClick( e );

			if ( UseTriStateCheckBoxes )
			{
				if ( e.Button == MouseButtons.Left )
				{
					TreeViewHitTestInfo htInfo = HitTest( e.Location );

					if ( htInfo.Location ==
						TreeViewHitTestLocations.StateImage )
					{
						ToggleTriStateCheck(
							e.Node,
							TreeViewAction.ByMouse );
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
					if ( SelectedNode != null )
					{
						ToggleTriStateCheck(
							SelectedNode,
							TreeViewAction.ByKeyboard );
					}
				}
			}
		}

		/// <summary>
		/// Toggles the tri state check.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="action">The action.</param>
		private void ToggleTriStateCheck(
			TreeNode node,
			TreeViewAction action )
		{
			if ( node.StateImageIndex == TriStateUncheckedStateImageIndex )
			{
				SetTriStateToNode(
				node,
				action,
				TriStateCheckedStateImageIndex,
				true );
			}
			else if ( node.StateImageIndex == TriStateIndeterminateStateImageIndex &&
				indeterminateToChecked )
			{
				SetTriStateToNode(
				node,
				action,
				TriStateCheckedStateImageIndex,
				true );
			}
			else
			{
				SetTriStateToNode(
				node,
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
		/// Applies the given state to the given node.
		/// Raises events, too.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="action">The action.</param>
		/// <param name="stateImageIndex">Index of the state image.</param>
		/// <param name="raiseEvents">if set to <c>true</c> [raise events].</param>
		private void SetTriStateToNode(
			TreeNode node,
			TreeViewAction action,
			int stateImageIndex,
			bool raiseEvents )
		{
			bool canSet = true;

			if ( raiseEvents )
			{
				TreeViewCancelEventArgs args =
					new TreeViewCancelEventArgs( node, false, action );

				OnBeforeCheck( args );

				TreeViewCancelTriStateEventArgs triArgs =
					new TreeViewCancelTriStateEventArgs(
					node,
					args.Cancel,
					action,
					node.StateImageIndex,
					stateImageIndex );

				OnBeforeTriStateCheck( triArgs );

				canSet = !triArgs.Cancel;
			}

			if ( canSet )
			{
				// Actually set.
				// THIS IS THE ONLY PLACE to set.
				node.StateImageIndex = stateImageIndex;

				if ( raiseEvents )
				{
					TreeViewEventArgs args =
						new TreeViewEventArgs( node, action );

					OnAfterCheck( args );

					TreeViewTriStateEventArgs triArgs =
						new TreeViewTriStateEventArgs(
						node,
						action,
						node.StateImageIndex );

					OnAfterTriStateCheck( triArgs );
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="E:BeforeTriStateCheck"/> event.
		/// </summary>
		/// <param name="args">The <see cref="ZetaLib.Windows.Controls.TriStateCheckBoxesTreeView.TreeViewCancelTriStateEventArgs"/> instance containing the event data.</param>
		protected virtual void OnBeforeTriStateCheck(
			TreeViewCancelTriStateEventArgs args )
		{
			if ( BeforeTriStateCheck != null )
			{
				BeforeTriStateCheck( this, args );
			}
		}

		/// <summary>
		/// Raises the <see cref="E:AfterTriStateCheck"/> event.
		/// </summary>
		/// <param name="args">The <see cref="ZetaLib.Windows.Controls.TriStateCheckBoxesTreeView.TreeViewTriStateEventArgs"/> instance containing the event data.</param>
		protected virtual void OnAfterTriStateCheck(
			TreeViewTriStateEventArgs args )
		{
			if ( AfterTriStateCheck != null )
			{
				AfterTriStateCheck( this, args );
			}

			CheckPerformAutoStepsAfterTriStateCheck( args );
		}

		/// <summary>
		/// If configured, some auto-steps could be performed after a check.
		/// </summary>
		/// <param name="args">The <see cref="ZetaLib.Windows.Controls.TriStateCheckBoxesTreeView.TreeViewTriStateEventArgs"/> instance containing the event data.</param>
		private void CheckPerformAutoStepsAfterTriStateCheck(
			TreeViewTriStateEventArgs args )
		{
			if ( args.Action == TreeViewAction.ByKeyboard ||
				args.Action == TreeViewAction.ByMouse )
			{
				if ( AutoCheckChilds )
				{
					if ( args.CheckState == CheckState.Checked )
					{
						ChangeNodesCheckStates(
							args.Node,
							TriStateCheckBoxesTreeView.NodesCheckState.
							CheckAllChilds );
					}
					else if ( args.CheckState == CheckState.Unchecked )
					{
						ChangeNodesCheckStates(
							args.Node,
							TriStateCheckBoxesTreeView.NodesCheckState.
							UncheckAllChilds );
					}
				}

				// --

				if ( AutoCheckParents )
				{
					ChangeNodesCheckStates(
						args.Node,
						TriStateCheckBoxesTreeView.NodesCheckState.
						UpdateParentStates );
				}
			}
		}

		/// <summary>
		/// Internal helper. Do not access directly but through the
		/// CheckedTriStateImageList property instead.
		/// </summary>
		private static ImageList _internalCheckedTriStateImageList = null;

		/// <summary>
		/// Access an image list with "CHECKED", "UNCHECKED" and
		/// "INDETERMINATE" button states.
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
						GenerateCheckedTriStateImageList();
				}

				return _internalCheckedTriStateImageList;
			}
		}

		/// <summary>
		/// Generates the checked tri state image list.
		/// </summary>
		/// <returns></returns>
		/// Creates an image list with "checked", "unchecked" and
		/// "tri state" button states.
		internal static ImageList GenerateCheckedTriStateImageList()
		{
			const int width = 16;
			const int height = 16;

			ImageList result = new ImageList();
			result.ColorDepth = ColorDepth.Depth32Bit;
			result.ImageSize = new Size( width, height );

			bool success = false;

			// Draw with theming.
			if ( Application.RenderWithVisualStyles )
			{

				for ( int i = 0; i < 3; i++ )
				{
					Bitmap bmp = new Bitmap( width, height );
					using ( Graphics g = Graphics.FromImage( bmp ) )
					{
						switch ( i )
						{
							case 0:
								CheckBoxRenderer.DrawCheckBox( g, new Point( 1, 1 ), System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal );
								bmp.Tag = CheckState.Unchecked;
								break;
							case 1:
								CheckBoxRenderer.DrawCheckBox( g, new Point( 1, 1 ), System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal );
								bmp.Tag = CheckState.Checked;
								break;
							case 2:
								CheckBoxRenderer.DrawCheckBox( g, new Point( 1, 1 ), System.Windows.Forms.VisualStyles.CheckBoxState.MixedNormal );
								bmp.Tag = CheckState.Indeterminate;
								break;
						}
					}
					result.Images.Add( bmp );
				}
				success = true;
			}

			// Draw with no theming.
			if ( !success )
			{
				for ( int i = 0; i < 3; i++ )
				{
					Bitmap bmp = new Bitmap( width, height );
					using ( Graphics g = Graphics.FromImage( bmp ) )
					{
						Rectangle rect = new Rectangle( 0, 0, width, height );
						switch ( i )
						{
							case 0:
								ControlPaint.DrawCheckBox( g, rect, ButtonState.Normal | ButtonState.Flat );
								bmp.Tag = CheckState.Unchecked;
								break;
							case 1:
								ControlPaint.DrawCheckBox( g, rect, ButtonState.Checked | ButtonState.Flat );
								bmp.Tag = CheckState.Checked;
								break;
							case 2:
								ControlPaint.DrawCheckBox( g, rect, ButtonState.Checked | ButtonState.Flat | ButtonState.Inactive );
								bmp.Tag = CheckState.Indeterminate;
								break;
						}
					}
					result.Images.Add( bmp );
				}
			}

			return result;
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

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private bool autoCheckParents = true;

		/// <summary>
		/// 
		/// </summary>
		private bool autoCheckChilds = true;

		/// <summary>
		/// http://www.codeproject.com/useritems/ZetaLibNet.asp?msg=1734376#xx1734376xx.
		/// </summary>
		private bool indeterminateToChecked = true;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}