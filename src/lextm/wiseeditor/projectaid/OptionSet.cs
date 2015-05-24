/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/3/30
 * Time: 12:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Xml;

namespace Lextm.WiseEditor.ProjectAid
{
	class OptionSet
	{
		string _fileName;
		string _condition;
		bool _signAssembly;
		string _keyFile;
		XmlNode _signNode;
		XmlNode _keyNode;
		XmlNode _root;
		XmlDocument _doc;
		
		OptionSet() {}
		
		internal string Condition
		{
			get
			{
				return _condition;
			}
		}
		
		void Clear()
		{
			_signAssembly = false;
			_keyFile = string.Empty;
			if (_signNode != null) {
				_signNode.InnerText = "False";
			}
			if (_keyNode != null) {
				_keyNode.InnerText = string.Empty;
			}	
			_doc.Save(_fileName);
		}
		
		void Fill(string keyFile)
		{
			_keyFile = keyFile;
			_signAssembly = true;
			if (_signNode == null) {
				_signNode = _doc.CreateElement("DCC_SignAssembly", _root.NamespaceURI);
				_root.AppendChild(_signNode);
			}
			_signNode.InnerText = "True";
			if (_keyNode == null) {
				_keyNode = _doc.CreateElement("DCC_KeyFile", _root.NamespaceURI);
				_root.AppendChild(_keyNode);
			}
			_keyNode.InnerText = keyFile;
			_doc.Save(_fileName);
		}
		#region static methods
		internal static void Clear(IList<OptionSet> list)
		{
			foreach (OptionSet option in list)
			{
				option.Clear();
			}
		}
		
		internal static void Fill(IList<OptionSet> list, string keyFile)
		{
			foreach (OptionSet option in list)
			{
				option.Fill(keyFile);
			}
		}
		
		internal static OptionSet CreateFrom(XmlDocument doc, XmlNode element, string fileName)
		{
			OptionSet result = new OptionSet();
			result._fileName = fileName;
			result._doc = doc;
			result._root = element;
			if (element.Attributes.GetNamedItem("Condition") == null) {
				result._condition = string.Empty;
			}
			else
			{
				result._condition = element.Attributes.GetNamedItem("Condition").Value;
			}
			foreach (XmlNode propsetting in element.ChildNodes)
			{
				if (propsetting.Name == "DCC_SignAssembly") {
					result._signAssembly = Convert.ToBoolean(propsetting.InnerText);
					result._signNode = propsetting;
				}
				if (propsetting.Name == "DCC_KeyFile") {
					result._keyFile = propsetting.InnerText;
					result._keyNode = propsetting;
				}
			}
			return result;
		}
		
		internal static bool IsCorrupt(IList<OptionSet> options)
		{
			string temp = null;
			foreach (OptionSet option in options)
			{
				if (!option._signAssembly) {
					return true;
				}
				if (string.IsNullOrEmpty(option._keyFile)) {
					return true;
				}
				if (temp == null) {
					temp = option._keyFile;					
				}
				else if (temp != option._keyFile) {
					return true;	
				}
			}
			return false;
		}
		#endregion
	}
}
