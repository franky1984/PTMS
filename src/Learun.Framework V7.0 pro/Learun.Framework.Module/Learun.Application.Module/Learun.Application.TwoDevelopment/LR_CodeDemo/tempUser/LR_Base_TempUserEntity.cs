using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-07 15:08
    /// 描 述：临时工管理
    /// </summary>
    public class LR_Base_TempUserEntity 
    {
        #region 实体成员
        /// <summary>
        /// F_UserId
        /// </summary>
        [Column("F_USERID")]
        public string F_UserId { get; set; }

        /// <summary>
        /// F_CreateUser
        /// </summary>
        [Column("F_CreateUser")]
        public string F_CreateUser { get; set; }
        /// <summary>
        /// F_EnCode
        /// </summary>
        [Column("F_ENCODE")]
        public string F_EnCode { get; set; }
        /// <summary>
        /// F_RealName
        /// </summary>
        [Column("F_REALNAME")]
        public string F_RealName { get; set; }
        /// <summary>
        /// F_EmployerId
        /// </summary>
        [Column("F_EMPLOYERID")]
        public string F_EmployerId { get; set; }
        /// <summary>
        /// F_EmployerTypeId
        /// </summary>
        [Column("F_EMPLOYERTYPEID")]
        public string F_EmployerTypeId { get; set; }
        /// <summary>
        /// F_Mobile
        /// </summary>
        [Column("F_MOBILE")]
        public string F_Mobile { get; set; }
        /// <summary>
        /// F_Gender
        /// </summary>
        [Column("F_GENDER")]
        public int? F_Gender { get; set; }
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
        /// F_OrderId
        /// </summary>
        [Column("F_ORDERID")]
        public string F_OrderId { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        [Column("F_ENABLEDMARK")]
        public int? F_EnabledMark { get; set; }

        /// <summary>
        /// 是否是替工
        /// </summary>
        [Column( "F_WorkSubstitute" )]
        public int? F_WorkSubstitute
        {
            get; set;
        }

        /// <summary> 
        /// 图片文件名 
        /// </summary> 
        /// <returns></returns> 
        [Column( "F_FILENAME" )]
        public string F_FileName
        {
            get; set;
        }

        /// <summary>
        /// 被替人
        /// </summary>
        [Column( "F_Replacement" )]
        public string F_Replacement
        {
            get; set;
        }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.F_UserId      = Guid.NewGuid().ToString();
            this.F_CreateUser  = LoginUserInfo.Get().userId;
            this.F_CreateTime  = DateTime.Now;
            this.F_EnabledMark = 1;  //默认都是启用状态 
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.F_UserId = keyValue;
        }
        #endregion
        #region 扩展字段
        #endregion
    }
}

