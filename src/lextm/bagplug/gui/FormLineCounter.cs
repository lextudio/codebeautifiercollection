using System;
using System.Globalization;
using System.Windows.Forms;
using Lextm.BagPlug.LineCounter;

namespace Lextm.BagPlug.Gui
{
	/// <summary>
	/// Summary description for WinForm.
	/// </summary>
	public class FormLineCounter : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem miLineCounter;
		private System.Windows.Forms.MenuItem miOptions;
		private System.Windows.Forms.MenuItem miRecalculate;
		private Vista_Api.ListView lvFileList;
		private System.Windows.Forms.ContextMenu cmFileList;
		private System.Windows.Forms.ColumnHeader chFileName;
		private System.Windows.Forms.ColumnHeader chFileTotalLines;
		private System.Windows.Forms.ColumnHeader chFileTotalCode;
		private System.Windows.Forms.ColumnHeader chFileTotalComment;
		private System.Windows.Forms.ColumnHeader chFileExt;
		private System.Windows.Forms.ColumnHeader chMode;
		private System.Windows.Forms.ImageList imgFileTypes;
		private System.Windows.Forms.ImageList imgProjectTyps;
		private System.Windows.Forms.ColumnHeader chCodeLines;
		private System.Windows.Forms.ColumnHeader chTotalLines;
		private System.Windows.Forms.ColumnHeader chProject;
		private System.Windows.Forms.ColumnHeader chBlankLines;
		private System.Windows.Forms.ColumnHeader chNetLines;
		private System.Windows.Forms.ColumnHeader chComments;
		private Vista_Api.ListView lvSummary;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ProgressBar tsprgTotal;
		private System.Windows.Forms.ProgressBar tsprgTask;
		private System.Windows.Forms.Label tstxtLinesCounted;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Splitter splitter1;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.miLineCounter = new System.Windows.Forms.MenuItem();
			this.miOptions = new System.Windows.Forms.MenuItem();
			this.miRecalculate = new System.Windows.Forms.MenuItem();
			this.lvFileList = new Vista_Api.ListView();
			this.chFileName = new System.Windows.Forms.ColumnHeader();
			this.chFileTotalLines = new System.Windows.Forms.ColumnHeader();
			this.chFileTotalCode = new System.Windows.Forms.ColumnHeader();
			this.chFileTotalComment = new System.Windows.Forms.ColumnHeader();
			this.chFileExt = new System.Windows.Forms.ColumnHeader();
			this.chMode = new System.Windows.Forms.ColumnHeader();
			this.cmFileList = new System.Windows.Forms.ContextMenu();
			this.imgFileTypes = new System.Windows.Forms.ImageList(this.components);
			this.imgProjectTyps = new System.Windows.Forms.ImageList(this.components);
			this.chCodeLines = new System.Windows.Forms.ColumnHeader();
			this.chTotalLines = new System.Windows.Forms.ColumnHeader();
			this.chProject = new System.Windows.Forms.ColumnHeader();
			this.chBlankLines = new System.Windows.Forms.ColumnHeader();
			this.chNetLines = new System.Windows.Forms.ColumnHeader();
			this.chComments = new System.Windows.Forms.ColumnHeader();
			this.lvSummary = new Vista_Api.ListView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tstxtLinesCounted = new System.Windows.Forms.Label();
			this.tsprgTask = new System.Windows.Forms.ProgressBar();
			this.tsprgTotal = new System.Windows.Forms.ProgressBar();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.miLineCounter});
			// 
			// miLineCounter
			// 
			this.miLineCounter.Index = 0;
			this.miLineCounter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.miOptions,
									this.miRecalculate});
			this.miLineCounter.Text = "Line Counter";
			// 
			// miOptions
			// 
			this.miOptions.Index = 0;
			this.miOptions.Text = "Options";
			// 
			// miRecalculate
			// 
			this.miRecalculate.Index = 1;
			this.miRecalculate.Text = "Recalculate";
			this.miRecalculate.Click += new System.EventHandler(this.miRecalculate_Click);
			// 
			// lvFileList
			// 
			this.lvFileList.AllowColumnReorder = true;
			this.lvFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.chFileName,
									this.chFileTotalLines,
									this.chFileTotalCode,
									this.chFileTotalComment,
									this.chFileExt,
									this.chMode});
			this.lvFileList.ContextMenu = this.cmFileList;
			this.lvFileList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvFileList.FullRowSelect = true;
			this.lvFileList.GridLines = true;
			this.lvFileList.HideSelection = false;
			this.lvFileList.Location = new System.Drawing.Point(0, 0);
			this.lvFileList.Name = "lvFileList";
			this.lvFileList.Size = new System.Drawing.Size(592, 242);
			this.lvFileList.SmallImageList = this.imgFileTypes;
			this.lvFileList.TabIndex = 0;
			this.lvFileList.UseCompatibleStateImageBehavior = false;
			this.lvFileList.View = System.Windows.Forms.View.Details;
			this.lvFileList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvFileList_ColumnClick);
			// 
			// chFileName
			// 
			this.chFileName.Text = "File";
			this.chFileName.Width = 192;
			// 
			// chFileTotalLines
			// 
			this.chFileTotalLines.Text = "Total Lines";
			this.chFileTotalLines.Width = 90;
			// 
			// chFileTotalCode
			// 
			this.chFileTotalCode.Text = "Code Lines";
			this.chFileTotalCode.Width = 90;
			// 
			// chFileTotalComment
			// 
			this.chFileTotalComment.Text = "Comments";
			this.chFileTotalComment.Width = 90;
			// 
			// chFileExt
			// 
			this.chFileExt.Text = "Extension";
			this.chFileExt.Width = 64;
			// 
			// chMode
			// 
			this.chMode.Text = "Sum-Mode";
			this.chMode.Width = 70;
			// 
			// imgFileTypes
			// 
			this.imgFileTypes.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imgFileTypes.ImageSize = new System.Drawing.Size(16, 16);
			this.imgFileTypes.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// imgProjectTyps
			// 
			this.imgProjectTyps.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imgProjectTyps.ImageSize = new System.Drawing.Size(16, 16);
			this.imgProjectTyps.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// chCodeLines
			// 
			this.chCodeLines.Text = "Code Lines";
			this.chCodeLines.Width = 90;
			// 
			// chTotalLines
			// 
			this.chTotalLines.Text = "Total Lines";
			this.chTotalLines.Width = 90;
			// 
			// chProject
			// 
			this.chProject.Text = "Project";
			this.chProject.Width = 146;
			// 
			// chBlankLines
			// 
			this.chBlankLines.Text = "Blank Lines";
			this.chBlankLines.Width = 90;
			// 
			// chNetLines
			// 
			this.chNetLines.Text = "Net Lines";
			this.chNetLines.Width = 90;
			// 
			// chComments
			// 
			this.chComments.Text = "Comments";
			this.chComments.Width = 90;
			// 
			// lvSummary
			// 
			this.lvSummary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.chProject,
									this.chTotalLines,
									this.chCodeLines,
									this.chComments,
									this.chBlankLines,
									this.chNetLines});
			this.lvSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lvSummary.FullRowSelect = true;
			this.lvSummary.HideSelection = false;
			this.lvSummary.Location = new System.Drawing.Point(0, 242);
			this.lvSummary.Name = "lvSummary";
			this.lvSummary.Size = new System.Drawing.Size(592, 97);
			this.lvSummary.SmallImageList = this.imgProjectTyps;
			this.lvSummary.TabIndex = 2;
			this.lvSummary.UseCompatibleStateImageBehavior = false;
			this.lvSummary.View = System.Windows.Forms.View.Details;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tstxtLinesCounted);
			this.panel1.Controls.Add(this.tsprgTask);
			this.panel1.Controls.Add(this.tsprgTotal);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 339);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(592, 24);
			this.panel1.TabIndex = 3;
			// 
			// tstxtLinesCounted
			// 
			this.tstxtLinesCounted.Location = new System.Drawing.Point(504, 8);
			this.tstxtLinesCounted.Name = "tstxtLinesCounted";
			this.tstxtLinesCounted.Size = new System.Drawing.Size(100, 23);
			this.tstxtLinesCounted.TabIndex = 4;
			// 
			// tsprgTask
			// 
			this.tsprgTask.Location = new System.Drawing.Point(360, 0);
			this.tsprgTask.Name = "tsprgTask";
			this.tsprgTask.Size = new System.Drawing.Size(144, 23);
			this.tsprgTask.TabIndex = 3;
			// 
			// tsprgTotal
			// 
			this.tsprgTotal.Location = new System.Drawing.Point(112, 0);
			this.tsprgTotal.Name = "tsprgTotal";
			this.tsprgTotal.Size = new System.Drawing.Size(136, 23);
			this.tsprgTotal.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(256, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Task Progress";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Total Progress";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.splitter1);
			this.panel2.Controls.Add(this.lvFileList);
			this.panel2.Controls.Add(this.lvSummary);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(592, 339);
			this.panel2.TabIndex = 4;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 239);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(592, 3);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// FormLineCounter
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(592, 363);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Menu = this.mainMenu1;
			this.Name = "FormLineCounter";
			this.Text = "LineCounter";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		#region Nested Classes
		abstract class ListViewItemComparer : System.Collections.IComparer
		{
			public abstract int Compare(ListViewItem item1, ListViewItem item2);

			public Vista_Api.ListView SortingList;
			public int Column;

			#region IComparer Members
			int System.Collections.IComparer.Compare(object x, object y)
			{
                ListViewItem left = x as ListViewItem;
                ListViewItem right = y as ListViewItem;
				if (left == null || right == null)
				{					
					throw new ArgumentException("One or both of the arguments are not Vista_Api.ListViewItem objects.");
				}
                else
                {
                    int diff = Compare(left, right);
					if (SortingList.Sorting == SortOrder.Descending)
						diff *= -1;

					return diff;
				}
			}
			#endregion
		}

		/// <summary>
		/// Compares items based on file name.
		/// </summary>
		class FileNameComparer : ListViewItemComparer
		{
			public override int Compare(ListViewItem item1, ListViewItem item2)
			{
				return String.Compare(item1.Text, item2.Text, StringComparison.Ordinal);
			}
		}

		/// <summary>
		/// Compares items based on total lines primarily, and
		/// the filename secondarily.
		/// </summary>
		class FileLinesComparer : ListViewItemComparer
		{

			public override int Compare(ListViewItem item1, ListViewItem item2)
			{
				string string1 = item1.SubItems[Column].Text;
				string string2 = item2.SubItems[Column].Text;

				if (string1 != null && string2 != null)
				{
					int total1 = int.Parse(string1, CultureInfo.InvariantCulture);
					int total2 = int.Parse(string2, CultureInfo.InvariantCulture);

					// Compare the totals...
					int diff = total1 - total2;

					// If totals are equal...
					if (diff == 0)
					{
						// Compare the filenames
						diff = String.Compare(item1.Text, item2.Text, StringComparison.Ordinal);
					}

					return diff;
				}

				return 0;
			}
		}

		/// <summary>
		/// Compares items based on file extension.
		/// </summary>
		class FileExtensionComparer : ListViewItemComparer
		{

			public override int Compare(ListViewItem item1, ListViewItem item2)
			{
				string string1 = item1.SubItems[4].Text;
				string string2 = item2.SubItems[4].Text;

				return String.Compare(string1, string2, StringComparison.OrdinalIgnoreCase);
			}
		}
		#endregion
		#region Constructor
		/// <summary>
		/// Construct the line counter user interface and
		/// the countable file type mappings (to icons and
		/// counting algorithms).
		/// </summary>
		public FormLineCounter()
		{
			InitializeComponent();
			
			#if IMPR1
			projectImageListHelper = new ImageListHelper(imgProjectTypes);
			fileImageListHelper    = new ImageListHelper(imgFileTypes);
			#endif


		}
		#endregion

		#region Handlers
		private int lastSortColumn = -1;	// Track the last clicked column

		/// <summary>
		/// Sorts the Vista_Api.ListView by the clicked column, automatically
		/// reversing the sort order on subsequent clicks of the
		/// same column.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e">Provides the index of the clicked column.</param>
		private void lvFileList_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			// Define a variable of the abstract (generic) comparer
			ListViewItemComparer comparer = null;

			// Create an instance of the specific comparer in the 'comparer'
			// variable. Since each of the explicit comparer classes is
			// derived from the abstract case class, polymorphism applies.
			switch (e.Column)
			{
					// Line count columns
				case 1:
				case 2:
				case 3:
					comparer = new FileLinesComparer();
					break;
					// The file extension column
				case 4:
					comparer = new FileExtensionComparer();
					break;
					// All other columns sort by file name
				default:
					comparer = new FileNameComparer();
					break;
			}

			// Set the sorting order
			if (lastSortColumn == e.Column)
			{
				if (lvFileList.Sorting == SortOrder.Ascending)
				{
					lvFileList.Sorting = SortOrder.Descending;
				}
				else
				{
					lvFileList.Sorting = SortOrder.Ascending;
				}
			}
			else
			{
				lvFileList.Sorting = SortOrder.Ascending;
			}
			lastSortColumn = e.Column;

			// Send the comparer the list view and column being sorted
			comparer.SortingList = lvFileList;
			comparer.Column = e.Column;

			// Attach the comparer to the list view and sort
			lvFileList.ListViewItemSorter = comparer;
			lvFileList.Sort();
		}

		/// <summary>
		/// Forces a recount of all files in all projects, and
		/// updates the view.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void miRecalculate_Click(object sender, System.EventArgs e)
		{
			LineCounterHelper.ScanSolution(tsprgTotal, tsprgTask);
			LineCounterHelper.SumSolution(tsprgTotal, tsprgTask, lvFileList, lvSummary);
		}
		#endregion


	}

}
