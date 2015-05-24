//v00010档案对象操作类
DB.EvaluatingPaperServer = function() {
};
//根据试卷类别和试卷学院获得试卷列表 页码长度,页码下标,学院ID,试卷ID		参数(1) System.Int32参数(2) System.Int32参数(3) System.Int32参数(4) System.Int32参数(5) System.Int32
DB.EvaluatingPaperServer.SelectPaper=function(pageSize,index,collegeId,teacherTypeId,schoolId,success){
var array=new Array();
array[0]=pageSize;
array[1]=index;
array[2]=collegeId;
array[3]=teacherTypeId;
array[4]=schoolId;

DB.CreateWebLog("EvaluatingPaperServer", "SelectPaper", array,success);
};
//根据试卷类别学院ID获得试卷页长 页码长度,学院ID,试卷类别		参数(1) System.Int32参数(2) System.Int32参数(3) System.Int32参数(4) System.Int32
DB.EvaluatingPaperServer.PaperCount=function(pageSize,collegeId,teacherTypeId,schoolId,success){
var array=new Array();
array[0]=pageSize;
array[1]=collegeId;
array[2]=teacherTypeId;
array[3]=schoolId;

DB.CreateWebLog("EvaluatingPaperServer", "PaperCount", array,success);
};
//根据学院编号修改专家 		参数(1) System.Int32
DB.EvaluatingPaperServer.UpdateCollege=function(collegeId,success){
var array=new Array();
array[0]=collegeId;

DB.CreateWebLog("EvaluatingPaperServer", "UpdateCollege", array,success);
};
//根据抽取试卷ID批量修改教师 数组档案ID,教师ID		参数(1) System.String参数(2) System.Int32
DB.EvaluatingPaperServer.UpdatePapers=function(papersStr,teacherId,success){
var array=new Array();
array[0]=papersStr;
array[1]=teacherId;

DB.CreateWebLog("EvaluatingPaperServer", "UpdatePapers", array,success);
};
//重新设置试卷分配 档案ID,教师编号		参数(1) System.Int32参数(2) System.Int32
DB.EvaluatingPaperServer.UpdateTeacher=function(did,teacherId,success){
var array=new Array();
array[0]=did;
array[1]=teacherId;

DB.CreateWebLog("EvaluatingPaperServer", "UpdateTeacher", array,success);
};
//根据条件分写查询试卷分配情况 学院ID,用户帐号,测评档次名称,多少页,测评耗时		参数(1) System.Int32参数(2) System.String参数(3) System.String参数(4) System.Int32参数(5) System.Decimal参数(6) System.Int32
DB.EvaluatingPaperServer.EvaluatingPageList=function(collegeId,loginId,typeName,index,time,state,success){
var array=new Array();
array[0]=collegeId;
array[1]=loginId;
array[2]=typeName;
array[3]=index;
array[4]=time;
array[5]=state;

DB.CreateWebLog("EvaluatingPaperServer", "EvaluatingPageList", array,success);
};
//获取试卷列表 学号,教师编号,是否分配到教师,页数		参数(1) System.String参数(2) System.String参数(3) System.Boolean参数(4) System.Int32参数(5) System.Int32
DB.EvaluatingPaperServer.EvaluatingAllotList=function(studentId,teacherId,isAllot,teacherType,index,success){
var array=new Array();
array[0]=studentId;
array[1]=teacherId;
array[2]=isAllot;
array[3]=teacherType;
array[4]=index;

DB.CreateWebLog("EvaluatingPaperServer", "EvaluatingAllotList", array,success);
};
//获得试卷列表页数 学号,教师号,是否已经分配		参数(1) System.String参数(2) System.String参数(3) System.Boolean参数(4) System.Int32
DB.EvaluatingPaperServer.EvaluatingAllotCount=function(studentId,teacherId,isAllot,teacherType,success){
var array=new Array();
array[0]=studentId;
array[1]=teacherId;
array[2]=isAllot;
array[3]=teacherType;

DB.CreateWebLog("EvaluatingPaperServer", "EvaluatingAllotCount", array,success);
};
//根据条件分写查询试卷分配页数 学院ID,用户帐号,测评档次名称,测评耗时		参数(1) System.Int32参数(2) System.String参数(3) System.String参数(4) System.Decimal参数(5) System.Int32
DB.EvaluatingPaperServer.EvaluatingPageCount=function(collegeId,loginId,typeName,time,state,success){
var array=new Array();
array[0]=collegeId;
array[1]=loginId;
array[2]=typeName;
array[3]=time;
array[4]=state;

DB.CreateWebLog("EvaluatingPaperServer", "EvaluatingPageCount", array,success);
};
//根据试卷ID获得档案对象 试卷ID		参数(1) System.Int32
DB.EvaluatingPaperServer.EvaluatingList=function(paperId,success){
var array=new Array();
array[0]=paperId;

DB.CreateWebLog("EvaluatingPaperServer", "EvaluatingList", array,success);
};
//获得对应的试卷 教师ID,试卷状态		参数(1) System.Int32参数(2) System.Int32
DB.EvaluatingPaperServer.SelectTeacherIdList=function(teacherId,status,success){
var array=new Array();
array[0]=teacherId;
array[1]=status;

DB.CreateWebLog("EvaluatingPaperServer", "SelectTeacherIdList", array,success);
};
