using System;

namespace Lextm.CodeBeautifierCollection.Collections {
	/// <summary>Enabled record.</summary>
	[Serializable]
	public class EnabledRecord : IComparable {
		/// <summary>
		/// Compares to.
		/// </summary>
		/// <param name="obj">Object</param>
		/// <returns>1 if greater than, 0 if equal to, and -1 if less than.</returns>
		public int CompareTo(object obj) {
			if (obj is EnabledRecord) {
				EnabledRecord record = obj as EnabledRecord;
				return this.version.CompareTo(record.version);
			} else if (obj is int) {
				return this.version.CompareTo((int)obj);
            }
			throw new ArgumentException("object is not an Ending");
		}

		private int version;
		private bool enabled;
        /// <summary>BDS version.</summary>
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