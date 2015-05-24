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
    [APIServer("V00015申请材料退回操作类")]
    public class ParperSendServer : Help<ParperSendType>
    {
        /// <summary>
        /// 创建申请退回消息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [APIServer("创建申请材料退回申请")]
        public ParperSendType SaveSend(ParperSendType type)
        {
            string sql = "update v00010 set disable=1 where paperId=@paperId";
            _help.ExecuteNonQuery(sql,new SqlParameter("paperId",type.PaperId));
            return base.Save(type);
        }
        [APIServer("获取需要打回的材料","获取是否处理过的试卷")]
        public List<ParperSendType> ListPaper(bool state)
        {
            string sql =
                @"select a.*,B.Name TeacherName, d.Name StudentName,c.TeachingPlan,c.Courseware,c.Reflection,c.Video,c.TypeName,c.TeachingName,c.CoursewareName,c.VideoName,C.ReflectionName from v00015 a join V00001 b on a.teacherId=b.Id join V00009 c on a.paperId=c.Id join V00001 d on c.Rid=d.Id
where a.state=@state";
            return _help.Select<ParperSendType>(sql,new SqlParameter("state",state));
        }
        /// <summary>
        /// 同意打回材料
        /// </summary>
        /// <param name="id"></param>
        [APIServer("同意打回材料","打回材料ID")]
        public void OkSend(int id)
        {

            string sql = @"declare @paperId int  select @paperId=paperId from v00015 where id=@id
delete v00012 where DId in(select id from V00010 where PaperId=@paperId)
delete V00010 where PaperId=@paperId
update V00009 set State=0 where Id=@paperId
update V00004 set DataState=0 where Rid in(select Rid from V00009 where id=@paperId)
";
             _help.ExecuteTrain(sql, new SqlParameter("id", id));
        }
        /// <summary>
        /// 打回材料
        /// </summary>
        /// <param name="paperId">试卷编号</param>
        [APIServer("根据试卷编号打回材料","试卷编号")]
        public void BackPaper(int paperId)
        {
            string sql = @"
delete v00012 where DId in(select id from V00010 where PaperId=@paperId)
delete V00010 where PaperId=@paperId
update V00009 set State=0 where Id=@paperId
update V00004 set DataState=0 where Rid in(select Rid from V00009 where id=@paperId)
";
            _help.ExecuteTrain(sql,new SqlParameter("paperId",paperId));
        }
    }
}
