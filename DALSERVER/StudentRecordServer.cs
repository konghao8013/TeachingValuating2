using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ALOS.Expand;
using ALOS.Model;

namespace ALOS.DALSERVER
{
    [APIServer("v00009学生试卷操作类")]
    public class StudentRecordServer : Help<StudentRecordType>
    {
        /// <summary>
        /// 根据学院和测评批次统计测评数量
        /// </summary>
        /// <returns></returns>
        [APIServer("根据学院和测评批次统计测评数量")]
        public List<StudentRecordType> GroupSchoolNumber()
        {
            string sql =
                "select CollegeId,b.College,b.School,SUM(1)Number,TypeName from v00009 a join V00004 b on a.Rid=b.Rid group by CollegeId,b.College,b.School ,TypeName";
            return _help.Select<StudentRecordType>(sql);
        }

        /// <summary>
        /// 根据用户ID返回测评资料
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public StudentRecordType RenderRecord(long userid,bool state=false)
        {
            string sql = "select * from V00009 where state=@state and Rid=@userid";
            return _help.Reader<StudentRecordType>(sql, new SqlParameter("userid", userid),new SqlParameter("state",state));
        }
        /// <summary>
        /// 获得学生没有被禁用的档案
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override StudentRecordType Reader(string name, object value)
        {
            string sql = string.Format("select * from v00009 where Disable=0 and {0}=@{0}",name);
            return _help.Reader<StudentRecordType>(sql, new SqlParameter(name, value));
        }
        /// <summary>
        /// 重置学生测评材料
        /// </summary>
        /// <param name="studentId"></param>
        
        public int ResetScore(long studentId)
        {
            string sql = @"update V00009 set Disable=1 where Rid=@rid and Apply=1
update V00004 set DataState=0, AppraisalNumber=AppraisalNumber where rid=@rid";
             _help.ExecuteTrain(sql, new SqlParameter("rid", studentId));
            return 1;
        }
        /// <summary>
        /// 根据档案ID获得 资料对象
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        public StudentRecordType ReaderDid(int did)
        {
            string sql = "select * from V00009 where id in(select PaperId from V00010 where id=@did)";
           return  _help.Reader<StudentRecordType>(sql, new SqlParameter("did", did));
        }

        /// <summary>
        /// 根据教师编号抽取未评测的试卷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StudentRecordType ExtractPaper(long teacherId, int id = 0)
        {
            string sql = "select a.*,b.Id dID,b.teacherTypeid TeacherTypeId from V00009 a join V00010 b on a.Id=b.PaperId where b.UserId=@teacherId and a.Id not in(@id) and b.state=0 and b.disable=0";
            return _help.Reader<StudentRecordType>(sql, new SqlParameter("teacherId", teacherId),
                new SqlParameter("id", id));
        }
    }
}
