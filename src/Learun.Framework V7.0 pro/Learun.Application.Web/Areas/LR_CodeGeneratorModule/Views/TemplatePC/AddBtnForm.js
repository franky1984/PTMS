/*
 * 版 本  V7.0.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2018 山东众数信息科技有限公司
 * 创建人：山东众数-前端开发组
 * 日 期：2018.05.11
 * 描 述：添加扩展按钮	
 */
var acceptClick;
var bootstrap = function ($, learun) {
    "use strict";

    // 保存数据
    acceptClick = function (callBack) {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var formData = $('#form').lrGetFormData();
        if (!!callBack) {
            callBack(formData);
        }

        return true;
    };
}