using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ALOS.Expand;
using ALOS.Model;

namespace ALOS.DALSERVER
{
    /// <summary>
    /// V00011
    /// </summary>
    [APIServer("指标操作类")]
    public class PresentServer : Help<PresentType>
    {
        /// <summary>
        /// 根据试卷编号获得成绩结果
        /// </summary>
        /// <param name="paperId"></param>
        /// <returns></returns>
        public List<PresentType> SelectPaperList(int paperId)
        {
//            declare @paperId int
//set @paperId=1
            string sql = @"declare @count int
select @count=count(distinct  TeacherTypeId) from V00010 where PaperId=@paperId
select b.*,a.Grade Cgrade from (
select ZId,AVG(Grade) Grade from V00012 where did in(select id from V00010 where PaperId=@paperId and teacherTypeId>(case when @count=4 then 3 else 0 end)) group by  ZId) a join v00011 b on a.ZId=b.Id ";
            return _help.Select<PresentType>(sql, new SqlParameter("paperId", paperId));
        }
        /// <summary>
        /// 根据ID获得对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIServer("根据ID获得对象","ID")]
        public PresentType SelectId(int id)
        {
            return base.Reader(id);
        }
        [APIServer("根据ID删除对象","Id")]
        public void DeleteId(int id)
        {
            base.Delete(id);
        }
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [APIServer("保存对象","对象")]
        public PresentType SaveType(PresentType type)
        {
            return base.Save(type);
        }

        /// <summary>
        /// 根据指标类别获得指标
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<PresentType> SelectPresentList(EnumStudentDataType type,int did)
        {
            string sql = "select a.*,ISNULL(b.id,0) CID,ISNULL(b.grade,0.0)CGRADE from (select * from v00011 where typeid=@typeId and Version in(select id from V00016 where State=1)) a left join  v00012 b on a.id=b.zid and b.did=@Did order by orderId";
            return _help.Select<PresentType>(sql, new SqlParameter("typeid", type),new SqlParameter("did",did));
        }
        /// <summary>
        /// 获得所有指标
        /// </summary>
        /// <returns></returns>
        [APIServer("获得所有指标")]
        public List<PresentType> PresentList(int typeId)
        {
            return _help.Select<PresentType>("select * from v00011 where Version=@version order by id desc", new SqlParameter("version", typeId));
        }
        [APIServer("获得评语")]
        public List<PresentType> GetPY(int did)
        {
            string sql = @"select c.id,c.typeId,c.Name,(case 
when a.grade=0.95 then b.a 
when a.grade=0.85 then b.b 
when a.grade=0.75 then b.c 
when a.grade=0.65 then b.d 
when a.grade=0.55 then b.e else '' end
)Content from (select * from v00012 where did=@did) a join v00013 b on a.zid=b.zid join v00011 c on c.id=b.zid ";
            return _help.Select<PresentType>(sql, new SqlParameter("did", did));
           
        }
    }
}
