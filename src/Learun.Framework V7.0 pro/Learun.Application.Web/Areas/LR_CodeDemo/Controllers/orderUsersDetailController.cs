using Learun.Util;
using System.Data;
using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Learun.Application.Web.Areas.LR_CodeDemo.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-11 20:58
    /// 描 述：订单人员明细管理
    /// </summary>
    public class orderUsersDetailController : MvcControllerBase
    {
        private orderUsersDetailIBLL orderUsersDetailIBLL = new orderUsersDetailBLL();

        #region 视图功能

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
            var data = orderUsersDetailIBLL.GetPageList(paginationobj, queryJson);
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
        public ActionResult GetFormData(string keyValue, string orderID)
        {
            var LR_Base_TempUserData = orderUsersDetailIBLL.GetLR_Base_TempUserEntity( keyValue, orderID );
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
        public ActionResult DeleteForm(string keyValue, string orderID)
        {
            orderUsersDetailIBLL.DeleteEntity(keyValue, orderID);
            return Success("删除成功！");
        }
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string strEntity, string orderID)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = string.Empty;
            }

            LR_Base_TempUserEntity entity = strEntity.ToObject<LR_Base_TempUserEntity>();
            int checkError = orderUsersDetailIBLL.SaveEntity( ref keyValue,entity, orderID);

            if( checkError == 2 )
            {
                return Fail("该用户在活动中已存在！");
            }
            else
            {
                return Success( "保存成功！",new{userid=keyValue} );
            }
        }
        #endregion

    }
}
