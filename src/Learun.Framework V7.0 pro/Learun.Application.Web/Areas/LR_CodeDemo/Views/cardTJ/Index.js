/* * 版 本  V7.0.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2018 山东众数信息科技有限公司
 * 创建人：超级管理员
 * 日  期：2018-11-08 15:16
 * 描  述：打卡统计
 */
var refreshGirdData;
var orderID = "";
var bootstrap = function ($, learun) {
    "use strict";
    var startTime;
    var endTime;
    var page = {
        init: function () {
            page.initGird();
            page.bind();
        },
        bind: function () {
            $('#multiple_condition_query').lrMultipleQuery(function (queryJson) {
                page.search(queryJson);
            }, 120, 400);

            // 初始化左侧树形数据
            $('#dataTree').lrtree({
                url: top.$.rootUrl + '/LR_SystemModule/DataSource/GetOrdersTree?code=order&parentId=f_orderid&Id=f_orderid&showId=f_meetingname',
                nodeClick: function (item) {
                    orderID = item.value;
                    page.search({});
                }
            });

            //$('#F_OrderId').lrDataSourceSelect({ code: 'order', value: 'f_orderid', text: 'f_meetingname' });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });

        },
        // 初始化列表
        initGird: function () {
            $('#gridtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/cardTJ/GetPageList',
                headData: [
                    { label: "活动名称", name: "F_MeetingName", width: 100, align: "left" },
                    { label: "单位名称", name: "f_employername", width: 100, align: "left" },
                    { label: "姓名", name: "f_realname", width: 100, align: "left" },
                    { label: "身份证号", name: "f_identity", width: 100, align: "left" },
                    { label: "工种", name: "F_CategoryName", width: 100, align: "left" },
                    { label: "正常", name: "f_normal", width: 100, align: "left" },
                    { label: "迟到", name: "f_late", width: 100, align: "left" },
                    { label: "早退", name: "f_leave", width: 100, align: "left" },
                    { label: "旷工", name: "f_absenteeism", width: 100, align: "left" }
                ],
                mainId: 'F_RecordId',
                isPage: true
            });
        },
        search: function (param) {
            param = param || {};
            param.F_OrderId = orderID;
            $('#gridtable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    page.init();
}
