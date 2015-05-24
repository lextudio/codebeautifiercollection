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

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Controls implementing this interface get called upon 
	/// FormHelper.SaveState() and FormHelper.RestoreState() to allow them
	/// to store additional values.
	/// </summary>
	public interface ISaveRestoreState
	{
		#region Interface methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Called after saving values from the control to the storage.
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="prefix">The prefix.</param>
		void OnSaveState(
			IPersistentPairStorage storage,
			string prefix );

		/// <summary>
		/// Called after restoring values from the storage to the control.
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="prefix">The prefix.</param>
		void OnRestoreState(
			IPersistentPairStorage storage,
			string prefix );

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}