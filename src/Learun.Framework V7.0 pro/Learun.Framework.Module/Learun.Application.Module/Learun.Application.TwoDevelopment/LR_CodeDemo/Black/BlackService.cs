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
    /// 日 期：2018-11-08 10:39
    /// 描 述：黑名单管理
    /// </summary>
    public class BlackService : RepositoryFactory
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// <summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<F_Base_BlackListEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append( @"
                t.F_BlackId,
                t.F_Cause,
                u.F_RealName,
                t.F_CreateTime,
                employer.F_EmployerName
                " );
                strSql.Append( " FROM F_Base_BlackList t INNER JOIN LR_Base_User o ON t.F_CreateUser=o.f_userID INNER JOIN LR_Base_TempUser u ON t.F_TempUserId=u.F_UserId LEFT JOIN F_Base_Employer employer ON u.F_EmployerId=employer.F_EmployerId" );
                strSql.Append( "  WHERE o.F_CompanyId=@companyID AND 1=1 " );
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                dp.Add( "companyID", LoginUserInfo.Get().companyId, DbType.String );

                if (!queryParam["F_EmployerName"].IsEmpty())
                {
                    dp.Add("F_EmployerName", queryParam["F_EmployerName"].ToString(), DbType.String);
                    strSql.Append( " AND employer.F_EmployerId = @F_EmployerName " );
                }

                if (!queryParam["F_RealName"].IsEmpty())
                {
                    dp.Add("F_RealName", "%" + queryParam["F_RealName"].ToString() + "%", DbType.String);
                    strSql.Append(" AND u.F_RealName Like @F_RealName ");
                }
                return this.BaseRepository().FindList<F_Base_BlackListEntity>(strSql.ToString(),dp, pagination);
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
        /// 获取F_Base_BlackList表实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public F_Base_BlackListEntity GetF_Base_BlackListEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository().FindEntity<F_Base_BlackListEntity>(keyValue);
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
                // 虚拟参数
                var dp = new DynamicParameters( new
                {
                } );

                dp.Add( "id", keyValue, DbType.String );

                string userId = Convert.ToString( this.BaseRepository().FindObject("SELECT f_tempuserid FROM f_base_blacklist WHERE f_blackid=@id", dp ) );
                this.BaseRepository().Delete<F_Base_BlackListEntity>(t=>t.F_BlackId == keyValue);
                //修改用户状态为有效状态
                this.BaseRepository().ExecuteBySql( "UPDATE LR_Base_TempUser SET F_EnabledMark=1 WHERE F_UserId='" + userId + "'" );
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
        public void SaveEntity(string keyValue, F_Base_BlackListEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
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

    }
}
