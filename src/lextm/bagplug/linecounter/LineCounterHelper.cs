// this is the line counter helper feature.
// 		It is the controller behind Line Counter Dialog.
// Copyright (C) 2007  Lex Y. Li
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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;
using Lextm.Windows.Forms;

namespace Lextm.BagPlug.LineCounter
{
	// Delegate for pluggable line counting methods
	delegate void CountLines(LineCountInfo info);

	/// <summary>
	/// Description of LineCounterHelper.
	/// </summary>
	public sealed class LineCounterHelper
	{
		private LineCounterHelper() { }

		static LineCounterHelper()		{
			// List all the countable file types (so we don't try to count .dll's,
			// images, executables, etc.
			
			m_countableTypes = new System.Collections.Specialized.StringCollection();
			#if IMPR2
			countingAlgorithms = AddInTree.BuildItems<CountingAlgorithmDescriptor>
				("/AddIns/LineCounter/CountingAlgorithms", this);
			// Iterate through algorithms to fill list of known countable types
			foreach (CountingAlgorithmDescriptor desc in countingAlgorithms) {
				m_countableTypes.AddRange(desc.extensions);
			}
			#else
			m_countableTypes.Add("*");
			m_countableTypes.Add(".cs");
			m_countableTypes.Add(".vb");
			m_countableTypes.Add(".vj");
			m_countableTypes.Add(".cpp");
			m_countableTypes.Add(".cc");
			m_countableTypes.Add(".cxx");
			m_countableTypes.Add(".c");
			m_countableTypes.Add(".hpp");
			m_countableTypes.Add(".hh");
			m_countableTypes.Add(".hxx");
			m_countableTypes.Add(".h");
			m_countableTypes.Add(".js");
			m_countableTypes.Add(".cd");
			m_countableTypes.Add(".resx");
			m_countableTypes.Add(".res");
			m_countableTypes.Add(".css");
			m_countableTypes.Add(".htm");
			m_countableTypes.Add(".html");
			m_countableTypes.Add(".xml");
			m_countableTypes.Add(".xsl");
			m_countableTypes.Add(".xslt");
			m_countableTypes.Add(".xsd");
			m_countableTypes.Add(".config");
			m_countableTypes.Add(".asax");
			m_countableTypes.Add(".ascx");
			m_countableTypes.Add(".asmx");
			m_countableTypes.Add(".aspx");
			m_countableTypes.Add(".ashx");
			m_countableTypes.Add(".idl");
			m_countableTypes.Add(".odl");
			m_countableTypes.Add(".txt");
			m_countableTypes.Add(".sql");
			// TODO: add Delphi related.
			m_countableTypes.Add(".pas");
			m_countableTypes.Add(".dpr");
			m_countableTypes.Add(".inc");
			#endif

			// Map project types to icons for use in the projects list
			m_projIconMappings = new Dictionary<string, int>();
			m_projIconMappings.Add("{00000000-0000-0000-0000-000000000000}", 0);
			m_projIconMappings.Add("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}", 1); // C#
			m_projIconMappings.Add("{F184B08F-C81C-45F6-A57F-5ABD9991F28F}", 2); // VB
			m_projIconMappings.Add("{00000001-0000-0000-0000-000000000000}", 5);

			// Map file extensions to icons for use in the file list
			m_fileIconMappings = new Dictionary<string, int>(33);
			m_fileIconMappings.Add("*", 0);
			m_fileIconMappings.Add(".cs", 1);
			m_fileIconMappings.Add(".vb", 2);
			m_fileIconMappings.Add(".vj", 3);
			m_fileIconMappings.Add(".cpp", 4);
			m_fileIconMappings.Add(".cc", 4);
			m_fileIconMappings.Add(".cxx", 4);
			m_fileIconMappings.Add(".c", 5);
			m_fileIconMappings.Add(".hpp", 6);
			m_fileIconMappings.Add(".hh", 6);
			m_fileIconMappings.Add(".hxx", 6);
			m_fileIconMappings.Add(".h", 6);
			m_fileIconMappings.Add(".js", 7);
			m_fileIconMappings.Add(".cd", 8);
			m_fileIconMappings.Add(".resx", 9);
			m_fileIconMappings.Add(".res", 9);
			m_fileIconMappings.Add(".css", 10);
			m_fileIconMappings.Add(".htm", 11);
			m_fileIconMappings.Add(".html", 11);
			m_fileIconMappings.Add(".xml", 12);
			m_fileIconMappings.Add(".xsl", 13);
			m_fileIconMappings.Add(".xslt", 13);
			m_fileIconMappings.Add(".xsd", 14);
			m_fileIconMappings.Add(".config", 15);
			m_fileIconMappings.Add(".asax", 16);
			m_fileIconMappings.Add(".ascx", 17);
			m_fileIconMappings.Add(".asmx", 18);
			m_fileIconMappings.Add(".aspx", 19);
			m_fileIconMappings.Add(".ashx", 0);
			m_fileIconMappings.Add(".idl", 0);
			m_fileIconMappings.Add(".odl", 0);
			m_fileIconMappings.Add(".txt", 0);
			m_fileIconMappings.Add(".sql", 0);

			// Prepare counting algorithm mappings
			CountLines countLinesGeneric = new CountLines(CountLinesGeneric);
			CountLines countLinesCStyle = new CountLines(CountLinesCStyle);
			CountLines countLinesVBStyle = new CountLines(CountLinesVBStyle);
			CountLines countLinesXMLStyle = new CountLines(CountLinesXMLStyle);
//		TODO: Add delphi
			CountLines countLinesDelphiStyle = new CountLines(CountLinesDelphiStyle);
			
			m_countAlgorithms = new Dictionary<string, CountLines>(33);
			m_countAlgorithms.Add("*", countLinesGeneric);
			m_countAlgorithms.Add(".cs", countLinesCStyle);
			m_countAlgorithms.Add(".vb", countLinesVBStyle);
			m_countAlgorithms.Add(".vj", countLinesCStyle);
			m_countAlgorithms.Add(".js", countLinesCStyle);
			m_countAlgorithms.Add(".cpp", countLinesCStyle);
			m_countAlgorithms.Add(".cc", countLinesCStyle);
			m_countAlgorithms.Add(".cxx", countLinesCStyle);
			m_countAlgorithms.Add(".c", countLinesCStyle);
			m_countAlgorithms.Add(".hpp", countLinesCStyle);
			m_countAlgorithms.Add(".hh", countLinesCStyle);
			m_countAlgorithms.Add(".hxx", countLinesCStyle);
			m_countAlgorithms.Add(".h", countLinesCStyle);
			m_countAlgorithms.Add(".idl", countLinesCStyle);
			m_countAlgorithms.Add(".odl", countLinesCStyle);
			m_countAlgorithms.Add(".txt", countLinesGeneric);
			m_countAlgorithms.Add(".xml", countLinesXMLStyle);
			m_countAlgorithms.Add(".xsl", countLinesXMLStyle);
			m_countAlgorithms.Add(".xslt", countLinesXMLStyle);
			m_countAlgorithms.Add(".xsd", countLinesXMLStyle);
			m_countAlgorithms.Add(".config", countLinesXMLStyle);
			m_countAlgorithms.Add(".res", countLinesGeneric);
			m_countAlgorithms.Add(".resx", countLinesXMLStyle);
			m_countAlgorithms.Add(".aspx", countLinesXMLStyle);
			m_countAlgorithms.Add(".ascx", countLinesXMLStyle);
			m_countAlgorithms.Add(".ashx", countLinesXMLStyle);
			m_countAlgorithms.Add(".asmx", countLinesXMLStyle);
			m_countAlgorithms.Add(".asax", countLinesXMLStyle);
			m_countAlgorithms.Add(".htm", countLinesXMLStyle);
			m_countAlgorithms.Add(".html", countLinesXMLStyle);
			m_countAlgorithms.Add(".css", countLinesCStyle);
			m_countAlgorithms.Add(".sql", countLinesGeneric);
			m_countAlgorithms.Add(".cd", countLinesGeneric);
//		TODO: add delphi
			m_countAlgorithms.Add(".pas", countLinesDelphiStyle);
			m_countAlgorithms.Add(".dpr", countLinesDelphiStyle);
			m_countAlgorithms.Add(".inc", countLinesDelphiStyle);
		}
		#region Helpers
		#region Line Counting Methods
		/// <summary>
		/// Count each line in a text file, logging
		/// blank lines.
		/// </summary>
		/// <param name="info">The file information data to use.</param>
		internal static void CountLinesGeneric(LineCountInfo info)
		{
			StreamReader sr = new StreamReader(info.FileName);

			string line;
			while ((line = sr.ReadLine()) != null)
			{
				info.LineCountInfoDetails.Add("TotalLines", 1);
				if (string.IsNullOrEmpty(line.Trim()))
				{
					info.LineCountInfoDetails.Add("BlankLines", 1);
				}

				info.SumMode = "Generic";
			}

			sr.Close();
		}

		/// <summary>
		/// Count each line in a c-style source file, scanning
		/// for single and multi-line comments, code, and blank lines.
		/// </summary>
		/// <param name="info">The file information data to use.</param>
		/// <remarks>This algorithm was originally created by Oz Solomon,
		/// for his PLC line counter add-in for Visual Studio 2002/2003.</remarks>
		internal static void CountLinesCStyle(LineCountInfo info)
		{
			try
			{
				StreamReader reader = new StreamReader(info.FileName);

				string line;
				bool multiLineComment = false;
				bool hasCode = false;
				bool hasComments = false;
				while ((line = reader.ReadLine()) != null)
				{
					ParseCLine(line, ref multiLineComment, ref hasCode, ref hasComments);

					if (hasComments)
					{
						info.LineCountInfoDetails.Add("normcmtlines", 1);
					}

					if (hasCode)
					{
						info.LineCountInfoDetails.Add("codelines", 1);
					}

					if (!hasCode && !hasComments)
					{
						info.LineCountInfoDetails.Add("blanklines", 1);
					}

					info.LineCountInfoDetails.Add("totallines", 1);
				}

				reader.Close();

				info.SumMode = "C-Style";
			}
			catch (Exception ex)
			{
				Lextm.Windows.Forms.MessageBoxFactory.Fatal(ex);
				throw;
			}
		}
		internal static void CountLinesDelphiStyle(LineCountInfo info) {
			using(StreamReader reader = new StreamReader(info.FileName))
			{
				string line;
				bool multiLineComment = false;
				bool hasCode = false;
				bool hasComments = false;
				char commentChar = char.MinValue;
				while ((line = reader.ReadLine()) != null)
				{
					ParseDelphiLine(line, ref commentChar, ref multiLineComment, ref hasCode, ref hasComments);

					if (hasComments)
					{
						info.LineCountInfoDetails.Add("normcmtlines", 1);
					}

					if (hasCode)
					{
						info.LineCountInfoDetails.Add("codelines", 1);
					}

					if (!hasCode && !hasComments)
					{
						info.LineCountInfoDetails.Add("blanklines", 1);
					}

					info.LineCountInfoDetails.Add("totallines", 1);
				}
				
				reader.Close();
			}
			info.SumMode = "Delphi-Style";
		}
		/// <summary>
		/// Count each line in a vb-style source file, scanning
		/// for comments, code, and blank lines.
		/// </summary>
		/// <param name="info">The file information data to use.</param>
		/// <remarks>This algorithm was originally created by Oz Solomon,
		/// for his PLC line counter add-in for Visual Studio 2002/2003.</remarks>
		internal static void CountLinesVBStyle(LineCountInfo info)
		{
			try
			{
				StreamReader reader = new StreamReader(info.FileName);

				string line;
				bool multiLineComment = false;
				bool hasCode = false;
				bool hasComments = false;
				while ((line = reader.ReadLine()) != null)
				{
					ParseVBLine(line, ref multiLineComment, ref hasCode, ref hasComments);

					if (hasComments)
					{
						info.LineCountInfoDetails.Add("normcmtlines", 1);
					}

					if (hasCode)
					{
						info.LineCountInfoDetails.Add("codelines", 1);
					}

					if (!hasCode && !hasComments)
					{
						info.LineCountInfoDetails.Add("blanklines", 1);
					}

					info.LineCountInfoDetails.Add("totallines", 1);
				}

				reader.Close();

				info.SumMode = "Visual Basic";
			}
			catch (Exception ex)
			{
				Lextm.Windows.Forms.MessageBoxFactory.Fatal(ex);
				throw;
			}
		}

		/// <summary>
		/// Count each line in an xml source file, scanning
		/// for comments, code, and blank lines.
		/// </summary>
		/// <param name="info">The file information data to use.</param>
		/// <remarks>This algorithm is based on one created by Oz Solomon,
		/// for his PLC line counter add-in for Visual Studio 2002/2003.</remarks>
		internal static void CountLinesXMLStyle(LineCountInfo info)
		{
			try
			{
				StreamReader reader = new StreamReader(info.FileName);

				string line;
				bool multiLineComment = false;
				bool hasCode = false;
				bool hasComments = false;
				while ((line = reader.ReadLine()) != null)
				{
					ParseXMLLine(line, ref multiLineComment, ref hasCode, ref hasComments);

					if (hasComments)
					{
						info.LineCountInfoDetails.Add("normcmtlines", 1);
					}

					if (hasCode)
					{
						info.LineCountInfoDetails.Add("codelines", 1);
					}

					if (!hasCode && !hasComments)
					{
						info.LineCountInfoDetails.Add("blanklines", 1);
					}

					info.LineCountInfoDetails.Add("totallines", 1);
				}

				reader.Close();

				info.SumMode = "XML";
			}
			catch (Exception ex)
			{
				Lextm.Windows.Forms.MessageBoxFactory.Fatal(ex);
				throw;
			}
		}

		/// <summary>
		/// Determines if the two input characters ch and chNext
		/// match the given pair of characters a and b.
		/// </summary>
		/// <param name="a">First char requirement.</param>
		/// <param name="b">Second char requirement.</param>
		/// <param name="ch">First char to test.</param>
		/// <param name="chNext">Second char to test.</param>
		/// <returns></returns>
		private static bool IsPair(char a, char b, char ch, char chNext)
		{
			return (ch == a && chNext == b);
		}

		/// <summary>
		/// Parses a c-style code line for comments, code, and blanks.
		/// </summary>
		/// <param name="line"></param>
		/// <param name="multiLineComment"></param>
		/// <param name="hasCode"></param>
		/// <param name="hasComments"></param>
		/// <remarks>This algorithm was originally created by Oz Solomon,
		/// for his PLC line counter add-in for Visual Studio 2002/2003.</remarks>
		private static void ParseCLine(string line, ref bool multiLineComment, ref bool hasCode, ref bool hasComments)
		{
			bool inString = false;
			//bool inTwoPairSequence = false;

			hasComments = multiLineComment;
			hasCode = false;

			for (int i = 0; i < line.Length; i++)
			{
				char ch = line[i];
				char chNext = (i < line.Length - 1 ? line[i + 1] : '\0');

				// Process a single-line comment
				if (IsPair('/', '/', ch, chNext) && !multiLineComment && !inString)
				{
					hasComments = true;
					return;
				}

				// Process start of a multiline comment
				else if (IsPair('/', '*', ch, chNext) && !multiLineComment && !inString)
				{
					multiLineComment = true;
					hasComments = true;
					++i;
				}

				// Process end of a multiline comment
				else if (IsPair('*', '/', ch, chNext) && !inString)
				{
					multiLineComment = false;
					++i;
				}

				// Process escaped character
				else if (ch == '\\' && !multiLineComment)
				{
					++i;
					hasCode = true;
				}

				// Process string
				else if (ch == '"' && !multiLineComment)
				{
					inString = !inString;
					hasCode = true;
				}

				else if (!multiLineComment)
				{
					if (!Char.IsWhiteSpace(ch))
					{
						hasCode = true;
					}
				}
			}
		}
		private static void ParseDelphiLine(string line, ref char commentChar, ref bool multiLineComment, ref bool hasCode, ref bool hasComments) {
			bool inString = false;
			//bool inTwoPairSequence = false;

			hasComments = multiLineComment;
			hasCode = false;

			for (int i = 0; i < line.Length; i++)
			{
				char ch = line[i];
				char chNext = (i < line.Length - 1 ? line[i + 1] : '\0');

				// Process a single-line comment
				if (IsPair('/', '/', ch, chNext) && !multiLineComment && !inString)
				{
					hasComments = true;
					return;
				}
				else if (IsPair('(', '*', ch, chNext) && !multiLineComment && !inString)
				{
					multiLineComment = true;
					commentChar = '(';
					hasComments = true;
					++i;
				}
				// Process end of a multiline comment
				else if (IsPair('*',')', ch, chNext) && commentChar == '(' && !inString)
				{
					multiLineComment = false;
					++i;
				}
				// Process start of a multiline comment
				else if (ch == '{' && !multiLineComment && !inString)
				{
					multiLineComment = true;
					commentChar = '{';
					hasComments = true;
				}

				// Process end of a multiline comment
				else if (ch == '}' && commentChar == '{' && !inString)
				{
					multiLineComment = false;
				}

				// Process escaped character
				else if (ch == '\\' && !multiLineComment)
				{
					++i;
					hasCode = true;
				}

				// Process string
				else if (ch == '\'' && !multiLineComment)
				{
					inString = !inString;
					hasCode = true;
				}

				else if (!multiLineComment)
				{
					if (!Char.IsWhiteSpace(ch))
					{
						hasCode = true;
					}
				}
			}
		}
		/// <summary>
		/// Parses a vb-style code line for comments, code and blanks.
		/// </summary>
		/// <param name="line"></param>
		/// <param name="multiLineComment"></param>
		/// <param name="hasCode"></param>
		/// <param name="hasComments"></param>
		/// <remarks>This algorithm was originally created by Oz Solomon,
		/// for his PLC line counter add-in for Visual Studio 2002/2003.</remarks>
		private static void ParseVBLine(string line, ref bool multiLineComment, ref bool hasCode, ref bool hasComments)
		{
			//bool inString = false;
			//bool inTwoPairSequence = false;

			multiLineComment = false;

			line = line.Trim();

			if (line.Length == 0)
			{
				hasCode = false;
				hasComments = false;
				return;
			}

			if (line[0] == '\'')
			{
				hasCode = false;
				hasComments = true;
				return;
			}

			if (line.IndexOf('\'') != -1)
			{
				hasCode = true;
				hasComments = true;
				return;
			}

			hasCode = true;
			hasComments = true;
		}

		/// <summary>
		/// Parses an xml-style code line for comments, markup, and blanks.
		/// </summary>
		/// <param name="line"></param>
		/// <param name="multiLineComment"></param>
		/// <param name="hasCode"></param>
		/// <param name="hasComments"></param>
		/// <remarks>This algorithm is based on one created by Oz Solomon,
		/// for his PLC line counter add-in for Visual Studio 2002/2003.</remarks>
		private static void ParseXMLLine(string line, ref bool multiLineComment, ref bool hasCode, ref bool hasComments)
		{
			bool inString = false;
			//bool inTwoPairSequence = false;

			hasComments = multiLineComment;
			hasCode = false;

			for (int i = 0; i < line.Length; i++)
			{
				char ch1 = line[i];
				char ch2 = (i < line.Length-1 ? line[i + 1] : '\0');
				char ch3 = (i+1 < line.Length-1 ? line[i + 2] : '\0');
				char ch4 = (i+2 < line.Length-1 ? line[i + 3] : '\0');

				// Process start of XML comment
				if (IsPair('<', '!', ch1, ch2) && IsPair('-', '-', ch3, ch4) && !multiLineComment && !inString)
				{
					multiLineComment = true;
					hasComments = true;
					i += 3;
				}

				// Process end of XML comment
				else if (IsPair('-', '-', ch1, ch2) && ch3 == '>' && !inString)
				{
					multiLineComment = false;
					i += 2;
				}

				// Process string
				else if (ch3 == '"' && !multiLineComment)
				{
					inString = !inString;
					hasCode = true;
				}

				else if (!multiLineComment)
				{
					if (!Char.IsWhiteSpace(ch1))
					{
						hasCode = true;
					}
				}
			}
		}
		#endregion

		#region Scanning and Summation Methods
		/// <summary>
		/// Scans the solution and creates a hierarchy of
		/// support objects for each project and file
		/// within each project.
		/// </summary>
		internal static void ScanSolution(ProgressBar tsprgTotal, ProgressBar tsprgTask)
		{
			if (m_summaryList == null)
				m_summaryList = new List<LineCountSummary>();

			m_summaryList.Clear();

			IOTAProjectGroup solution = OtaUtils.GetCurrentProjectGroup();
			//Solution solution = ProjectService.OpenSolution;
			if (solution != null) // OpenSolution is null when no solution is opened
			{
				//FileInfo fiSolution = new FileInfo(solution.FileName);
				LineCountSummary summary = new LineCountSummary("All Projects", (int)m_projIconMappings["{00000000-0000-0000-0000-000000000000}"]);
				m_summaryList.Add(summary);

				// Configure progress bars
				tsprgTotal.Minimum = 0;
				tsprgTotal.Step = 1;
				tsprgTask.Minimum = 0;
				tsprgTask.Step = 1;

				IList<IOTAProject> projects = OtaUtils.GetAllProjectsOf(solution);
				tsprgTotal.Maximum = projects.Count;
				tsprgTask.Value = 0;
				foreach (IOTAProject fiProject in projects) {
					tsprgTotal.PerformStep();
					string projName, lang;
					if (fiProject.FileName.IndexOf("://", StringComparison.Ordinal) != -1)
					{
						projName = fiProject.FileName; // this is a web project
						lang = "{00000001-0000-0000-0000-000000000000}";
					} else {
						projName = fiProject.FileName;
						lang = fiProject.ProjectGUID.ToString();
					}

					int iconIndex;
					//#if IMPR1
					//iconIndex = projectImageListHelper.GetIndex(IconService.GetImageForProjectType(fiProject.Language ?? "defaultLanguageName"));
					//#else
					if (m_projIconMappings.ContainsKey(lang))
					{
						iconIndex = (int)m_projIconMappings[lang];
					} else {
						iconIndex = 0;
					}
					//m_projIconMappings.TryGetValue(lang, out iconIndex); // default icon 0
					//#endif
					summary = new LineCountSummary(projName, iconIndex);
					m_summaryList.Add(summary);

					tsprgTask.Maximum = 0;
					tsprgTotal.Value = 0;
					ScanProjectItems(tsprgTask, OtaUtils.GetAllModuleInfoOf(fiProject), summary);
				}

				tsprgTask.Value = tsprgTask.Maximum;
				tsprgTotal.Value = tsprgTotal.Maximum;
			}
			else
			{
				MessageBoxFactory.Warn("There is no project group open.");
			}
		}

		/// <summary>
		/// Scans the project items (files, usually) of
		/// a project's ProjectItems collection.
		/// </summary>
		/// <param name="projectItems">The ProjectItems collection to scan.</param>
		/// <param name="summary">The root summary data object that these
		/// files belong to.</param>
		private static void ScanProjectItems(ProgressBar tsprgTask, System.Collections.Generic.IList<IOTAModuleInfo> projectItems, LineCountSummary summary)
		{
			tsprgTask.Maximum += projectItems.Count;
			foreach (IOTAModuleInfo projectItem in projectItems)
			{
				tsprgTask.PerformStep();
				//if (!(projectItem is FileProjectItem)) {
				// Skip references and other special MSBuild things
				//continue;
				//}
				string projectFile = projectItem.FileName;
				if ((!String.IsNullOrEmpty(projectFile)) && (!Directory.Exists(projectFile)))
				{
					int iconIndex = 0;
					//#if IMPR1
					//iconIndex = fileImageListHelper.GetIndex(IconService.GetImageForFile(projectFile));
					//#else
					string ext = Path.GetExtension(projectFile);
					if (m_fileIconMappings.ContainsKey(ext)) {
						iconIndex = (int)m_fileIconMappings[ext];
					} else {
						iconIndex = 0;
					}
					//m_fileIconMappings.TryGetValue(Path.GetExtension(projectFile), out iconIndex);
					//#endif
					summary.FileLineCountInfo.Add(new LineCountInfo(projectFile, iconIndex, summary));
				}
			}
		}

		/// <summary>
		/// Performs a complete counting and summation of all lines
		/// in all projects and files.
		/// </summary>
		internal static void SumSolution(ProgressBar tsprgTotal, ProgressBar tsprgTask, Vista_Api.ListView lvFileList, Vista_Api.ListView lvSummary)
		{

			// Clean the list
			lvSummary.Items.Clear();
			lvFileList.Items.Clear();
			//lvFileList.Groups.Clear();

			// Configure progress bars
			tsprgTotal.Minimum = 0;
			tsprgTotal.Step = 1;
			tsprgTask.Minimum = 0;
			tsprgTask.Step = 1;

			// Skip if there are no projects
			if (m_summaryList == null || (m_summaryList != null && m_summaryList.Count == 1))
			{
				MessageBoxFactory.Warn("There are no projects loaded to summarize.");
				return;
			}

			// Get all projects summary
			LineCountSummary allProjects = (LineCountSummary)m_summaryList[0];
			allProjects.LineCountSummaryDetails.Reset();
			AddSummaryListItem(lvSummary, allProjects/*, lvSummary.Groups["lvgAllProj"]*/);

			tsprgTotal.Maximum = m_summaryList.Count;
			tsprgTotal.Value = 0;
			for (int s = 1; s < m_summaryList.Count; s++)
			{
				tsprgTotal.PerformStep();

				LineCountSummary summary = (LineCountSummary)m_summaryList[s];
				summary.LineCountSummaryDetails.Reset();
				AddSummaryListItem(lvSummary, summary/*, lvSummary.Groups["lvgEachProj"]*/);

				tsprgTask.Maximum = summary.FileLineCountInfo.Count;
				tsprgTask.Value = 0;
				for (int i = 0; i < summary.FileLineCountInfo.Count; i++)
				{
					tsprgTask.PerformStep();

					LineCountInfo info = (LineCountInfo)summary.FileLineCountInfo[i];
					if (m_countableTypes.Contains(info.FileType))
					{
						info.LineCountInfoDetails.Reset();
						#if IMPR2
						foreach (CountingAlgorithmDescriptor desc in countingAlgorithms) {
							if (desc.CanCountLines(info)) {
								desc.GetAlgorithm().CountLines(info);
								break;
							}
						}
						#else
						try
						{
							CountLines counter = (CountLines)m_countAlgorithms[info.FileType];
							counter(info);
						}
						catch (Exception ex)
						{
							Lextm.Windows.Forms.MessageBoxFactory.Fatal(ex);
							throw;
						}
						
						#endif
						info.LineCountInfoDetails.Summarize();

						allProjects.LineCountSummaryDetails.Add(info.LineCountInfoDetails);
						summary.LineCountSummaryDetails.Add(info.LineCountInfoDetails);

						//tstxtLinesCounted.Text = allProjects.LineCountSummaryDetails.TotalLines.ToString();

						AddFileListItem(lvFileList, info);
					}
				}

				summary.LineCountSummaryDetails.Summarize();
				LineCountDetails details = summary.LineCountSummaryDetails;
				summary.LinkedListViewItem.SubItems[1].Text = details.TotalLines.ToString(CultureInfo.InvariantCulture);
				summary.LinkedListViewItem.SubItems[2].Text = details.CodeLines.ToString(CultureInfo.InvariantCulture);
				summary.LinkedListViewItem.SubItems[3].Text = details.CommentLines.ToString(CultureInfo.InvariantCulture);
				summary.LinkedListViewItem.SubItems[4].Text = details.BlankLines.ToString(CultureInfo.InvariantCulture);
				summary.LinkedListViewItem.SubItems[5].Text = details.NetLines.ToString(CultureInfo.InvariantCulture);
				details = null;
			}

			allProjects.LineCountSummaryDetails.Summarize();
			LineCountDetails totals = allProjects.LineCountSummaryDetails;
			allProjects.LinkedListViewItem.SubItems[1].Text = totals.TotalLines.ToString(CultureInfo.InvariantCulture);
			allProjects.LinkedListViewItem.SubItems[2].Text = totals.CodeLines.ToString(CultureInfo.InvariantCulture);
			allProjects.LinkedListViewItem.SubItems[3].Text = totals.CommentLines.ToString(CultureInfo.InvariantCulture);
			allProjects.LinkedListViewItem.SubItems[4].Text = totals.BlankLines.ToString(CultureInfo.InvariantCulture);
			allProjects.LinkedListViewItem.SubItems[5].Text = totals.NetLines.ToString(CultureInfo.InvariantCulture);


			tsprgTotal.Value = tsprgTotal.Maximum;
		}

		/// <summary>
		/// Adds a summary item to the projects list view.
		/// </summary>
		/// <param name="summary">The summary data object to reference.</param>
		/// <param name="group">The summary list view group this item
		/// should be listed under.</param>
		private static void AddSummaryListItem(Vista_Api.ListView lvSummary, LineCountSummary summary/*, Vista_Api.ListViewGroup group*/)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text = summary.ProjectName;
			lvi.SubItems.Add("0");
			lvi.SubItems.Add("0");
			lvi.SubItems.Add("0");
			lvi.SubItems.Add("0");
			lvi.SubItems.Add("0");

			lvi.Tag = summary;
			lvi.ImageIndex = summary.IconIndex;
			//lvi.StateImageIndex = summary.IconIndex;
			//lvi.Group = group;

			summary.LinkedListViewItem = lvi;

			lvSummary.Items.Add(lvi);
		}

		/// <summary>
		/// Adds a file information item to the file list view.
		/// </summary>
		/// <param name="info">The file information data object.</param>
		private static void AddFileListItem(Vista_Api.ListView lvFileList, LineCountInfo info)
		{
			FileInfo fileInfo = new FileInfo(info.FileName);

			ListViewItem lvi = new ListViewItem();
			lvi.Text = fileInfo.Name;
			lvi.SubItems.Add(info.LineCountInfoDetails.TotalLines.ToString(CultureInfo.InvariantCulture));
			lvi.SubItems.Add(info.LineCountInfoDetails.CodeLines.ToString(CultureInfo.InvariantCulture));
			lvi.SubItems.Add(info.LineCountInfoDetails.CommentLines.ToString(CultureInfo.InvariantCulture));
			lvi.SubItems.Add(info.FileType);
			lvi.SubItems.Add(info.SumMode);

			lvi.Tag = info;

			lvi.ImageIndex = info.IconIndex;
			//lvi.StateImageIndex = iconIndex;


//			if (tsmiGroupByType.Checked)
//			{
//				Vista_Api.ListViewGroup group = lvFileList.Groups["groupType" + info.FileType.Substring(1)];
//				if (group == null)
//				{
//					group = new Vista_Api.ListViewGroup("groupType" + info.FileType.Substring(1), info.FileType.Substring(1).ToUpperInvariant() + " Files");
//					lvFileList.Groups.Add(group);
//				}
//
//				lvi.Group = group;
//			}
//			else if (tsmiGroupByProj.Checked)
//			{
//				Vista_Api.ListViewGroup group = lvFileList.Groups["groupProj" + info.ProjectSummary.ProjectName];
//				if (group == null)
//				{
//					group = new Vista_Api.ListViewGroup("groupProj" + info.ProjectSummary.ProjectName, info.ProjectSummary.ProjectName + " Files");
//					lvFileList.Groups.Add(group);
//				}
//
//				lvi.Group = group;
//			}

			lvFileList.Items.Add(lvi);
		}
		#endregion

		#endregion
		#region Variables
		private static IList<LineCountSummary> m_summaryList;
		private static IDictionary<string, int> m_projIconMappings;
		private static IDictionary<string, int> m_fileIconMappings;
		private static IDictionary<string, CountLines> m_countAlgorithms;
		private static System.Collections.Specialized.StringCollection m_countableTypes;
		
		#if IMPR1
		ImageListHelper fileImageListHelper;
		ImageListHelper projectImageListHelper;
		#endif
		#if IMPR2
		List<CountingAlgorithmDescriptor> countingAlgorithms;
		#endif
		#endregion
	}
}
