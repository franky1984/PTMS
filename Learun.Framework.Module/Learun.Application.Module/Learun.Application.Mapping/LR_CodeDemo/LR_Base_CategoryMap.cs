using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-07 14:19
    /// 描 述：工种管理
    /// </summary>
    public class LR_Base_CategoryMap : EntityTypeConfiguration<LR_Base_CategoryEntity>
    {
        public LR_Base_CategoryMap()
        {
            #region 表、主键
            //表
            this.ToTable("LR_BASE_CATEGORY");
            //主键
            this.HasKey(t => t.F_CategoryId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

