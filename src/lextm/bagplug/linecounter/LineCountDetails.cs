
namespace Lextm.BagPlug.LineCounter
{
	/// <summary>
	/// Encapsulates line count sum details.
	/// </summary>
	public class LineCountDetails
	{
		public LineCountDetails()
		{
			Reset();
		}
	
		private int m_totalLines;
		private int m_codeLines;
		private int m_commentLines;
		private int m_doccmtLines;
		private int m_normcmtLines;
		private int m_blankLines;
		private int m_netLines;
	
		public int TotalLines
		{
			get { return m_totalLines; }
		}
	
		public int CodeLines
		{
			get { return m_codeLines; }
		}
	
		public int CommentLines
		{
			get { return m_commentLines; }
		}
	
		public int DocCommentLines
		{
			get { return m_doccmtLines; }
		}
	
		public int NormalCommentLines
		{
			get { return m_normcmtLines; }
		}
	
		public int BlankLines
		{
			get { return m_blankLines; }
		}
	
		public int NetLines
		{
			get { return m_netLines; }
		}
	
		public void Reset()
		{
			m_totalLines = 0;
			m_codeLines = 0;
			m_commentLines = 0;
			m_doccmtLines = 0;
			m_normcmtLines = 0;
			m_blankLines = 0;
			m_netLines = 0;
		}
	
		public void Summarize()
		{
			m_commentLines = m_doccmtLines + m_normcmtLines;
			m_netLines = m_totalLines - m_blankLines;
		}
	
		public void Add(string toWhat, int amount)
		{
			if (toWhat == null)
				toWhat = "totallines";
	
			toWhat = toWhat.ToUpperInvariant();
			switch (toWhat)
			{
					case "TOTALLINES": m_totalLines += amount; break;
					case "CODELINES": m_codeLines += amount; break;
					case "DOCCMTLINES": m_doccmtLines += amount; break;
					case "NORMCMTLINES": m_normcmtLines += amount; break;
					case "BLANKLINES": m_blankLines += amount; break;
			}
		}
	
		public void Add(LineCountDetails details)
		{
			m_totalLines += details.m_totalLines;
			m_codeLines += details.m_codeLines;
			m_doccmtLines += details.m_doccmtLines;
			m_normcmtLines += details.m_normcmtLines;
			m_blankLines += details.m_blankLines;
		}
	}
}
