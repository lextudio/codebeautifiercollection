using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PSTaskDialog
{
  public partial class frmTaskDialog : Form
  {
    //--------------------------------------------------------------------------------
    #region PRIVATE members
    //--------------------------------------------------------------------------------
    eSysIcons m_mainIcon = eSysIcons.Question;
    eSysIcons m_footerIcon = eSysIcons.Warning;

    List<RadioButton> m_radioButtonCtrls = new List<RadioButton>();
    string m_radioButtons = "";
    int m_initialRadioButtonIndex = 0;

    List<Button> m_cmdButtons = new List<Button>();
    string m_commandButtons = "";
    int m_commandButtonClicked = -1;

    eTaskDialogButtons m_Buttons = eTaskDialogButtons.YesNoCancel;

    bool m_Expanded = false;
    bool m_isVista = false;
    #endregion

    //--------------------------------------------------------------------------------
    #region PROPERTIES
    //--------------------------------------------------------------------------------
    public eSysIcons MainIcon { get { return m_mainIcon; } set { m_mainIcon = value; } }
    public eSysIcons FooterIcon { get { return m_footerIcon; } set { m_footerIcon = value; } }

    public string Title { get { return this.Text; } set { this.Text = value; } }
    public string MainInstruction { get { return lbMainInstruction.Text; } set { lbMainInstruction.Text = value; } }
    public string Content { get { return lbContent.Text; } set { lbContent.Text = value; } }
    public string ExpandedInfo { get { return lbExpandedInfo.Text; } set { lbExpandedInfo.Text = value; } }
    public string Footer { get { return lbFooter.Text; } set { lbFooter.Text = value; } }

    public string RadioButtons { get { return m_radioButtons; } set { m_radioButtons = value; } }
    public int InitialRadioButtonIndex { get { return m_initialRadioButtonIndex; } set { m_initialRadioButtonIndex = value; } }
    public int RadioButtonIndex
    {
      get
      {
        foreach (RadioButton rb in m_radioButtonCtrls)
          if (rb.Checked)
            return (int)rb.Tag;
        return -1;
      }
    }

    public string CommandButtons { get { return m_commandButtons; } set { m_commandButtons = value; } }
    public int CommandButtonClickedIndex { get { return m_commandButtonClicked; } }

    public eTaskDialogButtons Buttons { get { return m_Buttons; } set { m_Buttons = value; } }

    public string VerificationText { get { return cbVerify.Text; } set { cbVerify.Text = value; } }
    public bool VerificationCheckBoxChecked { get { return cbVerify.Checked; } set { cbVerify.Checked = value; } }

    public bool Expanded { get { return m_Expanded; } set { m_Expanded = value; } }
    #endregion

    //--------------------------------------------------------------------------------
    #region CONSTRUCTOR
    //--------------------------------------------------------------------------------
    public frmTaskDialog()
    {
      InitializeComponent();

      m_isVista = VistaTaskDialog.IsAvailableOnThisOS;
      if (m_isVista)
      {
        // We're emulating on Vista, so tweak the font's a little...
        lbMainInstruction.Font = new Font(lbMainInstruction.Font, FontStyle.Regular);
      }
      else
      {
        // not on Vista
        if (cTaskDialog.UseToolWindowOnXP) // <- shall we use the smaller toolbar?
          this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      }

      MainInstruction = "Main Instruction";
      Content = "";
      ExpandedInfo = "";
      Footer = "";
      VerificationText = "";
    }
    #endregion 

    //--------------------------------------------------------------------------------
    #region BuildForm
    // This is the main routine that should be called before .ShowDialog()
    //--------------------------------------------------------------------------------
    bool m_formBuilt = false;
    public void BuildForm()
    {
      int form_height = 0;

      // Setup Main Instruction
      switch (m_mainIcon)
      {
        case eSysIcons.Information: imgMain.Image = SystemIcons.Information.ToBitmap(); break;
        case eSysIcons.Question: imgMain.Image = SystemIcons.Question.ToBitmap(); break;
        case eSysIcons.Warning: imgMain.Image = SystemIcons.Warning.ToBitmap(); break;
        case eSysIcons.Error: imgMain.Image = SystemIcons.Error.ToBitmap(); break;
      }

      AdjustLabelHeight(lbMainInstruction);
      pnlMainInstruction.Height = Math.Max(41, lbMainInstruction.Height + 16);
      form_height += pnlMainInstruction.Height;

      // Setup Content
      pnlContent.Visible = (Content != "");
      if (Content != "")
      {
        AdjustLabelHeight(lbContent);
        pnlContent.Height = lbContent.Height + 4;
        form_height += pnlContent.Height;
      }

      bool show_verify_checkbox = (cbVerify.Text != "");
      cbVerify.Visible = show_verify_checkbox;

      // Setup Expanded Info and Buttons panels
      if (ExpandedInfo == "")
      {
        pnlExpandedInfo.Visible = false;
        lbShowHideDetails.Visible = false;
        cbVerify.Top = 12;
        pnlButtons.Height = 40;
      }
      else
      {
        AdjustLabelHeight(lbExpandedInfo);
        pnlExpandedInfo.Height = lbExpandedInfo.Height + 4;
        pnlExpandedInfo.Visible = m_Expanded;
        lbShowHideDetails.Text = (m_Expanded ? "        Hide details" : "        Show details");
        lbShowHideDetails.ImageIndex = (m_Expanded ? 0 : 3);
        if (!show_verify_checkbox)
          pnlButtons.Height = 40;
        if (m_Expanded)
          form_height += pnlExpandedInfo.Height;
      }

      // Setup RadioButtons
      pnlRadioButtons.Visible = (m_radioButtons != "");
      if (m_radioButtons != "")
      {
        string[] arr = m_radioButtons.Split(new char[] { '|' });
        int pnl_height = 12;
        for (int i = 0; i < arr.Length; i++)
        {
          RadioButton rb = new RadioButton();
          rb.Parent = pnlRadioButtons;
          rb.Location = new Point(60, 4 + (i * rb.Height));
          rb.Text = arr[i];
          rb.Tag = i;
          rb.Checked = (m_initialRadioButtonIndex == i);
          rb.Width = this.Width - rb.Left - 15;
          pnl_height += rb.Height;
          m_radioButtonCtrls.Add(rb);
        }
        pnlRadioButtons.Height = pnl_height;
        form_height += pnlRadioButtons.Height;
      }

      // Setup CommandButtons
      pnlCommandButtons.Visible = (m_commandButtons != "");
      if (m_commandButtons != "")
      {
        string[] arr = m_commandButtons.Split(new char[] { '|' });
        int t = 8;
        int pnl_height = 16;
        for (int i = 0; i < arr.Length; i++)
        {
          CommandButton btn = new CommandButton();
          btn.Parent = pnlCommandButtons;
          btn.Location = new Point(50, t);
          if (m_isVista)  // <- tweak font if vista
            btn.Font = new Font(btn.Font, FontStyle.Regular);
          btn.Text = arr[i];
          btn.Size = new Size(this.Width - btn.Left - 15, btn.GetBestHeight());
          t += btn.Height;
          pnl_height += btn.Height;
          btn.Tag = i;
          btn.Click += new EventHandler(CommandButton_Click);
        }
        pnlCommandButtons.Height = pnl_height;
        form_height += pnlCommandButtons.Height;
      }

      // Setup Buttons
      switch (m_Buttons)
      {
        case eTaskDialogButtons.YesNo:
          bt1.Visible = false;
          bt2.Text = "&Yes";
          bt2.DialogResult = DialogResult.Yes;
          bt3.Text = "&No";
          bt3.DialogResult = DialogResult.No;
          this.AcceptButton = bt2;
          this.CancelButton = bt3;
          break;
        case eTaskDialogButtons.YesNoCancel:
          bt1.Text = "&Yes";
          bt1.DialogResult = DialogResult.Yes;
          bt2.Text = "&No";
          bt2.DialogResult = DialogResult.No;
          bt3.Text = "&Cancel";
          bt3.DialogResult = DialogResult.Cancel;
          this.AcceptButton = bt1;
          this.CancelButton = bt3;
          break;
        case eTaskDialogButtons.OKCancel:
          bt1.Visible = false;
          bt2.Text = "&OK";
          bt2.DialogResult = DialogResult.OK;
          bt3.Text = "&Cancel";
          bt3.DialogResult = DialogResult.Cancel;
          this.AcceptButton = bt2;
          this.CancelButton = bt3;
          break;
        case eTaskDialogButtons.OK:
          bt1.Visible = false;
          bt2.Visible = false;
          bt3.Text = "&OK";
          bt3.DialogResult = DialogResult.OK;
          this.AcceptButton = bt3;
          this.CancelButton = bt3;
          break;
        case eTaskDialogButtons.Close:
          bt1.Visible = false;
          bt2.Visible = false;
          bt3.Text = "&Close";
          bt3.DialogResult = DialogResult.Cancel;
          this.CancelButton = bt3;
          break;
        case eTaskDialogButtons.Cancel:
          bt1.Visible = false;
          bt2.Visible = false;
          bt3.Text = "&Cancel";
          bt3.DialogResult = DialogResult.Cancel;
          this.CancelButton = bt3;
          break;
        case eTaskDialogButtons.None:
          bt1.Visible = false;
          bt2.Visible = false;
          bt3.Visible = false;
          break;
      }

      this.ControlBox = (Buttons == eTaskDialogButtons.Cancel ||
                         Buttons == eTaskDialogButtons.Close ||
                         Buttons == eTaskDialogButtons.OKCancel ||
                         Buttons == eTaskDialogButtons.YesNoCancel);

      if (!show_verify_checkbox && ExpandedInfo == "" && m_Buttons == eTaskDialogButtons.None)
        pnlButtons.Visible = false;
      else
        form_height += pnlButtons.Height;

      pnlFooter.Visible = (Footer != "");
      if (Footer != "")
      {
        AdjustLabelHeight(lbFooter);
        pnlFooter.Height = Math.Max(28, lbFooter.Height + 16);
        switch (m_footerIcon)
        {
          case eSysIcons.Information: imgFooter.Image = SystemIcons.Information.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero); break;
          case eSysIcons.Question: imgFooter.Image = SystemIcons.Question.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero); break;
          case eSysIcons.Warning: imgFooter.Image = SystemIcons.Warning.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero); break;
          case eSysIcons.Error: imgFooter.Image = SystemIcons.Error.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero); break;
        }
        form_height += pnlFooter.Height;
      }

      this.ClientSize = new Size(ClientSize.Width, form_height);

      m_formBuilt = true;
    }

    //--------------------------------------------------------------------------------
    // utility function for setting a Label's height
    void AdjustLabelHeight(Label lb)
    {
      string text = lb.Text;
      Font textFont = lb.Font;
      SizeF layoutSize = new SizeF(lb.ClientSize.Width, 5000.0F);
      Graphics g = Graphics.FromHwnd(lb.Handle);
      SizeF stringSize = g.MeasureString(text, textFont, layoutSize);
      lb.Height = (int)stringSize.Height + 4;
      g.Dispose();
    }
    #endregion

    //--------------------------------------------------------------------------------
    #region EVENTS
    //--------------------------------------------------------------------------------
    void CommandButton_Click(object sender, EventArgs e)
    {
     	m_commandButtonClicked = (int)((CommandButton)sender).Tag;
      this.DialogResult = DialogResult.OK;
    }

    //--------------------------------------------------------------------------------
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
    }

    //--------------------------------------------------------------------------------
    protected override void OnShown(EventArgs e)
    {
      if (!m_formBuilt)
        throw new Exception("frmTaskDialog : Please call .BuildForm() before showing the TaskDialog");
      base.OnShown(e);
    }

    //--------------------------------------------------------------------------------
    private void lbDetails_MouseEnter(object sender, EventArgs e)
    {
      lbShowHideDetails.ImageIndex = (m_Expanded ? 1 : 4);
    }

    //--------------------------------------------------------------------------------
    private void lbDetails_MouseLeave(object sender, EventArgs e)
    {
      lbShowHideDetails.ImageIndex = (m_Expanded ? 0 : 3);
    }

    //--------------------------------------------------------------------------------
    private void lbDetails_MouseUp(object sender, MouseEventArgs e)
    {
      lbShowHideDetails.ImageIndex = (m_Expanded ? 1 : 4);
    }

    //--------------------------------------------------------------------------------
    private void lbDetails_MouseDown(object sender, MouseEventArgs e)
    {
      lbShowHideDetails.ImageIndex =(m_Expanded ? 2 : 5);
    }

    //--------------------------------------------------------------------------------
    private void lbDetails_Click(object sender, EventArgs e)
    {
      m_Expanded = !m_Expanded;
      pnlExpandedInfo.Visible = m_Expanded;
      lbShowHideDetails.Text = (m_Expanded ? "        Hide details" : "        Show details");
      if (m_Expanded)
        this.Height += pnlExpandedInfo.Height;
      else
        this.Height -= pnlExpandedInfo.Height;
    }

    #endregion
    
    //--------------------------------------------------------------------------------
  }
}
