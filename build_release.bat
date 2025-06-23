@echo off

rem update resources
python scripts/apply_versions.py

REM pushd scripts
REM python download_translations.py
popd ..

rem clean old builds
taskkill /im platypdfs.exe /f
REM taskkill /im unigetui.exe /f

rem Run tests
REM dotnet test src/UniGetUI.sln -v q --nologo

rem check exit code of the last command
REM if %errorlevel% neq 0 (
    REM echo "The tests failed!."
    REM pause
REM )

rem build executable
echo %signcommand%
dotnet clean PlatyPdfs.sln -v m -nologo
dotnet publish src/PlatyPdfs.App/PlatyPdfs.App.csproj /noLogo /property:Configuration=Release /property:Platform=x64 -v m
if %errorlevel% neq 0 (
    echo "DotNet publish has failed!"
    pause
)
rem sign code

rmdir /Q /S platypdfs_bin

mkdir platypdfs_bin
REM robocopy src\PlatyPdfs.App\bin\Release\net9.0-windows10.0.19041.0\win-x64\publish\ platypdfs_bin *.* /MOVE /E
robocopy src\PlatyPdfs.App\bin\x64\Release\net9.0-windows10.0.22621.0\win-x64 platypdfs_bin *.* /MOVE /E

set /p signfiles="Do you want to sign the files? [Y/n]: "
if /i "%signfiles%" neq "n" (
    %signcommand% "platypdfs_bin/PlatyPdfs.exe" "platypdfs_bin/PlatyPdfs.dll" "platypdfs_bin/PlatyPdfs.*.dll" "platypdfs_bin/ExternalLibraries.*.dll"
    if %errorlevel% neq 0 (
        echo "Signing has failed!"
        pause
    )
)

REM pushd unigetui_bin
REM copy UniGetUI.exe WingetUI.exe
REM popd

set INSTALLATOR="%SYSTEMDRIVE%\Scoop\apps\innosetup-np\current\ISCC.exe"
if exist %INSTALLATOR% (
    %INSTALLATOR% "PlatyPdfs.iss"
    %signcommand% "PlatyPdfs Installer.exe"
    REM del "PlatyPdfs Installer.exe"
    REM copy "UniG Installer.exe" "WingetUI Installer.exe" 
    pause
    echo Hash: 
    pwsh.exe -Command "(Get-FileHash '.\PlatyPdfs Installer.exe').Hash"
    echo .
    REM "PlatyPdfs Installer.exe"
) else (
    echo "Make installer was skipped, because the installer is missing."
)

pause
