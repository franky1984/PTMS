using Learun.Util;
using System;
using System.Data;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-09 13:47
    /// 描 述：薪资核算统计
    /// </summary>
    public class payTJBLL : payTJIBLL
    {
        private payTJService payTJService = new payTJService();

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
                return payTJService.GetPageList(pagination, queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
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
                return payTJService.GetLR_Base_TJ2Entity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
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
                payTJService.DeleteEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
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
                payTJService.SaveEntity(keyValue, entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        #endregion


        /// <summary>
        /// 用户列表（导出Excel）
        /// </summary>
        /// <returns></returns>
        public void GetExportList()
        {
            try
            {
                //取出数据源
                DataTable exportTable = payTJService.GetExportList();
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "薪资合算统计";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = "薪资合算统计.xls";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                excelconfig.ColumnEntity = new List<ColumnModel>();
                excelconfig.ColumnEntity.Add( new ColumnModel() { Column = "F_MeetingName", ExcelColumn = "活动名称" } );
                excelconfig.ColumnEntity.Add( new ColumnModel() { Column = "F_EmployerName", ExcelColumn = "单位名称" } );
                excelconfig.ColumnEntity.Add( new ColumnModel() { Column = "F_StartTime", ExcelColumn = "开始时间" } );
                excelconfig.ColumnEntity.Add( new ColumnModel() { Column = "F_EndTime", ExcelColumn = "结束时间" } );
                excelconfig.ColumnEntity.Add( new ColumnModel() { Column = "sumPeople", ExcelColumn = "总人数"} );
                excelconfig.ColumnEntity.Add( new ColumnModel() { Column = "sumWorkday", ExcelColumn = "工作日(总)" } );
                excelconfig.ColumnEntity.Add( new ColumnModel() { Column = "f_shouldDaysalary", ExcelColumn = "应发工资" } );
                excelconfig.ColumnEntity.Add( new ColumnModel() { Column = "f_realDaySalary", ExcelColumn = "实发工资" } );
                //调用导出方法
                ExcelHelper.ExcelDownload( exportTable, excelconfig );
            }
            catch( Exception ex )
            {
                if( ex is ExceptionEx )
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException( ex );
                }
            }
        }
    }
}
