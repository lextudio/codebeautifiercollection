set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v3.5
call %MSBuildDir%\msbuild package.proj
@IF %ERRORLEVEL% NEQ 0 PAUSE
Pause
