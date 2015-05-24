unit UnhandledExceptionManager.Form;

interface

uses
  System.Collections,
  System.ComponentModel,
  System.Collections.Specialized,
  System.Drawing,
  System.Threading,
  System.Windows.Forms,
  UnhandledExceptionManager.InfoInterface;

type
  TExceptionDialog = class sealed (System.Windows.Forms.Form,IExceptionInfoManager)
  {$REGION 'Designer Managed Code'}
  strict private
    /// <summary>
    /// Required designer variable.
    /// </summary>
    Components: System.ComponentModel.Container;
    BottomPanel: System.Windows.Forms.Panel;
    TopPanel: System.Windows.Forms.Panel;
    CenterPanel: System.Windows.Forms.Panel;
    CopyBtn: System.Windows.Forms.Button;
    CloseBtn: System.Windows.Forms.Button;
    ContentPanel: System.Windows.Forms.Panel;
    lblDescription: System.Windows.Forms.Label;
    lblErrorText: System.Windows.Forms.Label;
    MoreBtn: System.Windows.Forms.Button;
    ContinueBtn: System.Windows.Forms.Button;
    eFullErrorText: System.Windows.Forms.RichTextBox;
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    procedure InitializeComponent;
    procedure CopyBtn_Click(sender: System.Object; e: System.EventArgs);
    procedure MoreBtn_Click(sender: System.Object; e: System.EventArgs);
  {$ENDREGION}
  strict protected
    procedure Dispose(Disposing: Boolean); override;
  strict private
    OriginalHeight, ShortHeight: Integer;
    FKind: TExceptionKind;
    FUserContinue,FCanContinue: Boolean;
    FExceptionObject: TObject;
    FProductInfo,FThreadInfo,FExceptionText,FExceptionFullText: string;
    FLoadedAssemblyList: StringCollection;
    FFailedInfoManagerNames: string;
  strict protected
    procedure set_ApartmentState(const Value: System.Threading.ApartmentState);
    property Kind: TExceptionKind write FKind;
    property UserContinue: Boolean write FUserContinue;
    property CanContinue: Boolean write FCanContinue;
    property ProductInfo: string write FProductInfo;
    property ThreadInfo: string write FThreadInfo;
    property ExceptionObject: TObject write FExceptionObject;
    property ExceptionText: string write FExceptionText;
    property ExceptionFullText: string write FExceptionFullText;
    property LoadedAssemblyList: StringCollection write FLoadedAssemblyList;
    property FailedInfoManagerNames: string write FFailedInfoManagerNames;
    function Execute: Boolean;
  public
    constructor Create;
  end;

  [assembly: RuntimeRequiredAttribute(TypeOf(TExceptionDialog))]

implementation

uses
  System.Text;

{$REGION 'Windows Form Designer generated code'}

procedure TExceptionDialog.InitializeComponent;
begin
  Self.BottomPanel := System.Windows.Forms.Panel.Create;
  Self.ContentPanel := System.Windows.Forms.Panel.Create;
  Self.CloseBtn := System.Windows.Forms.Button.Create;
  Self.ContinueBtn := System.Windows.Forms.Button.Create;
  Self.CopyBtn := System.Windows.Forms.Button.Create;
  Self.TopPanel := System.Windows.Forms.Panel.Create;
  Self.MoreBtn := System.Windows.Forms.Button.Create;
  Self.lblErrorText := System.Windows.Forms.Label.Create;
  Self.lblDescription := System.Windows.Forms.Label.Create;
  Self.CenterPanel := System.Windows.Forms.Panel.Create;
  Self.eFullErrorText := System.Windows.Forms.RichTextBox.Create;
  Self.BottomPanel.SuspendLayout;
  Self.TopPanel.SuspendLayout;
  Self.CenterPanel.SuspendLayout;
  Self.SuspendLayout;
  //
  // BottomPanel
  //
  Self.BottomPanel.Controls.Add(Self.ContentPanel);
  Self.BottomPanel.Controls.Add(Self.CloseBtn);
  Self.BottomPanel.Controls.Add(Self.ContinueBtn);
  Self.BottomPanel.Controls.Add(Self.CopyBtn);
  Self.BottomPanel.Dock := System.Windows.Forms.DockStyle.Bottom;
  Self.BottomPanel.Location := System.Drawing.Point.Create(0, 242);
  Self.BottomPanel.Name := 'BottomPanel';
  Self.BottomPanel.Size := System.Drawing.Size.Create(468, 46);
  Self.BottomPanel.TabIndex := 1;
  //
  // ContentPanel
  //
  Self.ContentPanel.Anchor := (System.Windows.Forms.AnchorStyles(((System.Windows.Forms.AnchorStyles.Bottom
    or System.Windows.Forms.AnchorStyles.Left) or System.Windows.Forms.AnchorStyles.Right)));
  Self.ContentPanel.BorderStyle := System.Windows.Forms.BorderStyle.Fixed3D;
  Self.ContentPanel.Location := System.Drawing.Point.Create(0, 0);
  Self.ContentPanel.Name := 'ContentPanel';
  Self.ContentPanel.Size := System.Drawing.Size.Create(469, 4);
  Self.ContentPanel.TabIndex := 3;
  //
  // CloseBtn
  //
  Self.CloseBtn.Anchor := (System.Windows.Forms.AnchorStyles((System.Windows.Forms.AnchorStyles.Bottom
    or System.Windows.Forms.AnchorStyles.Right)));
  Self.CloseBtn.DialogResult := System.Windows.Forms.DialogResult.Cancel;
  Self.CloseBtn.FlatStyle := System.Windows.Forms.FlatStyle.Popup;
  Self.CloseBtn.Location := System.Drawing.Point.Create(385, 14);
  Self.CloseBtn.Name := 'CloseBtn';
  Self.CloseBtn.TabIndex := 0;
  Self.CloseBtn.Text := 'Close';
  //
  // ContinueBtn
  //
  Self.ContinueBtn.Anchor := (System.Windows.Forms.AnchorStyles((System.Windows.Forms.AnchorStyles.Bottom
    or System.Windows.Forms.AnchorStyles.Right)));
  Self.ContinueBtn.DialogResult := System.Windows.Forms.DialogResult.OK;
  Self.ContinueBtn.FlatStyle := System.Windows.Forms.FlatStyle.Popup;
  Self.ContinueBtn.Location := System.Drawing.Point.Create(304, 14);
  Self.ContinueBtn.Name := 'ContinueBtn';
  Self.ContinueBtn.TabIndex := 1;
  Self.ContinueBtn.Text := 'Continue';
  //
  // CopyBtn
  //
  Self.CopyBtn.Anchor := (System.Windows.Forms.AnchorStyles((System.Windows.Forms.AnchorStyles.Bottom
    or System.Windows.Forms.AnchorStyles.Left)));
  Self.CopyBtn.FlatStyle := System.Windows.Forms.FlatStyle.Popup;
  Self.CopyBtn.Location := System.Drawing.Point.Create(8, 14);
  Self.CopyBtn.Name := 'CopyBtn';
  Self.CopyBtn.Size := System.Drawing.Size.Create(108, 23);
  Self.CopyBtn.TabIndex := 2;
  Self.CopyBtn.Text := '&Copy to Clipboard';
  Include(Self.CopyBtn.Click, Self.CopyBtn_Click);
  //
  // TopPanel
  //
  Self.TopPanel.Controls.Add(Self.MoreBtn);
  Self.TopPanel.Controls.Add(Self.lblErrorText);
  Self.TopPanel.Controls.Add(Self.lblDescription);
  Self.TopPanel.Dock := System.Windows.Forms.DockStyle.Top;
  Self.TopPanel.Location := System.Drawing.Point.Create(0, 0);
  Self.TopPanel.Name := 'TopPanel';
  Self.TopPanel.Size := System.Drawing.Size.Create(468, 108);
  Self.TopPanel.TabIndex := 2;
  //
  // MoreBtn
  //
  Self.MoreBtn.Anchor := (System.Windows.Forms.AnchorStyles((System.Windows.Forms.AnchorStyles.Top
    or System.Windows.Forms.AnchorStyles.Right)));
  Self.MoreBtn.FlatStyle := System.Windows.Forms.FlatStyle.Popup;
  Self.MoreBtn.Location := System.Drawing.Point.Create(385, 83);
  Self.MoreBtn.Name := 'MoreBtn';
  Self.MoreBtn.TabIndex := 0;
  Self.MoreBtn.Text := '&More >>';
  Include(Self.MoreBtn.Click, Self.MoreBtn_Click);
  //
  // lblErrorText
  //
  Self.lblErrorText.Anchor := (System.Windows.Forms.AnchorStyles(((System.Windows.Forms.AnchorStyles.Top
    or System.Windows.Forms.AnchorStyles.Left) or System.Windows.Forms.AnchorStyles.Right)));
  Self.lblErrorText.Location := System.Drawing.Point.Create(8, 77);
  Self.lblErrorText.Name := 'lblErrorText';
  Self.lblErrorText.Size := System.Drawing.Size.Create(364, 28);
  Self.lblErrorText.TabIndex := 1;
  //
  // lblDescription
  //
  Self.lblDescription.Anchor := (System.Windows.Forms.AnchorStyles(((System.Windows.Forms.AnchorStyles.Top
    or System.Windows.Forms.AnchorStyles.Left) or System.Windows.Forms.AnchorStyles.Right)));
  Self.lblDescription.Location := System.Drawing.Point.Create(8, 3);
  Self.lblDescription.Name := 'lblDescription';
  Self.lblDescription.Size := System.Drawing.Size.Create(452, 65);
  Self.lblDescription.TabIndex := 0;
  //
  // CenterPanel
  //
  Self.CenterPanel.Controls.Add(Self.eFullErrorText);
  Self.CenterPanel.Dock := System.Windows.Forms.DockStyle.Fill;
  Self.CenterPanel.Location := System.Drawing.Point.Create(0, 108);
  Self.CenterPanel.Name := 'CenterPanel';
  Self.CenterPanel.Size := System.Drawing.Size.Create(468, 134);
  Self.CenterPanel.TabIndex := 3;
  //
  // eFullErrorText
  //
  Self.eFullErrorText.Anchor := (System.Windows.Forms.AnchorStyles((((System.Windows.Forms.AnchorStyles.Top
    or System.Windows.Forms.AnchorStyles.Bottom) or System.Windows.Forms.AnchorStyles.Left)
    or System.Windows.Forms.AnchorStyles.Right)));
  Self.eFullErrorText.BackColor := System.Drawing.SystemColors.Control;
  Self.eFullErrorText.Location := System.Drawing.Point.Create(8, 8);
  Self.eFullErrorText.Name := 'eFullErrorText';
  Self.eFullErrorText.ReadOnly := True;
  Self.eFullErrorText.Size := System.Drawing.Size.Create(452, 116);
  Self.eFullErrorText.TabIndex := 0;
  Self.eFullErrorText.Text := '';
  Self.eFullErrorText.WordWrap := False;
  //
  // TExceptionDialog
  //
  Self.AutoScaleBaseSize := System.Drawing.Size.Create(5, 13);
  Self.CancelButton := Self.CloseBtn;
  Self.ClientSize := System.Drawing.Size.Create(468, 288);
  Self.Controls.Add(Self.CenterPanel);
  Self.Controls.Add(Self.TopPanel);
  Self.Controls.Add(Self.BottomPanel);
  Self.MaximizeBox := False;
  Self.MinimizeBox := False;
  Self.Name := 'TExceptionDialog';
  Self.ShowInTaskbar := False;
  Self.SizeGripStyle := System.Windows.Forms.SizeGripStyle.Hide;
  Self.StartPosition := System.Windows.Forms.FormStartPosition.CenterScreen;
  Self.TopMost := True;
  Self.BottomPanel.ResumeLayout(False);
  Self.TopPanel.ResumeLayout(False);
  Self.CenterPanel.ResumeLayout(False);
  Self.ResumeLayout(False);
end;
{$ENDREGION}

resourcestring
  SDescriptionHead = 'An error has occurred in the application that has not been handled automatically.';
  SDescriptionContinue = ' Choose the Continue button if you wish to continue using the application. Choose the Close button if you wish to terminate the application.';
  SDescriptionCloseOnly = ' Use the Close button to terminate the application.';
  SDescriptionCopy = ' Use the Copy to Clipboard button to place the error information in the clipboard for use in providing to the application support group.';
  SMore = '&More >>';
  SLess = '&Less <<';
  SUnhandledException = 'Unhandled';
  SThreadException = 'Thread';
  SFullErrorFormat = 'Application: {0}'#13#10'Thread Id / Name: {1}'#13#10'Kind: {2}'#13#10'Message: {3}'#13#10#13#10'Exception Text:'#13#10'{4}'#13#10;
  SLoadedAssembly = #13#10'Loaded Assemblies:'#13#10;
  SFailedInfoManagerNames = #13#10'Warning, unable to execute some Info Manager:'#13#10'  ';

procedure TExceptionDialog.Dispose(Disposing: Boolean);
begin
  if Disposing then begin
    if Components <> nil then
      Components.Dispose();
  end;
  inherited Dispose(Disposing);
end;

constructor TExceptionDialog.Create;
begin
  inherited Create;
  InitializeComponent;
  OriginalHeight := Self.Size.Height;
  ShortHeight := OriginalHeight - CenterPanel.Size.Height + 8;
  Self.Size := System.Drawing.Size.Create(Self.Size.Width, ShortHeight);
	eFullErrorText.BackColor := System.Drawing.Color.FromArgb(eFullErrorText.BackColor.A,
                                                            eFullErrorText.BackColor.R + 10,
                                                            eFullErrorText.BackColor.G + 10,
                                                            eFullErrorText.BackColor.B + 10);
end;

procedure TExceptionDialog.MoreBtn_Click(sender: System.Object; e: System.EventArgs);
begin
  if MoreBtn.Text = SLess then begin
    MoreBtn.Text := SMore;
    Self.Size := System.Drawing.Size.Create(Self.Size.Width,ShortHeight);
  end
  else begin
    MoreBtn.Text := SLess;
    Self.Size := System.Drawing.Size.Create(Self.Size.Width,OriginalHeight);
  end;
end;

procedure TExceptionDialog.CopyBtn_Click(sender: System.Object; e: System.EventArgs);
begin
  Clipboard.SetDataObject(eFullErrorText.Text,true);
end;

procedure TExceptionDialog.set_ApartmentState(const Value: System.Threading.ApartmentState);
begin
  CopyBtn.Enabled := (Value = ApartmentState.STA);
end;

function TExceptionDialog.Execute: Boolean;
var
  S: string;
begin
  ContinueBtn.Visible := FUserContinue;
  ContinueBtn.Enabled := FCanContinue;
  Text := FProductInfo;
  with StringBuilder.Create(SDescriptionHead) do begin
    if FCanContinue then
      Append(SDescriptionContinue)
    else
      Append(SDescriptionCloseOnly);
    if CopyBtn.Enabled then
      Append(SDescriptionCopy);
    lblDescription.Text := ToString;
    Length := 0;
    lblErrorText.Text := FExceptionText;
    S := '';
    if FKind = UnhandledException then
      S := SUnhandledException
    else if FKind = ThreadException then
      S := SThreadException;
    Append(System.String.Format(SFullErrorFormat,[FProductInfo,FThreadInfo,S,FExceptionText,FExceptionFullText]));
    Append(SLoadedAssembly);
    for S in FLoadedAssemblyList do
      Append(S);
    if FFailedInfoManagerNames <> '' then begin
      Append(SFailedInfoManagerNames);
      Append(FFailedInfoManagerNames);
    end;
    eFullErrorText.Text := ToString;
  end;
  Result := (ShowDialog = System.Windows.Forms.DialogResult.Ok);
end;


end.
