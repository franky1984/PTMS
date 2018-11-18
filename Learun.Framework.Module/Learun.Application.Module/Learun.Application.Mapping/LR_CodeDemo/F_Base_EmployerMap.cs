using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-07 14:38
    /// 描 述：用人单位管理
    /// </summary>
    public class F_Base_EmployerMap : EntityTypeConfiguration<F_Base_EmployerEntity>
    {
        public F_Base_EmployerMap()
        {
            #region 表、主键
            //表
            this.ToTable("F_BASE_EMPLOYER");
            //主键
            this.HasKey(t => t.F_EmployerId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

