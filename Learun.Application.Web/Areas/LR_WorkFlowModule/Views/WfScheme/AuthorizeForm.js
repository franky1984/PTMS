/*
 * 版 本  V7.0.0 山东众数敏捷开发框架(http://www.zoshu.cn/)
 * Copyright (c) 2013-2018 山东众数信息科技有限公司
 * 创建人：山东众数-前端开发组
 * 日 期：2017.04.18
 * 描 述：表单权限添加	
 */
var id = request('id');
var acceptClick;
var bootstrap = function ($, learun) {
    "use strict";
    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            $('#isLook').lrselect({// 是否可查看1.是0.否
                placeholder: false,
                data: [{ id: '1', text: '是' }, { id: '0', text: '否' }]
            }).lrselectSet('1');
            $('#isEdit').lrselect({// 是否可编辑1.是2.否
                placeholder: false,
                data: [{ id: '1', text: '是' }, { id: '0', text: '否' }]
            }).lrselectSet('1');
        },
        initData: function () {
            if (!!id) {
                var authorize = top.layer_NodeForm.authorize;
                for (var i = 0, l = authorize.length; i < l; i++) {
                    if (authorize[i].id == id) {
                        if (!!authorize[i].formId) {
                            $('#formName').attr('readonly', 'readonly');
                            $('#fieldName').attr('readonly', 'readonly');
                            $('#fieldId').attr('readonly', 'readonly');
                        }
                        $('#form').lrSetFormData(authorize[i]);
                        break;
                    }
                }
            }
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var formData = $('#form').lrGetFormData();
        formData.id = id;
        callBack(formData);
        return true;
    };
    page.init();
}