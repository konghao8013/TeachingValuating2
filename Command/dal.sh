#!/bin/bash

basepath=$(cd `dirname $0`; cd .. ; pwd)

mode=Debug

if [ -n "$1" ];then
	mode=$1
fi

echo Mode:$mode

cp $basepath/Model/Bin/$mode/Model.dll $basepath/DALSERVER/Bin/DALSERVER.dll

mono $basepath/EFJS/Bin/$mode/EFJS.exe DAL $basepath/Valuating/Resource/Scripts/dbserver $basepath/DALSERVER/Bin/$mode/DALSERVER.dll Yes
