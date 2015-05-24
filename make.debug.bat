set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v3.5
call %MSBuildDir%\msbuild All.proj /t:Debug
@IF %ERRORLEVEL% NEQ 0 PAUSE