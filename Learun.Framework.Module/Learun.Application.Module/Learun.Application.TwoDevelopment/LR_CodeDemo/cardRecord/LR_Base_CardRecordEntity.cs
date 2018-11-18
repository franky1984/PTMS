using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-08 11:38
    /// 描 述：打卡管理
    /// </summary>
    public class LR_Base_CardRecordEntity 
    {
        #region 实体成员
        /// <summary>
        /// F_RecordId
        /// </summary>
        [Column("F_RECORDID")]
        public string F_RecordId { get; set; }
        /// <summary>
        /// F_First
        /// </summary>
        [Column("F_FIRST")]
        public DateTime? F_First { get; set; }
        /// <summary>
        /// F_Second
        /// </summary>
        [Column("F_SECOND")]
        public DateTime? F_Second { get; set; }
        /// <summary>
        /// F_Identity
        /// </summary>
        [Column("F_IDENTITY")]
        public string F_Identity { get; set; }
        /// <summary>
        /// F_CreateTime
        /// </summary>
        [Column("F_CREATETIME")]
        public DateTime? F_CreateTime { get; set; }
        /// <summary>
        /// F_RealName
        /// </summary>
        [Column("F_REALNAME")]
        public string F_RealName { get; set; }
        /// <summary>
        /// F_Gender
        /// </summary>
        [Column("F_GENDER")]
        public int? F_Gender { get; set; }
        /// <summary>
        /// F_OrderId
        /// </summary>
        [Column("F_ORDERID")]
        public string F_OrderId { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.F_RecordId = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.F_RecordId = keyValue;
        }
        #endregion
        #region 扩展字段
        #endregion
    }
}

