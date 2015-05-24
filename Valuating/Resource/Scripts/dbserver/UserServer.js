//用户表操作类V00001
DB.UserServer = function() {
};
//用户guid登录方式 guid字符串		参数(1) System.String
DB.UserServer.UserGuidLogin=function(guid,success){
var array=new Array();
array[0]=guid;

DB.CreateWebLog("UserServer", "UserGuidLogin", array,success);
};
//查询用户详细信息 用户对象		参数(1) ALOS.Model.UserType
DB.UserServer.SelectUser=function(type,success){
var array=new Array();
array[0]=type;

DB.CreateWebLog("UserServer", "SelectUser", array,success);
};
//根据用户编号获得用户信息 id用户编号		参数(1) System.Int32
DB.UserServer.Reader=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("UserServer", "Reader", array,success);
};
//根据用户编号删除用户信息 用户编号		参数(1) System.Int32
DB.UserServer.DeleteUser=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("UserServer", "DeleteUser", array,success);
};
//保存用户信息 用户对象		参数(1) ALOS.Model.UserType
DB.UserServer.SaveUser=function(user,success){
var array=new Array();
array[0]=user;

DB.CreateWebLog("UserServer", "SaveUser", array,success);
};
//测试方法是否调用成功 参数一,参数二		参数(1) System.Int32参数(2) System.Int32
DB.UserServer.Test=function(a,b,success){
var array=new Array();
array[0]=a;
array[1]=b;

DB.CreateWebLog("UserServer", "Test", array,success);
};
//获得所有的用户资料 		
DB.UserServer.List=function(success){
var array=new Array();

DB.CreateWebLog("UserServer", "List", array,success);
};
//根据搜索获得所有的数据条数 搜索值		参数(1) System.String参数(2) System.Int32参数(3) System.Int32参数(4) System.Int32
DB.UserServer.Count=function(value,typeId,schoolId,collegeId,success){
var array=new Array();
array[0]=value;
array[1]=typeId;
array[2]=schoolId;
array[3]=collegeId;

DB.CreateWebLog("UserServer", "Count", array,success);
};
// 获取每页20条数据 搜索值,下标		参数(1) System.String参数(2) System.Int32参数(3) System.Int32参数(4) System.Int32参数(5) System.Int32
DB.UserServer.ListPageUser=function(value,index,typeId,schoolId,collegeId,success){
var array=new Array();
array[0]=value;
array[1]=index;
array[2]=typeId;
array[3]=schoolId;
array[4]=collegeId;

DB.CreateWebLog("UserServer", "ListPageUser", array,success);
};
