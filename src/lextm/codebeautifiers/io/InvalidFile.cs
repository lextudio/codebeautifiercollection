// this is the invalid file class.
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

    using System.Globalization;
    using Lextm.OpenTools;

	///<summary>
	///Invalid file type.
	///</summary>
	internal sealed class InvalidFile : CustomFile, IFormattableFile {

		///<summary>
		///Beautifies.
		///</summary>
		public override void Beautify( ) {
			MessageService.Show(System.String.Format(CultureInfo.InvariantCulture, 
				"File type is not supported: {0}", FileName));
		}

		
		private InvalidFile( ) { }
		
		private static InvalidFile instance;

		/// <summary>
		/// Gets singleton instance.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns>Singleton instance.</returns>
		internal static InvalidFile getInstance( string fileName ) {
			lock(typeof(Lextm.CodeBeautifiers.IO.InvalidFile)){
				if (instance == null)
			    {
					instance = new Lextm.CodeBeautifiers.IO.InvalidFile();
				}
				instance.FileName = fileName;
			}     			
		    return instance;
        }
	}
}
