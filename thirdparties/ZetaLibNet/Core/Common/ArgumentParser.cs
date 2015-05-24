namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Persing of arguments.
	/// </summary>
	public class ArgumentParser
	{
		#region Public routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="args">The args.</param>
		public ArgumentParser( 
			string[] args )
		{
			this.args = args;
		}

		/// <summary>
		/// Checks for the presence of a name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>
		/// 	<c>true</c> if [has] [the specified name]; otherwise, <c>false</c>.
		/// </returns>
		public bool Has( 
			string name )
		{
			foreach ( string arg in args )
			{
				if ( arg==name )
				{
					return true;
				}
			}

			// if here, not found.
			return false;
		}

		// ------------------------------------------------------------------
		#endregion
		
		#region Private member.
		// ------------------------------------------------------------------

		private string[] args;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}