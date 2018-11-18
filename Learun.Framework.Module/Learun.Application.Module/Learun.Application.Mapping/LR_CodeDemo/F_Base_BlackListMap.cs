using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-08 10:39
    /// 描 述：黑名单管理
    /// </summary>
    public class F_Base_BlackListMap : EntityTypeConfiguration<F_Base_BlackListEntity>
    {
        public F_Base_BlackListMap()
        {
            #region 表、主键
            //表
            this.ToTable("F_BASE_BLACKLIST");
            //主键
            this.HasKey(t => t.F_BlackId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

