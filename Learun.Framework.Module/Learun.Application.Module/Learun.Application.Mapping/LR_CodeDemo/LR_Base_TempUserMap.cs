using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-11 20:58
    /// 描 述：订单人员明细管理
    /// </summary>
    public class LR_Base_TempUserMap : EntityTypeConfiguration<LR_Base_TempUserEntity>
    {
        public LR_Base_TempUserMap()
        {
            #region 表、主键
            //表
            this.ToTable("LR_BASE_TEMPUSER");
            //主键
            this.HasKey(t => t.F_UserId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

