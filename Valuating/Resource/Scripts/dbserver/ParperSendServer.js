//V00015申请材料退回操作类
DB.ParperSendServer = function() {
};
//创建申请材料退回申请 		参数(1) ALOS.Model.ParperSendType
DB.ParperSendServer.SaveSend=function(type,success){
var array=new Array();
array[0]=type;

DB.CreateWebLog("ParperSendServer", "SaveSend", array,success);
};
//获取需要打回的材料 获取是否处理过的试卷		参数(1) System.Boolean
DB.ParperSendServer.ListPaper=function(state,success){
var array=new Array();
array[0]=state;

DB.CreateWebLog("ParperSendServer", "ListPaper", array,success);
};
//同意打回材料 打回材料ID		参数(1) System.Int32
DB.ParperSendServer.OkSend=function(id,success){
var array=new Array();
array[0]=id;

DB.CreateWebLog("ParperSendServer", "OkSend", array,success);
};
//根据试卷编号打回材料 试卷编号		参数(1) System.Int32
DB.ParperSendServer.BackPaper=function(paperId,success){
var array=new Array();
array[0]=paperId;

DB.CreateWebLog("ParperSendServer", "BackPaper", array,success);
};
