using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-08 15:16
    /// 描 述：打卡统计
    /// </summary>
    public class LR_Base_CardTJEntity 
    {
        #region 实体成员
        /// <summary>
        /// f_id
        /// </summary>
        [Column("F_ID")]
        public string f_id { get; set; }
        /// <summary>
        /// F_MeetingName
        /// </summary>
        [Column("F_MEETINGNAME")]
        public string F_MeetingName { get; set; }
        /// <summary>
        /// f_employername
        /// </summary>
        [Column("F_EMPLOYERNAME")]
        public string f_employername { get; set; }
        /// <summary>
        /// f_realname
        /// </summary>
        [Column("F_REALNAME")]
        public string f_realname { get; set; }
        /// <summary>
        /// f_gender
        /// </summary>
        [Column("F_GENDER")]
        public string f_gender { get; set; }
        /// <summary>
        /// f_identity
        /// </summary>
        [Column("F_IDENTITY")]
        public string f_identity { get; set; }
        /// <summary>
        /// f_price
        /// </summary>
        [Column( "F_CategoryName" )]
        public string F_CategoryName
        { get; set; }
        /// <summary>
        /// f_cardNum
        /// </summary>
        [Column("F_CARDNUM")]
        public int? f_cardNum { get; set; }
        /// <summary>
        /// f_normal
        /// </summary>
        [Column("F_NORMAL")]
        public int? f_normal { get; set; }
        /// <summary>
        /// f_late
        /// </summary>
        [Column("F_LATE")]
        public int? f_late { get; set; }

        /// <summary>
        /// f_late
        /// </summary>
        [Column( "f_leave" )]
        public int? f_leave
        {
            get; set;
        }
        /// <summary>
        /// f_absenteeism
        /// </summary>
        [Column("F_ABSENTEEISM")]
        public int? f_absenteeism { get; set; }
        /// <summary>
        /// f_realSalary
        /// </summary>
        [Column("F_REALSALARY")]
        public decimal? f_realSalary { get; set; }
        /// <summary>
        /// f_createtime
        /// </summary>
        [Column( "F_CREATETIME" )]
        public DateTime? f_createtime
        {
            get; set;
        }

        [Column( "F_StartTime" )]
        public DateTime? F_StartTime
        {
            get;
            set;
        }

        [Column( "F_EndTime" )]
        public DateTime? F_EndTime
        {
            get;
            set;
        }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.f_id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify( string keyValue )
        {
            this.f_id = keyValue;
        }
        #endregion
        #region 扩展字段
        #endregion
    }
}

