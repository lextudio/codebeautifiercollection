// this is the custom file class.
// Copyright (C) 2005-2006  Lex Y. Li
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
namespace Lextm.CodeBeautifiers.IO {


	///<summary>
	///File base type.
	///</summary>
	public abstract class CustomFile : IFormattableFile {

		///<summary>
		///Beautifies.
		/// </summary>
		public abstract void Beautify( );

		private string name;

		///<summary>
		///Name.
		///</summary>
		protected string FileName {
            get {
				return name;
			}

			set {
                name = value;
            }
		}

		/// <summary>
		/// Constructor.
        /// </summary>
		protected CustomFile( ) { }
	}
}
