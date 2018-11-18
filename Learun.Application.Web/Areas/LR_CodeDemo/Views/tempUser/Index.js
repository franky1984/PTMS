/* * 版 本  V7.0.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2018 山东众数信息科技有限公司
 * 创建人：超级管理员
 * 日  期：2018-11-07 15:08
 * 描  述：临时工管理
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
            }, 250, 400);
            $('#F_Gender').lrDataItemSelect({ code: 'usersex' });
            $('#F_EmployerId').lrDataSourceSelect({ code: 'employer',value: 'f_employerid',text: 'f_employername' });
            $('#F_EmployerTypeId').lrDataSourceSelect({ code: 'Category', value: 'f_categoryid', text: 'f_categoryname' });
            $('#F_OrderId').lrDataSourceSelect({ code: 'order', value: 'f_orderid', text: 'f_meetingname' });

            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });
            // 新增
            $('#lr_add').on('click', function () {
                learun.layerForm({
                    id: 'form',
                    title: '新增',
                    url: top.$.rootUrl + '/LR_CodeDemo/tempUser/Form',
                    width: 600,
                    height: 400,
                    callBack: function (id) {
                        return top[id].acceptClick(refreshGirdData);
                    }
                });
            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('F_UserId');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/LR_CodeDemo/tempUser/Form?keyValue=' + keyValue,
                        width: 600,
                        height: 400,
                        callBack: function (id) {
                            return top[id].acceptClick(refreshGirdData);
                        }
                    });
                }
            });
            // 替工管理
            $('#lr_WorkSubstitute').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('F_UserId');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/LR_CodeDemo/tempUser/WorkSubstituteForm?keyValue=' + keyValue,
                        width: 600,
                        height: 400,
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
                            learun.deleteForm(top.$.rootUrl + '/LR_CodeDemo/tempUser/DeleteForm', { keyValue: keyValue}, function () {
                                refreshGirdData();
                            });
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
        },
        // 初始化列表
        initGird: function () {
            $('#gridtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/tempUser/GetPageList',
                headData: [
                    { label: "活动名称", name: "F_OrderId", width: 100, align: "left",
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
                    { label: "联系方式", name: "F_Mobile", width: 100, align: "left"},
                    { label: "身份证号", name: "F_Identity", width: 100, align: "left"},
                    { label: "性别", name: "F_Gender", width: 100, align: "left",
                        formatterAsync: function (callback, value, row, op,$cell) {
                             learun.clientdata.getAsync('dataItem', {
                                 key: value,
                                 code: 'usersex',
                                 callback: function (_data) {
                                     callback(_data.text);
                                 }
                             });
                        }
                    },
                    {
                        label: '状态', name: 'F_WorkSubstitute', width: 45, align: 'center',
                        formatter: function (cellvalue) {
                            return cellvalue == 0 ? "正常" : "替工";
                        }
                    },
                    {
                        label: '被替人', name: 'F_Replacement', width: 45, align: 'center'
                    }
                ],
                mainId:'F_UserId',
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
