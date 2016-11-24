@echo off

if "%1"=="" goto USAGE

copy "..\Internal Installer\bin\Release\sugarcube_installer-internal.msi" sugarcube_installer-internal-%1.msi
copy "..\\Installer Builder Installer\bin\Release\Installer Builder Installer.msi" "SCMInstallerBuilder-%1.msi"
copy "..\\Decrypter Installer\bin\Release\Decrypter Installer.msi" "SugarCubeOrderDecrypter-%1.msi"

copy "..\Client Installer\bin\Release\sugarcube_installer-client.msi" SugarCubeManager.msi
copy "..\Client Installer\bin\Release\sugarcube_installer-client.msi" SugarCubeManager-%1.msi
copy "..\Client Installer\bin\Release\BrandLogo.bmp" .
copy "..\Client Installer\bin\Release\OrderForm.pdf" .
copy "..\Client Installer\bin\Release\OrderSec.cfg" .

copy "..\InstallationAssets\vc_redist-2012.x86.exe" .

"..\Custom Installer Builder\7zip\7za.exe" a -t7z scmdeploy.7z SugarCubeManager.msi vc_redist-2012.x86.exe OrderSec.cfg OrderForm.pdf BrandLogo.bmp
copy /b "..\Custom Installer Builder\7zip\7zSD.sfx" + 7zipconfig.txt + scmdeploy.7z scmsetup-%1.exe
del SugarCubeManager.msi
del vc_redist-2012.x86.exe
del BrandLogo.bmp
del OrderForm.pdf
del OrderSec.cfg
del scmdeploy.7z
goto END

:USAGE
echo You must specify the version number
echo    build_deployment_modules.bat {version number}
echo e.g. to prepare release 1.7.2 the command line would be 
echo    build_deployment_modules.bat 1.7.2
echo .

:END
