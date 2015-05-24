//v00009学生试卷操作类
DB.StudentRecordServer = function() {
};
//根据学院和测评批次统计测评数量 		
DB.StudentRecordServer.GroupSchoolNumber=function(success){
var array=new Array();

DB.CreateWebLog("StudentRecordServer", "GroupSchoolNumber", array,success);
};
