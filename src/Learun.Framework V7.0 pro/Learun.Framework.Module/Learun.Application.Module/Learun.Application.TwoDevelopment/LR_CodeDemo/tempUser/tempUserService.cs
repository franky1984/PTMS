using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-07 15:08
    /// 描 述：临时工管理
    /// </summary>
    public class tempUserService : RepositoryFactory
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
                strSql.Append( @"
                t.F_UserId  AS F_UserId,
                t.F_OrderId,
                t.F_RealName,
                t.F_EmployerId,
                t.F_EmployerTypeId,
                t.F_Mobile,
                t.F_Identity,
                t.F_Gender,t.F_WorkSubstitute,u.F_RealName AS F_Replacement 
                " );

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                if( !queryParam[ "F_OrderId" ].IsEmpty() )
                {
                    strSql.Append( " FROM LR_Base_TempUser t RIGHT JOIN F_Base_TempWorkOrderUserDetail o ON t.f_userid=o.f_userid LEFT JOIN LR_Base_TempUser u ON t.F_Replacement=u.f_userid " );
                    strSql.Append( " WHERE 1=1 AND (t.F_EnabledMark=1 OR t.F_EnabledMark IS NULL) " );

                    dp.Add( "f_orderid", queryParam[ "F_OrderId" ].ToString(), DbType.String );
                    strSql.Append( " AND o.F_TempWorkOrderId = @f_orderid " );
                }
                else
                {
                    strSql.Append( " FROM LR_Base_TempUser t LEFT JOIN LR_Base_TempUser u ON t.F_Replacement=u.f_userid " );
                    strSql.Append( " WHERE 1=1 AND F_EnabledMark=1 " );
                }

                if (!queryParam["F_RealName"].IsEmpty())
                {
                    dp.Add("F_RealName", "%" + queryParam["F_RealName"].ToString() + "%", DbType.String);
                    strSql.Append(" AND t.F_RealName Like @F_RealName ");
                }
                if (!queryParam["F_Gender"].IsEmpty())
                {
                    dp.Add("F_Gender",queryParam["F_Gender"].ToString(), DbType.String);
                    strSql.Append(" AND t.F_Gender = @F_Gender ");
                }
                if (!queryParam["F_EmployerId"].IsEmpty())
                {
                    dp.Add("F_EmployerId",queryParam["F_EmployerId"].ToString(), DbType.String);
                    strSql.Append(" AND t.F_EmployerId = @F_EmployerId ");
                }
                if (!queryParam["F_EmployerTypeId"].IsEmpty())
                {
                    dp.Add("F_EmployerTypeId",queryParam["F_EmployerTypeId"].ToString(), DbType.String);
                    strSql.Append(" AND t.F_EmployerTypeId = @F_EmployerTypeId ");
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
        /// 判断用户是否已加入黑名单
        /// </summary>
        /// <param name="keyValue"></param>
        public string CheckUserBlack( string keyValue )
        {
            try
            {
                var dp = new DynamicParameters( new
                {
                } );

                dp.Add( "userID", keyValue, DbType.String );
                return Convert.ToString( this.BaseRepository().FindObject( "SELECT COUNT(*) AS num FROM F_Base_BlackList WHERE F_TempUserId=@userID", dp ) );
            }
            catch( Exception ex )
            {
                if( ex is ExceptionEx )
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException( ex );
                }
            }
        }

        /// <summary>
        /// 将临时工添加到黑名单
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void AddBlack( string keyValue, F_Base_BlackListEntity entity )
        {
            try
            {
                entity.Create();
                entity.F_TempUserId = keyValue;
                //如果将用户加入黑名单，同时该用户在用户表中代表无效状态 
                this.BaseRepository().ExecuteBySql( "UPDATE LR_Base_TempUser SET F_EnabledMark=0 WHERE F_UserId='" + keyValue + "'" );
                //往黑名单表中插入一条记录
                this.BaseRepository().Insert( entity );
            }
            catch( Exception ex )
            {
                if( ex is ExceptionEx )
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException( ex );
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
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void DeleteEntity(string keyValue)
        {
            try
            {
                this.BaseRepository().Delete<LR_Base_TempUserEntity>(t=>t.F_UserId == keyValue);
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
        public void SaveEntity(string keyValue, LR_Base_TempUserEntity entity)
        {
            try
            {
                string userid = keyValue;

                if (!string.IsNullOrEmpty( userid ) )
                {
                    entity.Modify( userid );
                    this.BaseRepository().Update(entity);

                    var dp = new DynamicParameters( new
                    {
                    } );

                    dp.Add( "orderid", entity.F_OrderId, DbType.String );
                    dp.Add( "userid", entity.F_UserId, DbType.String );
                    dp.Add( "F_EmployerId", entity.F_EmployerId, DbType.String );
                    dp.Add( "F_CategoryId", entity.F_EmployerTypeId, DbType.String );

                    //插入临时工的同时往订单与用户细表中插入一条记录（关联订单）
                    this.BaseRepository().ExecuteBySql( "UPDATE F_Base_TempWorkOrderUserDetail SET F_TempWorkOrderId=@orderid,F_EmployerId=@F_EmployerId,F_CategoryId=@F_CategoryId WHERE F_UserId=@userid", dp );
                }
                else
                {
                    DataTable dt =  this.BaseRepository().FindTable( "SELECT F_UserId FROM LR_Base_TempUser WHERE F_Identity='" + entity.F_Identity + "'" );
                    entity.Create();

                    //根据身份证号判断该人是否已在DB中，如果存在就直接往订单与用户细表中插入一条记录
                    if( dt != null && dt.Rows != null && dt.Rows.Count > 0 )
                    {
                        entity.F_UserId = dt.Rows[0][ "F_UserId" ].ToString();
                    }
                    else
                    {
                        this.BaseRepository().Insert(entity);
                    }

                    var dp = new DynamicParameters( new
                    {
                    } );

                    dp.Add( "f_id", Guid.NewGuid().ToString(), DbType.String );
                    dp.Add( "orderid", entity.F_OrderId, DbType.String );
                    dp.Add( "userid", entity.F_UserId, DbType.String );
                    dp.Add( "F_EmployerId", entity.F_EmployerId, DbType.String );
                    dp.Add( "F_CategoryId", entity.F_EmployerTypeId, DbType.String );

                    //插入临时工的同时往订单与用户细表中插入一条记录（关联订单）
                    this.BaseRepository().ExecuteBySql( "INSERT F_Base_TempWorkOrderUserDetail(f_id,F_TempWorkOrderId,F_UserId,F_EmployerId,F_CategoryId,F_WorkSubstitute) VALUES(@f_id,@orderid,@userid,@F_EmployerId,@F_CategoryId,0)", dp );
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
        /// 保存替工数据（新增、修改）
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="orderID"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int WorkSubstituteSaveForm( string keyValue,string orderID, LR_Base_TempUserEntity entity )
        {
            int checkError = 0;

            try
            {
                //根据替工者的身份证获取他在表中主键
                object userid = this.BaseRepository().FindObject( "SELECT f_userid AS num FROM LR_Base_TempUser WHERE F_Identity='" + entity.F_Identity + "'" );

                var dp = new DynamicParameters( new
                {
                } );

                dp.Add( "orderID", orderID, DbType.String );

                //根据订单ID获取用人单位ID
                entity.F_EmployerId = this.BaseRepository().FindObject( "SELECT F_EmployerId AS employerID FROM F_Base_TempWorkOrder WHERE f_orderid=@orderID", dp ).ToString();

                //用身份证判断用户是否存在，如果存在直接修改临时工表数据(因为用户基本信息是通用一样的，只有工种是挂在每个订单下)。
                if( userid != null && (string)userid != string.Empty )
                {
                    entity.Modify( (string)userid );

                    // 虚拟参数
                    var dp2 = new DynamicParameters( new
                    {
                    } );
                    dp2.Add( "userID", (string)userid, DbType.String );
                    dp2.Add( "orderID", orderID, DbType.String );

                    int num = Convert.ToInt32( this.BaseRepository().FindObject( "SELECT COUNT(*) FROM F_Base_TempWorkOrderUserDetail WHERE f_userid=@userID AND F_TempWorkOrderId=@orderID", dp2 ) );

                    //判断替工人是否在订单中
                    if( num > 0 )
                    {
                        checkError = 2;  //代表该订单已经包含该临时工
                    }
                    else
                    {
                        this.BaseRepository().Update( entity );
                    }
                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert( entity );
                    userid = entity.F_UserId;
                }

                if( checkError != 2 )
                {
                    // 虚拟参数
                    dp = new DynamicParameters( new
                    {
                    } );
                    dp.Add( "userid", (string)userid, DbType.String );
                    dp.Add( "orderid", orderID, DbType.String );

                    string type = Convert.ToString( this.BaseRepository().FindObject( "SELECT F_CategoryId FROM F_Base_TempWorkOrderUserDetail WHERE F_TempWorkOrderId='" + orderID + "' AND F_UserId='" + keyValue + "'" ) );
                    dp.Add( "type", type, DbType.String );

                    dp.Add( "employerid", entity.F_EmployerId, DbType.String );
                    dp.Add( "F_Replacement", keyValue, DbType.String );
                    dp.Add( "createUser", LoginUserInfo.Get().userId, DbType.String );
                    //往关联细表中添加临时工与订单关联
                    this.BaseRepository().ExecuteBySql( "INSERT F_Base_TempWorkOrderUserDetail(F_TempWorkOrderId,f_userid,f_employerid,f_categoryid,F_WorkSubstitute,F_CreateUser,F_Replacement) VALUES(@orderid,@userid,employerid,@type,1,@createUser,@F_Replacement)", dp );
                }
            }
            catch( Exception ex )
            {
                if( ex is ExceptionEx )
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException( ex );
                }
            }

            return checkError;
        }

        /// <summary>
        /// 判断被替工人是否在此次活动里
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="keyValue"></param>
        public int CheckReplacement( string orderID, string keyValue )
        {
            string userid = keyValue;
            int num = Convert.ToInt32( this.BaseRepository().FindObject( "SELECT COUNT(*) AS num FROM F_Base_TempWorkOrderUserDetail WHERE F_TempWorkOrderId='" + orderID + "' AND F_UserId='" + userid + "'" ) );
            return num;
        }
        #endregion

    }
}
