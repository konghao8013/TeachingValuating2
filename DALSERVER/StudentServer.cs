using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ALOS.Expand;
using ALOS.Model;

namespace ALOS.DALSERVER
{
    [APIServer("V00004学生账户操作表")]
    public class StudentServer : Help<StudentType>
    {
        /// <summary>
        /// 保存资料
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [APIServer("保存资料", "教师对象")]
        public StudentType SaveStudent(StudentType type)
        {
            return base.Save(type);
        }
        /// <summary>
        /// 根据用户编号获得学生数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIServer("根据用户编号获得学生数据","用户编号")]
        public StudentType ReaderStudent(long rid)
        {
            string sql = "select * from v00004 where rid=@rid";
            return _help.Reader<StudentType>(sql, new SqlParameter("rid", rid));
        }

        /// <summary>
        /// 删除学生账户
        /// </summary>
        /// <param name="id"></param>
        [APIServer("删除学生帐号", "学生对象")]
        public void DeleteStudent(StudentType stu)
        {
            _help.ExecuteNonQuery("delete v00004 where id=@id", new SqlParameter("id", stu.Id));
            _help.ExecuteNonQuery("delete v00001 where id=@rid",new SqlParameter("rid",stu.Rid));
         //   base.Delete(id);
        }
        /// <summary>
        /// 获得所有的学生账户
        /// </summary>
        /// <returns></returns>
        [APIServer(" 获得所有的学生帐号")]
        public List<StudentType> SelectStudentList()
        {
            return base.List();
        }
       /// <summary>
        /// 获得学生试卷列表
       /// </summary>
       /// <param name="apply"></param>
       /// <param name="collegeId"></param>
       /// <param name="studentId"></param>
       /// <param name="index"></param>
       /// <returns></returns>
       [APIServer("获得学生试卷列表","是否完成评测,学院编号,学生帐号,下标")]
        public List<StudentType> SelectPaperList(int apply,int collegeId,string studentId,int index)
        {
//            declare @apply int,@CollegeId int,@studentId varchar(max)
//select @studentId='',@CollegeId=0,@apply=-1
           
            string sql =
                "select b.* ,c.loginId,a.TeachingPlan,a.Courseware,a.Reflection,a.Video,a.Apply,a.TeachingName,a.CoursewareName,a.ReflectionName,a.VideoName,a.Id paperId from(select * from V00009 where State=1) a join V00004 b on a.Rid=b.Rid join V00001 c on c.Id=b.Rid where (@CollegeId=0 or b.CollegeId=@CollegeId) and c.LoginId like ''+@studentId+'%' and (@apply=-1 or a.Apply=@apply)";
           if (apply == -2)
           {
               sql = " select a.*,b.LoginId,'' TeachingPlan,'' Courseware,'' Reflection,'' Video,'' TeachingName,'' CoursewareName,'' ReflectionName,'' VideoName ,0 paperId   from V00004 a join V00001 b on a.Rid=b.Id   where rid not in(select studentid from V00010 )  and (@CollegeId=0 or CollegeId=@CollegeId)";
           }
            return SqlList<StudentType>(sql, 20, index, new SqlParameter("apply", apply),
                new SqlParameter("collegeId", collegeId), new SqlParameter("studentId", studentId));


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apply"></param>
        /// <param name="collegeId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [APIServer("获得学生试卷列表", "是否完成评测,学院编号,学生帐号")]
        public int SelectPaperCount(int apply, int collegeId, string studentId)
        {
            string sql =
             "select b.* ,c.loginId,a.TeachingPlan,a.Courseware,a.Reflection,a.Video,a.Apply,a.TeachingName,a.CoursewareName,a.ReflectionName,a.VideoName from(select * from V00009 where State=1) a join V00004 b on a.Rid=b.Rid join V00001 c on c.Id=b.Rid where (@CollegeId=0 or b.CollegeId=@CollegeId) and c.LoginId like ''+@studentId+'%' and (@apply=-1 or a.Apply=@apply)";
            if (apply == -2)
            {
                sql = " select a.*,b.LoginId,'' TeachingPlan,'' Courseware,'' Reflection,'' Video,'' TeachingName,'' CoursewareName,'' ReflectionName,'' VideoName ,0 paperId   from V00004 a join V00001 b on a.Rid=b.Id   where rid not in(select studentid from V00010 )  and (@CollegeId=0 or CollegeId=@CollegeId)";
            }
            return SQLCount(sql, 20, new SqlParameter("apply", apply),
                new SqlParameter("collegeId", collegeId), new SqlParameter("studentId", studentId));
        }
        [APIServer("根据学生ID清除学生测评材料","学生ID")]
        public void ResetStudentData(int studentId)
        {
            string sql = @"update V00004 set DataState=0 ,AppraisalNumber=0 where Rid=@studentId
delete V00014 where PaperId in(select id from V00009 where Rid=@studentId)
delete V00012 where did in(select id from V00010 where StudentId=@studentId)
delete V00010 where StudentId=@studentId
delete V00009 where Rid=@studentId";
            _help.ExecuteNonQuery(sql,new SqlParameter("studentId",studentId));
        }
    }
}
