@echo off
if not "%1"=="am_admin" (powershell start -verb runas '%0' am_admin & exit /b)

cd {0}
ConexaoA3 install
ConexaoA3 configure -u %USERNAME% -p {1}
net start ConexaoA3

pause