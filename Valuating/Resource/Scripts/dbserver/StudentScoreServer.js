//V00014学生成绩结果操作类
DB.StudentScoreServer = function() {
};
//根据测评批次学生帐号学院信息 查询所有试卷数量 测评批次,学生帐号,学院编号,页码		参数(1) System.String参数(2) System.String参数(3) System.Int32参数(4) System.Int32
DB.StudentScoreServer.SelectStudentScore=function(typeName,studentId,collegeId,index,success){
var array=new Array();
array[0]=typeName;
array[1]=studentId;
array[2]=collegeId;
array[3]=index;

DB.CreateWebLog("StudentScoreServer", "SelectStudentScore", array,success);
};
//根据测评批次学生帐号学院信息 查询所有试卷数量 测评批次,学生帐号,学院编号		参数(1) System.String参数(2) System.String参数(3) System.Int32
DB.StudentScoreServer.SelectStudentScoreCount=function(typeName,studentId,collegeId,success){
var array=new Array();
array[0]=typeName;
array[1]=studentId;
array[2]=collegeId;

DB.CreateWebLog("StudentScoreServer", "SelectStudentScoreCount", array,success);
};
