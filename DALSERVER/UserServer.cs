using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ALOS.Expand;
using ALOS.Model;


namespace ALOS.DALSERVER
{
    [APIServer("用户表操作类V00001")]
    public class UserServer:Help<UserType>
    {
        /// <summary>
        /// 用户登录方法
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserType UserLogin(string loginId,string password)
        {
             
            string sql = "if exists(select 1 from V00001 where loginid=@loginid and password=@password) begin update v00001 set loginDate=getdate(),userGuid=newId() where loginid=@loginid and password=@password   select * from V00001 where loginid=@loginid and password=@password end else begin select * from V00001 where 1=2 end";
            return _help.Reader<UserType>(sql, new SqlParameter("loginid", loginId),
                new SqlParameter("password", password.SHA512_Encrypt()));
        }
        /// <summary>
        /// 根据用户Guid登录
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [APIServer("用户guid登录方式","guid字符串")]
        public UserType UserGuidLogin(string guid)
        {
            string sql = "select * from v00001 where userguid=@guid";
            return _help.Reader<UserType>(sql, new SqlParameter("guid", guid));
        }
        [APIServer("查询用户详细信息","用户对象")]
        public UserType SelectUser(UserType type)
        {
            return _help.Reader<UserType>("select * from v00001 where id=@id",new SqlParameter("id",type.Id));
        }
        /// <summary>
        /// 根据用户编号获得用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIServer("根据用户编号获得用户信息","id用户编号")]
        public UserType Reader(int id)
        {
            return base.Reader(id);
        }
        /// <summary>
        /// 根据用户编号删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIServer("根据用户编号删除用户信息","用户编号")]
        public string DeleteUser(int id)
        {
            base.Delete(id);
            return "用户信息删除成功";
        }
        /// <summary>
        /// 判断当前登录帐号是否有数据
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public bool SelectUser(string loginId)
        {
            string sql = "select count(1) from v00001 where loginId=@loginId";
            return _help.ExecuteScalar<int>(sql, new SqlParameter("loginId", loginId)) > 0;
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [APIServer("保存用户信息","用户对象")]
        public UserType SaveUser(UserType user)
        {
        
            if (user.Id == 0)
            {
               var ruser=_help.Reader<UserType>("select * from V00001 where loginid=@loginid", new SqlParameter("loginid", user.LoginId));
                if (ruser != null)
                {
                  
                    return new UserType();
                }
             
            }
            if (user.Password.Length != 128)
            {
                user.Password = user.Password.SHA512_Encrypt();
            }
            if (user.UserGuid == Guid.Empty)
            {
                user.UserGuid = Guid.NewGuid();
            }
            return base.Save(user);
            
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [APIServer("测试方法是否调用成功","参数一,参数二")]
        public string Test(int a,int b)
        {
            return "测试"+a+":"+b;
        }
        [APIServer("获得所有的用户资料")]
        public override List<UserType> List()
        {
            string sql = "select * from V00001 ";
            return _help.Select<UserType>(sql);
        }
      
        /// <summary>
        /// 返回所有的数据条数
        /// </summary>
        /// <returns></returns>
        [APIServer("根据搜索获得所有的数据条数","搜索值")]
        public int Count(string value,int typeId,int schoolId,int collegeId)
        {
            string sql = "";

            sql = @"select * from (select * from V00001 a left join(
select Rid,School,SchoolId,College,CollegeId,Email,Phone,0 TypeId from V00004 union
select Rid,School,SchoolId,CollegeName,CollegeId,Email,Phone,TypeId from v00005) b on  a.Id=b.Rid)V00001 where (LoginId like '%'+@value+'%' or Name like '%'+@value+'%') and(usertype=@userType or @userType=0) and(@schoolId=0 or schoolId=@schoolId) and(@collegeId=0 or collegeId=@collegeId) and (@TypeId=0 or typeId=@typeId) ";
            var tempType = 0;
            if (typeId > 3)
            {
                tempType = typeId - 3;
                typeId = 2;
            }
            return SQLCount(sql, 20, new SqlParameter("value", value), new SqlParameter("userType", typeId), new SqlParameter("TypeId", tempType), new SqlParameter("collegeId", collegeId), new SqlParameter("schoolId", schoolId));
        }

        /// <summary>
        /// 获取每页20条数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <param name="typeId"></param>
        /// <param name="schoolId"></param>
        /// <param name="collegeId"></param>
        /// <returns></returns>
        [APIServer(" 获取每页20条数据","搜索值,下标")]
        public List<UserType> ListPageUser(string value, int index, int typeId, int schoolId, int collegeId)
        {
            //string sql ="select * from V00001 where LoginId like '%'+@value+'%' or Name like '%'+@value+'%'";
            //return SqlList<UserType>(sql, 20, index, new SqlParameter("value", value));


            string sql = "";

            sql = @"select * from (select * from V00001 a left join(
select Rid,School,SchoolId,College,CollegeId,Email,Phone,0 TypeId from V00004 union
select Rid,School,SchoolId,CollegeName,CollegeId,Email,Phone,TypeId from v00005) b on  a.Id=b.Rid)V00001 where (LoginId like '%'+@value+'%' or Name like '%'+@value+'%') and(usertype=@userType or @userType=0) and(@schoolId=0 or schoolId=@schoolId) and(@collegeId=0 or collegeId=@collegeId) and (@TypeId=0 or typeId=@typeId) ";
            var tempType = 0;
            if (typeId > 3)
            {
                tempType = typeId - 3;
                typeId = 2;
            }
            return SqlList<UserType>(sql, 20, index, new SqlParameter("value", value), new SqlParameter("userType", typeId),new SqlParameter("TypeId",tempType),new SqlParameter("collegeId",collegeId),new SqlParameter("schoolId",schoolId));
        }

    }
}
