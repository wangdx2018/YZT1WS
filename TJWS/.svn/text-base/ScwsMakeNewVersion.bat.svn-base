@echo off
echo    Starting Make TJ SCWS update package...
echo -----------------------------------------------------
echo 1. Create temp directory: temp
rd /S /Q temp 

md temp 
md temp\Config 
md temp\Dll
md temp\Resources
md temp\Image
md temp\RuleFiles
md temp\SelfLogFile
md temp\Style
md temp\ReportDir

echo 2. Copy files... (¹²?¸ö)

copy Config\BuinessControlCfg.xml temp\Config\BuinessControlCfg.xml
copy Config\FunctionCfg.xml temp\Config\FunctionCfg.xml
copy Config\layoutTemplate.xml temp\Config\layoutTemplate.xml
copy Config\logCppConfig.ini temp\Config\logCppConfig.ini
copy Config\ParamConfig.xml temp\Config\ParamConfig.xml
copy config\MessageConfig.xml temp\Config\MessageConfig.xml
copy config\MonitorImageCfg.xml temp\Config\MonitorImageCfg.xml




copy *.dll temp\
copy *.pdb temp\
copy *.bpl temp\

copy style\*.xaml  temp\style\


xcopy ReportDir\*.*  temp\ReportDir/e
xcopy RuleFiles\*.*  temp\RuleFiles/e
xcopy Image\*.*  temp\Image/e


copy SCWS.exe temp\
copy SCWS.exe.config temp\

echo 3. Create Self Extract File (RAR)...

cd temp 
attrib -R /S /D
winrar a -r  -idcdpq  -sfx SCWSEXE

echo 4. Prepare SFX Comments...

echo Silent=1 > sfx.txt
echo Overwrite=1 >> sfx.txt
rem echo Path=.\ >> sfx.txt

echo 5. Add SFX Comments...

winrar c -zsfx.txt SCWSEXE.exe 

copy SCWSEXE.exe ..\

echo 6. Update package completed!!

echo on 
pause 
