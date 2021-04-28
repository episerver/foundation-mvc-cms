@echo off
cd /d %~dp0
mode con:cols=120 lines=2500
set ROOTPATH=%cd%
set ROOTDIR=%cd%
set SOURCEPATH=%ROOTPATH%\src

echo ## Building Foundation
echo ## Gettting MSBuildPath ##
for /f "usebackq tokens=*" %%i in (`.\build\vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do (
  set InstallDir=%%i
)

for %%v in (15.0, 14.0) do (
  if exist "%InstallDir%\MSBuild\%%v\Bin\MSBuild.exe" (
    set msBuildPath=\MSBuild\%%v\Bin\MSBuild.exe
	goto :finish
  )
  set  msBuildPath=\MSBuild\Current\Bin\MSBuild.exe
)

:finish

echo msbuild.exe path: %InstallDir%%msBuildPath%
REM Set Release or Debug configuration.
IF "%1"=="Release" (set CONFIGURATION=Release) ELSE (set CONFIGURATION=Debug)
ECHO Building in %CONFIGURATION%

echo ## Clean and build ##
"%InstallDir%%msBuildPath%" "%ROOTPATH%\Foundation.sln" /p:Configuration=Release /t:Clean,Build,Publish 
IF %errorlevel% NEQ 0 EXIT /B %errorlevel%


REM Build Client
cd %SOURCEPATH%\Foundation
IF "%CONFIGURATION%"=="Release" ( CALL npm run prod ) ELSE ( CALL npm run dev )
cd %ROOTPATH%
IF %errorlevel% NEQ 0 EXIT /B %errorlevel%

pause