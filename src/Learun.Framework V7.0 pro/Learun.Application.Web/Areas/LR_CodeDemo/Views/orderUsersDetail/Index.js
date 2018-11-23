/* * 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2018 众数信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2018-11-11 20:58
 * 描  述：订单人员明细管理
 */
var refreshGirdData;
var orderID="";
var bootstrap = function ($, learun) {
    "use strict";
    var page = {
        init: function () {
            page.initGird();
            page.bind();
        },
        bind: function () {
            // 初始化左侧树形数据
            $('#dataTree').lrtree({
                url: top.$.rootUrl + '/LR_SystemModule/DataSource/GetOrdersTree?code=order&parentId=f_orderid&Id=f_orderid&showId=f_meetingname',
                nodeClick: function (item) {
                    orderID = item.value;
                    page.search({});
                }
            });
            $('#multiple_condition_query').lrMultipleQuery(function (queryJson) {
                page.search(queryJson);
            }, 220, 400);   
            $('#F_EmployerId').lrDataSourceSelect({ code: 'employer',value: 'f_employerid',text: 'f_employerid' });
            $('#F_Gender').lrDataItemSelect({ code: 'usersex' });
            $('#F_EmployerTypeId').lrDataSourceSelect({ code: 'Category',value: 'f_categoryid',text: 'f_categoryid' });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });
            // 新增
            $('#lr_add').on('click', function () {
                if (learun.checkrow(orderID)) {
                    learun.layerForm({
                        id: 'form',
                        title: '新增',
                        url: top.$.rootUrl + '/LR_CodeDemo/orderUsersDetail/Form?orderID=' + orderID,
                        width: 600,
                        height: 500,
                        callBack: function (id) {
                            return top[id].acceptClick(refreshGirdData);
                        }
                    });
                }
            });

            // 替工管理
            $('#lr_workSubstitute').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('F_UserId');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '替工管理',
                        url: top.$.rootUrl + '/LR_CodeDemo/tempUser/WorkSubstituteForm?keyValue=' + keyValue + "&orderID=" + orderID,
                        width: 600,
                        height: 280,
                        callBack: function (id) {
                            return top[id].acceptClick(refreshGirdData);
                        }
                    });
                }
            });
            //添加黑名单
            $('#lr_addBlack').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('F_UserId');

                if (learun.checkrow(keyValue)) {
                    var res = learun.httpGet(top.$.rootUrl + '/LR_CodeDemo/tempUser/CheckUserBlack', { keyValue: keyValue });
                    if (res.data == 0) {
                        learun.layerForm({
                            id: 'form',
                            title: '加入黑名单',
                            url: top.$.rootUrl + '/LR_CodeDemo/tempUser/blackCause?keyValue=' + keyValue,
                            width: 600,
                            height: 250,
                            callBack: function (id) {
                                return top[id].acceptClick(refreshGirdData);
                            }
                        });
                    } else {
                        learun.alert.warning('当前临时工已在黑名单中！');
                    }
                }
            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('F_UserId');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/LR_CodeDemo/orderUsersDetail/Form?keyValue=' + keyValue + "&orderID=" + orderID,
                        width: 600,
                        height: 500,
                        callBack: function (id) {
                            return top[id].acceptClick(refreshGirdData);
                        }
                    });
                }
            });
            // 删除
            $('#lr_delete').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('F_UserId');
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/LR_CodeDemo/orderUsersDetail/DeleteForm', { keyValue: keyValue, orderID: orderID }, function () {
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
                url: top.$.rootUrl + '/LR_CodeDemo/orderUsersDetail/GetPageList',
                headData: [
                    { label: "单位名称", name: "F_EmployerId", width: 100, align: "left",
                        formatterAsync: function (callback, value, row, op,$cell) {
                             learun.clientdata.getAsync('custmerData', {
                                 url: '/LR_SystemModule/DataSource/GetDataTable?code=' + 'employer',
                                 key: value,
                                 keyId: 'f_employerid',
                                 callback: function (_data) {
                                     callback(_data['f_employername']);
                                 }
                             });
                        }},
                    { label: "工种", name: "F_EmployerTypeId", width: 100, align: "left",
                        formatterAsync: function (callback, value, row, op,$cell) {
                             learun.clientdata.getAsync('custmerData', {
                                 url: '/LR_SystemModule/DataSource/GetDataTable?code=' + 'Category',
                                 key: value,
                                 keyId: 'f_categoryid',
                                 callback: function (_data) {
                                     callback(_data['f_categoryname']);
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
                    { label: "联系方式", name: "F_Mobile", width: 100, align: "left"},
                ],
                mainId:'F_UserId',
                isPage: true
            });
            page.search();
        },
        search: function (param) {
            param = param || {};
            param.F_OrderId = orderID;
            $('#gridtable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
            
        }
    };
    refreshGirdData = function () {
        page.search({});
    };
    page.init();
}
