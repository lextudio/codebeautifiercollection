
#define MyAppID "{571B30F3-ECF9-46b5-9572-9AD6ACA5E9C6}"
#define MyAppCopyright "Copyright (C) 2005-2008 Lex Y. Li and other contributors."
#define MyAppName "Code Beautifier Collection 7 for RAD Studio 2009"
#define MyAppVersion GetFileVersion("..\bin\release\Lextm.CodeBeautifierCollection.Framework.dll")
#pragma message "Detailed version info: " + MyAppVersion
#define OtaVersion "12.0.3170.16989"
#define IdeRegKey "SOFTWARE\CodeGear\BDS\6.0"

[Setup]
AppName={#MyAppName}
AppVerName={#MyAppName}
AppPublisher=Lex Y. Li (lextm)
AppPublisherURL=http://lextm.cn
AppSupportURL=http://lextm.cn
AppUpdatesURL=http://code.google.com/p/lextudio
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=true
InfoAfterFile=After.txt
OutputDir=.
;SetupIconFile=G:\Borland Studio Projects\CSharp\CBCBackup\images\appearance.ico
SolidCompression=true
AppCopyright={#MyAppCopyright}
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany=LeXtudio
VersionInfoDescription={#MyAppName} Setup
VersionInfoTextVersion=CandyCan
InternalCompressLevel=max
VersionInfoCopyright={#MyAppCopyright}
Compression=zip
PrivilegesRequired=admin
ShowLanguageDialog=yes
LicenseFile=License.txt
WindowVisible=false
AppVersion={#MyAppVersion}
AppID={{#MyAppID}
UninstallDisplayName={#MyAppName}

[Languages]
Name: english; MessagesFile: compiler:Default.isl
[Types]
Name: Full; Description: All pluses are installed; Languages: 
Name: Compact; Description: Basic pluses are installed
Name: Custom; Description: Custom; Flags: iscustom
[Components]
Name: LeXDK; Description: LeXDK and basic pluses; Types: Custom Full Compact; Languages: 
Name: Plus; Description: Extra pluses; Types: Custom Full; Languages: 
;Name: ExpReg; Description: Expert Registry experimental binaries; Types: Custom Full
[Files]
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

; dll used to check running notepad at install time
Source: ..\bin\release\psvince.dll; flags: dontcopy
;psvince is installed in {app} folder, so it will be
;loaded at uninstall time ;to check if notepad is running
Source: ..\bin\release\psvince.dll; DestDir: {app}

;Source: ..\bin\release\expertregistry.exe; DestDir: {app}; Flags: ignoreversion; Components: ExpReg
;Source: ..\bin\release\expertmaintainer.exe; DestDir: {app}; Flags: ignoreversion; Components: ExpReg
;Source: ..\bin\release\bds.exe; DestDir: {app}; Flags: ignoreversion; Components: ExpReg

Source: ..\bin\release\installforallusers.exe; DestDir: {app}; Flags: ignoreversion; Components: LeXDK
Source: ..\bin\release\installforcurrentuser.exe; DestDir: {app}; Flags: ignoreversion; Components: LeXDK
Source: ..\bin\release\GACutil.exe.config; DestDir: {app}; Flags: ignoreversion; Components: LeXDK
Source: ..\bin\release\GACutil.exe; DestDir: {app}; Flags: ignoreversion; Components: LeXDK
;Source: ..\bin\release\lextm.expertlibrary.minus.dll; DestDir: {app}; Flags: ignoreversion; Components: LeXDK
;Source: ..\bin\release\lextm.expertlibrary.minus.pdb; DestDir: {app}; Flags: ignoreversion; Components: LeXDK

Source: ..\bin\release\bundled\astyle.exe; DestDir: {app}\bundled; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\bundled\clearer.exe; DestDir: {app}\bundled; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\bundled\clearer.xml; DestDir: {app}\bundled; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\bundled\dilmerge.exe; DestDir: {app}\bundled; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\bundled\dilmerge.xml; DestDir: {app}\bundled; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\bundled\kerr.iconbrowser.exe; DestDir: {app}\bundled; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\bundled\processchecker.exe; DestDir: {app}\bundled; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\bundled\sandbar.dll; DestDir: {app}\bundled; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\bundled\jcf.exe; DestDir: {app}\bundled; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK

Source: ..\bin\release\doc\cnpack.txt; DestDir: {app}\doc; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\doc\fdl.txt; DestDir: {app}\doc; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\doc\gpl.txt; DestDir: {app}\doc; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\doc\lgpl.txt; DestDir: {app}\doc; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\doc\otahelpplus.txt; DestDir: {app}\doc; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\doc\lexdk.pdf; DestDir: {app}\doc; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\doc\license.pdf; DestDir: {app}\doc; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\doc\manual.pdf; DestDir: {app}\doc; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\doc\readme.pdf; DestDir: {app}\doc; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK

Source: ..\bin\release\preferences\codeformat.chm; DestDir: {app}\preferences; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\preferences\defaultsettings.cfg; DestDir: {app}\preferences; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\preferences\jcfstyle.exe; DestDir: {app}\preferences; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK

Source: ..\bin\release\data\en-us.dic; DestDir: {app}\data; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\data\msdnhelp.css; DestDir: {app}\data; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\data\ShowXmlDocumentation.xsl; DestDir: {app}\data; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\data\templates\*.*; DestDir: {app}\data\templates; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\data\images\*.*; DestDir: {app}\data\images; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK

Source: ..\bin\release\templates.exe; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\unhandledexceptionmanager.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\NDepend.Helpers.FileDirectoryPath.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\NDepend.Helpers.FileDirectoryPath.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.common.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.common.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.common.xml; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\bewise.sharpbuildertools.minus.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\bewise.sharpbuildertools.minus.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\bewise.sharpbuildertools.minus.xml; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.opentools.core.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.opentools.core.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.opentools.core.xml; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.codebeautifiercollection.framework.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.codebeautifiercollection.framework.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\thirdparty.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\thirdparty.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\vistaapi.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\vistaapi.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\pstaskdialog.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\pstaskdialog.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\log4net.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\log4net.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\MRG.Controls.UI.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\MRG.Controls.UI.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\sharpdevtools.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\sharpdevtools.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\ICSharpCode.SharpZipLib.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\tipoftheday.ini; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\plusmanager.exe; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\plusmanager.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\indate.exe; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\indate.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\expertmanager.exe; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\expertmanager.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.utilities.plus.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.utilities.plus.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.utilities.plus.plus2; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.codebeautifiers.plus.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.codebeautifiers.plus.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\lextm.codebeautifiers.plus.plus2; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: LeXDK
Source: ..\bin\release\mono.cecil.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\office2007renderer.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\office2007renderer.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\Crad.Windows.Forms.Actions.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\Crad.Windows.Forms.Actions.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\interop.adodb.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\interop.jro.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\zetalib.core.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\zetalib.windows.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\zetalib.core.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\zetalib.windows.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus

Source: ..\bin\release\addmany.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\addmany.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\visibles.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\xmlserializers.applicationsts.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\xmlserializers.options.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\xmlserializers.radstudio.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\coderslab.windows.controls.treeview.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\lextm.addmany.plus.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\lextm.addmany.plus.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\lextm.artcsb.plus.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\lextm.artcsb.plus.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\lextm.wiseeditor.plus.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\lextm.wiseeditor.plus.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\lextm.bagplug.plus.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\lextm.bagplug.plus.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\lextm.nfamily.plus.exe; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\lextm.nfamily.plus.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\lextm.csbuildergoodies.plus.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\lextm.csbuildergoodies.plus.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\*.plus2; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\netspell.spellchecker.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\netspell.spellchecker.pdb; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
Source: ..\bin\release\nunit.framework.dll; DestDir: {app}; Flags: recursesubdirs ignoreversion createallsubdirs; Components: Plus
[Icons]
Name: {group}\{cm:UninstallProgram,Code Beautifier Collection}; Filename: {uninstallexe}; Components: LeXDK
Name: {group}\Help\License; Filename: {app}\doc\license.pdf; Comment: License; Flags: createonlyiffileexists; Components: LeXDK
Name: {group}\Help\User Manual; Filename: {app}\doc\Manual.pdf; Comment: User Manual; Flags: createonlyiffileexists; Components: LeXDK
Name: {group}\Help\LeXDK Developer's Guide; Filename: {app}\doc\LeXDK.pdf; Comment: LeXDK Documentation; Flags: createonlyiffileexists; Components: LeXDK
Name: {group}\Help\Read Me; Filename: {app}\doc\readme.pdf; Comment: Read Me; Flags: createonlyiffileexists; Components: LeXDK
Name: {group}\Help\AStyle Website; Filename: http://www.sourceforge.net/projects/astyle/; Components: LeXDK
Name: {group}\Help\JCF Website; Filename: http://jedicodeformat.sourceforge.net/; Components: LeXDK
Name: {group}\Author's Blog; Filename: http://lextm.blogspot.com; Components: LeXDK
Name: {group}\Report A Bug; Filename: http://code.google.com/p/lextudio/issues/entry; Components: LeXDK
Name: {group}\Homepage; Filename: http://code.google.com/p/lextudio; Components: LeXDK
Name: {group}\Expert Manager; Filename: {app}\expertmanager.exe; Components: LeXDK; Comment: Expert Manager; Flags: createonlyiffileexists
Name: {group}\Plus Manager; Filename: {app}\plusmanager.exe; Components: LeXDK; Comment: Plus Manager; Flags: createonlyiffileexists
Name: {group}\Get Update...; Filename: {app}\indate.exe; Components: LeXDK; Comment: Plus Manager; Flags: createonlyiffileexists
Name: {group}\Install Extra Templates; Filename: {app}\templates.exe; WorkingDir: {app}; Flags: runminimized closeonexit; Components: LeXDK

[Run]
;Filename: {app}\InstalltoGac.bat; Flags: waituntilidle runhidden; WorkingDir: {app}
[Code]
// ======================================
// Testing CodeGear RAD Studio is running
// ======================================

// function IsModuleLoaded to call at install time
// added also setuponly flag
function IsModuleLoaded(modulename: String ):  Boolean;
external 'IsModuleLoaded@files:psvince.dll stdcall setuponly';

// function IsModuleLoadedU to call at uninstall time
// added also uninstallonly flag
function IsModuleLoadedU(modulename: String ):  Boolean;
external 'IsModuleLoaded@{app}\psvince.dll stdcall uninstallonly' ;

// =======================================
// Testing if under Windows safe mode
// =======================================
function GetSystemMetrics( define: Integer ): Integer; external
'GetSystemMetrics@user32.dll stdcall';

Const SM_CLEANBOOT = 67;

function IsSafeModeBoot(): Boolean;
begin
  // 0 = normal boot, 1 = safe mode, 2 = safe mode with networking
 Result := ( GetSystemMetrics( SM_CLEANBOOT ) <> 0 );
end;

// ======================================
// Testing version number string
// ======================================
function GetNumber(var temp: String): Integer;
var
  part: String;
  pos1: Integer;
begin
  if Length(temp) = 0 then
  begin
    Result := -1;
    Exit;
  end;
	pos1 := Pos('.', temp);
	if (pos1 = 0) then
	begin
	  Result := StrToInt(temp);
	temp := '';
	end
	else
	begin
	part := Copy(temp, 1, pos1 - 1);
	  temp := Copy(temp, pos1 + 1, Length(temp));
	  Result := StrToInt(part);
	end;
end;

function CompareInner(var temp1, temp2: String): Integer;
var
  num1, num2: Integer;
begin
	num1 := GetNumber(temp1);
  num2 := GetNumber(temp2);
  if (num1 = -1) or (num2 = -1) then
  begin
    Result := 0;
    Exit;
  end;
  if (num1 > num2) then
  begin
	Result := 1;
  end
  else if (num1 < num2) then
  begin
	Result := -1;
  end
  else
  begin
	Result := CompareInner(temp1, temp2);
  end;
end;

function CompareVersion(str1, str2: String): Integer;
var
  temp1, temp2: String;
begin
	temp1 := str1;
	temp2 := str2;
	Result := CompareInner(temp1, temp2);
end;

function RadStudioInstalled: Boolean;
var
  existed: Boolean;
  rootFolder: String;
  otaFile: String;
  version: String;
begin
  existed := RegKeyExists(HKEY_LOCAL_MACHINE,
	'{#IdeRegKey}');
  if not existed then
  begin
    // RAD Studio is not there.
    Result := False;
    Exit;
  end;

  RegQueryStringValue(HKEY_LOCAL_MACHINE,
			'{#IdeRegKey}',
			'RootDir', rootFolder);
			  //MsgBox('rootFolder is ' + rootFolder, mbError, MB_OK);
  otaFile := rootFolder + 'Bin\Borland.Studio.ToolsAPI.dll';
    //MsgBox('otaFile is ' + otaFile, mbError, MB_OK);
  GetVersionNumbersString(otaFile, version);
  //MsgBox('version is ' + version, mbError, MB_OK);
  Result := (version = '{#OtaVersion}');
end;

function RadStudioRunning(): Boolean;
begin
  Result := IsModuleLoaded( 'bds.exe' );
end;

function CbcInstalled(): Boolean;
begin
  Result := RegKeyExists(HKEY_LOCAL_MACHINE,
	'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{#MyAppID}_is1');
end;

function InitializeSetup(): Boolean;
var
  oldVersion: String;
  uninstaller: String;
  ErrorCode: Integer;
  compareResult: Integer;
begin
  if IsSafeModeBoot then
  begin
    MsgBox( 'Cannot install under Windows Safe Mode.', mbError, MB_OK);
    Result := False;
    Exit;
  end;

  if not RadStudioInstalled then
  begin
    if MsgBox( 'This CodeGear RAD Studio version is not supported by this version of CBC. Continue to install?',
             mbError, MB_YESNO ) = IDNO then
    begin
		Result := False;
		Exit;
    end;
  end;

  if RadStudioRunning then
  begin
    MsgBox( 'CodeGear RAD Studio is running, please close it and run setup again.',
             mbError, MB_OK );
    Result := False;
    Exit;
  end;

  if not CbcInstalled then
  begin
    Result := True;
    Exit;
  end;

  RegQueryStringValue(HKEY_LOCAL_MACHINE,
    'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{#MyAppID}_is1',
    'DisplayVersion', oldVersion);
  compareResult := CompareVersion(oldVersion, '{#MyAppVersion}');
  if (compareResult > 0) then
  begin
    MsgBox('Version ' + oldVersion + ' of Code Beautifier Collection is already installed. This installer will exit.',
	  mbInformation, MB_OK);
    Result := False;
    Exit;
  end
  else if (compareResult = 0) then
  begin
    if (MsgBox('Code Beautifier Collection ' + oldVersion + ' is already installed. Do you want to repair it now?',
	  mbConfirmation, MB_YESNO) = IDNO) then
	begin
	  Result := False;
	  Exit;
    end;
  end
  else
  begin
    if (MsgBox('Code Beautifier Collection ' + oldVersion + ' is already installed. Do you want to override it with {#MyAppVersion} now?',
	  mbConfirmation, MB_YESNO) = IDYES) then
	begin
	  Result := False;
	  Exit;
    end;
  end;
  // remove old version
  RegQueryStringValue(HKEY_LOCAL_MACHINE,
	'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{#MyAppID}_is1',
	'UninstallString', uninstaller);
  ShellExec('runas', uninstaller, '/SILENT', '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);
  if (ErrorCode <> 0) then
  begin
	MsgBox( 'Failed to uninstall Code Beautifier Collection version ' + oldVersion + '. Please restart Windows and run setup again.',
	 mbError, MB_OK );
	Result := False;
	Exit;
  end;

  Result := True;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  ErrorCode: Integer;
begin
  if (CurStep = ssPostInstall) then
  begin
    //run uninstallforallusers
    ShellExec('', ExpandConstant('{app}\installforallusers.exe'),
      '/i', '', SW_SHOW, ewWaitUntilTerminated, ErrorCode);
  end;
end;

function InitializeUninstall(): Boolean;
begin
  if IsSafeModeBoot then
  begin
    MsgBox( 'Cannot uninstall under Windows Safe Mode.', mbError, MB_OK);
    Result := False;
    Exit;
  end;

  // check if notepad is running
  if IsModuleLoadedU( 'bds.exe' ) then
  begin
    MsgBox( 'CodeGear RAD Studio is running, please close it and run again uninstall.',
             mbError, MB_OK );
    Result := false;
    Exit;
  end
  else Result := true;

  // Unload the DLL, otherwise the dll psvince is not deleted
  UnloadDLL(ExpandConstant('{app}\psvince.dll'));
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  ErrorCode: Integer;
begin
  if (CurUninstallStep = usAppMutexCheck) then
  begin
    //run uninstallforallusers
    ShellExec('', ExpandConstant('{app}\installforallusers.exe'),
      '/u', '', SW_SHOW, ewWaitUntilTerminated, ErrorCode);
  end;
end;
