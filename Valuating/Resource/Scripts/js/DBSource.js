﻿//获得用户数据源
DB.EnumUserTypeStore = function () {
    var states = Ext.create('Ext.data.Store', {
        fields: ['typeId', 'name'],
        data: EnumUserTypeJson()

    });
    return states;
};
//获得用户类别JSon数据
DB.EnumUserTypeJson = function () {
    return [
        { "typeId": 0, "name": "禁用账户" },
        { "typeId": 1, "name": "学生账户" },
        { "typeId": 2, "name": "教师账户" },
        { "typeId": 3, "name": "管理员账户" }

    ];
};
//获得教师类别枚举
DB.EnumTeacherTypeJson = function () {
    return [
        { Id: 1, Name: "一评一专家" },
        { Id: 2, Name: "一评二专家" },
        { Id: 3, Name: "二评专家" },
        { Id: 4, Name: "仲裁专家" }
    ];
};