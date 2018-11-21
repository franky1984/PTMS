/* * 版 本  V7.0.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2018 山东众数信息科技有限公司
 * 创建人：超级管理员
 * 日  期：2018-11-08 10:39
 * 描  述：黑名单管理
 */
var refreshGirdData;
var bootstrap = function ($, learun) {
    "use strict";
    var page = {
        init: function () {
            page.initGird();
            page.bind();
        },
        bind: function () {
           
            $('#multiple_condition_query').lrMultipleQuery(function (queryJson) {
                page.search(queryJson);
            }, 220, 400);

            $('#F_EmployerName').lrDataSourceSelect({ code: 'employer', value: 'f_employerid', text: 'f_employername' });
            //$('#F_CategoryName').lrDataSourceSelect({ code: 'Category', value: 'f_categoryid', text: 'f_categoryname' });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });

            // 删除
            $('#lr_delete').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('F_BlackId');

                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否恢复临时工有效状态？', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/LR_CodeDemo/Black/DeleteForm', { keyValue: keyValue}, function () {
                                refreshGirdData();
                            });
                        }
                    });
                }
            });
        },
        // 初始化列表
        initGird: function () {
            $('#gridtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/Black/GetPageList',
                headData: [
                    { label: "用人单位", name: "F_EmployerName", width: 100, align: "left"},
                    { label: "姓名", name: "F_RealName", width: 100, align: "left"},
                    { label: "原因", name: "F_Cause", width: 100, align: "left" },
                    {
                        label: "时间", name: "F_CreateTime", width: 100, align: "left", formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd hh:mm');
                        }
                    }
                ],
                mainId:'F_BlackId',
                isPage: true
            });
            page.search();
        },
        search: function (param) {
            param = param || {};
            $('#gridtable').jfGridSet('reload',{ queryJson: JSON.stringify(param) });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    page.init();
}
