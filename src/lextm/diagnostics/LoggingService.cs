// this is logging service class.
// Copyright (C) 2006  Lex Y. Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using log4net;
using log4net.Config;
using System.Globalization;

namespace Lextm.Diagnostics
{
	/// <summary>
	/// Logging service class.
	/// </summary>
	/// <summary>Debug related functions such as <see cref="Debug(object)">Debug</see>,
	/// <see cref="EnterMethod()">EnterMethod</see>, and <see cref="LeaveMethod()">
	/// LeaveMethod</see> are
	/// available only in Debug Build mode.</summary>
	public sealed class LoggingService
	{
		private LoggingService()
		{
		}

		/// <summary>
		/// Starts logging service.
		/// </summary>
		/// <param name="configuration">Configuration file name</param>
		/// <remarks>Must be called first to finish configuration. The settings
		/// file path is sent as <paramref name="configuration"/>.</remarks>
		public static void Start(string configuration) {
        	if (File.Exists(configuration)) {
				XmlConfigurator.Configure(new FileInfo(configuration));
				Info("=========================================================");
                Info(String.Format(CultureInfo.InvariantCulture, "Debug Logging Initialized: {0}", DateTime.Now));
			}
		}
#region log4net wrappers
		/// <summary>
		/// Logs a warning.
		/// </summary>
		/// <param name="message">Message</param>
		public static void Warn(object message) {
			WarnInternal(message);
		}
		/// <summary>
		/// Logs a warning.
		/// </summary>
		/// <param name="format">Format</param>
		/// <param name="args">Arguments</param>
		public static void Warn(string format, params object[] args)
		{
			WarnInternal(string.Format(CultureInfo.CurrentCulture, format, args));
		}		                        
		
		static void WarnInternal(object message)
		{
			Logger.Warn(AppendIndentationTo(message));
		}
		
		/// <summary>
		/// Logs a debug word.
		/// </summary>
		/// <param name="message">Message</param>
		public static void Debug(object message) {
			DebugInternal(message);
		}
		
		/// <summary>
		/// Logs a debug word.
		/// </summary>
		/// <param name="format">Format</param>
		/// <param name="args">Arguments</param>
		public static void Debug(string format, params object[] args)
		{
			DebugInternal(string.Format(CultureInfo.CurrentCulture, format, args));
		}	
		
		static void DebugInternal(object message) {			
			Logger.Debug(AppendIndentationTo(message));
		}
		/// <summary>
		/// Logs an info.
		/// </summary>
		/// <param name="message">Message</param>
		public static void Info(object message) {
			InfoInternal(message);
		}
		/// <summary>
		/// Logs an info.
		/// </summary>
		/// <param name="format">Format</param>
		/// <param name="args">Arguments</param>
		public static void Info(string format, params object[] args)
		{
			InfoInternal(string.Format(CultureInfo.CurrentCulture, format, args));
		}	
		
		static void InfoInternal(object message) {
			Logger.Info(AppendIndentationTo(message));
		}
		/// <summary>
		/// Logs an error.
		/// </summary>
		/// <param name="message">Message</param>
		public static void Error(object message) {
			ErrorInternal(message);
		}
		
		/// <summary>
		/// Logs an error.
		/// </summary>
		/// <param name="format">Format</param>
		/// <param name="args">Arguments</param>
		public static void Error(string format, params object[] args)
		{
			ErrorInternal(string.Format(CultureInfo.CurrentCulture, format, args));
		}	
		
		static void ErrorInternal(object message) {
			Logger.Error(AppendIndentationTo(message));
		}
		/// <summary>
		/// Logs a fatal.
		/// </summary>
		/// <param name="message">Message</param>
		public static void Fatal(object message) {
			FatalInternal(message);
		}
		
		/// <summary>
		/// Logs a fatal.
		/// </summary>
		/// <param name="format">Format</param>
		/// <param name="args">Arguments</param>
		public static void Fatal(string format, params object[] args)
		{
			FatalInternal(string.Format(CultureInfo.CurrentCulture, format, args));
		}	
		
		static void FatalInternal(object message) {
			Logger.Fatal(AppendIndentationTo(message));
		}
		
		private static log4net.ILog Logger {
			get {
				StackTrace st = new StackTrace(true);
				if (st.FrameCount <= FRAMELEVEL) {
					return log4net.LogManager.GetLogger("FrameCountError");
				}
				StackFrame sf = st.GetFrame(FRAMELEVEL);
				MethodBase mi = sf.GetMethod();
				return log4net.LogManager.GetLogger(mi.ReflectedType);
			}
		}
		
		const int FRAMELEVEL = 3;
#endregion

#region Indent, Unindent, EnterMethod and LeaveMethod enhancements
		private static string AppendIndentationTo(object message) {
			StringBuilder text = new StringBuilder();
			for(int i = 0; i < indentLevel; i++) {
				text.Append("    ");
			}
			text.Append(message);
			return text.ToString();
		}
		private static int indentLevel;

		private static string GetMethodName() {
			MethodBase mi = (new StackTrace(true)).GetFrame(2).GetMethod();
			return mi.ReflectedType.Name + "." + mi.Name;
        }
		/// <summary>
		/// Enters a method.
		/// </summary>
		/// <seealso cref="LeaveMethod()"/>
		public static void EnterMethod() {
			DebugInternal("-->" + GetMethodName());
			indentLevel++;
		}
				/// <summary>
		/// Enters a method.
		/// </summary>
		/// <seealso cref="LeaveMethod()"/>
		public static void EnterMethod(object message) {
			DebugInternal(string.Format(CultureInfo.CurrentCulture, "--> {0} {1}", GetMethodName(), message));
			indentLevel++;
		}
		/// <summary>
		/// Leaves a method.
		/// </summary>
		/// <seealso cref="EnterMethod()"/>
		public static void LeaveMethod() {
			indentLevel--;
			DebugInternal("<--" + GetMethodName());
		}
		/// <summary>
		/// Leaves a method.
		/// </summary>
		/// <remarks> Extra message can be passed as <paramref name="message"/>.</remarks>
		/// <param name="message">Extra message</param>
        /// <seealso cref="EnterMethod()"/>
		public static void LeaveMethod(object message) {
			indentLevel--;
			DebugInternal(String.Format(CultureInfo.InvariantCulture, "<--{0} {1}", GetMethodName(), message));
		}
		/// <summary>
		/// Indents.
		/// </summary>
		/// <seealso cref="Unindent"/>
		public static void Indent() {
			indentLevel++;
		}
		/// <summary>
		/// Unindents.
		/// </summary>
        /// <seealso cref="Indent"/>
		public static void Unindent() {
			if (indentLevel > 0) {
				indentLevel--;
			}
		}
		//private static readonly bool isDebugEnabled = log.IsDebugEnabled;
#endregion

	}
}
