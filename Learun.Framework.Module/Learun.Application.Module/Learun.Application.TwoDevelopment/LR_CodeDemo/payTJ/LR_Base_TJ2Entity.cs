using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-09 13:47
    /// 描 述：薪资核算统计
    /// </summary>
    public class LR_Base_TJ2Entity 
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
        /// F_EmployerName
        /// </summary>
        [Column("F_EMPLOYERNAME")]
        public string F_EmployerName { get; set; }
        /// <summary>
        /// sumPeople
        /// </summary>
        [Column("SUMPEOPLE")]
        public int? sumPeople { get; set; }
        /// <summary>
        /// F_StartTime
        /// </summary>
        [Column("F_STARTTIME")]
        public DateTime? F_StartTime { get; set; }
        /// <summary>
        /// F_EndTime
        /// </summary>
        [Column("F_ENDTIME")]
        public DateTime? F_EndTime { get; set; }
        /// <summary>
        /// sumWorkday
        /// </summary>
        [Column("SUMWORKDAY")]
        public int? sumWorkday { get; set; }
        /// <summary>
        /// f_realDaySalary
        /// </summary>
        [Column("F_REALDAYSALARY")]
        public decimal? f_realDaySalary { get; set; }
        /// <summary>
        /// f_shouldDaysalary
        /// </summary>
        [Column("F_SHOULDDAYSALARY")]
        public decimal? f_shouldDaysalary { get; set; }
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
        public void Modify(string keyValue)
        {
            this.f_id = keyValue;
        }
        #endregion
        #region 扩展字段
        #endregion
    }
}

