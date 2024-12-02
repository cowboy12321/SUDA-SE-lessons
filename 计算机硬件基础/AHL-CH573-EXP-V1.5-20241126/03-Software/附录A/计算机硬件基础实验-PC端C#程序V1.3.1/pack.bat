@echo off
for %%a in ("%~dp0\\.") do set "parent=%%~nxa"
set name=%parent%
set d=%date:~,4%%date:~5,2%%date:~8,2%
set filename=%name%-%d%
set ppath=%Temp%\%filename%
if exist "%ppath%" rd "%ppath%" /s /q
mkdir "%ppath%"
xcopy .\* "%ppath%" /e /y
@REM 下面一行代表需要删除的文件夹，比如这里把目录下的vscode文件夹删除掉
if exist "%ppath%\.vscode" rd "%ppath%\.vscode" /s /q
@REM 下面一行代表需要删除的文件，比如这里把目录下的pack.bat文件删除掉
del "%ppath%\pack.bat" /q
echo D|xcopy "%ppath%"\* "%filename%" /e /y
cd %Temp%
tar -cf "%USERPROFILE%\desktop\%filename%.tar" %filename%
rd "%ppath%" /s /q
rd %filename% /s /q
