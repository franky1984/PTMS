using Learun.DataBase;
using Learun.DataBase.Repository;
using Learun.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;

namespace Learun.Application.Base.SystemModule
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创建人：众数-框架开发组
    /// 日 期：2017.03.08
    /// 描 述：数据库连接
    /// </summary>
    public class DatabaseLinkService : RepositoryFactory
    {
        #region 构造函数和属性
        private string fieldSql;
        public DatabaseLinkService()
        {
            fieldSql = @"
                    t.F_DatabaseLinkId,
                    t.F_ServerAddress,
                    t.F_DBName,
                    t.F_DBAlias,
                    t.F_DbType,
                    t.F_DbConnection,
                    t.F_DESEncrypt,
                    t.F_SortCode,
                    t.F_DeleteMark,
                    t.F_EnabledMark,
                    t.F_Description,
                    t.F_CreateDate,
                    t.F_CreateUserId,
                    t.F_CreateUserName,
                    t.F_ModifyDate,
                    t.F_ModifyUserId,
                    t.F_ModifyUserName
                ";
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取自定义查询条件（公共）
        /// </summary>
        /// <param name="moduleUrl">访问的功能链接地址</param>
        /// <param name="userId">用户ID（用户ID为null表示公共）</param>
        /// <returns></returns>
        public IEnumerable<DatabaseLinkEntity> GetList()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM LR_Base_DatabaseLink t WHERE  t.F_DeleteMark = 0 ");
                return this.BaseRepository().FindList<DatabaseLinkEntity>(strSql.ToString());
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除自定义查询条件
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void VirtualDelete(string keyValue)
        {
            try
            {
                DatabaseLinkEntity entity = new DatabaseLinkEntity()
                {
                    F_DatabaseLinkId = keyValue,
                    F_DeleteMark = 1
                };
                this.BaseRepository().Update(entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 保存自定义查询（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="departmentEntity">部门实体</param>
        /// <returns></returns>
        public bool SaveEntity(string keyValue, DatabaseLinkEntity databaseLinkEntity)
        {
            try
            {
                /*测试数据库连接串"******";*/
                if (!string.IsNullOrEmpty(keyValue) && databaseLinkEntity.F_DbConnection == "******")
                {
                    databaseLinkEntity.F_DbConnection = null;// 不更新连接串地址
                }
                else
                {
                    try
                    {
                        databaseLinkEntity.F_ServerAddress = this.BaseRepository(databaseLinkEntity.F_DbConnection, databaseLinkEntity.F_DbType).getDbConnection().DataSource;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                if (!string.IsNullOrEmpty(keyValue))
                {
                    databaseLinkEntity.Modify(keyValue);
                    this.BaseRepository().Update(databaseLinkEntity);
                }
                else
                {
                    databaseLinkEntity.Create();
                    this.BaseRepository().Insert(databaseLinkEntity);
                }
                return true;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        #endregion

        #region 扩展方法
        /// <summary>
        /// 测试数据数据库是否能连接成功
        /// </summary>
        /// <param name="connection">连接串</param>
        /// <param name="dbType">数据库类型</param>
        public bool TestConnection(string connection, string dbType)
        {
            try
            {
                var db = this.BaseRepository(connection, dbType).BeginTrans();
                db.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 根据指定数据库执行sql语句
        /// </summary>
        /// <param name="entity">数据库实体</param>
        /// <param name="sql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        public void ExecuteBySql(DatabaseLinkEntity entity, string sql, object dbParameter)
        {
            try
            {
                if (dbParameter is JObject)
                {
                    var paramter = SqlHelper.JObjectToParameter((JObject)dbParameter);
                    this.BaseRepository(entity.F_DbConnection, entity.F_DbType).ExecuteBySql(sql, paramter);
                }
                else if (dbParameter is List<FieldValueParam>)
                {
                    var paramter = SqlHelper.FieldValueParamToParameter((List<FieldValueParam>)dbParameter);
                    this.BaseRepository(entity.F_DbConnection, entity.F_DbType).ExecuteBySql(sql, paramter);
                }
                else
                {
                    this.BaseRepository(entity.F_DbConnection, entity.F_DbType).ExecuteBySql(sql, dbParameter);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 判断用户是否在黑名单
        /// </summary>
        /// <param name="Identity"></param>
        public int CheckBlack(string identity)
        {
            string userid = Convert.ToString(this.BaseRepository().FindObject("SELECT f_userid FROM LR_Base_TempUser WHERE F_Identity='" + identity.Trim() + "'"));
            return Convert.ToInt32(this.BaseRepository().FindObject("SELECT COUNT(*) num FROM F_Base_BlackList WHERE F_TempUserId='" + userid + "'"));
        }

        /// <summary>
        /// 往临时工大表中导入数据  by ChengQing
        /// </summary>
        /// <param name="entity">数据库实体</param>
        /// <param name="sql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <param name="identity">身份证号</param>
        public void ExecuteBySql(DatabaseLinkEntity entity, string sql, object dbParameter, string identity, string orderID, string realName = "", string categoryID = "", string gender = "", string employerID = "", string mobile = "")
        {
            try
            {
                if (dbParameter is JObject)
                {
                    var paramter = SqlHelper.JObjectToParameter((JObject)dbParameter);
                    this.BaseRepository(entity.F_DbConnection, entity.F_DbType).ExecuteBySql(sql, paramter);
                }
                else if (dbParameter is List<FieldValueParam>)
                {
                    var paramter = SqlHelper.FieldValueParamToParameter((List<FieldValueParam>)dbParameter);
                    this.BaseRepository(entity.F_DbConnection, entity.F_DbType).ExecuteBySql(sql, paramter);
                }
                else
                {
                    this.BaseRepository(entity.F_DbConnection, entity.F_DbType).ExecuteBySql(sql, dbParameter);
                    InsertOrderDetail( identity, orderID, realName, categoryID, gender, employerID, mobile );
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 添加订单细表 by ChengQing
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="orderID"></param>
        /// <param name="realName"></param>
        /// <param name="categoryID"></param>
        /// <param name="gender"></param>
        /// <param name="employerID"></param>
        /// <param name="mobile"></param>
        public void InsertOrderDetail(string identity, string orderID, string realName, string categoryID, string gender, string employerID, string mobile )
        {
            //通过身份证号获取人员ID和所属用户单位ID
            DataTable dt = this.BaseRepository().FindTable("SELECT F_UserId,F_EmployerId FROM LR_Base_TempUser WHERE F_Identity='" + identity.Trim() + "'");

            var dp = new DynamicParameters(new
            {
            });

            dp.Add("orderID", orderID, DbType.String);
            dp.Add("f_userid", dt.Rows[0]["F_UserId"].ToString(), DbType.String);

            int num = Convert.ToInt32(this.BaseRepository().FindObject("SELECT COUNT(*) num FROM F_Base_TempWorkOrderUserDetail WHERE F_TempWorkOrderId=@orderID AND f_userid=@f_userid", dp));

            //判断该人员是否已和订单关联
            if ( num <= 0 )
            {
                dp = new DynamicParameters(new
                {
                });

                dp.Add("f_id", Guid.NewGuid().ToString(), DbType.String);
                dp.Add("F_TempWorkOrderId", orderID, DbType.String);
                dp.Add("f_userid", dt.Rows[0]["F_UserId"].ToString(), DbType.String);
                dp.Add("F_EmployerId", dt.Rows[0]["F_EmployerId"].ToString(), DbType.String);
                dp.Add("F_CategoryId", categoryID, DbType.String);
                dp.Add("createUser", LoginUserInfo.Get().userId, DbType.String);

                this.BaseRepository().ExecuteBySql( "INSERT F_Base_TempWorkOrderUserDetail(f_id,F_TempWorkOrderId,f_userid,F_EmployerId,F_CategoryId,F_CreateUser,F_WorkSubstitute,F_CheckBlack) VALUES(@f_id,@F_TempWorkOrderId,@f_userid,@F_EmployerId,@F_CategoryId,@createUser,0,0)", dp );

                //获取订单的开始时间和结束时间
                dt                 = this.BaseRepository().FindTable("SELECT F_StartTime,F_EndTime FROM F_Base_TempWorkOrder WHERE f_orderid='" + orderID + "'");
                DateTime startTime = DateTime.Parse(dt.Rows[0]["F_StartTime"].ToString());
                DateTime endTime   = DateTime.Parse(dt.Rows[0]["F_EndTime"].ToString());

                for ( DateTime t = startTime; t <= endTime; t = t.AddDays(1) )
                {
                    // 虚拟参数
                    var dp3 = new DynamicParameters(new
                    {
                    });

                    dp3.Add("F_RecordId", Guid.NewGuid().ToString(), DbType.String);
                    dp3.Add("F_Identity", identity, DbType.String);
                    dp3.Add("F_CreateTime", t, DbType.String);
                    dp3.Add("F_RealName", realName, DbType.String);
                    dp3.Add("F_Gender", gender, DbType.Int32);
                    dp3.Add("F_OrderId", orderID, DbType.String);
                    dp3.Add("F_RecordDate", t.ToString("yyyy-MM-dd"), DbType.String);

                    //自动给临时工往打卡记录里初始化打卡记录
                    this.BaseRepository().ExecuteBySql("INSERT LR_Base_CardRecord(F_RecordId,F_Identity,F_CreateTime,F_RealName,F_Gender,F_OrderId,F_RecordDate) VALUES(@F_RecordId,@F_Identity,@F_CreateTime,@F_RealName,@F_Gender,@F_OrderId,@F_RecordDate)", dp3);
                }
            }

            var dp2 = new DynamicParameters(new
            {
            });

            dp2.Add("realName", realName, DbType.String);
            dp2.Add("employerID", employerID, DbType.String);
            dp2.Add("gender", gender, DbType.String);
            dp2.Add("mobile", mobile, DbType.String);
            dp2.Add("userid", dt.Rows[0]["F_UserId"].ToString(), DbType.String);
            //修改临时工大表
            this.BaseRepository().ExecuteBySql( "UPDATE LR_Base_TempUser SET F_RealName=@realName,F_EmployerId=@employerID,F_Gender=@gender,F_Mobile=@mobile WHERE f_userid=@userid", dp2 );
        }

        /// <summary>
        /// 根据数据库执行sql语句,查询数据->datatable
        /// </summary>
        /// <param name="entity">数据库实体</param>
        /// <param name="sql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        public DataTable FindTable(DatabaseLinkEntity entity, string sql, object dbParameter)
        {
            try
            {

                if (dbParameter is JObject)
                {
                    var paramter = SqlHelper.JObjectToParameter((JObject)dbParameter);
                    return this.BaseRepository(entity.F_DbConnection, entity.F_DbType).FindTable(sql, paramter);
                }
                else if (dbParameter is List<FieldValueParam>)
                {
                    var paramter = SqlHelper.FieldValueParamToParameter((List<FieldValueParam>)dbParameter);
                    return this.BaseRepository(entity.F_DbConnection, entity.F_DbType).FindTable(sql, paramter);
                }
                else
                {
                    return this.BaseRepository(entity.F_DbConnection, entity.F_DbType).FindTable(sql, dbParameter);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 根据数据库执行sql语句,查询数据->datatable（分页）
        /// </summary>
        /// <param name="entity">数据库实体</param>
        /// <param name="sql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public DataTable FindTable(DatabaseLinkEntity entity, string sql, object dbParameter, Pagination pagination)
        {
            try
            {

                if (dbParameter is JObject)
                {
                    var paramter = SqlHelper.JObjectToParameter((JObject)dbParameter);
                    return this.BaseRepository(entity.F_DbConnection, entity.F_DbType).FindTable(sql, paramter, pagination);
                }
                else if (dbParameter is List<FieldValueParam>)
                {
                    var paramter = SqlHelper.FieldValueParamToParameter((List<FieldValueParam>)dbParameter);
                    return this.BaseRepository(entity.F_DbConnection, entity.F_DbType).FindTable(sql, paramter, pagination);
                }
                else
                {

                    return this.BaseRepository(entity.F_DbConnection, entity.F_DbType).FindTable(sql, dbParameter, pagination);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        #region 事务执行
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="entity">数据库连接信息</param>
        /// <returns></returns>
        public IRepository BeginTrans(DatabaseLinkEntity entity)
        {
            try
            {
                return this.BaseRepository(entity.F_DbConnection, entity.F_DbType).BeginTrans();
            }
            catch (Exception ex)
            {

                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 根据指定数据库执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        public void ExecuteBySqlTrans(string sql, object dbParameter, IRepository db)
        {
            try
            {
                if (dbParameter is JObject)
                {
                    var paramter = SqlHelper.JObjectToParameter((JObject)dbParameter);
                    if (db != null)
                    {
                        db.ExecuteBySql(sql, paramter);
                    }
                }
                else if (dbParameter is List<FieldValueParam>)
                {
                    var paramter = SqlHelper.FieldValueParamToParameter((List<FieldValueParam>)dbParameter);
                    if (db != null)
                    {
                        db.ExecuteBySql(sql, paramter);
                    }
                }
                else
                {
                    if (db != null)
                    {
                        db.ExecuteBySql(sql, dbParameter);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        #endregion

        #endregion
    }
}
