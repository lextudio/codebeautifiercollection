unit UnhandledExceptionManager.DelayedBinding;

interface

uses
  System.Threading,
  System.Windows.Forms,
  System.Reflection;

type
  TExceptionManager = class sealed (TObject)
  strict private
    const
      UnhandledExceptionManagerAssemblyFullName = 'UnhandledExceptionManager, Version=2.3.0.0, Culture=neutral, PublicKeyToken=86419a2ecafb49f4';
      UnhandledExceptioManagerTypeFullName = 'UnhandledExceptionManager.TExceptionManager';
    type
      TInitialize = procedure;
      TUnhandledExceptionHandler = procedure (Sender: TOBject; Args: UnhandledExceptionEventArgs);
      TThreadExceptionHandler = procedure (Sender: TObject; Args: ThreadExceptionEventArgs);
      Tget_Type = function: &Type;
      TAddRemoveType = function (InfoManagerType: &Type): Boolean;
      Tget_Boolean = function: Boolean;
      Tset_Boolean = procedure (const Value: Boolean);
    class var
      FExternalAssemblyRequired: Boolean;
      FBinded: Boolean;
      Fget_DefaultInfoManagerType: Tget_Type;
      Fget_UserContinue: Tget_Boolean;
      Fset_UserContinue: Tset_Boolean;
      FAppDomainUnhandledHooked: Boolean;
      FApplicationThreadHooked: Boolean;
      FAddInfoManagerType,FRemoveInfoManagerType: TAddRemoveType;
      FInitialize: TInitialize;
      FUnhandledExceptionHandler: TUnhandledExceptionHandler;
      FThreadExceptionHandler: TThreadExceptionHandler;
    class constructor Create;
    class procedure ShowBindingError(E: Exception); static;
    class procedure RevertToUnbinded; static;
    class function Activate(ExceptionObject: TObject; CanContinue: Boolean): Boolean; static;
    class procedure DefaultUnbindedUnhandledExceptionHandler(Sender: TOBject; Args: UnhandledExceptionEventArgs); static;
    class procedure DefaultUnbindedThreadExceptionHandler(Sender: TObject; Args: ThreadExceptionEventArgs); static;
  strict protected
    constructor Create;
  public
    class function get_DefaultInfoManagerType: &Type; static;
    class function get_UserContinue: Boolean; static;
    class procedure set_UserContinue(const Value: Boolean); static;
    class procedure set_UnbindedUnhandledExceptionHandler(const Value: TUnhandledExceptionHandler); static;
    class procedure set_UnbindedUnhandledThreadExceptionHandler(const Value: TThreadExceptionHandler); static;
  public
    class property DefaultInfoManagerType: &Type read get_DefaultInfoManagerType;
    class property UserContinue: Boolean read get_UserContinue write set_UserContinue;
    class property AppDomainUnhandledHooked: Boolean read FAppDomainUnhandledHooked;
    class property ApplicationThreadHooked: Boolean read FApplicationThreadHooked;
    class function AddInfoManagerType(InfoManagerType: &Type): Boolean; static;
    class function RemoveInfoManagerType(InfoManagerType: &Type): Boolean; static;
    class procedure Initialize; static;
    class procedure UnhandledExceptionHandler(Sender: TObject; Args: UnhandledExceptionEventArgs); static;
    class procedure ThreadExceptionHandler(Sender: TObject; Args: ThreadExceptionEventArgs); static;
  public
    class property ExternalAssemblyRequired: Boolean read FExternalAssemblyRequired write FExternalAssemblyRequired;
    class property Binded: Boolean read FBinded;
    class property UnbindedUnhandledExceptionHandler: TUnhandledExceptionHandler read FUnhandledExceptionHandler write set_UnbindedUnhandledExceptionHandler;
    class property UnbindedThreadExceptionHandler: TThreadExceptionHandler read FThreadExceptionHandler write set_UnbindedUnhandledThreadExceptionHandler;
  end;

implementation

resourcestring
  SNonCLSException = '[non-CLS compliant exception]';
  SNonExceptionException = '[non-exception object ({0})]';
  SCanContinueInfo = #13#10'Use the OK button to try to continue with application execution, use the Cancel button to try to close the application';
  SCloseInfo = #13#10'Use the OK button to try to close the application';
  SBindingExceptionInfo = 'Exception on binding to UnhandledExceptionHandler'#13#10#13#10'{0}';

class constructor TExceptionManager.Create;
begin
  FExternalAssemblyRequired := true;
  FUnhandledExceptionHandler := DefaultUnbindedUnhandledExceptionHandler;
  FThreadExceptionHandler := DefaultUnbindedThreadExceptionHandler;
end;

constructor TExceptionManager.Create;
begin
  inherited Create;
end;

class procedure TExceptionManager.ShowBindingError(E: Exception);
var
  ExceptionInfo,ProductInfo: string;
begin
  if Assigned(E) then try
    ExceptionInfo := E.ToString;
  except
    ExceptionInfo := E.Message;
  end
  else
    ExceptionInfo := SNonCLSException;
  if (ExceptionInfo[Length(ExceptionInfo)] <> #13) and (ExceptionInfo[Length(ExceptionInfo)] <> #10) then
    ExceptionInfo := ExceptionInfo + #13#10;
  ExceptionInfo := System.string.Format(SBindingExceptionInfo + SCloseInfo,[ExceptionInfo]);
  try
    ProductInfo := Assembly.GetEntryAssembly.FullName;
  except
    ProductInfo := '';
  end;
  MessageBox.Show(ExceptionInfo,ProductInfo);
  try
    Environment.Exit(-1);
  except
    ;
  end;
end;

class procedure TExceptionManager.RevertToUnbinded;
begin
  FBinded := false;
  FUnhandledExceptionHandler := DefaultUnbindedUnhandledExceptionHandler;
  FThreadExceptionHandler := DefaultUnbindedThreadExceptionHandler;
  try
    Include(AppDomain.CurrentDomain.UnhandledException,UnhandledExceptionHandler);
    FAppDomainUnhandledHooked := true;
  except
    FAppDomainUnhandledHooked := false;
  end;
  try
    Include(Application.ThreadException,ThreadExceptionHandler);
    FApplicationThreadHooked := true;
  except
    FApplicationThreadHooked := false;
  end;
end;

class function TExceptionManager.Activate(ExceptionObject: TObject; CanContinue: Boolean): Boolean;
var
  ExceptionInfo,ProductInfo: string;
begin
  if not Assigned(ExceptionObject) then
    ExceptionInfo := SNonCLSException
  else if not (ExceptionObject is Exception) then
    ExceptionInfo := System.String.Format(SNonExceptionException,[ExceptionObject.GetType.ToString])
  else
    ExceptionInfo := Exception(ExceptionObject).ToString;
  if (ExceptionInfo[Length(ExceptionInfo)] <> #13) and (ExceptionInfo[Length(ExceptionInfo)] <> #10) then
    ExceptionInfo := ExceptionInfo + #13#10;
  try
    ProductInfo := Assembly.GetEntryAssembly.FullName;
  except
    ProductInfo := '';
  end;
  Result := false;
  if CanContinue then
    Result := (MessageBox.Show(ExceptionInfo + SCanContinueInfo,ProductInfo,MessageBoxButtons.OKCancel) = DialogResult.OK)
  else
    MessageBox.Show(ExceptionInfo + SCloseInfo,ProductInfo);
end;

class procedure TExceptionManager.DefaultUnbindedUnhandledExceptionHandler(Sender: TOBject; Args: UnhandledExceptionEventArgs);
begin
  if not TExceptionManager.Activate(Args.ExceptionObject,not Args.IsTerminating) then try
    Environment.Exit(-1);
  except
    ;
  end;
end;

class procedure TExceptionManager.DefaultUnbindedThreadExceptionHandler(Sender: TObject; Args: ThreadExceptionEventArgs);
begin
  if not TExceptionManager.Activate(Args.Exception,true) then try
    Environment.Exit(-1);
  except
    ;
  end;
end;

class function TExceptionManager.get_DefaultInfoManagerType: &Type;
begin
  if FBinded then
    Result := Fget_DefaultInfoManagerType
  else
    Result := nil;
end;

class function TExceptionManager.get_UserContinue: Boolean;
begin
  if FBinded then
    Result := Fget_UserContinue
  else
    Result := true;
end;

class procedure TExceptionManager.set_UserContinue(const Value: Boolean);
begin
  if FBinded then
    Fset_UserContinue(Value);
end;

class function TExceptionManager.AddInfoManagerType(InfoManagerType: &Type): Boolean;
begin
  Result := FBinded;
  if Result then
    Result := FAddInfoManagerType(InfoManagerType);
end;

class function TExceptionManager.RemoveInfoManagerType(InfoManagerType: &Type): Boolean;
begin
  Result := FBinded;
  if Result then
    Result := FRemoveInfoManagerType(InfoManagerType);
end;

class procedure TExceptionManager.Initialize;
var
  ExceptioManagerType: &Type;
  get_AppDomainUnhandledHooked: Tget_Boolean;
  get_ApplicationThreadHooked: Tget_Boolean;
begin
  try
    ExceptioManagerType := Assembly.Load(UnhandledExceptionManagerAssemblyFullName).GetType(UnhandledExceptioManagerTypeFullName,true);
    Fget_DefaultInfoManagerType := Delegate.CreateDelegate(TypeOf(Tget_Type), ExceptioManagerType, 'get_DefaultInfoManagerType') as Tget_Type;
    Fget_UserContinue := Delegate.CreateDelegate(TypeOf(Tget_Boolean), ExceptioManagerType, 'get_UserContinue') as Tget_Boolean;
    Fset_UserContinue := Delegate.CreateDelegate(TypeOf(Tset_Boolean), ExceptioManagerType, 'set_UserContinue') as Tset_Boolean;
    get_AppDomainUnhandledHooked := Delegate.CreateDelegate(TypeOf(Tget_Boolean), ExceptioManagerType, 'get_AppDomainUnhandledHooked') as Tget_Boolean;
    get_ApplicationThreadHooked := Delegate.CreateDelegate(TypeOf(Tget_Boolean), ExceptioManagerType, 'get_ApplicationThreadHooked') as Tget_Boolean;
    FAddInfoManagerType := Delegate.CreateDelegate(TypeOf(TAddRemoveType), ExceptioManagerType, 'AddInfoManagerType') as TAddRemoveType;
    FRemoveInfoManagerType := Delegate.CreateDelegate(TypeOf(TAddRemoveType), ExceptioManagerType, 'RemoveInfoManagerType') as TAddRemoveType;
    FInitialize := Delegate.CreateDelegate(TypeOf(TInitialize), ExceptioManagerType, 'Initialize') as TInitialize;
    FUnhandledExceptionHandler := Delegate.CreateDelegate(TypeOf(TUnhandledExceptionHandler), ExceptioManagerType, 'UnhandledExceptionHandler') as TUnhandledExceptionHandler;
    FThreadExceptionHandler := Delegate.CreateDelegate(TypeOf(TThreadExceptionHandler), ExceptioManagerType, 'ThreadExceptionHandler') as TThreadExceptionHandler;
    FInitialize;
    FAppDomainUnhandledHooked := get_AppDomainUnhandledHooked;
    FApplicationThreadHooked := get_ApplicationThreadHooked;
    FBinded := true;
  except
    on E: Exception do begin
      if FExternalAssemblyRequired then
        ShowBindingError(E)
      else
        RevertToUnbinded;
    end
    else begin
      if FExternalAssemblyRequired then
        ShowBindingError(nil)
      else
        RevertToUnbinded;
    end;
  end;
end;

class procedure TExceptionManager.UnhandledExceptionHandler(Sender: TOBject; Args: UnhandledExceptionEventArgs);
begin
  FUnhandledExceptionHandler(Sender,Args);
end;

class procedure TExceptionManager.ThreadExceptionHandler(Sender: TObject; Args: ThreadExceptionEventArgs);
begin
  FThreadExceptionHandler(Sender,Args);
end;

class procedure TExceptionManager.set_UnbindedUnhandledExceptionHandler(const Value: TUnhandledExceptionHandler);
begin
  if not FBinded then
    FUnhandledExceptionHandler := Value;
end;

class procedure TExceptionManager.set_UnbindedUnhandledThreadExceptionHandler(const Value: TThreadExceptionHandler);
begin
  if not FBinded then
    FThreadExceptionHandler := Value;
end;

end.
