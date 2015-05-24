//系统菜单数据
DB.SysMenuServer = function() {
};
//根据菜单类别获得菜单 EnumSysMenuType 1学生2教师3管理员		参数(1) System.Int32
DB.SysMenuServer.List=function(type,success){
var array=new Array();
array[0]=type;

DB.CreateWebLog("SysMenuServer", "List", array,success);
};
//获得所有的系统菜单 		
DB.SysMenuServer.Select=function(success){
var array=new Array();

DB.CreateWebLog("SysMenuServer", "Select", array,success);
};
//获得所有的系统菜单 		参数(1) System.Int32
DB.SysMenuServer.Reader=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("SysMenuServer", "Reader", array,success);
};
//获得OBJ为ID的菜单 ObjId		参数(1) System.Int32
DB.SysMenuServer.SelectObjIdList=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("SysMenuServer", "SelectObjIdList", array,success);
};
//保存菜单对象 菜单对象		参数(1) ALOS.Model.SysMenuType
DB.SysMenuServer.SaveModel=function(type,success){
var array=new Array();
array[0]=type;

DB.CreateWebLog("SysMenuServer", "SaveModel", array,success);
};
// 根据ID删除菜单 菜单ID		参数(1) System.Int32
DB.SysMenuServer.DeleteModel=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("SysMenuServer", "DeleteModel", array,success);
};
