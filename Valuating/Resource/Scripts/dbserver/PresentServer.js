//指标操作类
DB.PresentServer = function() {
};
//根据ID获得对象 ID		参数(1) System.Int32
DB.PresentServer.SelectId=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("PresentServer", "SelectId", array,success);
};
//根据ID删除对象 Id		参数(1) System.Int32
DB.PresentServer.DeleteId=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("PresentServer", "DeleteId", array,success);
};
//保存对象 对象		参数(1) ALOS.Model.PresentType
DB.PresentServer.SaveType=function(type,success){
var array=new Array();
array[0]=type;

DB.CreateWebLog("PresentServer", "SaveType", array,success);
};
//获得所有指标 		参数(1) System.Int32
DB.PresentServer.PresentList=function(typeId,success){
var array=new Array();
array[0]=typeId;

DB.CreateWebLog("PresentServer", "PresentList", array,success);
};
//获得评语 		参数(1) System.Int32
DB.PresentServer.GetPY=function(did,success){
var array=new Array();
array[0]=did;

DB.CreateWebLog("PresentServer", "GetPY", array,success);
};
