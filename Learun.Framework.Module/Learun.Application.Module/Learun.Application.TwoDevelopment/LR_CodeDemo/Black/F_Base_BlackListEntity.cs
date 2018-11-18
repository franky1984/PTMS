using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-08 10:39
    /// 描 述：黑名单管理
    /// </summary>
    public class F_Base_BlackListEntity 
    {
        #region 实体成员
        /// <summary>
        /// 黑名单主键
        /// </summary>
        [Column("F_BLACKID")]
        public string F_BlackId { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        [Column("F_CAUSE")]
        public string F_Cause { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("F_CREATETIME")]
        public DateTime? F_CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column( "F_CreateUser" )]
        public string F_CreateUser
        {
            get; set;
        }
        /// <summary>
        /// 工人主键
        /// </summary>
        [Column("F_TEMPUSERID")]
        public string F_TempUserId { get; set; }
        /// <summary>
        /// 用人单位名称
        /// </summary>
        [Column("F_EMPLOYERNAME")]
        public string F_EmployerName { get; set; }

        /// <summary>
        /// 工人姓名
        /// </summary>
        [Column("F_REALNAME")]
        public string F_RealName { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.F_BlackId    = Guid.NewGuid().ToString();
            this.F_CreateTime = DateTime.Now;
            this.F_CreateUser = LoginUserInfo.Get().userId;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.F_BlackId = keyValue;
        }
        #endregion
        #region 扩展字段
        #endregion
    }
}

