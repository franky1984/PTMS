using System.Web.Mvc;

namespace Learun.Application.Web.Areas.LR_CodeGeneratorModule.Controllers
{
    /// <summary>
    /// 版 本  V7.0.0 山东众数敏捷开发框架
    /// Copyright (c) 2013-2018 山东众数信息科技有限公司
    /// 创建人：山东众数-框架开发组
    /// 日 期：2017.03.09
    /// 描 述：JS插件Demo
    /// </summary>
    public class PluginDemoController : MvcControllerBase
    {
        #region 视图功能
        /// <summary>
        /// JS插件展示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion
        
    }
}