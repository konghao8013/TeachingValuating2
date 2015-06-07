copy ..\Model\bin\debug\model.dll ..\DALSERVER\bin\Debug\model.dll

copy ..\Model\bin\debug\model.dll ..\EFJS\bin\Debug\model.dll

start ../efjs/bin/Debug/EFJS.exe DAL ../valuating/Resource/Scripts/dbserver ../DALSERVER/bin/Debug/DALSERVER.dll Yes
pause