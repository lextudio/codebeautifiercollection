using System.IO;

namespace Lextm.BagPlug.LineCounter
{
	/// <summary>
	/// Wraps a project source code file and the line
	/// count info for that file. Also provides details
	/// about the file type and what icon should be shown
	/// for the file in the UI.
	/// </summary>
	public class LineCountInfo
	{
		public LineCountInfo(string fileName, int iconIndex)
		{
			m_fileName = fileName;
			m_fileType = Path.GetExtension(fileName);
			m_iconIndex = iconIndex;
			m_sumMode = "Generic";
	
			m_lineCountInfoDetails = new LineCountDetails();
		}
	
		public LineCountInfo(string fileName, int iconIndex, LineCountSummary projectSummary) : this(fileName, iconIndex)
		{
			m_projectSummary = projectSummary;
		}
	
		private LineCountDetails m_lineCountInfoDetails;
		private string m_fileName;
		private string m_fileType;
		private int m_iconIndex;
		private LineCountSummary m_projectSummary;
		private string m_sumMode;
	
		public string FileName
		{
			get { return m_fileName; }
		}
	
		public string FileType
		{
			get { return m_fileType; }
		}
	
		public int IconIndex
		{
			get { return m_iconIndex; }
		}
	
		public string SumMode
		{
			get { return m_sumMode; }
			set { m_sumMode = value; }
		}
	
		public LineCountDetails LineCountInfoDetails
		{
			get { return m_lineCountInfoDetails; }
		}
	
		public LineCountSummary ProjectSummary
		{
			get { return m_projectSummary; }
		}
	}
}
