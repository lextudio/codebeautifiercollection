// this is trace listener text log component.
//		The idea is from Tim Jarvis' blog entry.
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
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace Lextm.Windows.Forms
{
	/// <summary>
	/// TraceListenerTextLog class.
	/// </summary>
	public class TraceListenerTextLog : RichTextBox
	{
		private enum MessageType {
			LogMessage = 1,
			ErrorMessage
		};

		private class InternalListener: TraceListener {
			private RichTextBox _txtBox;

			internal InternalListener(RichTextBox txtBox) : base() {
				_txtBox = txtBox;
			}

			private void WriteToLog(string message, string category, MessageType mt) {
				if ((category == null) || (category.Length == 0))
				{
					category = "TRACE";
				}

				string messageText = "[" + category + "]" + message + "\n";

				if (mt == MessageType.ErrorMessage) {
					Color tmpColor = _txtBox.SelectionColor;
					_txtBox.SelectionColor = Color.Red;
					_txtBox.AppendText(messageText);
					_txtBox.SelectionColor = tmpColor;
                } else {
					_txtBox.AppendText(messageText);
				}            
			}

			public override void Write(object o) {
				MessageType mt = (o is Exception) ? MessageType.ErrorMessage : MessageType.LogMessage;
                WriteToLog(o.ToString(), String.Empty, mt);
			}

			public override void Write(string message) {
				WriteToLog(message, String.Empty, MessageType.LogMessage);
			}

			public override void Write(object o, string category) {
				MessageType mt = (o is Exception) ? MessageType.ErrorMessage : MessageType.LogMessage;
				WriteToLog(o.ToString(), category, mt);
			}

			public override void Write(string message, string category) {
				WriteToLog(message, category, MessageType.LogMessage);
			}

            public override void WriteLine(object o) {
				MessageType mt = (o is Exception) ? MessageType.ErrorMessage : MessageType.LogMessage;
				WriteToLog(o.ToString(), String.Empty, mt);
            }

			public override void WriteLine(string message) {
				WriteToLog(message, String.Empty, MessageType.LogMessage);
			}

			public override void WriteLine(object o, string category) {
				MessageType mt = (o is Exception) ? MessageType.ErrorMessage : MessageType.LogMessage;
				WriteToLog(o.ToString(), category, mt);
			}

			public override void WriteLine(string message, string category) {
				WriteToLog(message, category, MessageType.LogMessage);
			}
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		public TraceListenerTextLog() :base()
		{
			Trace.Listeners.Add(new InternalListener(this));
		}
		
        //private void InitializeComponent()
        //{
        //    // 
        //    // TraceListenerTextLog
        //    // 
        //    this.Font = new System.Drawing.Font("Tahoma", 8.25F);
        //}
	}
}
