using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-09 13:47
    /// 描 述：薪资核算统计
    /// </summary>
    public class LR_Base_TJ2Map : EntityTypeConfiguration<LR_Base_TJ2Entity>
    {
        public LR_Base_TJ2Map()
        {
            #region 表、主键
            //表
            this.ToTable("LR_BASE_TJ2");
            //主键
            this.HasKey(t => t.f_id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

