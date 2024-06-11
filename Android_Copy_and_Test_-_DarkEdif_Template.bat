@echo off
SETLOCAL EnableDelayedExpansion
for /F "tokens=1,2 delims=#" %%a in ('"prompt #$H#$E# & echo on & for %%b in (1) do     rem"') do (
  set "DEL=%%a"
)
title Android DarkEdif Template tester ARM64
color 07
echo Press any key for ZIP -^> Fusion ...
pause > nul
:redo
echo Copying Data ZIP -^> Fusion ...
::XCOPY "X:\Fusion-related\Public Exts\MMF2Exts\MFX\Extensions" "D:\Program Files (x86)\Steam\steamapps\common\Clickteam Fusion 2.5\Extensions" /I /S /Y /C /H /R
XCOPY "F:\repos\MMF2Exts-master\DarkEdif\GamejoltGameAPI\MFX\Data" "D:\SteamLibrary\steamapps\common\Clickteam Fusion 2.5\Data" /I /S /Y /C /H /R
::call :colorEcho 0E "Now building with Fusion..."
call :colorEcho 0E "Ready to build with Fusion..."
pause > nul
::echo .
::App says to restart MFX copying by returning 1; otherwise, assume success
::"F:\repos\MMF2Exts-master\DarkEdif\WaitForFusionToFinishBuilding.exe" /APKPath="F:\repos\MMF2Exts-master\DarkEdif\GamejoltGameAPI\MFX\Examples\GamejoltGameAPI\GamejoltAPIExampleAndroid.apk"
::if %ERRORLEVEL% equ 1 (
::	goto redo
::)
call :colorEcho 0E "Deploying to device..."
echo .
echo Fusion -^> device (uninstalling)
rem adb shell pm list packages com.yunivers.ctfgamejoltapi > nul
rem IF %ERRORLEVEL NEQ 0
"C:\Microsoft\AndroidSDK\25\platform-tools\adb.exe" uninstall com.yunivers.ctfgamejoltapi > nul
echo Fusion -^> device (pushing)
rem "C:\Microsoft\AndroidSDK\25\platform-tools\adb.exe" push "C:\Users\Phi\Desktop\EXEs\template tester.apk" "/storage/sdcard1/apks/template tester.apk"
rem "C:\Microsoft\AndroidSDK\25\platform-tools\adb.exe" push "APK PATH X\Phi Object Tester.apk" "/storage/emulated/0/Phi Object Tester.apk" > nul
"C:\Microsoft\AndroidSDK\25\platform-tools\adb.exe" push "F:\repos\MMF2Exts-master\DarkEdif\GamejoltGameAPI\MFX\Examples\GamejoltGameAPI\GamejoltAPIExampleAndroid.apk" "/data/local/tmp/template-test.apk"
echo Fusion -^> device (installing)
rem adb install effectively does this
rem "C:\Microsoft\AndroidSDK\25\platform-tools\adb.exe" install -lrtdg "X:\Fusion-related\DarkEdif MultiTarget SDK\DarkEdif MultiTarget SDK\MFX\Phi Object Tester.apk"
rem option -g (e.g. "-t -f -g") grants all permissions, but older devices may not support
"C:\Microsoft\AndroidSDK\25\platform-tools\adb.exe" shell pm install -t -f -g "/data/local/tmp/template-test.apk"
call :colorEcho 0E "Ready for VS debugging."

echo .
pause
clear
goto redo
rem "C:\Microsoft\AndroidSDK\25\platform-tools\adb.exe" logcat -s MMFRuntime:V DEBUG:V libc:V DarkEdif_Template:V

:colorEcho
echo off
<nul set /p ".=%DEL%" > "%~2"
findstr /v /a:%1 /R "^$" "%~2" nul
del "%~2" > nul 2>&1i