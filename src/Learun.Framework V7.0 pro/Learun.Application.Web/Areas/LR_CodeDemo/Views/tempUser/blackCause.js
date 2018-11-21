/* * 版 本  V6.1.6.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2017 山东众数信息科技有限公司
 * 创建人：超级管理员
 * 日  期：2018-11-01 09:23
 * 描  述：工人管理
 */
var acceptClick;
var keyValue = request('keyValue');
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            page.bind();
            page.initData();
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
        $.lrSaveForm(top.$.rootUrl + '/LR_CodeDemo/tempUser/AddBlack?keyValue=' + keyValue, postData, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack();
            }
        });
    };
    page.init();
}
