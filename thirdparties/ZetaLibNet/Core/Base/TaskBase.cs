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
	/// Base class for tasks.
	/// </summary>
	public abstract class TaskBase
	{
		#region Static methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Pass a string with one or several commands and
		/// executes these tasks successive.
		/// </summary>
		/// <param name="rawCommands">Usually this is the raw command
		/// line to a console application that is passed to this function.</param>
		/// <returns>Returns the number of executed tasks.</returns>
		public static int DispatchTasks(
			string[] rawCommands )
		{
			int count = 0;

			if ( rawCommands == null || rawCommands.Length <= 0 )
			{
				// No commands specified, execute the default task (if any).

				TaskBase[] tasks = AllTask;
				if ( tasks != null )
				{
					// Check for only one active default task.
					int defaultCount = 0;
					foreach ( TaskBase task in tasks )
					{
						if ( task.IsDefaultTask && task.IsActive )
						{
							defaultCount++;
						}
					}

					if ( defaultCount > 1 )
					{
						throw new ApplicationException(
							LocalizationHelper.Format(
							Resources.Str_ZetaLib_Core_Base_Tasks_TaskBase_DispatchTasks_01,
							LocalizationHelper.CreatePair( @"Count", defaultCount ) ) );
					}

					// Search and find task.
					foreach ( TaskBase task in tasks )
					{
						if ( task.IsDefaultTask && task.IsActive )
						{
							task.Process( null );
							count++;
							break;
						}
					}
				}
			}
			else
			{
				// Commands were specified, execute from left to right.

				string[] preparedRawCommands = rawCommands;

				foreach ( string rawCommand in preparedRawCommands )
				{
					if ( ExecuteTask( rawCommand ) )
					{
						count++;
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a single task by command name.
		/// </summary>
		/// <param name="rawCommand">The raw command.</param>
		/// <returns>
		/// Returns TRUE if found and executed,
		/// FALSE otherwise.
		/// </returns>
		public static bool ExecuteTask(
			string rawCommand )
		{
			TaskBase task = GetTask( rawCommand );
			if ( task != null && task.IsActive )
			{
				task.Process( task.GetCommandFromRawCommand( rawCommand ) );
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Uses Reflection to generate a list of all classes
		/// derived from this class.
		/// </summary>
		/// <value>All task.</value>
		public static TaskBase[] AllTask
		{
			get
			{
				List<TaskBase> list = new List<TaskBase>();

				Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
				if ( asms != null )
				{
					foreach ( Assembly asm in asms )
					{
						DoGetAllTasks( ref list, asm );
					}
				}

				Assembly a = Assembly.GetExecutingAssembly();
				DoGetAllTasks( ref list, a );

				a = Assembly.GetCallingAssembly();
				DoGetAllTasks( ref list, a );

				a = Assembly.GetEntryAssembly();
				DoGetAllTasks( ref list, a );

				if ( list.Count <= 0 )
				{
					return null;
				}
				else
				{
					return list.ToArray();
				}
			}
		}

		/// <summary>
		/// Does the get all tasks.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="a">A.</param>
		private static void DoGetAllTasks(
			ref List<TaskBase> list,
			Assembly a )
		{
			if ( a != null )
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"DoGetAllTasks(): Searching assembly '{0}'...",
					a.GetName().Name ) );

				Type[] types = a.GetTypes();

				foreach ( Type t in types )
				{
					if ( t.IsSubclassOf( typeof( TaskBase ) ) && !t.IsAbstract )
					{
						// Create instance.
						TaskBase b = Activator.CreateInstance( t ) as TaskBase;

						if ( b.IsActive )
						{
							// Don't insert duplicates.
							bool found = false;
							foreach ( TaskBase c in list )
							{
								if ( c.GetType() == b.GetType() )
								{
									found = true;
									break;
								}
							}

							if ( !found )
							{
								list.Add( b );
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Use Reflection to generate an instance of a class derived
		/// from this class.
		/// </summary>
		/// <param name="commandSymbolicName">The command of the class.</param>
		/// <returns>
		/// Returns instance if found, returns null if not found.
		/// </returns>
		public static TaskBase GetTask(
			string commandSymbolicName )
		{
			Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
			if ( asms != null )
			{
				foreach ( Assembly asm in asms )
				{
					TaskBase tba = DoGetTask( asm, commandSymbolicName );
					if ( tba != null )
					{
						return tba;
					}
				}
			}

			Assembly a = Assembly.GetExecutingAssembly();
			TaskBase tb = DoGetTask( a, commandSymbolicName );
			if ( tb != null )
			{
				return tb;
			}
			else
			{
				a = Assembly.GetCallingAssembly();
				tb = DoGetTask( a, commandSymbolicName );
				if ( tb != null )
				{
					return tb;
				}
				else
				{
					a = Assembly.GetEntryAssembly();
					tb = DoGetTask( a, commandSymbolicName );
					if ( tb != null )
					{
						return tb;
					}
					else
					{
						return null;
					}
				}
			}
		}

		/// <summary>
		/// Helper function for retrieving a task.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="commandSymbolicName">Name of the command symbolic.</param>
		/// <returns></returns>
		private static TaskBase DoGetTask(
			Assembly a,
			string commandSymbolicName )
		{
			return DoGetTask( a, commandSymbolicName, 0 );
		}

		/// <summary>
		/// Helper function for retrieving a task.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="commandSymbolicName">Name of the command symbolic.</param>
		/// <param name="depth">The depth.</param>
		/// <returns></returns>
		private static TaskBase DoGetTask(
			Assembly a,
			string commandSymbolicName,
			int depth )
		{
			if ( a == null || depth > 10 )
			{
				return null;
			}
			else
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"DoGetTask(...depth={0}): Searching assembly '{1}'...",
					depth,
					a.GetName().Name ) );

				Type[] types = a.GetTypes();

				foreach ( Type t in types )
				{
					if ( t.IsSubclassOf( typeof( TaskBase ) ) && !t.IsAbstract )
					{
						// Create instance.
						TaskBase b = Activator.CreateInstance( t ) as TaskBase;

						// If instance has correct symbolic name, return.
						if ( b.Command.SymbolicName == commandSymbolicName && b.IsActive )
						{
							return b;
						}
						else
						{
							if ( b.Commands != null && b.IsActive )
							{
								foreach ( TaskCommand cn in b.Commands )
								{
									if ( cn.SymbolicName == commandSymbolicName )
									{
										return b;
									}
								}
							}
						}
					}
				}

				// Not found.
				return null;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Do your main processing here.
		/// </summary>
		/// <param name="command">The command.</param>
		public abstract void Process(
			TaskCommand command );

		/// <summary>
		/// Get a task command from a given rawCommand.
		/// </summary>
		/// <param name="rawCommand">The raw command.</param>
		/// <returns></returns>
		public TaskCommand GetCommandFromRawCommand(
			string rawCommand )
		{
			if ( Commands == null )
			{
				return null;
			}
			else
			{
				foreach ( TaskCommand command in Commands )
				{
					if ( command.SymbolicName == rawCommand )
					{
						return command;
					}
				}

				return null;
			}
		}

		/// <summary>
		/// Log and pump the GUI. also calls the LogEvent delegate
		/// if assigned.
		/// </summary>
		/// <param name="text">The text.</param>
		public void PumpLog(
			string text )
		{
			if ( LogCentral.Current != null )
			{
				LogCentral.Current.LogInfo( text );
			}

			if ( GenericBase.ApplicationEnvironment != null )
			{
				GenericBase.ApplicationEnvironment.Pump( text );
			}
		}

		/// <summary>
		/// Log and pump the GUI. also calls the LogEvent delegate
		/// if assigned.
		/// </summary>
		/// <param name="text">The text.</param>
		public void PumpLogWarning(
			string text )
		{
			if ( LogCentral.Current != null )
			{
				LogCentral.Current.LogWarn( text );
			}

			if ( GenericBase.ApplicationEnvironment != null )
			{
				GenericBase.ApplicationEnvironment.Pump( text );
			}
		}

		/// <summary>
		/// Log and pump the GUI. also calls the LogEvent delegate
		/// if assigned.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="x">The x.</param>
		public void PumpLogWarning(
			string text,
			Exception x )
		{
			if ( LogCentral.Current != null )
			{
				LogCentral.Current.LogWarn( text, x );
			}

			if ( GenericBase.ApplicationEnvironment != null )
			{
				GenericBase.ApplicationEnvironment.Pump( text );
			}
		}

		/// <summary>
		/// Log and pump the GUI. also calls the LogEvent delegate
		/// if assigned.
		/// </summary>
		/// <param name="text">The text.</param>
		public void PumpLogError(
			string text )
		{
			if ( LogCentral.Current != null )
			{
				LogCentral.Current.LogError( text );
			}

			if ( GenericBase.ApplicationEnvironment != null )
			{
				GenericBase.ApplicationEnvironment.Pump( text );
			}
		}

		/// <summary>
		/// Log and pump the GUI. also calls the LogEvent delegate
		/// if assigned.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="x">The x.</param>
		public void PumpLogError(
			string text,
			Exception x )
		{
			if ( LogCentral.Current != null )
			{
				LogCentral.Current.LogError( text, x );
			}

			if ( GenericBase.ApplicationEnvironment != null )
			{
				GenericBase.ApplicationEnvironment.Pump( text );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// If returned FALSE, the task is not accessible from outside.
		/// </summary>
		/// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
		public abstract bool IsActive
		{
			get;
		}

		/// <summary>
		/// If returned TRUE, the task is the default task.
		/// Only ONE active task can be the default task.
		/// The default tasks is executed if no parameters are specified.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is default task; otherwise, <c>false</c>.
		/// </value>
		public abstract bool IsDefaultTask
		{
			get;
		}

		/// <summary>
		/// Provide a mandatory command name string. E.g. "import-frida-data".
		/// </summary>
		/// <value>The command.</value>
		public abstract TaskCommand Command
		{
			get;
		}

		/// <summary>
		/// Provide multiple command name strings. Optional. If a
		/// task does not provide multiple strings, this function
		/// can return 'null' or the same as 'CommandName'.
		/// </summary>
		/// <value>The commands.</value>
		public abstract TaskCommand[] Commands
		{
			get;
		}

		/// <summary>
		/// Provide an optional description of the task.
		/// </summary>
		/// <value>The description.</value>
		public virtual string Description
		{
			get
			{
				return null;
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}