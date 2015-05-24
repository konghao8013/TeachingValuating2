/// <reference path="~/Resource/Scripts/jquery-1.8.3.js"></script>
/// <reference path="~/Resource/Scripts/extjs/js/ext-all-debug.js"></script>
/// <reference path="~/Resource/Scripts/JqueryFn.js"></script>
/// <reference path="~/Resource/Scripts/model.js"></script>
/// <reference path="~/Resource/Scripts/model/WebServerLog.js"></script>
/// <reference path="~/Resource/Scripts/DB.js"></script>
/// <reference path="~/Resource/Scripts/dbserver/SysMenuServer.js"></script>
/// <reference path="~/Resource/Scripts/lodash.compat.js"></script>
/// <reference path="~/Resource/Scripts/extjs/js/ux/TabCloseMenu.js"></script>
/// <reference path="~/Resource/Scripts/dbserver/UserServer.js"></script>
/// <reference path="../../model/UserType.js" />
/// <reference path="~/Resource/Scripts/dbserver/DBSource.js"></script>
/// <reference path="../../linq.js" />
/// <reference path="../../linq.js" />
/// <reference path="../../model/SysMenuType.js" />
/// <reference path="../../model/SchoolType.js" />
/// <reference path="../../model/StudentType.js" />
/// <reference path="../../dbserver/SchoolServer.js" />
/// <reference path="../../dbserver/StudentServer.js" />
/// <reference path="../../model/TeacherType.js" />
/// <reference path="../../dbserver/TeacherServer.js" />
/// <reference path="../../dbserver/SysMenuServer.js" />
/// <reference path="../../dbserver/EvaluatingPaperServer.js" />

Ext.onReady(function () {
    CreateView();

});
function CreateView() {
    Ext.create('Ext.container.Viewport', {
        layout: 'border',

        items: [CreateGrid()]
    });
}

var StoreGrid;
var CollegeCom;
var CollegeStore;
var SchoolCom;
var Text_StudentId;
var Text_TypeName;
var Text_LessNumber;
var Text_CP;
function LoadCollegeStore() {
    var schoolId = SchoolCom.getValue();

    DB.SchoolServer.SelectTypeSchool(parseInt(schoolId), function (rul) {
        var list = rul.ReturnValue;
        list.splice(0, 0, { Id: 0, Name: '所有学院' });
        // alert(list[0].Name);
        //alert(CollegeStore);
        CollegeStore.loadData(list);
        if (list.length > 0 && CollegeCom != null) {
            CollegeCom.setValue(0);
        }
    });
}

function CreateGrid() {
    StoreGrid = new Ext.data.Store({
        fields: [
            'Id',
            'TeacherName',
            'StudentName',
            'LoginId',
            'TypeName',
            'TeacherLoginId',
            'State',
            'SumScore',
            'SumTime',
            'EndTime',
            'CreateTime',
            'Remark',
            'TeacherPhone'

        ]
    });
    var schoolStore = new Ext.data.Store({
        fields: [
            'Id',
            'Name'
        ]
    });
    CollegeStore = new Ext.data.Store({
        fields: ['Id', 'Name']
    });
    CollegeCom = new Ext.form.ComboBox({
        displayField: 'Name',
        valueField: 'Id',
        queryMode: 'local',
        store: CollegeStore,
        width: 160
    });
    SchoolCom = new Ext.form.ComboBox({
        displayField: 'Name',
        valueField: 'Id',
        queryMode: 'local',
        store: schoolStore,
        width: 120,
        listeners: {
            change: function () {
                LoadCollegeStore();
            }
        }
    });
    store_cp = new Ext.data.Store({
        fields: ['Id', 'Name']
    });
    Text_CP = new Ext.form.ComboBox({
        queryMode: 'local',
        displayField: 'Name',
        valueField: 'Id',
        store: store_cp
    });
    store_cp.loadData([
    { Id: -1, Name: '所有数据' },
        { Id: 1, Name: '已测评' },
        { Id: 0, Name: '未测评' }
    ]);
    Text_CP.setValue(-1);
    DB.SchoolServer.SelectTypeSchool(0, function (rul) {
        var list = rul.ReturnValue;
        schoolStore.loadData(list);
        SchoolCom.setValue(list[0].Id);
    });

    Text_StudentId = new Ext.form.Text({
        width: 120
    });
    Text_TypeName = new Ext.form.Text({
        width: 120
    });

    Text_LessNumber = new Ext.form.Text({ width: 80 });
    var button = Ext.Button({
        width: 80,
        text: "筛选数据",
        listeners: {
            click: function () {
                LoadData();
            }
        }
    });
    var btn = Ext.Button({
        width: 80,
        text: "所有数据",
        listeners: {
            click: function () {
                LoadData(0, '', '');
            }
        }
    });

    var tool = new Ext.toolbar.Toolbar({
        items: [
            SchoolCom,
            CollegeCom,
            new Ext.form.Label({ text: "学号：" }),
            Text_StudentId,
             new Ext.form.Label({ text: "批次名称：" }),
            Text_TypeName,
            new Ext.form.Label({ text: "小于耗时：" }),
            Text_LessNumber,
            Text_CP,
            button,
            btn
        ]
    });



    var grid = new Ext.grid.Panel({
        columns: [
            { header: '学号', dataIndex: 'LoginId', align: 'center',width:110 },
        { header: '教师', dataIndex: 'TeacherLoginId', align: 'center' },
         { header: '教师手机', dataIndex: 'TeacherPhone', align: 'center' },
            { header: '测试名称', dataIndex: 'TypeName', align: 'center' },
            {
                header: '状态', dataIndex: 'State',
                renderer: function (value) {
                    return value ? "测评完成" : "未完成测评";
                }, align: 'center'
            },
            { header: '总分', dataIndex: 'SumScore' },
            {
                header: '测评耗时', dataIndex: 'SumTime',
                renderer: function (value) {
                    return parseInt(value / 0.6) / 100 + "分钟";
                }, align: 'center'
            },
            {
                header: '完成时间', dataIndex: 'EndTime', width: 150,
                renderer: function (value) {

                    return value.format('yyyy年MM月dd日 hh:mm');
                }, align: 'center'
            },
            {
                header: '评语', dataIndex: 'Remark',
                renderer: function (value) {
                    return '<a href="javascript:ShowWin(\'' + value + '\')">查看详情</a>';
                }
            },
            {
                header: '申请时间', dataIndex: 'CreateTime', width: 150,
                renderer: function (value) {

                    return value.format('yyyy年MM月dd日 hh:mm');
                }, align: 'center'
            }

        ]
        , tbar: tool,
        region: 'center',
        store: StoreGrid,
        bbar: CreateBBar()
    });
    LoadData(0, '', '');
    return grid;
}

function ShowWin(value) {
    Ext.Msg.alert("消息提示", value);
}

function LoadData(c, l, t) {

    //set @CollegeId=0
    //set @loginId=''
    //set @typeName='测'
    var collegeId = 0;
    var state = -1;
    if (Text_CP != null) {
        state = Text_CP.getValue();
    }
    if (CollegeCom != null) {
        collegeId = CollegeCom.getValue();
    }
    var loginId = Text_StudentId.getValue();
    var typeName = Text_TypeName.getValue();
    var time = parseFloat(Text_LessNumber.getValue());
    if (c != null && l != null && t != null) {
        collegeId = c;
        loginId = l;
        typeName = t;
    }
    time = isNaN(time) ? 1000.0 : time;

    var index = PageIndex;

    DB.EvaluatingPaperServer.EvaluatingPageList(collegeId, loginId, typeName, index, time, state, function (rul) {

        StoreGrid.loadData(rul.ReturnValue);

    });
    DB.EvaluatingPaperServer.EvaluatingPageCount(collegeId, loginId, typeName, time, state, function (rul) {
        var count = rul.ReturnValue;
        PageMaxIndex = count;
        Ext.getCmp("txt_PageNumber").setValue(index + "/" + count);
    });

}

function PageClick(index) {
    PageIndex = index;
    LoadData();
}

var PageIndex = 1;
var PageMaxIndex = 0;
function CreateBBar() {
    var upPage = new Ext.Button({
        text: '上一页',
        listeners: {
            click: function () {
                if (PageIndex - 1 > 0) {
                    PageIndex -= 1;
                } else {
                    Ext.Msg.alert("消息提示", "当前为第一页");
                }
                PageClick(PageIndex);
            }
        }
    });
    var nextPage = new Ext.Button({
        text: '下一页',
        listeners: {
            click: function () {
                if (PageIndex + 1 <= PageMaxIndex) {
                    PageIndex += 1;
                } else {
                    Ext.Msg.alert("消息提示", "当前为最后页");
                }
                PageClick(PageIndex);
            }
        }
    });
    var txt_PageNumber = new Ext.form.Text({ width: 80, id: 'txt_PageNumber' });
    var firstPage = new Ext.Button({
        text: '首页',
        listeners: {
            click: function () {
                PageClick(1);
            }
        }
    });
    var lastPage = new Ext.Button({
        text: '尾页',
        listeners: {
            click: function () {
                PageClick(PageMaxIndex);
            }
        }
    });
    var skipPage = new Ext.Button({
        text: '跳页',
        listeners: {
            click: function () {

                PageClick(parseInt(txt_PageNumber.getValue()));
            }
        }
    });

    return new Ext.toolbar.Toolbar({
        items: [
            firstPage,
            upPage,
            txt_PageNumber,
            nextPage,
            lastPage,
            skipPage

        ]
    });

}