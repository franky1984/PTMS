using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-11 20:58
    /// 描 述：订单人员明细管理
    /// </summary>
    public class orderUsersDetailService : RepositoryFactory
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// <summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<LR_Base_TempUserEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.F_UserId,
                t.F_EmployerId,
                t.F_EmployerTypeId,
                t.F_RealName,
                t.F_Gender,
                t.F_Mobile
                ");

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                strSql.Append(" FROM LR_Base_TempUser t INNER JOIN F_Base_TempWorkOrderUserDetail o ON t.F_UserId = o.F_UserId ");
                strSql.Append( " WHERE 1=1 AND t.F_EnabledMark=1 " );

                dp.Add("F_OrderId", queryParam["F_OrderId"].ToString(), DbType.String);
                strSql.Append(" AND o.F_TempWorkOrderId = @F_OrderId ");

                if (!queryParam["F_EmployerId"].IsEmpty())
                {
                    dp.Add("F_EmployerId",queryParam["F_EmployerId"].ToString(), DbType.String);
                    strSql.Append(" AND t.F_EmployerId = @F_EmployerId ");
                }
                if (!queryParam["F_RealName"].IsEmpty())
                {
                    dp.Add("F_RealName", "%" + queryParam["F_RealName"].ToString().Trim() + "%", DbType.String);
                    strSql.Append(" AND t.F_RealName Like @F_RealName ");
                }
                if (!queryParam["F_Gender"].IsEmpty())
                {
                    dp.Add("F_Gender",queryParam["F_Gender"].ToString(), DbType.String);
                    strSql.Append(" AND t.F_Gender = @F_Gender ");
                }
                if (!queryParam["F_EmployerTypeId"].IsEmpty())
                {
                    dp.Add("F_EmployerTypeId",queryParam["F_EmployerTypeId"].ToString(), DbType.String);
                    strSql.Append( " AND o.F_CategoryId = @F_EmployerTypeId " );
                }

                return this.BaseRepository().FindList<LR_Base_TempUserEntity>(strSql.ToString(),dp, pagination);
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
        /// 获取LR_Base_TempUser表实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public LR_Base_TempUserEntity GetLR_Base_TempUserEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository().FindEntity<LR_Base_TempUserEntity>(keyValue);
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
        /// 删除实体数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="orderID"></param>
        public void DeleteEntity(string keyValue, string orderID)
        {
            try
            {
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                dp.Add("userid", keyValue, DbType.String);
                dp.Add("orderid", orderID, DbType.String);

                //物理删除临时工与订单关联细表
                this.BaseRepository().ExecuteBySql("DELETE F_Base_TempWorkOrderUserDetail WHERE f_userid=@userid AND F_TempWorkOrderId=@orderid",dp);

                int num = Convert.ToInt32( this.BaseRepository().FindObject( "SELECT COUNT(*) FROM F_Base_TempWorkOrderUserDetail WHERE f_userid=@userid AND F_TempWorkOrderId != @orderid", dp ) );

                //如果临时工在其它订单里没有出现过，那直接在用户大表中物理删除掉
                if( num <= 0 )
                {
                    var dp2 = new DynamicParameters( new
                    {
                    } );
                    dp2.Add( "userid", keyValue, DbType.String );

                    this.BaseRepository().ExecuteBySql( "DELETE LR_Base_TempUser WHERE f_userid=@userid",dp2 );
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
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public int SaveEntity(ref string keyValue, LR_Base_TempUserEntity entity, string orderID)
        {
            int checkError = 0;

            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    
                    // 虚拟参数
                    var dp = new DynamicParameters(new { });
                    dp.Add("userid", keyValue, DbType.String);
                    dp.Add("orderid", orderID, DbType.String);
                    dp.Add("type", entity.F_EmployerTypeId, DbType.String);

                    //修改该 临时工 在特定订单里的工种类型 
                    this.BaseRepository().ExecuteBySql("UPDATE F_Base_TempWorkOrderUserDetail SET F_CategoryId=@type WHERE F_TempWorkOrderId=@orderid AND f_userid=@userid", dp);

                    dp = new DynamicParameters( new{} );
                    dp.Add( "userid", keyValue, DbType.String );
                    dp.Add( "orderid", orderID, DbType.String );

                    entity.F_EmployerTypeId = string.Empty;
                    //修改临时工基本信息（大表）
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    var dp = new DynamicParameters( new
                    {
                    } );
                    dp.Add( "identity", entity.F_Identity, DbType.String );
                    //首先从大表获取新将要添加的小时工数据，用于判断该小时工是否以前已经存在
                    object userid           = this.BaseRepository().FindObject( "SELECT f_userid AS num FROM LR_Base_TempUser WHERE F_Identity=@identity", dp );
                    
                    dp = new DynamicParameters( new
                    {
                    } );
                    dp.Add( "orderID", orderID, DbType.String );
                    //根据订单ID获取劳务公司ID
                    entity.F_EmployerId     = this.BaseRepository().FindObject( "SELECT F_EmployerId FROM F_Base_TempWorkOrder WHERE f_orderid=@orderID", dp ).ToString();
                    string type             = entity.F_EmployerTypeId;
                    entity.F_EmployerTypeId = string.Empty;

                    //用身份证判断用户是否存在大表中，如果存在直接修改临时工表数据(因为用户基本信息是通用一样的，只有工种是挂在每个订单下)。
                    if (userid != null && (string)userid != string.Empty )
                    {
                        //entity.Modify( (string)userid );
                        
                        var dp2 = new DynamicParameters( new
                        {
                        } );
                        dp2.Add( "userID", (string)userid, DbType.String );
                        dp2.Add( "orderID", orderID, DbType.String );

                        //判断该小时工是否已在本次活动中
                        int num = Convert.ToInt32( this.BaseRepository().FindObject( "SELECT COUNT(*) FROM F_Base_TempWorkOrderUserDetail WHERE f_userid=@userID AND F_TempWorkOrderId=@orderID", dp2 ) );

                        if( num > 0 )
                        {
                            checkError = 2;  //代表该订单已经包含该临时工
                        }
//                        else
//                        {
//                            //修改临时工大表
//                            this.BaseRepository().Update( entity );
//                        }
                    }
                    else
                    {
                        entity.Create();
                        this.BaseRepository().Insert(entity);
                        userid   = entity.F_UserId;
                        keyValue = (string)userid;
                    }

                    if( checkError != 2 )
                    {
                        // 虚拟参数
                        dp = new DynamicParameters( new
                        {
                        } );

                        dp.Add( "f_id", Guid.NewGuid().ToString(), DbType.String );
                        dp.Add( "userid", (string)userid, DbType.String );
                        dp.Add( "orderid", orderID, DbType.String );
                        dp.Add( "type", type, DbType.String );
                        dp.Add( "employerid", entity.F_EmployerId, DbType.String );
                        dp.Add( "createUser", LoginUserInfo.Get().userId, DbType.String );
                        //往关联细表中添加临时工与订单关联
                        this.BaseRepository().ExecuteBySql( "INSERT F_Base_TempWorkOrderUserDetail(f_id,F_TempWorkOrderId,f_userid,f_employerid,f_categoryid,F_WorkSubstitute,F_CreateUser) VALUES(@f_id,@orderid,@userid,@employerid,@type,0,@createUser)", dp );

                        //获取订单的开始时间和结束时间
                        dp = new DynamicParameters( new
                        {
                        } );
                        dp.Add( "orderID", orderID, DbType.String );

                        DataTable dt       = this.BaseRepository().FindTable( "SELECT * FROM F_Base_TempWorkOrder WHERE f_orderid=@orderID", dp );
                        DateTime startTime = DateTime.Parse(dt.Rows[0]["F_StartTime"].ToString());
                        DateTime endTime   = DateTime.Parse(dt.Rows[0]["F_EndTime"].ToString());

                        for ( DateTime t = startTime; t <= endTime; t=t.AddDays(1) )
                        {
                            // 虚拟参数
                            var dp2 = new DynamicParameters(new
                            {
                            });

                            dp2.Add("F_RecordId", Guid.NewGuid().ToString(), DbType.String);
                            dp2.Add("F_Identity", entity.F_Identity, DbType.String);
                            dp2.Add("F_CreateTime", t, DbType.String);
                            dp2.Add("F_RealName", entity.F_RealName, DbType.String);
                            dp2.Add("F_Gender", entity.F_Gender, DbType.Int32);
                            dp2.Add("F_OrderId", orderID, DbType.String);
                            dp2.Add("F_RecordDate", t.ToString("yyyy-MM-dd"), DbType.String);

                            //自动给临时工往打卡记录里初始化打卡记录
                            this.BaseRepository().ExecuteBySql("INSERT LR_Base_CardRecord(F_RecordId,F_Identity,F_CreateTime,F_RealName,F_Gender,F_OrderId,F_RecordDate) VALUES(@F_RecordId,@F_Identity,@F_CreateTime,@F_RealName,@F_Gender,@F_OrderId,@F_RecordDate)", dp2 );
                        }
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

            return checkError;
        }

        #endregion

    }
}
