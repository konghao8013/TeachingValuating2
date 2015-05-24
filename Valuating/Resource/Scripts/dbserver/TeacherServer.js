//V00005#教师资料操作类
DB.TeacherServer = function() {
};
//保存资料 教师对象		参数(1) ALOS.Model.TeacherType
DB.TeacherServer.SaveTeacher=function(type,success){
var array=new Array();
array[0]=type;

DB.CreateWebLog("TeacherServer", "SaveTeacher", array,success);
};
//删除教师账户 教师编号		参数(1) ALOS.Model.TeacherType
DB.TeacherServer.DeleteTeacher=function(model,success){
var array=new Array();
array[0]=model;

DB.CreateWebLog("TeacherServer", "DeleteTeacher", array,success);
};
//根据用户ID获得教师资料 用户ID		参数(1) System.Int32
DB.TeacherServer.SelectTeacherRid=function(userId,success){
var array=new Array();
array[0]=userId;

DB.CreateWebLog("TeacherServer", "SelectTeacherRid", array,success);
};
// 获得所有的教师人数 		
DB.TeacherServer.SelectTeacherList=function(success){
var array=new Array();

DB.CreateWebLog("TeacherServer", "SelectTeacherList", array,success);
};
//根据学校学院获得教师 学校ID,学院ID,下标		参数(1) System.Int32参数(2) System.Int32参数(3) System.Int32
DB.TeacherServer.SelectSchoolTeacherList=function(schoolId,collegeId,index,success){
var array=new Array();
array[0]=schoolId;
array[1]=collegeId;
array[2]=index;

DB.CreateWebLog("TeacherServer", "SelectSchoolTeacherList", array,success);
};
//根据学院学校和教师类别获得教师列表 		参数(1) System.Int32参数(2) System.Int32参数(3) System.Int32参数(4) System.String
DB.TeacherServer.SelectTeacherList=function(schoolId,collegeId,teacherTypeId,loginId,success){
var array=new Array();
array[0]=schoolId;
array[1]=collegeId;
array[2]=teacherTypeId;
array[3]=loginId;

DB.CreateWebLog("TeacherServer", "SelectTeacherList", array,success);
};
//根据学校学院获得教师 学校ID,学院ID		参数(1) System.Int32参数(2) System.Int32
DB.TeacherServer.SelectSchoolTeacherCount=function(schoolId,collegeId,success){
var array=new Array();
array[0]=schoolId;
array[1]=collegeId;

DB.CreateWebLog("TeacherServer", "SelectSchoolTeacherCount", array,success);
};
//根据教师账户和学院编号获得分页教师工作量 教师编号,学院编号,下标		参数(1) System.String参数(2) System.Int32参数(3) System.Int32
DB.TeacherServer.SelectWorkList=function(teacherId,collegeId,index,success){
var array=new Array();
array[0]=teacherId;
array[1]=collegeId;
array[2]=index;

DB.CreateWebLog("TeacherServer", "SelectWorkList", array,success);
};
//根据教师账户和学院编号获得分页教师工作量数据条数 教师编号,学院编号		参数(1) System.String参数(2) System.Int32
DB.TeacherServer.SelectWorkCount=function(teacherId,collegeId,success){
var array=new Array();
array[0]=teacherId;
array[1]=collegeId;

DB.CreateWebLog("TeacherServer", "SelectWorkCount", array,success);
};
