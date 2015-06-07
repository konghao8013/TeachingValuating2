copy ..\Model\bin\debug\model.dll ..\DALSERVER\bin\model.dll

copy ..\Model\bin\debug\model.dll ..\efjs\bin\Debug\model.dll

start ../efjs/bin/Debug/EFJS.exe DAL ../valuating/Resource/Scripts/dbserver ../DALSERVER/bin/Debug/DALSERVER.dll Yes
pause