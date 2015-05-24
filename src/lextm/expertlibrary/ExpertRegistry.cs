/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2007/11/3
 * Time: 16:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Lextm.IO;
using Lextm.Win32;

namespace Lextm.ExpertLibrary
{
	public sealed class ExpertRegistry
	{
		ExpertRegistry() {}
		
		public static void CheckExperts()
		{
			IList<IExpert> experts = LoadExpertsFromFile();
			foreach (IExpert expert in experts)
			{
				expert.CheckInstall();
			}
		}
		
		public static void UnregisterExpert(string[] args)
		{
			string fileName = null;
			foreach(string arg in args)
			{
				if (arg.StartsWith("/f:", StringComparison.OrdinalIgnoreCase))
				{
					fileName = arg.Substring(3);
				}
			}
			if (String.IsNullOrEmpty(fileName))
			{
				return;
			}
			IList<IExpert> experts = LoadExpertsFromFile();
			for (int i = experts.Count - 1; i >= 0; i--)
			{
				if (FileHelper.IsTheSameFile(experts[i].FileName, fileName))
				{
					experts.RemoveAt(i);
				}
			}
			SaveExpertsToFile(experts);
		}
		
		public static void RegisterExpert(string[] args)
		{
			IExpert expert = CreateExpertFrom(args);
			AddToFile(expert);
		}
		
		static IExpert CreateExpertFrom(string[] args)
		{
			string name = null;
			string fileName = null;
			foreach (string arg in args)
			{
				if (arg.StartsWith("/n:", StringComparison.OrdinalIgnoreCase))
				{
					name = arg.Substring(3);
				}
				if (arg.StartsWith("/f:", StringComparison.OrdinalIgnoreCase))
				{
					fileName = arg.Substring(3);
				}
			}
			if (string.IsNullOrEmpty(fileName))
			{
				return null;
			}
			if (string.IsNullOrEmpty(name))
			{
				name = Path.GetFileNameWithoutExtension(fileName);
			}
			if (FileHelper.IsDotNetAssembly(fileName))
			{
				return new DotNetExpert(name, fileName);
			}
			else
			{
				return new Win32Expert(name, fileName);
			}
		}
		
		static void AddToFile(IExpert expert)
		{
			IList<IExpert> experts = LoadExpertsFromFile();
			foreach (IExpert e in experts)
			{
				if (FileHelper.IsTheSameFile(e.FileName, expert.FileName))
				{
					return;
				}
			}
			experts.Add(expert);
			SaveExpertsToFile(experts);
		}
		
		static IExpert GetExpertFrom(string line)
		{
			string[] content = line.Split(';');
			if (FileHelper.IsDotNetAssembly(content[1]))
			{
				return new DotNetExpert(content[0], content[1]);
			}
			else
			{
				return new Win32Expert(content[0], content[1]);
			}
		}
		
		readonly static string fileName = Path.Combine(
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
			             "LeXtudio"),
			"experts.list");
		
		static IList<IExpert> LoadExpertsFromFile()
		{
			IList<IExpert> result = new List<IExpert>();
			if (!File.Exists(fileName))
			{
				return result;
			}
			using (StreamReader reader = new StreamReader(fileName))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					IExpert expert = GetExpertFrom(line);
					result.Add(expert);
				}
				reader.Close();
			}
			return result;
		}
		
		static void SaveExpertsToFile(IList<IExpert> experts)
		{
			string folder = Path.GetDirectoryName(fileName);
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}
			using (StreamWriter writer = new StreamWriter(fileName))
			{
				foreach (IExpert expert in experts)
				{
					writer.WriteLine(expert);
				}
				writer.Close();
			}
		}

        internal static void Install(string name, string fileName)
        {
            RegistryHelper.SetValueToRegKey(@"Software\Borland\BDS\5.0\Known IDE Assemblies", fileName, name);
        }
	}
}
