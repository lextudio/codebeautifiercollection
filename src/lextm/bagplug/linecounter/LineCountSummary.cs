using System.Collections.Generic;
using System.Windows.Forms;

namespace Lextm.BagPlug.LineCounter
{
	/// <summary>
	/// Wraps a project and the line count total detail
	/// for that project. Enumerates all of the files
	/// within that project.
	/// </summary>
	public class LineCountSummary
	{
		public LineCountSummary(string projName, int iconIndex)
		{
			m_fileLineCountInfo = new List<LineCountInfo>();
			m_lineCountSummaryDetails = new LineCountDetails();
			m_projectName = projName;
			m_iconIndex = iconIndex;
		}
	
		private IList<LineCountInfo> m_fileLineCountInfo;
		private LineCountDetails m_lineCountSummaryDetails;
		private string m_projectName;
		private int m_iconIndex;
	
		internal ListViewItem LinkedListViewItem;
	
		public string ProjectName
		{
			get { return m_projectName; }
		}
	
		public int IconIndex
		{
			get { return m_iconIndex; }
		}
	
		public LineCountDetails LineCountSummaryDetails
		{
			get { return m_lineCountSummaryDetails; }
		}
	
		public IList<LineCountInfo> FileLineCountInfo
		{
			get { return m_fileLineCountInfo; }
		}
	}
}
