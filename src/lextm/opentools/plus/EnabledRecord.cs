// this is the enabled record class.
// Copyright (C) 2006  Lex Y. Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;

namespace Lextm.OpenTools.Plus {
	/// <summary>Enabled record.</summary>
	[Serializable]
	public class EnabledRecord {
//		/// <summary>
//		/// Compares to.
//		/// </summary>
//		/// <param name="obj">Object</param>
//		/// <returns>1 if greater than, 0 if equal to, and -1 if less than.</returns>
//        /// <remarks>Only EnabledRecord and int types are valid. Other types will raise exceptions.</remarks>
//		public int CompareTo(object obj) {
//			//if (obj is EnabledRecord) {
//			EnabledRecord record = obj as EnabledRecord;
//			if (record != null)
//			{
//				return this.version.CompareTo(record.version);
//			} else //if (obj is int) {
//			{
//				return this.version.CompareTo((int)obj);
//			}
//			//throw new ArgumentException("object is not an Ending");
//		}

		private int version;
		private bool enabled;
        /// <summary>IDE version.</summary>
        public int Version {
            get {
				return version;
			}
			set {
				version = value;
            }
		}
        /// <summary>Enabled.</summary>
		public bool Enabled {
            get {
				return enabled;
			}
			set {
                enabled = value;
            }
        }
		/// <summary>Constructor.</summary>
		public EnabledRecord() { }
	}
}
