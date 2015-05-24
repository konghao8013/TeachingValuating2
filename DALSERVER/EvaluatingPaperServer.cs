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
    /// <summary>
    /// V00010
    /// </summary>
    [APIServer("v00010档案对象操作类")]
    public class EvaluatingPaperServer : Help<EvaluatingPaperType>
    {
        /// <summary>
        /// 重置试卷状态
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="disable"></param>
        /// <returns></returns>
        public int ResetPaperDisable(int paperId, bool disable)
        {
            string sql = "update v00010 set disable=@disable where paperId=@paperId";
            _help.ExecuteNonQuery(sql, new SqlParameter("paperId", paperId), new SqlParameter("disable", disable));
            return 1;
        }
        [APIServer("根据试卷类别和试卷学院获得试卷列表","页码长度,页码下标,学院ID,试卷ID")]
        public List<EvaluatingPaperType> SelectPaper(int pageSize,int index,int collegeId,int teacherTypeId,int schoolId)
        {
            string sql =
                "select a.*,d.LoginId,e.College,c.LoginId TeacherLoginId,c.Name TeacherName,d.Name StudentName,b.TeachingPlan,b.TeachingName,b.Courseware,b.CoursewareName,b.Video,b.VideoName,b.Reflection,b.ReflectionName from V00010 a join V00009 b on a.PaperId=b.Id left join V00001 c on a.UserId=c.Id join V00001 d on d.Id=a.StudentId and a.state=0  join V00004 e on e.Rid=d.Id and(@CollegeId=0 or e.CollegeId=@CollegeId) and (@teacherTypeId=0 or a.TeacherTypeId=@teacherTypeId) and(@schoolId=0 or e.schoolId=@schoolId)";
            return SqlList<EvaluatingPaperType>(sql, pageSize, index, new SqlParameter("CollegeId", collegeId),
                new SqlParameter("teacherTypeId", teacherTypeId), new SqlParameter("schoolId", schoolId));
        }
        [APIServer("根据试卷类别学院ID获得试卷页长","页码长度,学院ID,试卷类别")]
        public int PaperCount(int pageSize, int collegeId, int teacherTypeId, int schoolId)
        {
            string sql =
              "select a.*,d.LoginId,e.College,c.LoginId TeacherLoginId,c.Name TeacherName,d.Name StudentName,b.TeachingPlan,b.TeachingName,b.Courseware,b.CoursewareName,b.Video,b.VideoName,b.Reflection,b.ReflectionName from V00010 a join V00009 b on a.PaperId=b.Id left join V00001 c on a.UserId=c.Id join V00001 d on d.Id=a.StudentId and a.state=0  join V00004 e on e.Rid=d.Id and(@CollegeId=0 or e.CollegeId=@CollegeId) and (@teacherTypeId=0 or a.TeacherTypeId=@teacherTypeId) and(@schoolId=0 or e.schoolId=@schoolId)";
            return SQLCount(sql, pageSize, new SqlParameter("CollegeId", collegeId),
                new SqlParameter("teacherTypeId", teacherTypeId), new SqlParameter("schoolId", schoolId));
        }
        /// <summary>
        /// 按照学院重新分配试卷
        /// </summary>
        /// <param name="collegeId"></param>
        [APIServer("根据学院编号修改专家")]
        public void UpdateCollege(int collegeId)
        {
            string sql =
                @"declare @schoolId int
select @schoolId=ObjId from V00003 where Id=@collegeId
declare UpdateV00010UserId cursor for select id,TeacherTypeId,StudentId from v00010  where StudentId in(select rid from V00004 where CollegeId=@collegeId) and State=0
open UpdateV00010UserId
declare @Id int,@teacherTypeId int,@studentId int
declare @value nvarchar(20)
fetch next from UpdateV00010UserId into @Id,@teacherTypeId,@studentId
while @@fetch_status=0
begin
	declare @teacherId int
	set @teacherId=0
	select top 1 @teacherId=Rid from (
select a.*,ISNULL(b.CountNumber,0)CountNumber from V00005 a  left join (select count(1) CountNumber,UserID from V00010 group by UserId)b on a.Rid=b.UserId)A   where TypeId=@teacherTypeId and (CollegeId=@CollegeId or @teacherTypeId=3 or @teacherTypeId=4) and SchoolId=@schoolId  and rid not in(select userid from V00010 where StudentId=@studentId) order by CountNUMBER ,Rid
	update V00010 set UserId=@teacherId where id =@Id
    fetch next from UpdateV00010UserId into @Id,@teacherTypeId,@studentId
end
close UpdateV00010UserId
deallocate UpdateV00010UserId";
             _help.ExecuteNonQuery(sql, new SqlParameter("collegeId", collegeId));
        }

        [APIServer("根据抽取试卷ID批量修改教师","数组档案ID,教师ID")]
        public void UpdatePapers(string papersStr, int teacherId)
        {

            var papers = papersStr.Deserialize<Int32[]>();
            string sql = "update v00010 set userId=@userId where id in({0})";
            var sb = new StringBuilder();
            var parameters = new SqlParameter[papers.Length+1];
            var index = 0;
            foreach (var paperId in papers)
            {
                sb.Append("@p"+ index+",");
                parameters[index]=new SqlParameter("p"+index,paperId);
                index++;
            }
            parameters[index] = new SqlParameter("userId", teacherId);
            string str = sb.ToString();
            str = str.Remove(str.Length - 1);
            sql = string.Format(sql, str);
            _help.ExecuteNonQuery(sql,parameters);
        }

        /// <summary>
        /// 根据试卷ID获得专家数量
        /// </summary>
        /// <param name="paperId"></param>
        /// <returns></returns>
        public int PaperIdCount(int paperId)
        {
            string sql = "select count(1) from v00010 where paperId=@paperId";
            return _help.ExecuteScalar<int>(sql, new SqlParameter("paperId", paperId));
        }
        /// <summary>
        /// 重新设置试卷分配
        /// </summary>
        /// <param name="did"></param>
        /// <param name="teacherId"></param>
        [APIServer("重新设置试卷分配","档案ID,教师编号")]
        public void UpdateTeacher(int did, int teacherId)
        {
            string sql = "update v00010 set userId=@teacherId where id=@did";
             _help.ExecuteNonQuery(sql, new SqlParameter("teacherId", teacherId), new SqlParameter("did", did));
        }
        /// <summary>
        /// 统计评分成绩
        /// </summary>
        /// <param name="paperId">试卷ID</param>
        /// <returns></returns>
        public DataTable GroupScore(int paperId)
        {
            string sql = "select TeachingPlanScore[教学设计评分],PresentScore[教学说课评分],CoursewareScore[多媒体教学评分],VideoScore[教学视频评分],ReflectionScore[教学评价评分],TeachingPlanScore+PresentScore+CoursewareScore+VideoScore+ReflectionScore[评价总分],Remark [教师评语] from V00014 where PaperId=@id";
            return _help.Table(sql, new SqlParameter("id", paperId));
        }

        [APIServer("根据条件分写查询试卷分配情况","学院ID,用户帐号,测评档次名称,多少页,测评耗时")]
        public List<EvaluatingPaperType> EvaluatingPageList(int collegeId, string loginId, string typeName, int index,decimal time,int state)
        {
//            declare @CollegeId int,@loginId varchar(max),@typeName varchar(max),@time money
//            set @CollegeId=0
//set @loginId=''
//set @typeName='测'
//set @time=0.7

//select a.*,b.LoginId TeacherLoginId,b.Name TeacherName,c.Name StudentName,c.LoginId,d.TypeName from V00010 a join V00001 b on a.UserId=b.Id join V00001 c on a.StudentId=c.Id join V00009 d on a.PaperId=d.Id join V00004 e on e.Rid=a.StudentId where (@CollegeId=0 or e.CollegeId=@CollegeId) and c.LoginId like ''+@loginId+'%' and d.TypeName like ''+@typeName+'%'
//and a.TeachingPlanTime+a.CoursewareTime+a.VideoTime+a.ReflectionTime>@time*60

            string sql =
                @"select a.*,b.LoginId TeacherLoginId,f.Phone TeacherPhone,b.Name TeacherName,c.Name StudentName,c.LoginId,d.TypeName from V00010 a join V00001 b on a.UserId=b.Id join v00005 f on a.userid=f.rid  join V00001 c on a.StudentId=c.Id join V00009 d on a.PaperId=d.Id join V00004 e on e.Rid=a.StudentId where (@CollegeId=0 or e.CollegeId=@CollegeId) and c.LoginId like ''+@loginId+'%' and d.TypeName like ''+@typeName+'%'
and a.TeachingPlanTime+a.CoursewareTime+a.VideoTime+a.ReflectionTime<@time*60 and (@state=-1 or a.state=@state)";
            return base.SqlList<EvaluatingPaperType>(sql,20,index, new SqlParameter("CollegeId", collegeId), new SqlParameter("loginId", loginId),
                new SqlParameter("typeName", typeName),new SqlParameter("time",time),new SqlParameter("state",state));

        }
        /// <summary>
        /// 获取试卷列表
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="teacherId"></param>
        /// <param name="isAllot"></param>
        /// <returns></returns>
        [APIServer("获取试卷列表","学号,教师编号,是否分配到教师,页数")]
        public List<EvaluatingPaperType> EvaluatingAllotList(string studentId,string teacherId,bool isAllot,int teacherType,int index)
        {
//declare @studentId varchar(max),@teacherId varchar(max),@userId int
//select @studentId='',@teacherId='',@userId=0

            string sql = @"select a.*,b.LoginId TeacherLoginId,b.Name TeacherName,c.LoginId LoginId ,c.Name StudentName,d.School,d.College from (select * from V00010 where Disable=0 and State=0 ) a   left join V00001 b on a.UserId=b.Id  join V00001 c on a.StudentId=c.Id join V00004 d on c.Id=d.Rid  
 where (b.LoginId is null or b.LoginId like ''+@teacherId+'%' ) and c.LoginId like ''+@studentId+'%' and (@userId<>0 or a.UserId=0) and (@teacherType=0 or a.teacherTypeId=@teacherType)";
            return base.SqlList<EvaluatingPaperType>(sql, 20, index, new SqlParameter("studentId", studentId),
                new SqlParameter("teacherId", teacherId), new SqlParameter("userId", isAllot ? 1 : 0), new SqlParameter("teacherType", teacherType));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="teacherId"></param>
        /// <param name="isAllot"></param>
        /// <returns></returns>
        [APIServer("获得试卷列表页数","学号,教师号,是否已经分配")]
        public int EvaluatingAllotCount(string studentId, string teacherId, bool isAllot, int teacherType)
        {
            string sql = @"select 1 N from (select * from V00010 where Disable=0 and State=0 ) a left join V00001 b on a.UserId=b.Id left join V00001 c on a.StudentId=c.Id where (b.LoginId is null or b.LoginId like ''+@teacherId+'%' ) and c.LoginId like ''+@studentId+'%' and (@userId<>0 or a.UserId=0) and (@teacherType=0 or a.teacherTypeId=@teacherType)";
            return base.SQLCount(sql, 20, new SqlParameter("studentId", studentId),
                new SqlParameter("teacherId", teacherId), new SqlParameter("userId", isAllot ? 1 : 0), new SqlParameter("teacherType", teacherType));
        }

        public List<EvaluatingPaperType> EvaluatingPageList(int collegeId, string loginId, string typeName, int index,
            int time,int state)
        {
            return EvaluatingPageList(collegeId, loginId, typeName,index,(decimal)time,state);
        }
         [APIServer("根据条件分写查询试卷分配页数", "学院ID,用户帐号,测评档次名称,测评耗时")]
        public int EvaluatingPageCount(int collegeId,string loginId,string typeName,decimal time,int state)
        {
            string sql =
                @"select 1 N from V00010 a join V00001 b on a.UserId=b.Id join V00001 c on a.StudentId=c.Id join V00009 d on a.PaperId=d.Id join V00004 e on e.Rid=a.StudentId where (@CollegeId=0 or e.CollegeId=@CollegeId) and c.LoginId like ''+@loginId+'%' and d.TypeName like ''+@typeName+'%'
and a.TeachingPlanTime+a.CoursewareTime+a.VideoTime+a.ReflectionTime<@time*60 and (@state=-1 or a.state=@state)";
            return base.SQLCount(sql,20,new SqlParameter("CollegeId", collegeId), new SqlParameter("loginId", loginId),
                new SqlParameter("typeName", typeName),new SqlParameter("time",time),new SqlParameter("state",state));
        }

        public int EvaluatingPageCount(int collegeId, string loginId, string typeName, int time,int state)
        {
            return EvaluatingPageCount(collegeId, loginId, typeName, (decimal)time,state);
        }

        /// <summary>
        /// 根据教师名称统计试卷数量 state=false统计未完成否则统计完成
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public int TeacherCount(long teacherId,bool state=false)
        {
            string sql = "select count(1) from v00010 where userid=@teacherId and state=@state and disable=0";
            return _help.ExecuteScalar<int>(sql, new SqlParameter("teacherId", teacherId),new SqlParameter("state",state));
        }

        [APIServer("根据试卷ID获得档案对象","试卷ID")]
        public List<EvaluatingPaperType> EvaluatingList(int paperId)
        {
            string sql = "select a.*,b.Name UserName from v00010 a join v00001 b on a.userid=b.id  where paperId=@paperId and a.state=1";
            return _help.Select<EvaluatingPaperType>(sql, new SqlParameter("paperId", paperId));
            //_help.List<EvaluatingPaperType>("paperId", paperId);
        }
        /// <summary>
        /// 检查评测是否全部完成
        /// </summary>
        public bool CheckFinally(int paperId)
        {
            string sql = "select count(1) from v00010 where paperid=@paperid and state=0";
            return _help.ExecuteScalar<int>(sql, new SqlParameter("paperid", paperId))== 0;
        }
        /// <summary>
        /// 计算学生成绩
        /// </summary>
        /// <param name="did"></param>
        /// <param name="type"></param>
        /// <param name="date"></param>
        public void UpdateScore(int did ,EnumStudentDataType type,DateTime date)
        {
            string sql =
                @"update V00010 set TeachingPlanScore=b.TeachingPlan,CoursewareScore=b.Courseware,VideoScore=b.Video,ReflectionScore=b.Reflection,RresentScore=b.present,TeachingPlanTime=(case when @typeId=1 then dbo.CcalculateMinuteValue(@endDate,@date) else TeachingPlanTime end )
,CoursewareTime=(case when @typeId=2 then dbo.CcalculateMinuteValue(@endDate,@date) else CoursewareScore end )
,VideoTime=(case when @typeId=3 then dbo.CcalculateMinuteValue(@endDate,@date) else VideoTime end )
,ReflectionTime=(case when @typeId=4 then dbo.CcalculateMinuteValue(@endDate,@date) else ReflectionTime end )
,PresentTime=(case when @typeId=5 then dbo.CcalculateMinuteValue(@endDate,@date) else PresentTime end )
  from (select a.J*b.TeachingPlan TeachingPlan,a.K*b.Courseware Courseware,a.V*b.Video Video,a.F*b.Reflection Reflection,a.S*b.present present
 from (
select sum(case when b.typeId=1  then a.gradeScore else 0 end) J,sum(case when b.typeId=2  then a.gradeScore else 0 end) K,sum(case when b.typeId=3  then a.gradeScore else 0 end) V,sum(case when b.typeId=4  then a.gradeScore else 0 end) F,sum(case when b.typeId=5  then a.gradeScore else 0 end) S from v00012 a join v00011 b on a.zid=b.id where a.did=@did 
) a , v00007 b)b where Id=@did";
            _help.ExecuteTrain(sql,new SqlParameter("did",did),new SqlParameter("date",date),new SqlParameter("typeid",type),new SqlParameter("endDate",DateTime.Now));
        }
        /// <summary>
        /// 一票否决
        /// </summary>
        /// <param name="did"></param>
        public void VoteVown(int did)
        {
//            declare @did int 
//set @did=40
            string sql = @"declare @paperId int,@TypeName varchar(max),@studentId int
select @paperId=PaperId from V00010 where id=@did
update V00010 set state=1 where PaperId=@paperId
update V00009 set Apply=1 where Id=@paperId
select @typeName=TypeName,@studentId=Rid from V00009 where id=@paperId
update V00004 set AppraisalNumber=AppraisalNumber+1 where Rid=@studentId
delete V00012 where did in(select id from V00010 where PaperId=@paperId)
insert V00012
select @did,Id,0,0 from v00011
insert V00014(Name,PaperId,TeachingPlanScore,CoursewareScore,VideoScore,ReflectionScore,Innovate,PresentScore,CreateTime,SumScore,Remark)
values(@typeName,@paperId,0,0,0,0,0,0,getdate(),0,'课件与教案不一致（成绩无效一票否决）')";
            _help.ExecuteTrain(sql,new SqlParameter("did",did));
        }
        /// <summary>
        /// 根据教师ID获得对应的试卷
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        [APIServer("获得对应的试卷","教师ID,试卷状态")]
        public List<EvaluatingPaperType> SelectTeacherIdList(int teacherId,int status)
        {
//            declare @teacherId int
//set @teacherId=11189

            string sql =
                "select a.*,b.School,b.College,b.Name StudentName from V00010 a join V00004 b on a.StudentId=b.Rid where a.State=0 and exists(select 1 from V00005 c where c.TypeId=a.TeacherTypeId and c.CollegeId=b.CollegeId and c.Rid=@teacherId) and a.userId not in(@teacherId) and(@status=-1 or (@status=0 and a.userId=0) or (@status=1 and a.userId>0))";
            return _help.Select<EvaluatingPaperType>(sql, new SqlParameter("teacherId", teacherId),new SqlParameter("status",status));

        }
      

    }
}
