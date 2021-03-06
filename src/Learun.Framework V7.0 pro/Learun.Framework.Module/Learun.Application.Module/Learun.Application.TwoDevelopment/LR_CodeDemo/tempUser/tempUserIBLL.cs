﻿using Learun.Util;
using System.Data;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-07 15:08
    /// 描 述：临时工管理
    /// </summary>
    public interface tempUserIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// <summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<LR_Base_TempUserEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取LR_Base_TempUser表实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        LR_Base_TempUserEntity GetLR_Base_TempUserEntity(string keyValue);
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void DeleteEntity(string keyValue);

        void AddBlack( string keyValue, F_Base_BlackListEntity entity );

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void SaveEntity(string keyValue, LR_Base_TempUserEntity entity);

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        int WorkSubstituteSaveForm( string keyValue,string orderID, LR_Base_TempUserEntity entity );

        string CheckUserBlack( string keyValue);

        /// <summary>
        /// 判断被替工人是否在此次活动里
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="keyValue"></param>
        int CheckReplacement( string orderID, string keyValue );

        #endregion

    }
}
