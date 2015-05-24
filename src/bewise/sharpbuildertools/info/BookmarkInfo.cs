
namespace BeWise.SharpBuilderTools.Info {
    /// <summary>
    /// Bookmark info.
    /// </summary>
	public class BookmarkInfo {
        /**************************************************************/
        /*                        Constructor
        /**************************************************************/
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <param name="bookmarkNumber">Bookmark number</param>
		/// <param name="lineNumber">Line number</param>
		/// <param name="lineValue">Line value</param>
        public BookmarkInfo(string fileName, int bookmarkNumber, int lineNumber, string lineValue) {
            this.fileName = fileName;
            this.bookmarkNumber = bookmarkNumber;
            this.lineNumber = lineNumber;
            this.lineValue = lineValue;
        }

        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private int bookmarkNumber;
        private string fileName;
        private int lineNumber;
        private string lineValue;

        /**************************************************************/
        /*                     Properties
        /**************************************************************/
		/// <summary>
		/// Bookmark number.
		/// </summary>
        public int BookmarkNumber {
            get {
                return bookmarkNumber;
            }
        }
		/// <summary>
		/// File name.
		/// </summary>
        public string FileName {
            get {
                return fileName;
            }
        }
		/// <summary>
		/// Line number.
		/// </summary>
        public int LineNumber {
            get {
                return lineNumber;
            }
        }
		/// <summary>
		/// Line value.
		/// </summary>
        public string LineValue {
            get {
                return lineValue;
            }
        }

    }
}
