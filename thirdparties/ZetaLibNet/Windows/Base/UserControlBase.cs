namespace ZetaLib.Windows.Base
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Windows.Forms;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Base class for user controls.
	/// </summary>
	public class UserControlBase :
		UserControl
	{
		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Checks whether a control or its parent is in design mode.
		/// </summary>
		/// <param name="c">The control to check.</param>
		/// <returns>
		/// Returns TRUE if in design mode, FALSE otherwise.
		/// </returns>
		public static bool IsDesignMode(
			Control c )
		{
			if ( c == null )
			{
				return
					ZetaLib.Windows.
					LibraryConfiguration.IsDesignMode;
			}
			else
			{
				if ( (c is FormBase) && (c as FormBase).IsDesignMode )
				{
					return true;
				}
				else
				{
					Form f = c.FindForm();

					if ( (f is FormBase) && (f as FormBase).IsDesignMode )
					{
						return true;
					}
					else
					{
						while ( c != null )
						{
							if ( c.Site != null && c.Site.DesignMode )
							{
								return true;
							}
							else
							{
								c = c.Parent;
							}
						}

						return
							ZetaLib.Windows.
							LibraryConfiguration.IsDesignMode;
					}
				}
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the <see cref="T:System.ComponentModel.Component"></see> is currently in design mode.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.ComponentModel.Component"></see> is in design mode; otherwise, false.</returns>
		public new bool DesignMode
		{
			get
			{
				return base.DesignMode || IsDesignMode( this );
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}