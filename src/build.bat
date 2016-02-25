@ECHO OFF
IF /I "%1"=="Debug" GOTO BuildDebug
IF /I "%1"=="Release" GOTO BuildRelease

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

:BuildRelease
msbuild /p:Configuration="Release";TargetFrameworkVersion="v4.5.2" Raspkate.sln
GOTO End

:End
@ECHO ON
