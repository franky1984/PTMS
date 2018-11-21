using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-08 15:16
    /// 描 述：打卡统计
    /// </summary>
    public class LR_Base_CardTJMap : EntityTypeConfiguration<LR_Base_CardTJEntity>
    {
        public LR_Base_CardTJMap()
        {
            #region 表、主键
            //表
            this.ToTable("LR_BASE_CARDTJ");
            //主键
            this.HasKey(t => t.f_id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

