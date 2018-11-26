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
    /// 日 期：2018-11-08 15:16
    /// 描 述：打卡统计
    /// </summary>
    public class cardTJService : RepositoryFactory
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// <summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<LR_Base_CardTJEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append( @"
                SELECT t.F_MeetingName,e.F_EmployerName,u.F_RealName,u.F_Identity,type.F_CategoryName,t.F_StartTime,t.F_EndTime,
 (SELECT COUNT(*) AS num FROM LR_Base_CardRecord WHERE F_OrderId=t.F_OrderId AND F_RealName=o.f_userid AND f_lateState=0 AND f_LeaveEarly=0) as f_normal,
 (SELECT COUNT(*) AS num FROM LR_Base_CardRecord WHERE F_OrderId=t.F_OrderId AND F_RealName=o.f_userid AND f_lateState=1) as f_late,
(SELECT COUNT(*) AS num FROM LR_Base_CardRecord WHERE F_OrderId=t.F_OrderId AND F_RealName=o.f_userid AND f_LeaveEarly=1) as f_leave,
 (SELECT COUNT(*) AS num FROM LR_Base_CardRecord WHERE F_OrderId=t.F_OrderId AND F_RealName=o.f_userid AND f_first IS NULL) as f_absenteeism 
FROM F_Base_TempWorkOrder t LEFT JOIN F_Base_TempWorkOrderUserDetail o ON t.f_orderid=o.F_TempWorkOrderId 
LEFT JOIN F_Base_Employer e ON o.F_EmployerId=e.F_EmployerId LEFT JOIN LR_Base_TempUser u ON o.f_userid=u.f_userid LEFT JOIN F_Base_EmployerType type ON o.f_categoryId=type.f_Id" );
                strSql.Append( "  WHERE GETDATE() >= dateadd(day,1,t.F_EndTime) AND 1=1 " );

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                if (!queryParam[ "F_OrderId" ].IsEmpty())
                {
                    dp.Add( "F_OrderId", queryParam[ "F_OrderId" ].ToString(), DbType.String);
                    strSql.Append( " AND t.F_OrderId = @F_OrderId " );
                }
                return this.BaseRepository().FindList<LR_Base_CardTJEntity>(strSql.ToString(),dp, pagination);
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
        /// 获取LR_Base_CardTJ表实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public LR_Base_CardTJEntity GetLR_Base_CardTJEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository().FindEntity<LR_Base_CardTJEntity>(keyValue);
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
                this.BaseRepository().Delete<LR_Base_CardTJEntity>(t=>t.f_id == keyValue);
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
        public void SaveEntity(string keyValue, LR_Base_CardTJEntity entity)
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
