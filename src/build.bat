@ECHO OFF
IF /I "%1"=="Debug" GOTO BuildDebug
IF /I "%1"=="All" GOTO BuildAll
IF /I "%1"=="Minimal" GOTO BuildMinimal

:ER
ECHO.
ECHO Raspkate Command-Line Build Tool v1.0
ECHO.
ECHO Usage:
ECHO     build.bat Debug
ECHO         Builds the Raspkate with Debug configuration.
ECHO.
ECHO     build.bat Release
ECHO         Builds the Raspkate with Release configuration.
ECHO.
GOTO End

:BuildDebug
msbuild /p:Configuration="Debug";TargetFrameworkVersion="v4.5.2" Raspkate.sln
GOTO End

:BuildAll
msbuild /p:Configuration="All";TargetFrameworkVersion="v4.5.2" Raspkate.sln
GOTO End

:BuildMinimal
msbuild /p:Configuration="Minimal";TargetFrameworkVersion="v4.5.2" Raspkate.sln
GOTO End

:End
@ECHO ON
