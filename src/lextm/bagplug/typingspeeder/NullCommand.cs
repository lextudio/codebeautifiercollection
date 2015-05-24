// this is null command class.
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

using Borland.Studio.ToolsAPI;

namespace Lextm.BagPlug.TypingSpeeder 
{
	/// <summary>Null command.</summary>
	/// <remarks>Part of the special cases pattern. It is also a singleton.</remarks>
	class NullCommand : ICommand 
	{
		public void Execute(IOTASourceEditor editor) {}

		private static NullCommand instance;

		public static NullCommand Instance
		{
			get
			{
				lock(typeof(NullCommand)) 
				{
					if (instance == null) 
					{
						instance = new NullCommand();
					}
				}
				return instance;
			}
		}
	}
}


