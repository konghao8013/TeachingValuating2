#!/bin/bash

basepath=$(cd `dirname $0`; cd .. ; pwd)

mode=Debug

if [ -n "$1" ];then
	mode=$1
fi

echo Mode:$mode

cp $basepath/Model/bin/$mode/Model.dll $basepath/DALSERVER/bin/$mode/Model.dll

cp $basepath/Model/bin/$mode/Model.dll $basepath/EFJS/bin/$mode/Model.dll

mono $basepath/EFJS/bin/$mode/EFJS.exe DAL $basepath/Valuating/Resource/Scripts/dbserver $basepath/DALSERVER/bin/$mode/DALSERVER.dll Yes
