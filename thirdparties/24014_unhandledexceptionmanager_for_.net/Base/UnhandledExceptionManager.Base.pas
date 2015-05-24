unit UnhandledExceptionManager.Base;

interface

uses
  System.Collections,
  System.Security.Permissions,
  System.Threading,
  System.Windows.Forms,
  UnhandledExceptionManager.InfoInterface;

type
  TExceptionManager = class sealed (TObject)
  strict private
    class var
      FDefaultInfoManagerType: &Type;
      FUserContinue: Boolean;
      FAppDomainUnhandledHooked, FApplicationThreadHooked: Boolean;
      //InfoManagerTypes: HashTable;
  strict private
    class constructor Create;
    class function Activate(ExceptionObject: TObject; Kind: TExceptionKind; const ProductInfo: string; CanContinue: Boolean = true): Boolean; static;
  strict protected
    constructor Create;
  public
    class property DefaultInfoManagerType: &Type read FDefaultInfoManagerType write FDefaultInfoManagerType;
    class property UserContinue: Boolean read FUserContinue write FUserContinue;
    class property AppDomainUnhandledHooked: Boolean read FAppDomainUnhandledHooked;
    class property ApplicationThreadHooked: Boolean read FApplicationThreadHooked;
    //class function AddInfoManagerType(InfoManagerType: &Type): Boolean; static;
    //class function RemoveInfoManagerType(InfoManagerType: &Type): Boolean; static;
    class procedure Initialize; static;
    class procedure UnhandledExceptionHandler(Sender: TObject; Args: UnhandledExceptionEventArgs); static;
    class procedure ThreadExceptionHandler(Sender: TObject; Args: ThreadExceptionEventArgs); static;
  end;

  [assembly: RuntimeRequiredAttribute(TypeOf(TExceptionManager))]

implementation

uses
  System.Collections.Specialized,
  System.Reflection,
  UnhandledExceptionManager.Form;

resourcestring
  SNonCLSException = '[non-CLS compliant exception]';
  SNonExceptionException = '[non-exception object ({0})]';
  SThreadInfoFormat = '{0} / {1}';
  SLoadedAssemblyFormat = '{0}'#13#10'  {1}'#13#10;
  SNoLocationInfo = '[location unavailable]';

class constructor TExceptionManager.Create;
begin
  FDefaultInfoManagerType := TypeOf(TExceptionDialog);
  //InfoManagerTypes := HashTable.Create;
  //InfoManagerTypes.Add(FDefaultInfoManagerType.FullName.ToUpper,FDefaultInfoManagerType);
end;

constructor TExceptionManager.Create;
begin
  inherited Create;
end;

class function TExceptionManager.Activate(ExceptionObject: TObject; Kind: TExceptionKind; const ProductInfo: string; CanContinue: Boolean): Boolean;
var
  ApartmentState: System.Threading.ApartmentState;
  ThreadInfo: string;
  LoadedAssemblyList: StringCollection;
  FailedInfoManagerNames: string;

  function InfoManagerActivate(InfoManager: IExceptionInfoManager): Boolean;
  begin
    InfoManager.ApartmentState := ApartmentState;
    InfoManager.Kind := Kind;
    InfoManager.ThreadInfo := ThreadInfo;
    InfoManager.UserContinue := FUserContinue;
    InfoManager.CanContinue := CanContinue and FUserContinue;
    InfoManager.ProductInfo := ProductInfo;
    InfoManager.ExceptionObject := ExceptionObject;
    if not Assigned(ExceptionObject) then begin
      InfoManager.ExceptionText := SNonCLSException;
      InfoManager.ExceptionFullText := '';
    end
    else if not (ExceptionObject is Exception) then begin
      InfoManager.ExceptionText := System.String.Format(SNonExceptionException,[ExceptionObject.GetType.ToString]);
      InfoManager.ExceptionFullText := ExceptionObject.ToString;
    end
    else begin
      InfoManager.ExceptionText := Exception(ExceptionObject).Message;
      InfoManager.ExceptionFullText := Exception(ExceptionObject).ToString;
    end;
    InfoManager.LoadedAssemblyList := LoadedAssemblyList;
    InfoManager.FailedInfoManagerNames := FailedInfoManagerNames;
    Result := InfoManager.Execute;
  end;

var
  //InfoManagerType: &Type;
  InfoManager: IExceptionInfoManager;
  CurrentThreadId: Integer;
  LoadedAssemblies: array of Assembly;
  LoadedAssembly: Assembly;
  FullName,Location: string;
  OneOk,ShowExceptionDialog: Boolean;
begin
  ApartmentState := Thread.CurrentThread.GetApartmentState;
  SecurityPermission.Create(SecurityPermissionFlag.UnmanagedCode).Assert;
  CurrentThreadId := Thread.CurrentThread.ManagedThreadId;
  ThreadInfo := System.String.Format(SThreadInfoFormat,[CurrentThreadId,Thread.CurrentThread.Name]);
  LoadedAssemblyList := StringCollection.Create;
  LoadedAssemblies := AppDomain.CurrentDomain.GetAssemblies;
  for LoadedAssembly in LoadedAssemblies do begin
    FullName := LoadedAssembly.FullName;
    FileIOPermission.RevertAssert;
    FileIOPermission.Create(PermissionState.Unrestricted).Assert;
    try
      Location := LoadedAssembly.Location;
    except
      Location := SNoLocationInfo;
    end;
    if not Assigned(Location) then
      Location := SNoLocationInfo
    else if Location.Trim = '' then
      Location := SNoLocationInfo;
    LoadedAssemblyList.Add(System.String.Format(SLoadedAssemblyFormat,[FullName,Location]));
  end;
  Result := CanContinue and FUserContinue;
  OneOk := false;
  FailedInfoManagerNames := '';
//  ShowExceptionDialog := false;
//  for InfoManagerType in InfoManagerTypes.Values do begin
//    if InfoManagerType = TypeOf(TExceptionDialog) then
      ShowExceptionDialog := true;
//    else try
//      InfoManager := Activator.CreateInstance(InfoManagerType) as IExceptionInfoManager;
//      try
//        Result := InfoManagerActivate(InfoManager) and Result;
//        OneOk := true;
//      finally
//        (InfoManager as IDisposable).Dispose;
//      end;
//    except
//      if FailedInfoManagerNames = '' then
//        FailedInfoManagerNames := InfoManagerType.Name
//      else
//        FailedInfoManagerNames := FailedInfoManagerNames + ',' + InfoManagerType.Name;
//    end;
//  end;
  if not OneOk or ShowExceptionDialog then try
    InfoManager := Activator.CreateInstance(FDefaultInfoManagerType) as IExceptionInfoManager;//TExceptionDialog.Create;
    try
      Result := InfoManagerActivate(InfoManager) and Result;
    finally
      (InfoManager as IDisposable).Dispose;
    end;
  except
  end;
end;

//class function TExceptionManager.AddInfoManagerType(InfoManagerType: &Type): Boolean;
//begin
//  Result := Assigned(InfoManagerType) and
//            Assigned(InfoManagerType.GetInterface(TypeOf(IDisposable).FullName)) and
//            Assigned(InfoManagerType.GetInterface(TypeOf(IExceptionInfoManager).FullName));
//  if Result then try
//    InfoManagerTypes.Item[InfoManagerType.FullName.ToUpper] := InfoManagerType;
//  except
//    Result := false;
//  end;
//end;

//class function TExceptionManager.RemoveInfoManagerType(InfoManagerType: &Type): Boolean;
//begin
//  Result := Assigned(InfoManagerType);
//  if Result then try
//    InfoManagerTypes.Remove(InfoManagerType.FullName.ToUpper);
//  except
//    Result := false;
//  end;
//end;

class procedure TExceptionManager.Initialize;
begin
  try
    Exclude(AppDomain.CurrentDomain.UnhandledException,TExceptionManager.UnhandledExceptionHandler);
  except
    ;
  end;
  try
    Include(AppDomain.CurrentDomain.UnhandledException,TExceptionManager.UnhandledExceptionHandler);
    FAppDomainUnhandledHooked := true;
  except
    ;
  end;
  try
  	Exclude(Application.ThreadException,TExceptionManager.ThreadExceptionHandler);
  except
    ;
  end;
  try
    SecurityPermission.Create(SecurityPermissionFlag.UnmanagedCode).Assert;
	  Include(Application.ThreadException,TExceptionManager.ThreadExceptionHandler);
    FApplicationThreadHooked := true;
  except
    ;
  end;
  FUserContinue := true;
end;

class procedure TExceptionManager.UnhandledExceptionHandler(Sender: TObject; Args: UnhandledExceptionEventArgs);
var
  ProductInfo: string;
begin
  try
    ProductInfo := Assembly.GetEntryAssembly.FullName;
  except
    ProductInfo := '';
  end;
  if not TExceptionManager.Activate(Args.ExceptionObject,UnhandledException,ProductInfo,not Args.IsTerminating) then begin
    SecurityPermission.Create(SecurityPermissionFlag.UnmanagedCode).Assert;
    Environment.Exit(-1);
  end;
end;

class procedure TExceptionManager.ThreadExceptionHandler(Sender: TObject; Args: ThreadExceptionEventArgs);
var
  ProductInfo: string;
begin
  try
    ProductInfo := Assembly.GetEntryAssembly.FullName;
  except
    ProductInfo := '';
  end;
  if not TExceptionManager.Activate(Args.Exception,ThreadException,ProductInfo,true) then begin
    SecurityPermission.Create(SecurityPermissionFlag.UnmanagedCode).Assert;
    Environment.Exit(-1);
  end;
end;

end.
