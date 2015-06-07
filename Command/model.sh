#!/bin/bash

basepath=$(cd `dirname $0`; cd .. ; pwd)

mono $basepath/efjs/bin/Debug/EFJS.exe Model $basepath/valuating/Resource/Scripts/model $basepath/Model/bin/Debug/Model.dll Yes
