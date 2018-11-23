/* * 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2018 众数信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2018-11-11 20:58
 * 描  述：订单人员明细管理
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
            $('#F_EmployerTypeId').lrDataSourceSelect({ code: 'Category', value: 'f_categoryid', text: 'f_categoryname' });

            $('#uploadFile').on('change', uploadImg);
            $('.file').prepend('<img id="uploadPreview"  src="' + top.$.rootUrl + '/AppManager/DTImg/GetImg?keyValue=' + keyValue + '" >');
        },
        initData: function () {
            if (!!keyValue) {
                $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/orderUsersDetail/GetFormData?keyValue=' + keyValue + "&orderID=" + orderID, function (data) {
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

    function uploadImg() {
        var f = document.getElementById('uploadFile').files[0];
        var src = window.URL.createObjectURL(f);
        document.getElementById('uploadPreview').src = src;
    };

    // 保存数据
    acceptClick = function (callBack) {
        if (!$('body').lrValidform()) {
            return false;
        }

        var postData = {
            strEntity: JSON.stringify($('body').lrGetFormData())
        };
        if (!$('#form').lrGetFormData().uploadFile) {
            learun.alert.error("请选择图片");
            return false;
        }

        var f = document.getElementById('uploadFile').files[0];

        if (!!f) {
            learun.loading(true, '正在保存...');

            $.lrSaveForm(
                top.$.rootUrl + '/LR_CodeDemo/orderUsersDetail/SaveForm?keyValue=' + keyValue + "&orderID=" + orderID,
                postData,
                function (res) {
                    // 保存成功后才回调
                    if (!!callBack) {
                        $.ajaxFileUpload({
                            data: postData,
                            url: top.$.rootUrl + "/AppManager/DTImg/UploadFile?keyValue=" + res.data.userid,
                            secureuri: false,
                            fileElementId: 'uploadFile',
                            dataType: 'json',
                            success: function (data) {
                                if (!!callBack) {
                                    callBack();
                                }
                                learun.loading(false);
                                learun.layerClose(window.name);
                            }
                        });
                    }
                });
        } else {
            $.lrSaveForm(
                top.$.rootUrl + '/LR_CodeDemo/orderUsersDetail/SaveForm?keyValue=' + keyValue + "&orderID=" + orderID,
                postData,
                function (res) {
                    // 保存成功后才回调
                    if (!!callBack) {
                        callBack();
                    }
                });
        }
    }

    page.init();
}
