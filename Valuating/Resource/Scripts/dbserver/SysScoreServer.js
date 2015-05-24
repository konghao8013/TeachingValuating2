//系统分值设置V00007操作类
DB.SysScoreServer = function() {
};
//获得第一条数据 		
DB.SysScoreServer.ReaderScoreType=function(success){
var array=new Array();

DB.CreateWebLog("SysScoreServer", "ReaderScoreType", array,success);
};
//保存数据 type对象		参数(1) ALOS.Model.SysScoreType
DB.SysScoreServer.Save=function(model,success){
var array=new Array();
array[0]=model;

DB.CreateWebLog("SysScoreServer", "Save", array,success);
};
