#!/bin/bash

basepath=$(cd `dirname $0`; cd .. ; pwd)


mono $basepath/efjs/bin/Debug/EFJS.exe DAL $basepath/valuating/Resource/Scripts/dbserver $basepath/DALSERVER/bin/Debug/DALSERVER.dll Yes
