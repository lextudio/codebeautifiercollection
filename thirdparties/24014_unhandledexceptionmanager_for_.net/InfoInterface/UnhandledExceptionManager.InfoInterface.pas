unit UnhandledExceptionManager.InfoInterface;

interface

uses
  System.Collections.Specialized,
  System.Threading;

type
  TExceptionKind = (UnknownException,UnhandledException,ThreadException);

  IExceptionInfoManager = interface
    procedure set_ApartmentState(const Value: System.Threading.ApartmentState);
    procedure set_Kind(const Value: TExceptionKind);
    procedure set_UserContinue(const Value: Boolean);
    procedure set_CanContinue(const Value: Boolean);
    procedure set_ProductInfo(const Value: string);
    procedure set_ThreadInfo(const Value: string);
    procedure set_ExceptionObject(const Value: TObject);
    procedure set_ExceptionText(const Value: string);
    procedure set_ExceptionFullText(const Value: string);
    procedure set_LoadedAssemblyList(Value: StringCollection);
    procedure set_FailedInfoManagerNames(const Value: string);
    property ApartmentState: ApartmentState write set_ApartmentState;
    property Kind: TExceptionKind write set_Kind;
    property UserContinue: Boolean write set_UserContinue;
    property CanContinue: Boolean write set_CanContinue;
    property ProductInfo: string write set_ProductInfo;
    property ThreadInfo: string write set_ThreadInfo;
    property ExceptionObject: TObject write set_ExceptionObject;
    property ExceptionText: string write set_ExceptionText;
    property ExceptionFullText: string write set_ExceptionFullText;
    property LoadedAssemblyList: StringCollection write set_LoadedAssemblyList;
    property FailedInfoManagerNames: string write set_FailedInfoManagerNames;
    function Execute: Boolean;
  end;

implementation

end.
