//用户表操作类V00001
DB.UserServer = function() {
};
//用户guid登录方式 guid字符串
DB.UserServer.UserGuidLogin=function(guid,success){
var array=new Array()
array[0]=guid;

DB.CreateWebLog("UserServer", "UserGuidLogin", array,success);
};
//测试方法是否调用成功 参数一,参数二
DB.UserServer.Test=function(a,b,success){
var array=new Array()
array[0]=a;
array[1]=b;

DB.CreateWebLog("UserServer", "Test", array,success);
};
