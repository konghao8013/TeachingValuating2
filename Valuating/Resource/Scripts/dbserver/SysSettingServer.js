//系统参数设置V00008
DB.SysSettingServer = function() {
};
//获得第一条数据 		
DB.SysSettingServer.ReaderSettingType=function(success){
var array=new Array();

DB.CreateWebLog("SysSettingServer", "ReaderSettingType", array,success);
};
//保存数据 type对象		参数(1) ALOS.Model.SysSettingType
DB.SysSettingServer.Save=function(model,success){
var array=new Array();
array[0]=model;

DB.CreateWebLog("SysSettingServer", "Save", array,success);
};
