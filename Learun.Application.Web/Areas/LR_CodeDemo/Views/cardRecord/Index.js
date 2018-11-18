/* * 版 本  V7.0.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2018 山东众数信息科技有限公司
 * 创建人：超级管理员
 * 日  期：2018-11-08 11:38
 * 描  述：打卡管理
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
            $('#F_OrderId').lrDataSourceSelect({ code: 'order',value: 'f_orderid',text: 'f_meetingname' });
            $('#F_Gender').lrDataItemSelect({ code: 'usersex' });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });
        },
        // 初始化列表
        initGird: function () {
            $('#gridtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/cardRecord/GetPageList',
                headData: [
                    { label: "订单", name: "F_OrderId", width: 100, align: "left",
                        formatterAsync: function (callback, value, row, op,$cell) {
                             learun.clientdata.getAsync('custmerData', {
                                 url: '/LR_SystemModule/DataSource/GetDataTable?code=' + 'order',
                                 key: value,
                                 keyId: 'f_orderid',
                                 callback: function (_data) {
                                     callback(_data['f_meetingname']);
                                 }
                             });
                        }},
                    { label: "姓名", name: "F_RealName", width: 100, align: "left"},
                    { label: "性别", name: "F_Gender", width: 100, align: "left",
                        formatterAsync: function (callback, value, row, op,$cell) {
                             learun.clientdata.getAsync('dataItem', {
                                 key: value,
                                 code: 'usersex',
                                 callback: function (_data) {
                                     callback(_data.text);
                                 }
                             });
                        }},
                    { label: "身份证号", name: "F_Identity", width: 100, align: "left"},
                    { label: "第一次打卡", name: "F_First", width: 100, align: "left"},
                    { label: "第二次打卡", name: "F_Second", width: 100, align: "left"},
                ],
                mainId:'F_RecordId',
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
