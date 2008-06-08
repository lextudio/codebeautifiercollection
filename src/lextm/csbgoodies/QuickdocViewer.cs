// this is quickdoc viewer class.
// Ported from C#Builder Goodies 1.1 by lextm.
// Original source code is copyrighted by Valentino Kyriakides (KyrSoft).
// Copyright (C) 2006  Lex Y. Li
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

using System;
using System.Text;
using System.Windows.Forms;
using BeWise.Common.Utils;
using Lextm.Diagnostics;

namespace Lextm.CsbGoodies {
    /// <summary>
    /// This class implements a XML to HTML WYSIWYG documentation comments popup viewer for C#Builder.
    /// </summary>
	class QuickdocViewer {
	
		private QuickdocViewer() {	}

		private const string DefaultHtml = "<body><i>(content is empty or corrupt)</i></body>";
		
		/// <summary>
		/// Performs the doc section handling and activates the quickdoc viewer.
		/// </summary>
		internal static void Activate() {
		
			LoggingService.EnterMethod();
			if (OtaUtils.CurrentModuleIsCSFile()) {				
				pglowacki.XmlDoc.FormXmlDocViewer.ShowViewer(PrepareHtml()); 
			} else {
				Lextm.Windows.Forms.MessageBoxFactory.Warn(null, "Not a C# file", "Quickdoc Viewer only apply to C# files");
			}
			LoggingService.LeaveMethod();
		
		}

		private readonly static string XsltFilePath
			= Lextm.OpenTools.IO.Path.GetDataFile("ShowXmlDocumentation.xsl");
		
		private static bool IsDocLine(int lineNumber) {
			//LoggingService.Info("This is the " + lineNumber.ToString() + " th line.");
			LoggingService.Info("this line is " + OtaUtils.GetLineOf(lineNumber).Trim());
			return OtaUtils.GetLineOf(lineNumber).Trim().StartsWith("///", StringComparison.Ordinal);
		}

		private static string PrepareHtml() {
		
			LoggingService.EnterMethod();
			string result;

			if (IsDocLine(OtaUtils.GetCurrentLineNumber())) {
				try {
					result = //ConvertXml.ConvertData(GetContent(), XsltFilePath, null);
						Lextm.Xml.XmlConvertor.ConvertString(GetContent(), XsltFilePath);
				} catch (System.Xml.Xsl.XsltException) {
					result = DefaultHtml;
				}
			} else {
				result = DefaultHtml;
				LoggingService.Warn("not a doc line");
			}
			LoggingService.Info("the result is " + result);
			LoggingService.LeaveMethod();						
			
			return result;
		}
		
		private static int GetStartLine( ) {
			int result = OtaUtils.GetCurrentLineNumber();

			while (result > 0) {
				if (IsDocLine(result)) {
					--result;
				} else {
					break;
				}
			}

			return result;
		}

		private static int GetEndLine( ) {
			int result = OtaUtils.GetCurrentLineNumber();

			while (result < OtaUtils.GetTotalLines(OtaUtils.GetCurrentSourceEditor()) - 1) {
				if (IsDocLine(result)) {
					++result;
				} else {
					break;
				}
			}
			
			return result;
		}

		private static string GetContent( ) {
			StringBuilder documentation = new StringBuilder();
			documentation.Append("<member>");

			for (int lineNumber = GetStartLine() + 1; lineNumber < GetEndLine(); ++lineNumber) {
				documentation.Append(
					OtaUtils.GetLineOf(lineNumber).Trim().Substring(3)
				);
			}

			documentation.Append("</member>");
			LoggingService.Info("XML Doc : " +documentation.ToString());
			return documentation.ToString();
		}


	}
}
