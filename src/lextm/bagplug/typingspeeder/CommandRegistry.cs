// this is command registry class.
//		It stores all valid keywords that should be expanded.
// Copyright (C) 2005-2007  Lex Y. Li
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

using System.Collections.Generic;
using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;
using Lextm.Diagnostics;
using Lextm.OpenTools;
using System.Drawing;

//using Castle.Core.Resource;
//using Castle.Windsor;
//using Castle.Windsor.Configuration.Interpreters;



namespace Lextm.BagPlug.TypingSpeeder
{
	class CommandRegistry {
		
		private CommandRegistry() {}

		private static IDictionary<string, IDictionary<string, ICommand>> storage;
		//static IWindsorContainer container;
		
		static CommandRegistry() 
		{
//			container = new WindsorContainer(
//				new XmlInterpreter(
//					new FileResource(OpenTools.IO.Path.GetDataFile("typingspeeder.config"))));
			storage = new Dictionary<string, IDictionary<string, ICommand>>();
			FillStorage();
		}
		/*TODO : use Environment.NewLine to replace all \r\n here*/
		private static void FillStorage() 
		{
			storage.Clear();
			//C#
			IDictionary<string, ICommand> csharp = new Dictionary<string, ICommand>();
			csharp.Add("if", new KeywordCommand("if", "() {\r\n}\r\n", /*Language.CSharp,*/ new Point(1, 1)));
			//csharp.Add("else if", new KeywordCommand("else if", "() {\r\n}", Language.CSharp, 1, -4));
			//new Ending("else", "{\r\n\r\n}", Language.CSharp, 1, -1),
			csharp.Add("for", new KeywordCommand("for", "( ; ; ) {\r\n}\r\n", /*Language.CSharp,*/ new Point(0, 2)));
			csharp.Add("foreach", new KeywordCommand("foreach", "(  in ) {\r\n}\r\n", /*Language.CSharp,*/ new Point(0, 2)));
			csharp.Add("try", new KeywordCommand("try", "{\r\n\r\n}\r\ncatch (Exception ex) {\r\n\r\n} finally {\r\n\r\n}\r\n",
			                                     /*Language.CSharp,*/ new Point(1, 0)));
			csharp.Add("///", new XmlCommentCommand());
			storage.Add(Language.CSharp.ToString(), csharp);
			// Delphi
			IDictionary<string, ICommand> delphi = new Dictionary<string, ICommand>();
			delphi.Add("constructor", new KeywordCommand("constructor", "Create;", /*Language.Delphi,*/ new Point(0, 6)));
			delphi.Add("destructor", new KeywordCommand("destructor", "Destroy; override;", /*Language.Delphi,*/ new Point(0, 7)));
			delphi.Add("procedure", new KeywordCommand("procedure", ";"/*, Language.Delphi*/));
			delphi.Add("function", new KeywordCommand("function", ": ;"/*, Language.Delphi*/));
			delphi.Add("property", new KeywordCommand("property", ":  read  write ;"/*, Language.Delphi*/));
			storage.Add(Language.Delphi.ToString(), delphi);
		}
		
		private static ICommand GetKeyword(string text, Language lang) {
			//IWindsorContainer commands = container.GetChildContainer(lang.ToString());
//			if (commands == null) 
//			{
//				return NullCommand.Instance;
//			}
//			ICommand command = (ICommand)commands[text];
//			if (command == null) 
//			{
//				return NullCommand.Instance;
//			}
//			return command;
			
//			if (!container.Resolvestorage.ContainsKey(lang.ToString())) {
//				return NullCommand.Instance;
//			}
			IDictionary<string, ICommand> table = storage[lang.ToString()];
			if (table.ContainsKey(text)) {
				return table[text] as ICommand;
			} else {
				return NullCommand.Instance;
			}
		}
		
		/// <summary>
		/// Dispatcher for ending matching.
		/// </summary>
		/// <param name="editor">Source editor</param>
		/// <param name="lang">Language</param>
		internal static void DispatchKeywordIn(
			IOTASourceEditor editor,
			Language lang) 
		{

			LoggingService.EnterMethod();
			string text = OtaUtils.GetWordBeforeCursor(editor);
			GetKeyword(text, lang).Execute(editor);
			LoggingService.LeaveMethod();
		}
	}
}
