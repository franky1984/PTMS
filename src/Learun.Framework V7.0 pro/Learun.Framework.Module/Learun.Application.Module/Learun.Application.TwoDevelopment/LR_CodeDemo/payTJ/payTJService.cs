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
    /// 日 期：2018-11-09 13:47
    /// 描 述：薪资核算统计
    /// </summary>
    public class payTJService : RepositoryFactory
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// <summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<LR_Base_TJ2Entity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                //只统计订单结束的
                var strSql = new StringBuilder();
                strSql.Append( @"
               SELECT t.F_MeetingName,e.F_EmployerName,(SELECT COUNT(*) FROM F_Base_TempWorkOrderUserDetail WHERE F_TempWorkOrderId=t.f_orderid) as sumPeople,
t.F_StartTime,t.F_EndTime,(SELECT COUNT(*) AS num FROM LR_Base_CardRecord WHERE F_OrderId=t.f_orderid GROUP BY CONVERT(varchar(10), F_CreateTime, 23)) as sumWorkday,
 (SELECT ISNULL(SUM(f_realDaySalary),0) FROM LR_Base_CardRecord WHERE f_orderid=t.F_OrderId) as f_realDaySalary,
  (SELECT ISNULL(SUM(f_shouldDaysalary),0) FROM LR_Base_CardRecord WHERE f_orderid=t.F_OrderId) as f_shouldDaysalary FROM F_Base_TempWorkOrder t INNER JOIN LR_Base_User u ON t.F_CreateUser=u.f_userID LEFT JOIN F_Base_Employer e ON t.F_EmployerId=e.F_EmployerId WHERE u.F_CompanyId=@companyID ANDGETDATE() >= dateadd(day,1,t.F_EndTime) AND 1=1 " );

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                dp.Add( "companyID", LoginUserInfo.Get().companyId, DbType.String );

//                if( !queryParam[ "F_OrderId" ].IsEmpty() )
//                {
//                    dp.Add( "F_OrderId", queryParam[ "F_OrderId" ].ToString(), DbType.String );
//                    strSql.Append( " AND t.F_OrderId = @F_OrderId " );
//                }
                return this.BaseRepository().FindList<LR_Base_TJ2Entity>(strSql.ToString(),dp, pagination);
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

        public DataTable GetExportList()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append( @"
               SELECT t.F_MeetingName,e.F_EmployerName,(SELECT COUNT(*) FROM F_Base_TempWorkOrderUserDetail WHERE F_TempWorkOrderId=t.f_orderid) as sumPeople,
t.F_StartTime,t.F_EndTime,(SELECT COUNT(*) AS num FROM LR_Base_CardRecord WHERE F_OrderId=t.f_orderid GROUP BY CONVERT(varchar(10), F_CreateTime, 23)) as sumWorkday,
 (SELECT ISNULL(SUM(f_realDaySalary),0) FROM LR_Base_CardRecord WHERE f_orderid=t.F_OrderId AND F_RealName=o.f_userid) as f_realDaySalary,
  (SELECT ISNULL(SUM(f_shouldDaysalary),0) FROM LR_Base_CardRecord WHERE f_orderid=t.F_OrderId AND F_RealName=o.f_userid) as f_shouldDaysalary FROM F_Base_TempWorkOrder t 
LEFT JOIN F_Base_TempWorkOrderUserDetail o ON t.f_orderid=o.F_TempWorkOrderId 
LEFT JOIN F_Base_Employer e ON o.F_EmployerId=e.F_EmployerId WHERE GETDATE()>dateadd(day,1,T.F_EndTime) AND 1=1 
                " );
                return this.BaseRepository().FindTable( strSql.ToString() );
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
        /// 获取LR_Base_TJ2表实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public LR_Base_TJ2Entity GetLR_Base_TJ2Entity(string keyValue)
        {
            try
            {
                return this.BaseRepository().FindEntity<LR_Base_TJ2Entity>(keyValue);
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
                this.BaseRepository().Delete<LR_Base_TJ2Entity>(t=>t.f_id == keyValue);
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
        public void SaveEntity(string keyValue, LR_Base_TJ2Entity entity)
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
