// this is the message service class.
// Copyright (C) 2005-2007  Lex Y. Li
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
using System.Collections.Generic;
using System.IO;
using BeWise.Common.Utils;
using System.Diagnostics;
using System.Globalization;

namespace Lextm.OpenTools
{
	/// <summary>
	/// Message service.
	/// </summary>
	/// <remarks>All messages are added by <see cref="Show"/>.
	/// <see cref="ShowAll"/> is used by CBC Framework exclusively.</remarks>
	public sealed class MessageService
	{
        private static IList<string> list = new List<string>();

		private MessageService() {}
		/// <summary>
		/// Adds a message.
		/// </summary>
		/// <remarks>Messages go to the Message Panel.</remarks>
		/// <param name="message">Message</param>
		public static void Show(string message) {
			if (!IDEStarted) {
				list.Add(message);
            } else {
				InnerAddMessage(message);
            }
		}
		
		private static bool IDEStarted;
		/// <summary>
		/// Shows all in Message Panel.
		/// </summary>
		/// <remarks>This method is used in CBC Framework exclusively.</remarks>
		public static void ShowAll() {
			foreach(string message in list) {
				InnerAddMessage(message);
			}
			list.Clear();
			IDEStarted = true;
		}

		private static void InnerAddMessage(string message) {
			OtaUtils.AddMessage(String.Format(CultureInfo.InvariantCulture, 
                                              "[{0}] {1}",
											  DateTime.Now,
											  message));
		}
        /// <summary>
        /// Debug use only.
        /// </summary>
        /// <param name="message"></param>
        [Conditional("DEBUG")]
        public static void Debug(string message)
        {
            Show(message);
        }
    }
}
