﻿/* * 版 本  V7.0.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2018 山东众数信息科技有限公司
 * 创建人：超级管理员
 * 日  期：2018-11-07 15:08
 * 描  述：临时工管理
 */
var acceptClick;
var keyValue = request('keyValue');
var orderID = request('orderID');
var bootstrap = function ($, learun) {
    "use strict";
    var page = {
        init: function () {
            $('.lr-form-wrap').lrscroll();
            page.bind();
            page.initData();
        },
        bind: function () {
            $('#F_Gender').lrDataItemSelect({ code: 'usersex' });
        },
        initData: function () {
            if (!!keyValue) {
                $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/tempUser/GetFormData?keyValue=' + keyValue, function (data) {
                    for (var id in data) {
                        if (!!data[id].length && data[id].length > 0) {
                            $('#' + id).jfGridSet('refreshdata', data[id]);
                        }
                        else {
                            $('[data-table="' + id + '"]').lrSetFormData(data[id]);
                        }
                    }
                });
            }
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        if (!$('body').lrValidform()) {
            return false;
        }
        var postData = {
            strEntity: JSON.stringify($('body').lrGetFormData())
        };
        $.lrSaveForm(top.$.rootUrl + '/LR_CodeDemo/tempUser/WorkSubstituteSaveForm?keyValue=' + keyValue + "&orderID=" + orderID, postData, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack();
            }
        });
    };
    page.init();
}
