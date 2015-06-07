copy ..\Model\bin\debug\Model.dll ..\DALSERVER\bin\Debug\Model.dll

copy ..\Model\bin\debug\Model.dll ..\EFJS\bin\Debug\Model.dll

start ../efjs/bin/Debug/EFJS.exe DAL ../valuating/Resource/Scripts/dbserver ../DALSERVER/bin/Debug/DALSERVER.dll Yes
pause