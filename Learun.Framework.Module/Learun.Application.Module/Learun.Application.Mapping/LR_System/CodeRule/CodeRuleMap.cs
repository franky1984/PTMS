﻿using Learun.Application.Base.SystemModule;
using System.Data.Entity.ModelConfiguration;

namespace Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创建人：众数-框架开发组
    /// 日 期：2017.03.04
    /// 描 述：编号规则
    /// </summary>
    public class CodeRuleMap : EntityTypeConfiguration<CodeRuleEntity>
    {
        public CodeRuleMap()
        {
            #region 表、主键
            //表
            this.ToTable("LR_BASE_CODERULE");//Base_CodeRule
            //主键
            this.HasKey(t => t.F_RuleId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
