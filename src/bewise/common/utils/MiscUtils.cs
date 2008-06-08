using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Lextm.Win32;

namespace BeWise.Common.Utils
{
	/// <summary>
	/// Some useful functions.
	/// </summary>
	public sealed class MiscUtils
	{

		private MiscUtils( )
		{ }
		/**************************************************************/
		/*                     Clipboard
        /**************************************************************/
		/// <summary>
		/// Gets text data from clip board.
		/// </summary>
		/// <returns>String.Empty if wrong.</returns>
		public static string TextDataFromClipboard
		{
			get
			{
				IDataObject _Data = Clipboard.GetDataObject();
				if (_Data.GetDataPresent(DataFormats.Text))
				{
					return _Data.GetData(DataFormats.Text).ToString();
				}
				else
				{
					return String.Empty;
				}
			}
		}
		/// <summary>
		/// Sets text data to clip board.
		/// </summary>
		/// <param name="text">Text</param>
		public static void SetTextDataToClipboard(string text)
		{
			Clipboard.SetDataObject(text);
		}
	}
}

