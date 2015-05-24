namespace ZetaLib.Core.Base
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Configuration;
	using System.Diagnostics;
	using System.IO;
	using System.Reflection;
	using ZetaLib.Core.Common;
	using System.Collections.Generic;
	using ZetaLib.Core.Properties;
	using ZetaLib.Core.Localization;
	using ZetaLib.Core.Logging;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// The command of a task. A task can have one or multiple task commands
	/// that are usually closely related to each other.
	/// </summary>
	public class TaskCommand
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor. Constructs an empty task command.
		/// </summary>
		public TaskCommand()
		{
		}

		/// <summary>
		/// Constructor. Constructs a task command with a
		/// given symbolic name.
		/// </summary>
		/// <param name="symbolicName">Name of the symbolic.</param>
		public TaskCommand(
			string symbolicName )
		{
			this.symbolicName = symbolicName;
		}

		/// <summary>
		/// Constructor. Constructs a task command with a
		/// given symbolic name and a description.
		/// </summary>
		/// <param name="symbolicName">Name of the symbolic.</param>
		/// <param name="description">The description.</param>
		public TaskCommand(
			string symbolicName,
			string description )
		{
			this.symbolicName = symbolicName;
			this.description = description;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// The symbolic name that is used to call the task command.
		/// </summary>
		/// <value>The name of the symbolic.</value>
		public string SymbolicName
		{
			get
			{
				return symbolicName;
			}
			set
			{
				symbolicName = value;
			}
		}

		/// <summary>
		/// The description that describes the task command.
		/// </summary>
		/// <value>The description.</value>
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private string symbolicName;
		private string description;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}