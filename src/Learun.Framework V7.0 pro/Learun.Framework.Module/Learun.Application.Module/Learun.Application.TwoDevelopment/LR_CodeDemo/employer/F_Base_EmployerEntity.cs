using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-07 14:38
    /// 描 述：用人单位管理
    /// </summary>
    public class F_Base_EmployerEntity 
    {
        #region 实体成员
        /// <summary>
        /// 用人单位主键
        /// </summary>
        [Column("F_EMPLOYERID")]
        public string F_EmployerId { get; set; }
        /// <summary>
        /// 用人单位名称
        /// </summary>
        [Column("F_EMPLOYERNAME")]
        public string F_EmployerName { get; set; }
        /// <summary>
        /// 规模
        /// </summary>
        [Column("F_SCALE")]
        public int? F_Scale { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Column("F_CONTACTS")]
        public string F_Contacts { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Column("F_PHONE")]
        public string F_Phone { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("F_CREATETIME")]
        public DateTime? F_Createtime { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.F_EmployerId = Guid.NewGuid().ToString();
            this.F_Createtime = DateTime.Now;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.F_EmployerId = keyValue;
        }
        #endregion
        #region 扩展字段
        #endregion
    }
}

