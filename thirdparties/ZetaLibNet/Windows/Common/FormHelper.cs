namespace ZetaLib.Windows.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Diagnostics;
	using System.Windows.Forms;
	using ZetaLib.Core.Common;
	using ZetaLib.Core.Collections;
	using System.Collections.Generic;
	using ZetaLib.Windows.Properties;
	using System.Runtime.Serialization;
	using ZetaLib.Core.Logging;
	using ZetaLib.Core.Localization;
	using System.Drawing;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Class with helper routines for Windows Forms applicatins.
	/// </summary>
	public sealed class FormHelper
	{
		#region Saving window states and values.
		// ------------------------------------------------------------------

		/// <summary>
		/// Saves the state of a control.
		/// </summary>
		/// <param name="c">The c.</param>
		public static void SaveState(
			Control c )
		{
			SaveState( Storage, c );
		}

		/// <summary>
		/// Saves the state of a control.
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="c">The c.</param>
		public static void SaveState(
			IPersistentPairStorage storage,
			Control c )
		{
			string prefix = c.Name;

			SaveValue( storage, prefix + @".Left", c.Left );
			SaveValue( storage, prefix + @".Top", c.Top );
			SaveValue( storage, prefix + @".Width", c.Width );
			SaveValue( storage, prefix + @".Height", c.Height );

			if ( c is Splitter )
			{
				SaveValue(
					storage,
					prefix + @".SplitPosition",
					((Splitter)c).SplitPosition );
			}
			else if ( c is Form )
			{
				Form s = c as Form;
				SaveState( s );
			}
			else if ( c is SplitContainer )
			{
				SplitContainer s = c as SplitContainer;
				SaveState( s );
			}
			else if ( c is TabControl )
			{
				SaveValue(
					storage,
					prefix + @".SelectedIndex",
					((TabControl)c).SelectedIndex );
			}
			else if ( c is ListView )
			{
				ListView listView = c as ListView;

				SaveValue(
					storage,
					prefix + @".Columns.Count",
					listView.Columns.Count );

				for ( int i = 0; i < listView.Columns.Count; ++i )
				{
					SaveValue(
						storage,
						prefix + @".Columns." + (i + 1) + @".Width",
						listView.Columns[i].Width );
				}

				// --

				SaveValue(
					storage,
					prefix + @".SelectedIndexes.Count",
					listView.SelectedIndices.Count );

				for ( int i = 0; i < listView.SelectedIndices.Count; ++i )
				{
					SaveValue(
						storage,
						prefix + @".SelectedIndexes." + (i + 1) + @".Index",
						listView.SelectedIndices[i] );
				}
			}
			else if ( c is TextBox )
			{
				TextBox t = c as TextBox;

				SaveValue( storage, prefix + @".Text", t.Text );
			}
			else if ( c is CheckBox )
			{
				CheckBox cb = c as CheckBox;

				SaveValue(
					storage,
					prefix + @".CheckState",
					cb.CheckState.ToString() );
			}
			else if ( c is RadioButton )
			{
				RadioButton rb = c as RadioButton;

				SaveValue(
					storage,
					prefix + @".Checked",
					rb.Checked );
			}
			else if ( c is DateTimePicker )
			{
				DateTimePicker rb = c as DateTimePicker;

				SaveValue(
					storage,
					prefix + @".Value",
					rb.Value );
				SaveValue(
					storage,
					prefix + @".Checked",
					rb.Checked );
			}
			else if ( c is CheckedListBox )
			{
				CheckedListBox clb = c as CheckedListBox;

				List<int> selectedIndices = new List<int>();

				foreach ( int si in clb.CheckedIndices )
				{
					selectedIndices.Add( si );
				}

				string sis = StringHelper.SerializeToString(
					selectedIndices.ToArray() );

				SaveValue(
					storage,
					prefix + @".CheckedIndices",
					sis );
			}
			else if ( c is ComboBox )
			{
				ComboBox rb = c as ComboBox;

				SaveValue(
					storage,
					prefix + @".SelectedIndex", rb.SelectedIndex );

				if ( rb.DropDownStyle == ComboBoxStyle.DropDown )
				{
					SaveValue( storage, prefix + @".Text", rb.Text );
				}
			}
			else
			{
				Debug.Assert(
					false,
					string.Format(
					@"Trying to save the state of an unknown control type: '{0}'.",
					c.GetType() ) );
			}

			// --

			if ( c is ISaveRestoreState )
			{
				ISaveRestoreState srs = c as ISaveRestoreState;

				if ( !protectRecursion.Contains( srs ) )
				{
					protectRecursion.Add( srs );
					try
					{
						srs.OnSaveState( storage, prefix );
					}
					finally
					{
						protectRecursion.Remove( srs );
					}
				}
			}
		}

		/// <summary>
		/// Persist a name-value pair inside the registry.
		/// Does this on a per-user-basis.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		public static void SaveValue(
			string name,
			object val )
		{
			SaveValue( Storage, name, val );
		}

		/// <summary>
		/// Persist a name-value pair inside the registry.
		/// Does this on a per-user-basis.
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		public static void SaveValue(
			IPersistentPairStorage storage,
			string name,
			object val )
		{
			storage.PersistValue( name, val );
		}

		/// <summary>
		/// Persist a name-value pair inside the registry.
		/// Does this on a per-user-basis.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		public static void SerializeValue(
			string name,
			object val )
		{
			SerializeValue( storage, name, val );
		}

		/// <summary>
		/// Persist a name-value pair inside the registry.
		/// Does this on a per-user-basis.
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		public static void SerializeValue(
			IPersistentPairStorage storage,
			string name,
			object val )
		{
			if ( val == null )
			{
				SaveValue( storage, name, null );
			}
			else
			{
				SaveValue(
					storage,
					name,
					StringHelper.SerializeToString( val ) );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Restoring window states and values.
		// ------------------------------------------------------------------

		/// <summary>
		/// Restores the state of a control.
		/// </summary>
		/// <param name="c">The c.</param>
		public static void RestoreState(
			Control c )
		{
			RestoreState( Storage, c, null );
		}

		/// <summary>
		/// Restores the state of a control.
		/// Works for list views, too (column widths).
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="c">The c.</param>
		public static void RestoreState(
			IPersistentPairStorage storage,
			Control c )
		{
			RestoreState( storage, c, null );
		}

		/// <summary>
		/// Restores the state of a control.
		/// </summary>
		/// <param name="c">The c.</param>
		/// <param name="info">The info.</param>
		public static void RestoreState(
			Control c,
			RestoreInformation info )
		{
			RestoreState( Storage, c, info );
		}

		/// <summary>
		/// Restores the state of a control.
		/// Works for list views, too (column widths).
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="c">The c.</param>
		/// <param name="info">The info.</param>
		public static void RestoreState(
			IPersistentPairStorage storage,
			Control c,
			RestoreInformation info )
		{
			string prefix = c.Name;

			object o = null;

			if ( c is Splitter )
			{
				o = RestoreValue( storage, prefix + @".SplitPosition" );
				if ( o != null )
				{
					((Splitter)c).SplitPosition = Convert.ToInt32( o );
				}
			}
			else if ( c is Form )
			{
				Form s = c as Form;
				RestoreState( storage, s, info );
			}
			else if ( c is SplitContainer )
			{
				SplitContainer s = c as SplitContainer;
				RestoreState( storage, s );
			}
			else if ( c is TabControl )
			{
				o = RestoreValue( storage, prefix + @".SelectedIndex" );
				if ( o != null )
				{
					((TabControl)c).SelectedIndex = Convert.ToInt32( o );
				}
			}
			else if ( c is ListView )
			{
				ListView listView = c as ListView;

				o = RestoreValue( storage, prefix + @".Columns.Count" );

				if ( o != null )
				{
					int count = Convert.ToInt32( o );
					for ( int i = 0; i < count; ++i )
					{
						o = RestoreValue( storage, prefix + @".Columns." + (i + 1) + @".Width" );

						if ( o != null && i < listView.Columns.Count )
						{
							listView.Columns[i].Width = Convert.ToInt32( o );
						}
					}
				}

				// --

				listView.SelectedItems.Clear();

				o = RestoreValue( storage, prefix + @".SelectedIndexes.Count" );

				if ( o != null )
				{
					int count = Convert.ToInt32( o );
					for ( int i = 0; i < count; ++i )
					{
						o = RestoreValue( storage, prefix + @".SelectedIndexes." + (i + 1) + @".Index" );

						if ( o != null )
						{
							int index = ConvertHelper.ToInt32( o );

							if ( index < listView.Items.Count )
							{
								listView.Items[index].Checked = true;
							}
						}
					}
				}
			}
			else if ( c is TextBox )
			{
				TextBox t = c as TextBox;

				o = RestoreValue( storage, prefix + @".Text" );
				if ( o != null )
				{
					t.Text = o as string;
				}
			}
			else if ( c is CheckBox )
			{
				CheckBox cb = c as CheckBox;

				o = RestoreValue( storage, prefix + @".CheckState" );
				if ( o != null )
				{
					cb.CheckState = (CheckState)Enum.Parse(
						typeof( CheckState ),
						o as string,
						true );
				}
			}
			else if ( c is RadioButton )
			{
				RadioButton rb = c as RadioButton;

				o = RestoreValue( storage, prefix + @".Checked" );
				if ( o != null )
				{
					rb.Checked = ConvertHelper.ToBoolean( o );
				}
			}
			else if ( c is DateTimePicker )
			{
				DateTimePicker rb = c as DateTimePicker;

				o = RestoreValue( storage, prefix + @".Value" );
				if ( o != null )
				{
					rb.Value = ConvertHelper.ToDateTime( o );
				}
				o = RestoreValue( storage, prefix + @".Checked" );
				if ( o != null )
				{
					rb.Checked = ConvertHelper.ToBoolean( o );
				}
			}
			else if ( c is CheckedListBox )
			{
				CheckedListBox clb = c as CheckedListBox;

				o = RestoreValue( storage, prefix + @".CheckedIndices" );
				if ( o is string )
				{
					o = StringHelper.DeserializeFromString( o as string );

					if ( o is int[] )
					{
						List<int> selectedIndices =
							new List<int>( o as int[] );

						for ( int i = 0; i < clb.Items.Count; i++ )
						{
							if ( selectedIndices.Contains( i ) )
							{
								clb.SetItemChecked( i, true );
							}
							else
							{
								clb.SetItemChecked( i, false );
							}
						}
					}
				}
			}
			else if ( c is ComboBox )
			{
				ComboBox rb = c as ComboBox;

				o = RestoreValue( storage, prefix + @".SelectedIndex" );
				if ( o != null )
				{
					int index = ConvertHelper.ToInt32( o );

					if ( index <= rb.Items.Count - 1 )
					{
						rb.SelectedIndex = index;
					}
				}

				if ( rb.DropDownStyle == ComboBoxStyle.DropDown )
				{
					rb.Text = RestoreValue( storage, prefix + @".Text" ) as string;
				}
			}
			else
			{
				Debug.Assert(
					false,
					string.Format(
					@"Trying to restore the state of an unknown control type: '{0}'.",
					c.GetType() ) );
			}

			// --

			if ( c is ISaveRestoreState )
			{
				ISaveRestoreState srs = c as ISaveRestoreState;

				if ( !protectRecursion.Contains( srs ) )
				{
					protectRecursion.Add( srs );
					try
					{
						srs.OnSaveState( storage, prefix );
					}
					finally
					{
						protectRecursion.Remove( srs );
					}
				}
			}
		}

		/// <summary>
		/// The opposite to "SaveValue".
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>Returns null if not found.</returns>
		public static object RestoreValue(
			string name )
		{
			return RestoreValue( Storage, name );
		}

		/// <summary>
		/// The opposite to "SaveValue".
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="name">The name.</param>
		/// <returns>Returns null if not found.</returns>
		public static object RestoreValue(
			IPersistentPairStorage storage,
			string name )
		{
			return RestoreValue( storage, name, null );
		}

		/// <summary>
		/// The opposite to "SaveValue".
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="fallBackValue">The fall back value.</param>
		/// <returns>Returns null if not found.</returns>
		public static object RestoreValue(
			string name,
			object fallBackValue )
		{
			return RestoreValue( Storage, name, fallBackValue );
		}

		/// <summary>
		/// The opposite to "SaveValue".
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="name">The name.</param>
		/// <param name="fallBackValue">The fall back value.</param>
		/// <returns>Returns null if not found.</returns>
		public static object RestoreValue(
			IPersistentPairStorage storage,
			string name,
			object fallBackValue )
		{
			return storage.RetrieveValue( name, fallBackValue );
		}

		/// <summary>
		/// The opposite to "SerializeValue".
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="fallBackValue">The fall back value.</param>
		/// <returns>Returns NULL if not found.</returns>
		public static object DeserializeValue(
			string name,
			object fallBackValue )
		{
			return DeserializeValue( Storage, name, fallBackValue );
		}

		/// <summary>
		/// The opposite to "SerializeValue".
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="name">The name.</param>
		/// <param name="fallBackValue">The fall back value.</param>
		/// <returns>Returns NULL if not found.</returns>
		public static object DeserializeValue(
			IPersistentPairStorage storage,
			string name,
			object fallBackValue )
		{
			try
			{
				object o = RestoreValue( storage, name );

				if ( o == null || !(o is string) )
				{
					return fallBackValue;
				}
				else
				{
					return StringHelper.DeserializeFromString( o.ToString() );
				}
			}
			catch ( SerializationException x )
			{
				LogCentral.Current.LogDebug(
					@"Silently caught SerializationException, returning fallback value.",
					x );
				return fallBackValue;
			}
		}

		/// <summary>
		/// The opposite to "SerializeValue".
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>Returns NULL if not found.</returns>
		public static object DeserializeValue(
			string name )
		{
			return DeserializeValue( Storage, name );
		}

		/// <summary>
		/// The opposite to "SerializeValue".
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="name">The name.</param>
		/// <returns>Returns NULL if not found.</returns>
		public static object DeserializeValue(
			IPersistentPairStorage storage,
			string name )
		{
			object o = RestoreValue( storage, name );

			if ( o == null || !(o is string) )
			{
				return null;
			}
			else
			{
				return StringHelper.DeserializeFromString( o.ToString() );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Resizing the columns of a list view.
		// ------------------------------------------------------------------

		/// <summary>
		/// Resizes the columns of a given list view according to the passed
		/// column widths.
		/// </summary>
		/// <param name="listView">The list view.</param>
		public static void ResizeListViewColumns(
			ListView listView )
		{
			ResizeListViewColumns( listView, new ListViewSizeOptions() );
		}

		/// <summary>
		/// Resizes the columns of a given list view according to the passed
		/// column widths.
		/// </summary>
		/// <param name="listView">The list view.</param>
		/// <param name="columnToStretch">The column to stretch.</param>
		public static void ResizeListViewColumns(
			ListView listView,
			int columnToStretch )
		{
			ResizeListViewColumns( listView, columnToStretch, new ListViewSizeOptions() );
		}

		/// <summary>
		/// Resizes the columns of a given list view according to the passed
		/// column widths.
		/// </summary>
		/// <param name="listView">The list view.</param>
		/// <param name="sizes">The sizes.</param>
		public static void ResizeListViewColumns(
			ListView listView,
			params double[] sizes )
		{
			ResizeListViewColumns( listView, new ListViewSizeOptions(), sizes );
		}

		/// <summary>
		/// Resizes the columns of a given list view according to the passed
		/// column widths.
		/// </summary>
		/// <param name="listView">The list view.</param>
		/// <param name="options">The options.</param>
		public static void ResizeListViewColumns(
			ListView listView,
			ListViewSizeOptions options )
		{
			ResizeListViewColumns( listView, listView.Columns.Count - 1, options );
		}

		/// <summary>
		/// Resizes the columns of a given list view according to the passed
		/// column widths.
		/// </summary>
		/// <param name="listView">The list view.</param>
		/// <param name="columnToStretch">The column to stretch.</param>
		/// <param name="options">The options.</param>
		public static void ResizeListViewColumns(
			ListView listView,
			int columnToStretch,
			ListViewSizeOptions options )
		{
			int width = listView.ClientRectangle.Width;

			if ( options.SubtractVerticalScrollBarWidth )
			{
				width -= VerticalScrollBarWidth;
			}

			width -= DefaultSubtractWidth;

			// The widths of all but the column to stretch.
			for ( int i = 0; i < listView.Columns.Count; ++i )
			{
				if ( i != columnToStretch )
				{
					width -= listView.Columns[i].Width;
				}
			}

			listView.Columns[columnToStretch].Width = width;
		}

		/// <summary>
		/// Resizes the columns of a given list view according to the passed
		/// column widths.
		/// </summary>
		/// <param name="listView">The list view.</param>
		/// <param name="options">The options.</param>
		/// <param name="sizes">Pass a value smaller than 1.0 to specify
		/// the value in percent (0.30 e.g. means 30%), pass a value
		/// of 0.0 to not size the column.</param>
		public static void ResizeListViewColumns(
			ListView listView,
			ListViewSizeOptions options,
			params double[] sizes )
		{
			if ( sizes.Length != listView.Columns.Count )
			{
				throw new ArgumentException(
					LocalizationHelper.Format(
					Resources.Str_ZetaLib_Windows_Common_FormHelper_01,
					LocalizationHelper.CreatePair( @"ColumnCount", listView.Columns.Count ),
					LocalizationHelper.CreatePair( @"SizedCount", sizes.Length ) ),
					@"sizes" );
			}
			else
			{
				// Avoid endless calls.
				if ( !insideSizes.Contains( listView ) )
				{
					insideSizes.Add( listView );

					try
					{
						int width = listView.ClientRectangle.Width;
						if ( options.SubtractVerticalScrollBarWidth )
						{
							width -= VerticalScrollBarWidth;
						}

						width -= DefaultSubtractWidth;

						// First pass - subtract all absolute sizes.
						for ( int i = 0; i < listView.Columns.Count; ++i )
						{
							double size = sizes[i];

							if ( size == 0.0 )
							{
								width -= listView.Columns[i].Width;
							}
							else if ( size <= 1.0 )
							{
								// Do nothing.
							}
							else
							{
								width -= Convert.ToInt32( size );
							}
						}

						// Second pass - actually resize.
						for ( int i = 0; i < listView.Columns.Count; ++i )
						{
							double size = sizes[i];

							if ( size <= 0.0001 )
							{
								// Do nothing.
							}
							else if ( size <= 1.0 )
							{
								// Size percentual.
								int theWidth =
									Convert.ToInt32( (((double)width) * size) );
								ColumnHeader col = listView.Columns[i];
								col.Width = theWidth;
							}
							else
							{
								// Size absolute.
								listView.Columns[i].Width =
									Convert.ToInt32( size );
							}
						}
					}
					finally
					{
						insideSizes.Remove( listView );
					}
				}
			}
		}

		/// <summary>
		/// Avoid duplicates.
		/// </summary>
		private static Set<ListView> insideSizes = new Set<ListView>();

		/// <summary>
		/// Gets the width of the vertical scroll bar.
		/// </summary>
		/// <value>The width of the vertical scroll bar.</value>
		private static int VerticalScrollBarWidth
		{
			get
			{
				return 0;
				/*return SystemInformation.VerticalScrollBarWidth;*/
			}
		}

		/// <summary>
		/// Always subtract a little bit to ensure those randomly appearing
		/// artificial scrollbars disappear.
		/// </summary>
		private const int DefaultSubtractWidth = 4;

		// ------------------------------------------------------------------
		#endregion

		#region ComboBox methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets the selected combo box item's value.
		/// </summary>
		/// <param name="cb">The cb.</param>
		/// <returns></returns>
		public static string GetSelectedComboBoxValue(
			ComboBox cb )
		{
			if ( cb.SelectedItem == null )
			{
				return null;
			}
			else
			{
				if ( cb.SelectedItem is StringPair )
				{
					return (cb.SelectedItem as StringPair).Value;
				}
				else
				{
					return cb.SelectedItem.ToString();
				}
			}
		}

		/// <summary>
		/// Select a combobox item by its value.
		/// </summary>
		/// <param name="cb">The cb.</param>
		/// <param name="val">The val.</param>
		public static void SelectComboBoxByValue(
			ComboBox cb,
			string val )
		{
			if ( val != null )
			{
				foreach ( object o in cb.Items )
				{
					if ( o is StringPair )
					{
						StringPair sp = o as StringPair;

						if ( sp.Value.ToString().ToLower() == val.ToLower() )
						{
							cb.SelectedItem = o;
							break;
						}
					}
					else
					{
						if ( o.ToString().ToLower() == val.ToLower() )
						{
							cb.SelectedItem = o;
							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Select a combobox item by its value.
		/// </summary>
		/// <param name="cb">The cb.</param>
		/// <param name="val">The val.</param>
		public static void SelectComboBoxByValue(
			ComboBox cb,
			object val )
		{
			if ( val != null )
			{
				foreach ( object o in cb.Items )
				{
					if ( o.Equals( val ) )
					{
						cb.SelectedItem = o;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Select a combobox item by its name.
		/// </summary>
		/// <param name="cb">The cb.</param>
		/// <param name="val">The val.</param>
		public static void SelectComboBoxByName(
			ComboBox cb,
			string val )
		{
			foreach ( object o in cb.Items )
			{
				if ( o is StringPair )
				{
					StringPair sp = o as StringPair;

					if ( sp.Name.ToString().ToLower() == val.ToLower() )
					{
						cb.SelectedItem = o;
						break;
					}
				}
				else
				{
					if ( o.ToString().ToLower() == val.ToLower() )
					{
						cb.SelectedItem = o;
						break;
					}
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private saving overloads.
		// ------------------------------------------------------------------

		/// <summary>
		/// Saves the state of a form.
		/// </summary>
		/// <param name="c">The c.</param>
		private static void SaveState(
			Form c )
		{
			SaveState( Storage, c );
		}

		/// <summary>
		/// Saves the state of a form.
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="c">The c.</param>
		private static void SaveState(
			IPersistentPairStorage storage,
			Form c )
		{
			string prefix = c.Name;

			SaveValue( storage, prefix + @".Left", c.Left );
			SaveValue( storage, prefix + @".Top", c.Top );
			SaveValue( storage, prefix + @".Width", c.Width );
			SaveValue( storage, prefix + @".Height", c.Height );
			SaveValue( storage, prefix + @".WindowState", Convert.ToInt32( c.WindowState ) );
		}

		/// <summary>
		/// Saves the state of a control.
		/// </summary>
		/// <param name="c">The c.</param>
		private static void SaveState(
			SplitContainer c )
		{
			SaveState( Storage, c );
		}

		/// <summary>
		/// Saves the state of a control.
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="c">The c.</param>
		private static void SaveState(
			IPersistentPairStorage storage,
			SplitContainer c )
		{
			int realDistance = 0;
			if ( c.Orientation == Orientation.Vertical )
			{
				if ( c.FixedPanel == FixedPanel.Panel1 )
				{
					realDistance = c.SplitterDistance;
				}
				else
				{
					Debug.Assert(
						c.FixedPanel == FixedPanel.Panel2,
						@"FixedPanel must be Panel2." );

					realDistance = c.Width - c.SplitterDistance;
				}
			}
			else
			{
				Debug.Assert(
					c.Orientation == Orientation.Horizontal,
					@"Orientation must be horizontal." );

				if ( c.FixedPanel == FixedPanel.Panel1 ||
					c.FixedPanel == FixedPanel.None )
				{
					realDistance = c.SplitterDistance;
				}
				else
				{
					Debug.Assert(
						c.FixedPanel == FixedPanel.Panel2,
						@"FixedPanel must be Panel2." );

					realDistance = c.Height - c.SplitterDistance;
				}
			}

			// --

			string prefix = c.Name;

			SaveValue( storage, prefix + @"SplitterDistance", c.SplitterDistance );
			SaveValue( storage, prefix + @"RealSplitterDistance", realDistance );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private restoring overloads.
		// ------------------------------------------------------------------

		/// <summary>
		/// Restores the state of a control.
		/// </summary>
		/// <param name="c">The c.</param>
		private static void RestoreState(
			Form c )
		{
			RestoreState( Storage, c );
		}

		/// <summary>
		/// Restores the state of a control.
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="c">The c.</param>
		/// <param name="info">The info.</param>
		private static void RestoreState(
			IPersistentPairStorage storage,
			Form c,
			RestoreInformation info )
		{
			// If child, center to parent.
			bool centerParent = !c.TopLevel || c.ParentForm != null;

			bool hasAnythingRestored = false;

			string prefix = c.Name;

			object o = null;
			if ( !centerParent )
			{
				o = RestoreValue( storage, prefix + @".Left" );
				if ( o != null )
				{
					hasAnythingRestored = true;
					c.Left = Convert.ToInt32( o );
				}
				o = RestoreValue( storage, prefix + @".Top" );
				if ( o != null )
				{
					hasAnythingRestored = true;
					c.Top = Convert.ToInt32( o );
				}
			}
			o = RestoreValue( storage, prefix + @".Width" );
			if ( o != null )
			{
				hasAnythingRestored = true;
				c.Width = Convert.ToInt32( o );
			}
			o = RestoreValue( storage, prefix + @".Height" );
			if ( o != null )
			{
				hasAnythingRestored = true;
				c.Height = Convert.ToInt32( o );
			}

			o = RestoreValue( storage, prefix + @".WindowState" );
			if ( o != null )
			{
				hasAnythingRestored = true;
				FormWindowState state = (FormWindowState)Convert.ToInt32( o );

				// Don't allow to start minimized.
				if ( state != c.WindowState &&
					state != FormWindowState.Minimized )
				{
					if ( c.StartPosition != FormStartPosition.Manual )
					{
						c.StartPosition = FormStartPosition.Manual;
					}
					c.WindowState = state;
				}

				if ( state == FormWindowState.Minimized )
				{
					c.WindowState = FormWindowState.Maximized;
				}
			}

			// --

			// 2007-01-28: Zoom if desired.
			if ( !hasAnythingRestored &&
				info != null &&
				info.SuggestZoomPercent > 0 )
			{
				double zoom = (double)info.SuggestZoomPercent / 100.0;

				Size screenSize = SystemInformation.WorkingArea.Size;
				Size windowSize = c.Size;

				int width;
				int height;

				if ( (double)screenSize.Width / (double)windowSize.Width <
					(double)screenSize.Height / (double)windowSize.Height )
				{
					// Scale in X.
					width = (int)((double)screenSize.Width * zoom);
					double fac = (double)width / (double)c.Width;
					height = (int)(fac * (double)c.Height);
				}
				else
				{
					// Scale in Y.
					height = (int)((double)screenSize.Height * zoom);
					double fac = (double)height / (double)c.Height;
					width = (int)(fac * (double)c.Width);
				}

				// Only apply if getting larger.
				if ( width > c.Width && height > c.Height )
				{
					c.Width = width;
					c.Height = height;
				}
			}

			// --

			// If child, center to parent.
			if ( centerParent )
			{
				c.StartPosition = FormStartPosition.CenterParent;
			}
		}

		/// <summary>
		/// Restores the state of a control.
		/// </summary>
		/// <param name="c">The c.</param>
		private static void RestoreState(
			SplitContainer c )
		{
			RestoreState( Storage, c );
		}

		/// <summary>
		/// Restores the state of a control.
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="c">The c.</param>
		private static void RestoreState(
			IPersistentPairStorage storage,
			SplitContainer c )
		{
			string prefix = c.Name;

			object o1 = RestoreValue( storage, prefix + @"SplitterDistance" );
			object o2 = RestoreValue( storage, prefix + @"RealSplitterDistance" );

			if ( o1 != null && o2 != null )
			{
				int distance = Convert.ToInt32( o1 );
				int realDistance = Convert.ToInt32( o2 );

				if ( c.Orientation == Orientation.Vertical )
				{
					if ( c.FixedPanel == FixedPanel.Panel1 ||
						c.FixedPanel == FixedPanel.None )
					{
						c.SplitterDistance = realDistance;
					}
					else
					{
						Debug.Assert(
							c.FixedPanel == FixedPanel.Panel2,
							@"FixedPanel must be Panel2." );

						if ( (c.Width - realDistance) > 0 )
						{
							c.SplitterDistance = c.Width - realDistance;
						}
					}
				}
				else
				{
					Debug.Assert(
						c.Orientation == Orientation.Horizontal,
						string.Format( @"Unknown orientation '{0}'.", c.Orientation ) );

					if ( c.FixedPanel == FixedPanel.Panel1 )
					{
						c.SplitterDistance = realDistance;
					}
					else
					{
						Debug.Assert(
							c.FixedPanel == FixedPanel.Panel2,
							@"You must set one panel inside a splitter to be fixed." );

						if ( (c.Height - realDistance) > 0 )
						{
							c.SplitterDistance = c.Height - realDistance;
						}
					}
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Persistance configuration.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private static object typeLock = new object();

		/// <summary>
		/// The storage is responsible for actual storing the values.
		/// </summary>
		/// <value>The storage.</value>
		public static IPersistentPairStorage Storage
		{
			get
			{
				lock ( typeLock )
				{
					return storage;
				}
			}
			set
			{
				lock ( typeLock )
				{
					storage = value;
				}
			}
		}

		/// <summary>
		/// The default is to the registry.
		/// </summary>
		private static IPersistentPairStorage storage =
			new PersistentRegistryPairStorage();

		/// <summary>
		/// Protect from being endless recursed.
		/// </summary>
		private static Set<ISaveRestoreState> protectRecursion =
			new Set<ISaveRestoreState>();

		// ------------------------------------------------------------------
		#endregion

		#region Miscellaneous helpers.
		// ------------------------------------------------------------------

		/// <summary>
		/// See http://www.codeproject.com/csharp/begininvoke.asp?msg=1613610#xx1613610xx
		/// Usage:
		/// private void SetLabelText( int number )
		/// {
		/// FormHelper.SyncInvoke( this, delegate()
		/// {
		/// label.Text = number.ToString();
		/// } );
		/// }
		/// </summary>
		/// <param name="control">The control.</param>
		/// <param name="del">The del.</param>
		public static void SyncInvoke(
			 Control control,
			 MethodInvoker del )
		{
			if ( (control != null) && control.InvokeRequired )
			{
				control.Invoke( del, null );
			}
			else
			{
				del();
			}
		}

		/// <summary>
		/// See http://www.codeproject.com/csharp/begininvoke.asp?msg=1613610#xx1613610xx
		/// Usage:
		/// private void SetLabelText( int number )
		/// {
		/// FormHelper.SyncBeginInvoke( this, delegate()
		/// {
		/// label.Text = number.ToString();
		/// } );
		/// }
		/// </summary>
		/// <param name="control">The control.</param>
		/// <param name="del">The del.</param>
		public static void SyncBeginInvoke(
			Control control,
			MethodInvoker del )
		{
			if ( (control != null) && control.InvokeRequired )
			{
				control.BeginInvoke( del, null );
			}
			else
			{
				del();
			}
		}

		/// <summary>
		/// Recursively traverses each control.
		/// </summary>
		/// <param name="parentControl">The parent control.</param>
		/// <param name="action">The action.</param>
		public static void IterateControls(
			Control parentControl,
			Action<Control> action )
		{
			if ( parentControl != null )
			{
				action( parentControl );

				IterateControls( parentControl.Controls, action );
			}
		}

		/// <summary>
		/// Recursively traverses each control.
		/// </summary>
		/// <param name="parentControls">The parent controls.</param>
		/// <param name="action">The action.</param>
		public static void IterateControls(
			Control.ControlCollection parentControls,
			Action<Control> action )
		{
			if ( parentControls != null )
			{
				foreach ( Control c in parentControls )
				{
					IterateControls( c, action );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}