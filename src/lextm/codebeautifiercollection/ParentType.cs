// This is parent type enum.
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
namespace Lextm.CodeBeautifierCollection {


	///<summary>
	///CBC menu position.
	///</summary>
	///<remarks>
	///CBC menu can be at three locations. Default location is on IDE main menu. It can be placed under Tools menu or CnPack menu as well.
	///</remarks>
	public enum ParentType {

		///<summary>
		///Top menu member.
		///</summary>
			Default = 0,
		///<summary>
		///Under Tools menu.
		///</summary>
		Tools,
		///<summary>
		///Under CnPack menu.
		///</summary>
		CNPack
	};
}
