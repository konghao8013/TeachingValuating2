//指标类别操作类
DB.IndexSortServer = function() {
};
//获得所有指标类别 		
DB.IndexSortServer.SortList=function(success){
var array=new Array();

DB.CreateWebLog("IndexSortServer", "SortList", array,success);
};
//根据ID删除指标类别 ID编号		参数(1) System.Int32
DB.IndexSortServer.DeleteSort=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("IndexSortServer", "DeleteSort", array,success);
};
//保存指标类别 类别对象		参数(1) ALOS.Model.IndexSortType
DB.IndexSortServer.SaveSort=function(type,success){
var array=new Array();
array[0]=type;

DB.CreateWebLog("IndexSortServer", "SaveSort", array,success);
};
//根据ID获得指标套对象 ID编号		参数(1) System.Int32
DB.IndexSortServer.ReaderSort=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("IndexSortServer", "ReaderSort", array,success);
};
//启用指标禁用其他 id		参数(1) System.Int32
DB.IndexSortServer.Start=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("IndexSortServer", "Start", array,success);
};
