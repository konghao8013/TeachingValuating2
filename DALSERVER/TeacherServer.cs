using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ALOS.Expand;
using ALOS.Model;

namespace ALOS.DALSERVER
{
    [APIServer("V00005#教师资料操作类")]
    public class TeacherServer : Help<TeacherType>
    {
        /// <summary>
        /// 保存资料
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [APIServer("保存资料", "教师对象")]
        public TeacherType SaveTeacher(TeacherType type)
        {

            return base.Save(type);
        }
        /// <summary>
        /// 删除教师账户
        /// </summary>
        /// <param name="id"></param>
        [APIServer("删除教师账户", "教师编号")]
        public void DeleteTeacher(TeacherType model)
        {

            base.Delete(model.Id);
            _help.ExecuteNonQuery("delete from v00001 where id=@id", new SqlParameter("id", model.Rid));
        }
        /// <summary>
        /// 根据用户ID获得教师资料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIServer("根据用户ID获得教师资料", "用户ID")]
        public TeacherType SelectTeacherRid(int userId)
        {
            return base.Reader("rid", userId);
        }

        /// <summary>
        /// 获得所有的教师人数
        /// </summary>
        /// <returns></returns>
        [APIServer(" 获得所有的教师人数")]
        public List<TeacherType> SelectTeacherList()
        {
            return base.List();
        }

        public List<TeacherType> TopSelectTeacherList(int pageSize, int index)
        {
            string sql = "select * from v00005";
            return null;
        }
        /// <summary>
        /// 根据学校学院获得教师
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="collegeId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [APIServer("根据学校学院获得教师","学校ID,学院ID,下标")]
        public List<TeacherType> SelectSchoolTeacherList(int schoolId, int collegeId, int index)
        {
 //            declare @schoolId int ,@collegeId int
 //select @schoolId=0,@collegeId=0
            string sql = @" 
 select * from V00005 where (@schoolId=0 or SchoolId=@schoolId) and (@CollegeId=0 or CollegeId=@CollegeId)";
            return SqlList<TeacherType>(sql, 14, index, new SqlParameter("schoolId", schoolId),
                new SqlParameter("CollegeId", collegeId));
        }
        [APIServer("根据学院学校和教师类别获得教师列表")]
        public List<TeacherType> SelectTeacherList(int schoolId, int collegeId,int teacherTypeId,string loginId)
        {
            string sql =
                "select a.*,b.LoginId from v00005 a join v00001 b on a.Rid=b.Id where schoolId=@schoolId and (@collegeId=0 or collegeId=@collegeId) and TypeId=@typeId and (@loginId='' or b.loginId like '%'+@loginId+'%')";
            return _help.Select<TeacherType>(sql, new SqlParameter("collegeId", collegeId),
                new SqlParameter("SCHOOLiD", schoolId), new SqlParameter("typeId", teacherTypeId),new SqlParameter("loginId",loginId));
        }

            /// <summary>
        /// 根据学校学院获得教师
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="collegeId"></param>
        /// <returns></returns>
        [APIServer("根据学校学院获得教师", "学校ID,学院ID")]
        public int SelectSchoolTeacherCount(int schoolId, int collegeId)
        {
            string sql =
                " select 1 rcount from V00005 where (@schoolId=0 or SchoolId=@schoolId) and (@CollegeId=0 or CollegeId=@CollegeId)";
            return SQLCount(sql, 14, new SqlParameter("schoolId", schoolId),
                new SqlParameter("CollegeId", collegeId));
        }
        /// <summary>
        /// 抽取测评专家
        /// </summary>
        /// <param name="teacherType">抽取专家类别 3评价 仲裁 不分学院</param>
        /// <param name="studentId">学生帐号</param>
        /// <returns></returns>
        public TeacherType ExtractTeacher(EnumTeacherType teacherType, long studentId)
        {
            //            declare @studentId int, @teacherType int
            //select @studentId=86,@teacherType=2
            string sql =
               @"select a.* from (select * from V00005  where Rid not in(select userid from v00010  where studentId=@studentId) and TypeId=@teacherType  and state=1 and  exists(select 1 from V00004 where Rid=@studentId and SchoolId=V00005.SchoolId and (@teacherType>2 or CollegeId=V00005.CollegeId))
)  a left join (select UserId,count(1) Number from V00010 group by UserId) b on a.Rid=b.UserId order by ISNULL(b.Number,0)";
            return _help.Reader<TeacherType>(sql, new SqlParameter("studentId", studentId),
                new SqlParameter("teacherType", teacherType));
        }
        /// <summary>
        /// 根据教师账户和学院编号获得分页教师工作量
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="collegeId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [APIServer("根据教师账户和学院编号获得分页教师工作量","教师编号,学院编号,下标")]
        public List<TeacherType> SelectWorkList(string teacherId,int collegeId,int index)
        {
//            declare @teacherId varchar(max),@schoolId int,@CollegeId int
//select @teacherId='',@CollegeId=0


            string sql = @"select b.*,a.QNumber,a.NWNumber,a.WNumber,c.loginid teacherLoginId from (
select UserId,SUM(1)QNumber,SUM(case when State=1 then 1 else 0 end)WNumber, SUM(case when State=0 then 1 else 0 end)NWNumber from V00010 group by UserId
) a join V00005 b on a.UserId=b.Rid join V00001 c on b.Rid=c.Id where c.LoginId like ''+@teacherId+'%' and (@CollegeId=0 or b.CollegeId=@CollegeId)";
            return SqlList<TeacherType>(sql, 20, index, new SqlParameter("teacherId", teacherId),
                new SqlParameter("collegeId", collegeId));
        }
        /// <summary>
        /// 根据教师账户和学院编号获得分页教师工作量数据条数
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="collegeId"></param>
        /// <returns></returns>
        [APIServer("根据教师账户和学院编号获得分页教师工作量数据条数", "教师编号,学院编号")]
        public int SelectWorkCount(string teacherId, int collegeId)
        {
            string sql=@"select b.*,a.QNumber,a.NWNumber,a.WNumber from (
select UserId,SUM(1)QNumber,SUM(case when State=1 then 1 else 0 end)WNumber, SUM(case when State=0 then 1 else 0 end)NWNumber from V00010 group by UserId
) a join V00005 b on a.UserId=b.Rid join V00001 c on b.Rid=c.Id where c.LoginId like ''+@teacherId+'%' and (@CollegeId=0 or b.CollegeId=@CollegeId)";
            return SQLCount(sql, 20, new SqlParameter("teacherId", teacherId),
                new SqlParameter("collegeId", collegeId));
        }
        

    }
}
