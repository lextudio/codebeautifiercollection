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
	using System.Text;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Independently usable class that returns various strings with
	/// environment-related logging information.
	/// </summary>
	public class LoggingInformation
	{
		#region Static member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Return a string with a collection of all the discrete information
		/// that this class provides.
		/// </summary>
		/// <value>All.</value>
		public static string All
		{
			get
			{
				string additionalInfo = string.Empty;

				if ( needMoreInfoEventHandlers != null )
				{
					foreach (
						EventHandler<LoggingInformationNeedMoreInfoEventArgs> handler
						in needMoreInfoEventHandlers )
					{
						LoggingInformationNeedMoreInfoEventArgs args =
							new LoggingInformationNeedMoreInfoEventArgs();

						// Call handler.
						Debug.Assert( handler != null );
						handler( null, args );

						if ( !string.IsNullOrEmpty( args.MoreInformation ) )
						{
							// Add information.
							additionalInfo +=
								args.MoreInformation +
								Environment.NewLine + Environment.NewLine;
						}
					}

					additionalInfo = additionalInfo.Trim();
				}

				string info =
					additionalInfo +
					Environment.NewLine + Environment.NewLine +
					WindowsEnvironment +
					Environment.NewLine + Environment.NewLine +
					NetworkEnvironment +
					Environment.NewLine + Environment.NewLine +
					Assemblies;

				return info.Trim();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static
			event EventHandler<LoggingInformationNeedMoreInfoEventArgs>
			NeedMoreInfo
		{
			add
			{
				Debug.Assert( value != null );

				lock ( typeLock )
				{
					if ( needMoreInfoEventHandlers == null )
					{
						needMoreInfoEventHandlers =
							new List<EventHandler<LoggingInformationNeedMoreInfoEventArgs>>();
					}

					Debug.Assert( !needMoreInfoEventHandlers.Contains( value ) );
					needMoreInfoEventHandlers.Add( value );
				}
			}
			remove
			{
				lock ( typeLock )
				{
					Debug.Assert( needMoreInfoEventHandlers != null );
					Debug.Assert( needMoreInfoEventHandlers.Contains( value ) );

					needMoreInfoEventHandlers.Remove( value );
				}
			}
		}

		/// <summary>
		/// Provide Windows-environment information.
		/// </summary>
		/// <value>The windows environment.</value>
		public static string WindowsEnvironment
		{
			get
			{
				string info =
					string.Format(
					CultureInfo.CurrentCulture,
					@"User domain name: {0}," + Environment.NewLine +
					@"User name: {1}," + Environment.NewLine +
					@"Machine name: {2}," + Environment.NewLine +
					@"OS version: {3}," + Environment.NewLine +
					@"CLR version: {4}," + Environment.NewLine +
					@"Command line: {5}," + Environment.NewLine +
					@"Current directory: {6}.",
					Environment.UserDomainName == null ? @"(null)" : Environment.UserDomainName,
					Environment.UserName == null ? @"(null)" : Environment.UserName,
					Environment.MachineName == null ? @"(null)" : Environment.MachineName,
					Environment.OSVersion == null ? @"(null)" : Environment.OSVersion.ToString(),
					Environment.Version == null ? @"(null)" : Environment.Version.ToString(),
					Environment.CommandLine == null ? @"(null)" : Environment.CommandLine,
					Environment.CurrentDirectory == null ? @"(null)" : Environment.CurrentDirectory );

				return info;
			}
		}

		/// <summary>
		/// DNSs the name of the get host entry.
		/// </summary>
		/// <param name="hostName">Name of the host.</param>
		/// <returns></returns>
		protected static string DnsGetHostEntryName(
			string hostName )
		{
			if ( string.IsNullOrEmpty( hostName ) )
			{
				return string.Empty;
			}
			else
			{
				try
				{
					IPHostEntry iphe = Dns.GetHostEntry( hostName );

					if ( iphe == null )
					{
						return string.Empty;
					}
					else
					{
						return iphe.HostName;
					}
				}
				catch ( SocketException )
				{
					return string.Empty;
				}
			}
		}

		/// <summary>
		/// Provide network-environment information.
		/// </summary>
		/// <value>The network environment.</value>
		public static string NetworkEnvironment
		{
			get
			{
				string hostName = Dns.GetHostName();

				StringBuilder infos = new StringBuilder();
				infos.AppendFormat(
					CultureInfo.CurrentCulture,
					@"Host name: {0}",
					hostName == null ? @"(null)" : hostName );

				if ( hostName != null )
				{
					IPHostEntry hostEntry = Dns.GetHostEntry( hostName );

					if ( hostEntry != null )
					{
						foreach ( IPAddress ipAddress in hostEntry.AddressList )
						{
							infos.AppendFormat(
								CultureInfo.CurrentCulture,
								@",{0}IP address: {1}{0}Lookup address: http://www.dnsstuff.com/tools/city.ch?ip={1}",
								Environment.NewLine,
								ipAddress == null ? @"(null)" : ipAddress.ToString() );
						}
					}
				}

				return infos.ToString().TrimEnd().TrimEnd( ',', '.' ).TrimEnd() + @".";
			}
		}

		/// <summary>
		/// Provide information about the current assemblies.
		/// </summary>
		/// <value>The assemblies.</value>
		public static string Assemblies
		{
			get
			{
				List<Pair<string, Assembly>> assemblies =
					new List<Pair<string, Assembly>>();

				// Add named assemblies.
				assemblies.Add( new Pair<string, Assembly>( @"Entry assembly", Assembly.GetEntryAssembly() ) );
				assemblies.Add( new Pair<string, Assembly>( @"Executing assembly", Assembly.GetExecutingAssembly() ) );
				assemblies.Add( new Pair<string, Assembly>( @"Calling assembly", Assembly.GetCallingAssembly() ) );

				// Add all loaded assemblies.
				// (This is redundant with the above but still OK).
				Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
				if ( asms != null )
				{
					int index = 0;
					foreach ( Assembly asm in asms )
					{
						assemblies.Add(
							new Pair<string, Assembly>(
							string.Format(
							@"Domain assembly {0}/{1}.",
							index + 1,
							asms.Length ),
							asm ) );

						index++;
					}
				}

				// --

				string infos = string.Empty;

				int loopCount = 0;
				foreach ( Pair<string, Assembly> pair in assemblies )
				{
					string assemblyType = pair.Name;
					Assembly assembly = pair.Value;

					if ( assembly != null )
					{
						string info = string.Format(
							CultureInfo.CurrentCulture,
							@"Assembly type: {0}," + Environment.NewLine +
							@"Assembly full name: {1}," + Environment.NewLine +
							@"Assembly location: {2}," + Environment.NewLine +
							@"Assembly date: {3}," + Environment.NewLine +
							@"Assembly version: {4}."
							,
							assemblyType,
							assembly.FullName == null ? @"(null)" : assembly.FullName,
							GetAssemblyLocation( assembly ) == null ? @"(null)" : GetAssemblyLocation( assembly ),
							GetAssemblyLocation( assembly ) == null ? @"(null)" : GetLastFileWriteTime( GetAssemblyLocation( assembly ) ).ToString( CultureInfo.CurrentCulture ),
							assembly.GetName() == null || assembly.GetName().Version == null ? @"(null)" : assembly.GetName().Version.ToString() );

						if ( loopCount > 0 )
						{
							infos += Environment.NewLine + Environment.NewLine;
						}

						infos += info;
					}

					loopCount++;
				}

				/*
				infos +=
					Environment.NewLine +
					@"<-------------" +
					Environment.NewLine +
					Environment.NewLine;
				*/

				return infos;
			}
		}

		/// <summary>
		/// Gets the last file write time.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns></returns>
		private static DateTime GetLastFileWriteTime(
			string filePath )
		{
			if ( string.IsNullOrEmpty( filePath ) )
			{
				return DateTime.MinValue;
			}
			else if ( !File.Exists( filePath ) )
			{
				return DateTime.MinValue;
			}
			else
			{
				return File.GetLastWriteTime( filePath );
			}
		}

		/// <summary>
		/// Dynamic assemblies don't support the location.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <returns></returns>
		private static string GetAssemblyLocation(
			Assembly assembly )
		{
			try
			{
				return assembly.Location;
			}
			catch ( NotSupportedException )
			{
				return null;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private helper.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private static object typeLock = new object();

		/// <summary>
		/// 
		/// </summary>
		private static List<EventHandler<LoggingInformationNeedMoreInfoEventArgs>>
			needMoreInfoEventHandlers;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}