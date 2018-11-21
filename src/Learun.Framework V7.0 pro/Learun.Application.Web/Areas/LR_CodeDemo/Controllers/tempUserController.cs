using Learun.Util;
using System.Data;
using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Learun.Application.Web.Areas.LR_CodeDemo.Controllers
{
    /// <summary>
    /// 版 本  V7.0.0 山东众数敏捷开发框架
    /// Copyright (c) 2013-2018 山东众数信息科技有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-07 15:08
    /// 描 述：临时工管理
    /// </summary>
    public class tempUserController : MvcControllerBase
    {
        private tempUserIBLL tempUserIBLL = new tempUserBLL();

        #region 视图功能

        /// <summary>
        /// 表单页
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult blackCause()
        {
            return View();
        }

        /// <summary>
        /// 主页面
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
             return View();
        }
        /// <summary>
        /// 表单页
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
             return View();
        }

        [HttpGet]
        public ActionResult WorkSubstituteForm()
        {
            return View();
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// <summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = tempUserIBLL.GetPageList(paginationobj, queryJson);
            var jsonData = new
            {
                rows = data,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取表单数据
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFormData(string keyValue)
        {
            var LR_Base_TempUserData = tempUserIBLL.GetLR_Base_TempUserEntity( keyValue );
            var jsonData = new {
                LR_Base_TempUser = LR_Base_TempUserData,
            };
            return Success(jsonData);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            tempUserIBLL.DeleteEntity(keyValue);
            return Success("删除成功！");
        }

        /// <summary>
        /// 将人员添加到黑名单
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult AddBlack( string keyValue, string strEntity )
        {
            F_Base_BlackListEntity entity = strEntity.ToObject<F_Base_BlackListEntity>();
            tempUserIBLL.AddBlack( keyValue, entity );
            return Success( "添加成功" );
        }

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string strEntity)
        {
            LR_Base_TempUserEntity entity = strEntity.ToObject<LR_Base_TempUserEntity>();

            try
            {

                tempUserIBLL.SaveEntity( keyValue, entity );
                return Success( "保存成功！" );
            }
            catch
            {
                return Fail("身份证号重复！");
            }
        }

        /// <summary>
        /// 保存实体数据（新增、修改）  替工人管理
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult WorkSubstituteSaveForm( string keyValue, string orderID, string strEntity )
        {
            LR_Base_TempUserEntity entity = strEntity.ToObject<LR_Base_TempUserEntity>();

            int checkError = tempUserIBLL.WorkSubstituteSaveForm( keyValue, orderID, entity );

            if( checkError == 2 )
            {
                return Fail( "该用户在活动中已存在！" );
            }
            else
            {
                return Success( "保存成功！" );
            }
        }


        /// <summary>
        /// 判断用户是否已加入黑名单
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult CheckUserBlack( string keyValue )
        {
            return SuccessString( tempUserIBLL.CheckUserBlack( keyValue ) );
        }
        #endregion

    }
}
