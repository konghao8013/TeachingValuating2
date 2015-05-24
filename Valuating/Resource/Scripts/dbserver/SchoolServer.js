// v00003#学校学院管理表
DB.SchoolServer = function() {
};
//获得所有的学院学校信息 		
DB.SchoolServer.SelectList=function(success){
var array=new Array();

DB.CreateWebLog("SchoolServer", "SelectList", array,success);
};
//保存学院学校信息 学校对象		参数(1) ALOS.Model.SchoolType
DB.SchoolServer.SaveSchool=function(type,success){
var array=new Array();
array[0]=type;

DB.CreateWebLog("SchoolServer", "SaveSchool", array,success);
};
//删除学校信息 学校ID		参数(1) System.Int32
DB.SchoolServer.DeleteSchool=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("SchoolServer", "DeleteSchool", array,success);
};
//根据OBJiD获得数据 objid		参数(1) System.Int32
DB.SchoolServer.SelectTypeSchool=function(objid,success){
var array=new Array();
array[0]=objid;

DB.CreateWebLog("SchoolServer", "SelectTypeSchool", array,success);
};
//根据ID获得对象 Id		参数(1) System.Int32
DB.SchoolServer.RenderSchool=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("SchoolServer", "RenderSchool", array,success);
};
