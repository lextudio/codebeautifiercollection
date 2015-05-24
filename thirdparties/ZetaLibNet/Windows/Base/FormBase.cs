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
	/// Base class for forms.
	/// </summary>
	public class FormBase :
		Form
	{
		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets a value indicating whether this instance is design mode.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is design mode; otherwise, <c>false</c>.
		/// </value>
		public bool IsDesignMode
		{
			get
			{
				return base.DesignMode;
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
				return
					base.DesignMode ||
					UserControlBase.IsDesignMode( this );
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}