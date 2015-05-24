//指标评语操作类
DB.IndexRemarkServer = function() {
};
//保存指标评语 		参数(1) ALOS.Model.IndexRemarkType
DB.IndexRemarkServer.SaveRemark=function(type,success){
var array=new Array();
array[0]=type;

DB.CreateWebLog("IndexRemarkServer", "SaveRemark", array,success);
};
//根据指标ID获得指标评语 指标ID		参数(1) System.Int32
DB.IndexRemarkServer.RenderRemark=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("IndexRemarkServer", "RenderRemark", array,success);
};
