/*
 * 版 本  V7.0.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2018 山东众数信息科技有限公司
 * 创建人：山东众数-前端开发组
 * 日 期：2017.11.11
 * 描 述：日程管理
 */
var acceptClick;
var bootstrap = function ($, learun) {
    "use strict";
    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            //开始时间
            $('#F_StartTime').lrselect({
                maxHeight: 150
            });
            //结束时间
            $('#F_EndTime').lrselect({
                maxHeight: 150
            });
        },
        initData: function () {
            $("#F_StartDate").val(request('startDate'));
            $("#F_StartTime").lrselectSet(request('startTime'));
        }
    };
    acceptClick = function () {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var postData = $('#form').lrGetFormData("");
        $.lrSaveForm(top.$.rootUrl + '/LR_OAModule/Schedule/SaveForm?keyValue=', postData, function (res) {
            learun.frameTab.currentIframe().location.reload();
        });
    }
    page.init();
}


