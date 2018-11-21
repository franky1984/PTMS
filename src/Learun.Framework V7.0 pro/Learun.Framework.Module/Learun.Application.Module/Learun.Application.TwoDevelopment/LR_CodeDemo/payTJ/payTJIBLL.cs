using Learun.Util;
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
    public interface payTJIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// <summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<LR_Base_TJ2Entity> GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        void GetExportList();
        /// <summary>
        /// 获取LR_Base_TJ2表实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        LR_Base_TJ2Entity GetLR_Base_TJ2Entity(string keyValue);
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void DeleteEntity(string keyValue);
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void SaveEntity(string keyValue, LR_Base_TJ2Entity entity);
        #endregion

    }
}
