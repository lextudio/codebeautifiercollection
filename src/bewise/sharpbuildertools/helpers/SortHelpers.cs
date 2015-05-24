using System;
using System.Collections.Generic;

namespace BeWise.SharpBuilderTools.Helpers {
	/// <summary>Sort helper.</summary>
	class SortHelper : IComparer<string> {

    	internal SortHelper() {}
        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private bool fAscending = true;
        private bool fCaseSensitive = true;

        /**************************************************************/
        /*                        Public
        /**************************************************************/
		/// <summary>Compares.</summary>
		public int Compare(string x,string y) {
            StringComparison option = (CaseSensitive) ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            if (Ascending) {
                return string.Compare(x, y, option);
            } else {
                return string.Compare(y, x, option);
            }
        }

        /**************************************************************/
		/*                        Properties
        /**************************************************************/

		/// <summary>Ascanding.</summary>
		internal bool Ascending {
            get {
                return fAscending;
            }

            set {
                fAscending = value;
            }
		}

		/// <summary>Case sensitive.</summary>
		internal bool CaseSensitive {
            get {
                return fCaseSensitive;   
            }

            set {
                fCaseSensitive = value;
            }
        }
    }
}
