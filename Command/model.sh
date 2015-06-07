#!/bin/bash

basepath=$(cd `dirname $0`; cd .. ; pwd)

mode=Debug

if [ -n "$1" ];then
	mode=$1
fi

echo Mode:$mode

mono $basepath/EFJS/Bin/$mode/EFJS.exe Model $basepath/Valuating/Resource/Scripts/model $basepath/Model/Bin/$mode/Model.dll Yes
