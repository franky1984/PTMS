﻿using Learun.Application.Base.SystemModule;
using System.Data.Entity.ModelConfiguration;

namespace Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创建人：众数-框架开发组
    /// 日 期：2017.03.04
    /// 描 述：系统功能模块
    /// </summary>
    public class ModuleMap : EntityTypeConfiguration<ModuleEntity>
    {
        public ModuleMap()
        {
            #region 表、主键
            //表
            this.ToTable("LR_BASE_MODULE");
            //主键
            this.HasKey(t => t.F_ModuleId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
