using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-07 14:19
    /// 描 述：工种管理
    /// </summary>
    public class LR_Base_CategoryEntity 
    {
        #region 实体成员
        /// <summary>
        /// F_CategoryId
        /// </summary>
        [Column("F_CATEGORYID")]
        public string F_CategoryId { get; set; }
        /// <summary>
        /// F_CategoryName
        /// </summary>
        [Column("F_CATEGORYNAME")]
        public string F_CategoryName { get; set; }
        /// <summary>
        /// F_CompanyId
        /// </summary>
        [Column("F_COMPANYID")]
        public string F_CompanyId { get; set; }
        /// <summary>
        /// 具体事项
        /// </summary>
        [Column("F_TASKS")]
        public string F_Tasks { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.F_CategoryId = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.F_CategoryId = keyValue;
        }
        #endregion
        #region 扩展字段
        /// <summary>
        /// 公司名称
        /// </summary>
        [NotMapped]
        public string F_FullName
        {
            get; set;
        }
        #endregion
    }
}

