using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ALOS.Expand;
using ALOS.Model;

namespace ALOS.DALSERVER
{
    [APIServer("V00014学生成绩结果操作类")]
    public class StudentScoreServer : Help<StudentScoreType>
    {
        /// <summary>
        /// 根据学生编号获得学生已经完成的试卷并且没有被禁用的档案
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public StudentScoreType ReadStudentScore(long studentId)
        {
            var sql = "select * from v00014 where paperId in(select id from V00009 where rid=@studentId and State=1 and Disable=0)";
            return _help.Reader<StudentScoreType>(sql, new SqlParameter("studentId", studentId));
        }
        /// <summary>
        /// 根据测评批次学生帐号学院信息 查询所有试卷
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="studentId"></param>
        /// <param name="collegeId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [APIServer("根据测评批次学生帐号学院信息 查询所有试卷数量", "测评批次,学生帐号,学院编号,页码")]
        public List<StudentScoreType> SelectStudentScore(string typeName, string studentId, int collegeId, int index)
        {
//            declare @typeName varchar(max),@CollegeId int,@studentId varchar(max)
//select @studentId='',@CollegeId=0,@typeName=''

            string sql =
                "select a.*,c.Name StudentName ,c.School,c.College,d.LoginId StudentLoginId,b.AppraisalNumber AppraisalNumber from V00014 a join V00009 b on a.PaperId=b.Id join V00004 c on c.Rid=b.Rid join V00001 d on d.Id=c.Rid where b.TypeName like ''+@typeName+'%' and d.LoginId like''+@studentId+'%' and (@CollegeId=0 or c.CollegeId=@CollegeId)";
            return base.SqlList<StudentScoreType>(sql, 20, index, new SqlParameter("typeName", typeName),
                new SqlParameter("collegeId", collegeId), new SqlParameter("studentId", studentId));
        }
        /// <summary>
        /// 根据测评批次学生帐号学院信息 查询所有试卷数量
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="studentId"></param>
        /// <param name="collegeId"></param>
        /// <returns></returns>
        [APIServer("根据测评批次学生帐号学院信息 查询所有试卷数量","测评批次,学生帐号,学院编号")]
        public int SelectStudentScoreCount(string typeName, string studentId, int collegeId)
        {
            string sql =
                "select a.*,c.Name StudentName ,c.School,c.College,d.LoginId StudentLoginId from V00014 a join V00009 b on a.PaperId=b.Id join V00004 c on c.Rid=b.Rid join V00001 d on d.Id=c.Rid where b.TypeName like ''+@typeName+'%' and d.LoginId like''+@studentId+'%' and (@CollegeId=0 or c.CollegeId=@CollegeId)";
            return base.SQLCount(sql, 20, new SqlParameter("typeName", typeName),
                new SqlParameter("collegeId", collegeId), new SqlParameter("studentId", studentId));
        }
    }
}
