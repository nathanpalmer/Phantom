@echo off
%windir%\microsoft.net\framework\v4.0.30128\msbuild Phantom.sln /v:minimal
src\phantom\bin\debug\phantom.exe %*