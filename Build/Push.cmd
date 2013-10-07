@echo off

echo Push nuget package
pause

"%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" BLToolkit.AzureSql.proj /t:Push

pause