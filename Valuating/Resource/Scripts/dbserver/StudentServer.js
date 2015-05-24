//V00004学生账户操作表
DB.StudentServer = function() {
};
//保存资料 教师对象		参数(1) ALOS.Model.StudentType
DB.StudentServer.SaveStudent=function(type,success){
var array=new Array();
array[0]=type;

DB.CreateWebLog("StudentServer", "SaveStudent", array,success);
};
//根据用户编号获得学生数据 用户编号		参数(1) System.Int64
DB.StudentServer.ReaderStudent=function(rid,success){
var array=new Array();
array[0]=rid;

DB.CreateWebLog("StudentServer", "ReaderStudent", array,success);
};
//删除学生帐号 学生对象		参数(1) ALOS.Model.StudentType
DB.StudentServer.DeleteStudent=function(stu,success){
var array=new Array();
array[0]=stu;

DB.CreateWebLog("StudentServer", "DeleteStudent", array,success);
};
// 获得所有的学生帐号 		
DB.StudentServer.SelectStudentList=function(success){
var array=new Array();

DB.CreateWebLog("StudentServer", "SelectStudentList", array,success);
};
//获得学生试卷列表 是否完成评测,学院编号,学生帐号,下标		参数(1) System.Int32参数(2) System.Int32参数(3) System.String参数(4) System.Int32
DB.StudentServer.SelectPaperList=function(apply,collegeId,studentId,index,success){
var array=new Array();
array[0]=apply;
array[1]=collegeId;
array[2]=studentId;
array[3]=index;

DB.CreateWebLog("StudentServer", "SelectPaperList", array,success);
};
//获得学生试卷列表 是否完成评测,学院编号,学生帐号		参数(1) System.Int32参数(2) System.Int32参数(3) System.String
DB.StudentServer.SelectPaperCount=function(apply,collegeId,studentId,success){
var array=new Array();
array[0]=apply;
array[1]=collegeId;
array[2]=studentId;

DB.CreateWebLog("StudentServer", "SelectPaperCount", array,success);
};
//根据学生ID清除学生测评材料 学生ID		参数(1) System.Int32
DB.StudentServer.ResetStudentData=function(studentId,success){
var array=new Array();
array[0]=studentId;

DB.CreateWebLog("StudentServer", "ResetStudentData", array,success);
};
