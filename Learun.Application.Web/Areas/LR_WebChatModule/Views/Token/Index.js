/*
 * 版 本  V7.0.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2018 山东众数信息科技有限公司
 * 创建人：山东众数-前端开发组
 * 日 期：2017.04.11
 * 描 述：微信企业号设置	
 */
var bootstrap = function ($, learun) {
    "use strict";

    var page = {
        init: function () {
            $('#btn_RevisePassword').on('click', function () {
                if (!$('#form').lrValidform()) {
                    return false;
                }
                var postData = $('#form').lrGetFormData();
                $.lrSaveForm(top.$.rootUrl + '/LR_WebChatModule/Token/SaveForm', postData, null, true);
            });
        }
    };
    page.init();
}