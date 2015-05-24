// this is AStyle style enum.
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
namespace Lextm.CodeBeautifiers {


	///<summary>
	///AStyle styles.
	/// </summary>
	/// <remarks>AStyle has many build-in styles. It can be extended by command line.</remarks>
	public enum AStyleStyle {
		/// <summary>
		/// Kernighan-Ritchie style.
		/// </summary>
		KernighanRitchie = 0,
		/// <summary>
		///User-defined command line.
		/// </summary>
		CommandLine,
		/// <summary>
		///GNU style.
		/// </summary>
		Gnu,
		/// <summary>
		///ANSI style.
		/// </summary>
		Ansi,
		/// <summary>
		///Java style.
		/// </summary>
		Java,
		/// <summary>
		///Linux style.
		/// </summary>
		Linux
	};
}
