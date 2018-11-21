/*
 * 版 本  V7.0.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2018 山东众数信息科技有限公司
 * 创建人：山东众数-前端开发组
 * 日 期：2018.05.10
 * 描 述：客户端语言包加载（菜单，tab条）
 */
(function ($, learun) {
    "use strict";

    learun.language = {
        getMainCode: function () {
            return '';
        },
        get: function (text, callback) {
            callback(text);
        },
        getSyn: function (text) {
            return text;
        }
    };
})(window.jQuery, top.learun);