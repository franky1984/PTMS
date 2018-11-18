/* * 版 本  V7.0.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2018 山东众数信息科技有限公司
 * 创建人：超级管理员
 * 日  期：2018-11-09 13:47
 * 描  述：薪资核算统计
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

            // 初始化左侧树形数据
            $('#dataTree').lrtree({
                url: top.$.rootUrl + '/LR_SystemModule/DataSource/GetOrdersTree?code=order&parentId=f_orderid&Id=f_orderid&showId=f_meetingname',
                nodeClick: function (item) {
                    orderID = item.value;
                    page.search({});
                }
            });

            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });
        },
        // 初始化列表
        initGird: function () {
            $('#gridtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/payTJ/GetPageList',
                headData: [
                    { label: "活动名称", name: "F_MeetingName", width: 100, align: "left"},
                    { label: "单位名称", name: "F_EmployerName", width: 100, align: "left"},
                    { label: "开始时间", name: "F_StartTime", width: 100, align: "left"},
                    { label: "结束时间", name: "F_EndTime", width: 100, align: "left"},
                    { label: "参与人数", name: "sumPeople", width: 100, align: "left"},
                    { label: "工作日(总)", name: "sumWorkday", width: 100, align: "left"},
                    { label: "应发总额", name: "f_shouldDaysalary", width: 100, align: "left"},
                    { label: "实发总额", name: "f_realDaySalary", width: 100, align: "left"}
                ],
                mainId: 'f_id',
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
