namespace ZetaLib.Core.Logging
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Globalization;
	using System.Net;
	using System.IO;
	using System.Reflection;
	using System.Collections.Generic;
	using ZetaLib.Core.Collections;
	using System.Net.Sockets;
	using System.ComponentModel;
	using System.Diagnostics;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// 
	/// </summary>
	public class LoggingInformationNeedMoreInfoEventArgs :
		EventArgs
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public LoggingInformationNeedMoreInfoEventArgs()
		{
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets or sets the more information.
		/// </summary>
		/// <value>The more information.</value>
		public string MoreInformation
		{
			get
			{
				return moreInformation;
			}
			set
			{
				moreInformation = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private string moreInformation;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}