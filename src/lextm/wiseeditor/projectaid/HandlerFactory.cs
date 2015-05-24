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
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;

namespace Lextm.WiseEditor.ProjectAid
{
	/// <summary>
	/// Description of SignedHandler.
	/// </summary>
	class HandlerFactory
	{
		HandlerFactory()
		{}
		
		internal static IHandler GetHandlerFor(IOTAProject proj)
		{
			//parse dpr or dpk to find 1.x key file. return true if file exists.
			string realProjectFile = OtaUtils.GetDelphiProjectFileName(proj.FileName);
			if (File.Exists(realProjectFile))
			{
				string content1x = File.ReadAllText(realProjectFile);
				Match m = regex1x.Match(content1x);
				if (m.Success) //TODO: what if in comments??
				{
					//1.x key found. 1.x manner.
					string file = m.Groups["key_file"].Value;
					if (!File.Exists(file)) //TODO: what about relative path??
					{
						MessageService.Show("Key file is missing: " + file);
					}
					return new Manner1xHandler(proj.FileName);
				}
			}
			ProjectRecord record = new ProjectRecord();
			record.Interface = proj;
			//2.0+ manner
			//if no key entries found,
			//generate new key and add key to project.
			XmlDocument doc = new XmlDocument();
			doc.Load(proj.FileName);
			XmlElement project = doc.DocumentElement;
			XmlNodeList propnodes = project.GetElementsByTagName("PropertyGroup");
			
			IList<OptionSet> options = new List<OptionSet>();
			foreach (XmlElement propgroup in propnodes)
			{
				OptionSet os = OptionSet.CreateFrom(doc, propgroup, proj.FileName);
				if (!string.IsNullOrEmpty(os.Condition))
				{
					options.Add(os);
				}
			}
			record.Options = options;
			//if debug and release does not match clear all and
			if (OptionSet.IsCorrupt(options))
			{
				OptionSet.Clear(options);
				return new Manner20UnsignedHandler(record);
			}
			return new Manner20SignedHandler(proj.FileName);
		}
		
		static Regex regex1x = new Regex(
			"^\\s*\\[assembly:\\s*AssemblyKeyFile\\(\\'(?<key_file>.+)\\'"+
			"\\)\\]\\s*$",
			RegexOptions.Multiline
			| RegexOptions.CultureInvariant
			| RegexOptions.Compiled
		);
	}

}
