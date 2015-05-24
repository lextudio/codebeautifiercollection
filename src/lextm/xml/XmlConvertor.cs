// this is the xml convertor class.
// Copyright (C) 2005-2006  Lex Y. Li
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
using System.Xml;
using System.Xml.Xsl;
using System.Globalization;

namespace Lextm.Xml
{
	/// <summary>
	/// Class that converts Xml.
	/// </summary>
	public sealed class XmlConvertor
	{
		private XmlConvertor()
		{
		}
		/// <summary>
		/// Converts XML file using XSL file.
		/// </summary>
		/// <param name="input">Input XML file</param>
		/// <param name="xsl">XSL file</param>
		/// <param name="output">Output XML file</param>
		public static void ConvertXmlUsingXsl(string input, string xsl, string output) {
			StreamWriter file = new StreamWriter(output);
			file.Write(ConvertFile(input, xsl));
			file.Close();
		}
		/// <summary>
		/// Converts string.
		/// </summary>
		/// <param name="input">Input XML string</param>
		/// <param name="xsl">XSL file</param>
		/// <returns>Output string.</returns>
		/// <exception cref="System.Xml.Xsl.XsltException">XSLT translation fails.</exception>
		public static string ConvertString(string input, string xsl) {
			// Load XML
//			XmlDocument XmlDoc = new XmlDocument();
//			XmlDoc.LoadXml(input);
			TextReader XmlDoc = new StringReader(input);
			return TranformInner(XmlDoc, xsl);
		}
		
		private static string TranformInner(TextReader input, string xsl) {
			// Load XSL
			//XslTransform XslDoc = new XslTransform();
			XslCompiledTransform XslDoc = new XslCompiledTransform();
			XslDoc.Load(xsl);
			
			// produce output
//			XmlTextWriter writer = new XmlTextWriter(output, null);
//			XslDoc.Transform(XmlDoc, null, writer, null);
//			writer.Close(); // this lines encode char < to special coded item.
			TextWriter output = new StringWriter(CultureInfo.InvariantCulture);
			XmlReader reader = XmlReader.Create(input);
			string result;
			using (XmlWriter writer = XmlWriter.Create(output))
			{
				//XslDoc.Transform(doc, null, writer, null);
				XslDoc.Transform(reader, writer);
				result = output.ToString();
				writer.Close();
			}
			return result;
		}
		/// <summary>
		/// Converts file.
		/// </summary>
		/// <param name="input">Input XML file</param>
		/// <param name="xsl">XSL file</param>
		/// <returns>Output string.</returns>
		public static string ConvertFile(string input, string xsl) {
			// Load XML
			//XmlDocument XmlDoc = new XmlDocument();
			//XmlDoc.Load(input);
			TextReader XmlDoc = new StreamReader(input);
			return TranformInner(XmlDoc, xsl);
		}
	}
}
