using System;
using System.Collections.Generic;
using System.Globalization;

using BeWise.Common.Utils;
using Lextm.CodeBeautifiers.IO;
using Lextm.OpenTools;

namespace BeWise.SharpBuilderTools.Helpers {
	/// <summary>
	/// Base of language related code helpers.
	/// </summary>
	public abstract class BaseLanguageCodeHelper {
		/// <summary>
		/// Default tool path.
		/// </summary>
		protected readonly static string DefaultPath =
			Lextm.OpenTools.IO.Path.BundledFolder;

		/// <summary>
		/// Beautifies file.
		/// </summary>
		/// <param name="fileName">File name</param>
		public void BeautifyFile( string fileName ) {
			IFormattableFile file = GetFormattableFile(fileName);
            file.Beautify();
		}
        /// <summary>
        /// Get IFormattable file.
        /// </summary>
        /// <param name="fileName">File name</param>
		protected abstract IFormattableFile GetFormattableFile(string fileName);
        /// <summary>
        /// Gets indentation.
        /// </summary>
        /// <returns></returns>
		public abstract int GetIndentation();
		/// <summary>
		/// Comments a line.
		/// </summary>
		/// <param name="aLine">Line to comment</param>
		/// <returns>Commented line.</returns>
		public abstract string CommentLine(string aLine);
		/// <summary>
		/// Creates region footer.
		/// </summary>
		/// <param name="aRegionText">Region text</param>
		/// <returns>Footer text.</returns>
        public abstract string CreateRegionFooter(string aRegionText);
        /// <summary>
        /// Creates region header.
        /// </summary>
        /// <param name="aRegionText">Region text</param>
        /// <returns>Header text.</returns>
        public abstract string CreateRegionHeader(string aRegionText);
        /// <summary>
        /// Creates string from lines.
        /// </summary>
        /// <param name="aLines">Lines</param>
        /// <returns></returns>
        public abstract IList<string> CreateString(IList<string> aLines);
        /// <summary>
        /// Comments line.
        /// </summary>
        /// <param name="aLines">Lines</param>
        /// <returns>Commented lines.</returns>
        public abstract IList<string> CommentLines(IList<string> aLines);
        /// <summary>
        /// Verifies if a line is commented.
        /// </summary>
        /// <param name="aLine">Line</param>
        /// <returns></returns>
        public abstract bool IsCommented(string aLine);
        /// <summary>
        /// Verifies if lines are commented.
        /// </summary>
        /// <param name="aLines">Lines</param>
        /// <returns></returns>
        public abstract bool IsCommented(IList<string> aLines);
        /// <summary>
        /// Uncomments a line.
        /// </summary>
        /// <param name="aLine">Line</param>
        /// <returns>Uncommented line.</returns>
        public abstract string UncommentLine(string aLine);
        /// <summary>
        /// Uncomments lines.
        /// </summary>
        /// <param name="aLines">Lines</param>
        /// <returns>Uncommented lines.</returns>
        public abstract IList<string> UncommentLines(IList<string> aLines);
	}
	
	/// <summary>
	/// Null object.
	/// </summary>
	internal class NullLanguageCodeHelper : BaseLanguageCodeHelper {

        internal NullLanguageCodeHelper() { }
		/// <summary>
		/// Gets IFormattable file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
        protected override IFormattableFile GetFormattableFile(string fileName) {
			return InvalidFile.getInstance(fileName);
		}
		/// <summary>
		/// Gets indentation.
		/// </summary>
		/// <returns></returns>
		public override int GetIndentation() {
            return 0;
        }
		/// <summary>
		/// Comments a line.
		/// </summary>
		/// <param name="aLine">Line</param>
		/// <returns></returns>
		public override string CommentLine(string aLine) {
            return aLine;
        }
		/// <summary>
		/// Comments lines.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> CommentLines(IList<string> aLines)
        {
//            aLines[0] = "/*" + aLines[0];
//            aLines[aLines.Length -1] = aLines[aLines.Length -1] + "*/";

			return aLines;
        }
		/// <summary>
		/// Creates region footer.
		/// </summary>
		/// <param name="aRegionText">Region text</param>
		/// <returns></returns>
        public override string CreateRegionFooter(string aRegionText) {
            throw new CoreException("Unsupported CreateRegionFooter for Xml");
        }
		/// <summary>
		/// Creates region header.
		/// </summary>
		/// <param name="aRegionText">Region text</param>
		/// <returns></returns>
        public override string CreateRegionHeader(string aRegionText) {
            throw new CoreException("Unsupported CreateRegionHeader for Xml");
        }
		/// <summary>
		/// Creates string.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> CreateString(IList<string> aLines) {
            // Replace Double Quotes
//            for(int i=0; i < aLines.Length; i++) {
//                aLines[i] = aLines[i].Replace("\"", "\\\"");
//            }
//
//            // Add the Double Quotes
//            for(int i=0; i < aLines.Length; i++) {
//				aLines[i] = "\"" + aLines[i] + "\\r\\n\"";
//
//				if (i != aLines.Length -1) {
//					aLines[i] += " +";
//				} else {
//					aLines[i] += ";";
//				}
//			}

			return aLines;
		}
		/// <summary>
		/// Is commented.
		/// </summary>
		/// <param name="aLine">Line.</param>
		/// <returns></returns>
        public override bool IsCommented(string aLine) {
			return false;//aLine.StartsWith("//");
		}
		/// <summary>
		/// Is commented.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override bool IsCommented(IList<string> aLines)
        {
			return false;//(aLines[0].StartsWith("/*") && aLines[aLines.Length-1].EndsWith("*/"));
        }
		/// <summary>
		/// Uncomments a line.
		/// </summary>
		/// <param name="aLine">Line</param>
		/// <returns></returns>
        public override string UncommentLine(string aLine) {
			return aLine;//.Substring(2);
        }
		/// <summary>
		/// Uncomments lines.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> UncommentLines(IList<string> aLines)
        {
//			aLines[0] = aLines[0].Substring(2);
//			aLines[aLines.Length -1] = aLines[aLines.Length -1].Substring(0, aLines[aLines.Length -1].Length -2);
            return aLines;
		}
	}
	
	/// <summary>
	/// BASIC code helper.
	/// </summary>
	internal class BasicLanguageCodeHelper : BaseLanguageCodeHelper {

    	internal BasicLanguageCodeHelper() {}
		/// <summary>
		/// Gets IFormattable file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
		protected override IFormattableFile GetFormattableFile(string fileName) {
			return InvalidFile.getInstance(fileName);
		}
		/// <summary>
		/// Gets indentation.
		/// </summary>
		/// <returns></returns>		
		public override int GetIndentation() {
            return OtaUtils.GetBasicIndentationFromOptions();
        }
		/// <summary>
		/// Comments a line.
		/// </summary>
		/// <param name="aLine">Line</param>
		/// <returns></returns>
		public override string CommentLine(string aLine) {
			aLine = "'" + aLine;
            return aLine;
        }
		/// <summary>
		/// Comments lines.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> CommentLines(IList<string> aLines)
        {
			for (int i = 0; i < aLines.Count; i++)
			{
				aLines[i] = CommentLine(aLines[i]);  
			}
			return aLines;
        }
		/// <summary>
		/// Creates region footer.
		/// </summary>
		/// <param name="aRegionText">Region text</param>
		/// <returns></returns>
        public override string CreateRegionFooter(string aRegionText) {
			return "#End Region";
        }
		/// <summary>
		/// Creates region header.
		/// </summary>
		/// <param name="aRegionText">Region text</param>
		/// <returns></returns>        
        public override string CreateRegionHeader(string aRegionText) {
            return "#Region " + Lextm.StringHelper.QuoteString(aRegionText);
        }
		/// <summary>
		/// Creates string.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> CreateString(IList<string> aLines)
        {
            throw new NotImplementedException("VB support is not yet finished");
		}
		/// <summary>
		/// Is commented.
		/// </summary>
		/// <param name="aLine">Line.</param>
		/// <returns></returns>		
        public override bool IsCommented(string aLine) {
			return aLine.StartsWith("'", StringComparison.Ordinal);
		}
		/// <summary>
		/// Is commented.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override bool IsCommented(IList<string> aLines)
        {
			for (int i = 0; i < aLines.Count; i++)
            {
				if (!aLines[i].StartsWith("'", StringComparison.Ordinal))
				{
					return false;
                }
            } 			
			return true;
        }
		/// <summary>
		/// Uncomments a line.
		/// </summary>
		/// <param name="aLine">Line</param>
		/// <returns></returns>
		public override string UncommentLine(string aLine) {
			return aLine.Substring(1);
        }
		/// <summary>
		/// Uncomments lines.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> UncommentLines(IList<string> aLines)
        {
			for (int i = 0; i < aLines.Count; i++)
			{
				aLines[i] = UncommentLine(aLines[i]);
            }
			return aLines;
        }

	}

	/// <summary>
	/// C# code helper.
	/// </summary>
	internal class CSLanguageCodeHelper : BaseLanguageCodeHelper {

    	internal CSLanguageCodeHelper() {}
		/// <summary>
		/// Gets IFormattable file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
		protected override IFormattableFile GetFormattableFile(string fileName) {
			return new AStyleFile(fileName, DefaultPath,
        	                              new object[] {System.String.Format(CultureInfo.InvariantCulture, "{0} {1}", 
			                                                                 Lextm.OpenTools.PropertyRegistry.Get("AStyleParams"),
        	                                                                 Lextm.StringHelper.QuoteString(fileName))});
		}
		/// <summary>
		/// Gets indentation.
		/// </summary>
		/// <returns></returns>
		public override int GetIndentation() {
            return OtaUtils.GetCSIndentationFromOptions();
        }
		/// <summary>
		/// Comments a line.
		/// </summary>
		/// <param name="aLine">Line</param>
		/// <returns></returns>
		public override string CommentLine(string aLine) {
            return "//" + aLine;
        }
		/// <summary>
		/// Comments lines.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> CommentLines(IList<string> aLines)
        {
            aLines[0] = "/*" + aLines[0];
            aLines[aLines.Count - 1] = aLines[aLines.Count - 1] + "*/";

            return aLines;
        }
		/// <summary>
		/// Creates region footer.
		/// </summary>
		/// <param name="aRegionText">Region text</param>
		/// <returns></returns>
        public override string CreateRegionFooter(string aRegionText) {
            return "#endregion";
        }
		/// <summary>
		/// Creates region header.
		/// </summary>
		/// <param name="aRegionText">Region text</param>
		/// <returns></returns>        
        public override string CreateRegionHeader(string aRegionText) {
            return "#region " + aRegionText;
        }
		/// <summary>
		/// Creates string.
		/// </summary>
		/// <param name="lines">Lines</param>
		/// <returns></returns>
        public override IList<string> CreateString(IList<string> lines)
        {
            List<string> result = new List<string>(lines.Count);
            // Add the Quotes
            for (int i = 0; i < lines.Count; i++)
            {
                if (i == lines.Count - 1)
                {
                    result[i] = "\"" + lines[i].Replace("\"", "\\\"") + "\"";
                }
                else
                {
                    result[i] = "\"" + lines[i].Replace("\"", "\\\"") + "\\r\\n\" +";
                }
            }
            return result;
        }
		/// <summary>
		/// Is commented.
		/// </summary>
		/// <param name="aLine">Line.</param>
		/// <returns></returns>
        public override bool IsCommented(string aLine) {
            return aLine.StartsWith("//", StringComparison.Ordinal);
        }
		/// <summary>
		/// Is commented.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override bool IsCommented(IList<string> aLines)
        {
            return (aLines[0].StartsWith("/*", StringComparison.Ordinal) && aLines[aLines.Count - 1].EndsWith("*/", StringComparison.Ordinal));
        }
		/// <summary>
		/// Uncomments a line.
		/// </summary>
		/// <param name="aLine">Line</param>
		/// <returns></returns>
        public override string UncommentLine(string aLine) {
            return aLine.Substring(2);
        }
		/// <summary>
		/// Uncomments lines.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> UncommentLines(IList<string> aLines)
        {
            aLines[0] = aLines[0].Substring(2);
			aLines[aLines.Count - 1] = aLines[aLines.Count - 1].Substring(0, aLines[aLines.Count - 1].Length -2);
            return aLines;
        }

    }

	/// <summary>
	/// Delphi code helper.
	/// </summary>
	internal class DelphiLanguageCodeHelper : BaseLanguageCodeHelper {

        internal DelphiLanguageCodeHelper() {}
		/// <summary>
		/// Gets IFormattable file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
		protected override IFormattableFile GetFormattableFile(string fileName) {
			return new JcfFile(fileName, DefaultPath,
        	                           new object[] {System.String.Format(CultureInfo.InvariantCulture, "{0} {1}", 
			                                                              Lextm.OpenTools.PropertyRegistry.Get("JcfParams"),
        	                                                              Lextm.StringHelper.QuoteString(fileName))});
		}
		/// <summary>
		/// Gets indentation.
		/// </summary>
		/// <returns></returns>		
		public override int GetIndentation() {
			return OtaUtils.GetDelphiIndentationFromOptions();
		}
		/// <summary>
		/// Comments a line.
		/// </summary>
		/// <param name="aLine">Line</param>
		/// <returns></returns>
		public override string CommentLine(string aLine) {
            return "//" + aLine;
        }
		/// <summary>
		/// Comments lines.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> CommentLines(IList<string> aLines)
        {
            aLines[0] = "(*" + aLines[0];
            aLines[aLines.Count - 1] = aLines[aLines.Count - 1] + "*)";

            return aLines;
        }
		/// <summary>
		/// Creates region footer.
		/// </summary>
		/// <param name="aRegionText">Region text</param>
		/// <returns></returns>
        public override string CreateRegionFooter(string aRegionText) {
            return "{$ENDREGION}";
        }
		/// <summary>
		/// Creates region header.
		/// </summary>
		/// <param name="aRegionText">Region text</param>
		/// <returns></returns>        
        public override string CreateRegionHeader(string aRegionText) {
            if (String.IsNullOrEmpty(aRegionText)) {
                return "{$REGION}";
            } else {
                return "{$REGION '" + aRegionText + "'}";
            }
        }
		/// <summary>
		/// Creates string.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> CreateString(IList<string> aLines)
        {
            List<string> result = new List<string>(aLines.Count);
            // Add the Quotes
            for(int i=0; i < aLines.Count; i++) {
                if (i == aLines.Count - 1)
                {
                    result[i] = "'" + aLines[i].Replace("'", "''") + "'";// "' + #13#10" + ";";
                }
                else
                {
                    result[i] = "'" + aLines[i].Replace("'", "''") + "' + #13#10 +";
                }
            }
            return result;
        }
		/// <summary>
		/// Is commented.
		/// </summary>
		/// <param name="aLine">Line.</param>
		/// <returns></returns>
        public override bool IsCommented(string aLine) {
            return aLine.StartsWith("//", StringComparison.Ordinal);
        }
		/// <summary>
		/// Is commented.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override bool IsCommented(IList<string> aLines)
        {
			return (aLines[0].StartsWith("(*", StringComparison.Ordinal) && aLines[aLines.Count - 1].EndsWith("*)", StringComparison.Ordinal));
        }
		/// <summary>
		/// Uncomments a line.
		/// </summary>
		/// <param name="aLine">Line</param>
		/// <returns></returns>
        public override string UncommentLine(string aLine) {
            return aLine.Substring(2);
        }
		/// <summary>
		/// Uncomments lines.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> UncommentLines(IList<string> aLines)
        {
            aLines[0] = aLines[0].Substring(2);
            aLines[aLines.Count - 1] = aLines[aLines.Count - 1].Substring(0, aLines[aLines.Count - 1].Length -2);
            return aLines;
        }

    }
	
	/// <summary>
	/// XML code helper.
	/// </summary>
	internal class XmlLanguageCodeHelper : BaseLanguageCodeHelper {

        internal XmlLanguageCodeHelper() {}
		/// <summary>
		/// Gets IFormattable file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
		protected override IFormattableFile GetFormattableFile(string fileName) {
			return new XmlFile(fileName, new object[] {fileName});
		}
		/// <summary>
		/// Gets indentation.
		/// </summary>
		/// <returns></returns>		
		public override int GetIndentation() {
			return OtaUtils.GetXmlIndentationFromOptions();
		}
		/// <summary>
		/// Comments a line.
		/// </summary>
		/// <param name="aLine">Line</param>
		/// <returns></returns>
		public override string CommentLine(string aLine) {
            return "<!--" + aLine + "-->";
        }
		/// <summary>
		/// Comments lines.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> CommentLines(IList<string> aLines)
        {
            //TODO: comments every line instead
			aLines[0] = "<!--" + aLines[0];
			aLines[aLines.Count - 1] = aLines[aLines.Count - 1] + "-->";

            return aLines;
        }
		/// <summary>
		/// Creates region footer.
		/// </summary>
		/// <param name="aRegionText">Region text</param>
		/// <returns></returns>
        public override string CreateRegionFooter(string aRegionText) {
            throw new CoreException("Unsupported CreateRegionFooter for Xml");
        }
		/// <summary>
		/// Creates region header.
		/// </summary>
		/// <param name="aRegionText">Region text</param>
		/// <returns></returns>
        public override string CreateRegionHeader(string aRegionText) {
            throw new CoreException("Unsupported CreateRegionHeader for Xml");
        }
		/// <summary>
		/// Creates string.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> CreateString(IList<string> aLines)
        {
            // Replace Double Quotes
            //TODO: validate this later.
            for(int i=0; i < aLines.Count; i++) {
                aLines[i] = aLines[i].Replace("<", "&lt");
                aLines[i] = aLines[i].Replace(">", "&gt");
            }

            return aLines;
        }
		/// <summary>
		/// Is commented.
		/// </summary>
		/// <param name="aLine">Line.</param>
		/// <returns></returns>
        public override bool IsCommented(string aLine) {
            return (aLine.StartsWith("<!--", StringComparison.Ordinal) && aLine.EndsWith("-->", StringComparison.Ordinal));
        }
		/// <summary>
		/// Is commented.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override bool IsCommented(IList<string> aLines)
        {
            return (aLines[0].StartsWith("<!--", StringComparison.Ordinal) && aLines[aLines.Count - 1].EndsWith("-->", StringComparison.Ordinal));
        }
		/// <summary>
		/// Uncomments a line.
		/// </summary>
		/// <param name="aLine">Line</param>
		/// <returns></returns>
        public override string UncommentLine(string aLine) {
            aLine = aLine.Substring(4);
            aLine = aLine.Remove(aLine.Length -3, 3);

            return aLine;
        }
		/// <summary>
		/// Uncomments lines.
		/// </summary>
		/// <param name="aLines">Lines</param>
		/// <returns></returns>
        public override IList<string> UncommentLines(IList<string> aLines)
        {
            aLines[0] = aLines[0].Substring(4);
			aLines[aLines.Count] = aLines[aLines.Count].Substring(0, aLines[aLines.Count].Length -3);
            return aLines;
        }
    }
	
	/// <summary>
	/// Language code helper factory.
	/// </summary>
	public sealed class LanguageCodeHelperFactory {

		/// <summary>
		/// Gets language code helper.
		/// </summary>
		/// <param name="aFileName">File name</param>
		/// <returns></returns>
		public static BaseLanguageCodeHelper GetLanguageCodeHelper(string aFileName) {
            if (OtaUtils.IsCOrCSFile(aFileName)) {
                return new CSLanguageCodeHelper();
            }

            if (OtaUtils.IsDelphiFile(aFileName)) {
                return new DelphiLanguageCodeHelper();
            }

            if (OtaUtils.IsXmlFile(aFileName)) {
				return new XmlLanguageCodeHelper();
            }

			if (OtaUtils.IsBasicFile(aFileName))
			{
                return new BasicLanguageCodeHelper();
			}
            return new NullLanguageCodeHelper();
		}

		private LanguageCodeHelperFactory() {}
    }
}
