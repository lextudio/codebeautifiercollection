@echo off

set RG="HKCU\Software\Borland\BDS\3.0\Known IDE Assemblies"
set JE=CBC_BDS3_Expt.dll

for %%x in (%0) do set CURRDIR=%%~dpsx
for %%x in (%CURRDIR%) do set CURRDIR=%%~dpsx
set PTH=%CURRDIR%BIN\RELEASE\%JE%

set VAL="CBC Expert 2.0"
reg add %RG% /v %PTH% /d %VAL%
