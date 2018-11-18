/*
 * 版 本  V6.1.6.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2017 山东众数信息科技有限公司
 * 创建人：山东众数-前端开发组
 * 日 期：2018.06.05
 * 描 述：写邮件	
 */
var keyValue = request('keyValue');
var data = request('data');

var acceptClick;
var ue;
var bootstrap = function ($, learun) {
    "use strict";

    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            //内容编辑器
            ue = UE.getEditor('F_BodyText');
            $('#F_Attachment').lrUploader({ isUpload: false });
        },
        initData: function () {
            if (!!keyValue) {
                $.lrSetForm(top.$.rootUrl + '/LR_OAModule/Email/GetReceiveEntity?keyValue=' + keyValue, function (data) {//
                    $('#form').lrSetFormData(data);
                });
            }
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var postData = $('#form').lrGetFormData(keyValue);
        postData['F_BodyText'] = ue.getContentTxt(null, null, true);
        $.lrSaveForm(top.$.rootUrl + '/LR_OAModule/Email/SendMail', postData, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack();
            }
        });
    };
    page.init();
}