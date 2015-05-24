/// <reference path="../../extjs/js/ext-all.js" />
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
/// <reference path="../../model/SysMenuType.js" />
/// <reference path="../../model/SchoolType.js" />
/// <reference path="../../model/StudentType.js" />
/// <reference path="../../dbserver/SchoolServer.js" />
/// <reference path="../../dbserver/StudentServer.js" />
/// <reference path="../../model/TeacherType.js" />
/// <reference path="../../dbserver/TeacherServer.js" />
/// <reference path="../../dbserver/SysMenuServer.js" />
/// <reference path="../../dbserver/ParperSendServer.js" />
/// <reference path="../../dbserver/StudentRecordServer.js" />
/// <reference path="../../model/ParperSendType.js" />
/// <reference path="../../ExtjsFn.js" />
/// <reference path="../../dbserver/StudentScoreServer.js" />
Ext.onReady(function () {
    CreateView();
});
function CreateView() {
    Ext.create('Ext.container.Viewport', {
        layout: 'border',

        items: [CreateGrid()]
    });
}

var Grid_Store;
var Text_StudentId;
var CollegeStore;
var CollegeCom;
var SchoolCom;
var store_cp;
var Text_CP;
var ExtFn;
function CreateGrid() {
    Grid_Store = new Ext.data.Store({
        fields: [
            'Id',
            'Name',
            'LoginId',
            'School',
            'College',
            'TeachingName',
            'TeachingPlan',
            'Courseware',
            'CoursewareName',
            'Video',
            'VideoName',
            'Reflection',
            'Apply',
            'PaperId'
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
                var schoolId = SchoolCom.getValue();
                DB.SchoolServer.SelectTypeSchool(schoolId, function (rul) {
                    var list = rul.ReturnValue;
                    list.splice(0, 0, { Id: 0, Name: '所有学院' });
                    CollegeStore.loadData(list);
                    CollegeCom.setValue(0);
                });
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
        { Id: 0, Name: '未测评' },
        { Id: -2, Name: '未提交档案' }
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



    var button = Ext.Button({
        width: 80,
        text: "筛选数据",
        listeners: {
            click: function () {
                LoadData();
            }
        }
    });


    var tool = new Ext.toolbar.Toolbar({
        items: [
            SchoolCom,
            CollegeCom,
            new Ext.form.Label({ text: "学号：" }),
            Text_StudentId,
             new Ext.form.Label({ text: "测评状态：" }),
            Text_CP,
            button

        ]
    });

    ExtFn = NewExtPageBar(function (index) {
        LoadData();
    });
    var grid = new Ext.grid.Panel({
        columns: [
            { header: '学生姓名', dataIndex: "Name", width: 80, align: 'center' },
            { header: '登录帐号', dataIndex: "LoginId", width: 120, align: 'center' },
            { header: '学校名称', dataIndex: "School", width: 120, align: 'center' },
            { header: '学院名称', dataIndex: "College", width: 120, align: 'center' },
            {
                header: '教案材料', dataIndex: "TeachingName", width: 120,
                renderer: function (value, b, c) {
                    if (value.length > 0) {
                        return '<a href="javascript:CreateUrl(\'' + c.data.TeachingPlan + '\')">' + value + '</a>';
                    }
                    return "";
                }, align: 'center'
            },
            {
                header: '课件材料', dataIndex: "CoursewareName", width: 120,
                renderer: function (value, b, c) {
                    if (value.length > 0) {
                        return '<a href="javascript:CreateUrl(\'' + c.data.Courseware + '\')">' + value + '</a>';
                    }
                    return "";
                }, align: 'center'
            },
            {
                header: '微格视频', dataIndex: "VideoName", width: 120,
                renderer: function (value, b, c) {
                    if (value.length > 0) {
                        return '<a href="javascript:CreateUrl(\'' + c.data.Video + '\')">' + value + '</a>';
                    }
                    return "";
                }, align: 'center'
            },
            {
                header: '反思材料', dataIndex: "ReflectionName", width: 120,
                renderer: function (value, b, c) {
                    if (value.length > 0) {
                        return '<a href="javascript:CreateUrl(\'' + c.data.Reflection + '\')">' + value + '</a>';
                    }
                    return "";
                }, align: 'center'
            },
            {
                header: '是否测评', dataIndex: "Apply", width: 80,
                renderer: function (value) {
                    return value ? "教师已测评" : "教师未测评";
                }, align: 'center'
            },
            {
                header: '操作', dataIndex: "PaperId", width: 200,
                renderer: function (value, b, c) {
                    var text = "";
                    if (!c.data.Apply&&value>0) {
                        text += '<a href="javascript:BackFile(\'' + value + '\')">打回材料</a>';
                        text += '<a style="margin-left: 15px;" href="javascript:ReSetData(' + c.data.Rid + ')">清除资料</a>';
                    }else if (value > 0) {
                        text += '<a style="margin-left: 15px;" href="javascript:ReSetData(' + c.data.Rid + ')">清除资料</a>';
                    }
                    else {
                        return "不可操作";
                    }
                    return text;
                }, align: 'center'
            }
        ],
        tbar: tool,
        bbar: ExtFn.CreatePageBBar(),
        store: Grid_Store,
        region: 'center'

    });
    LoadData();
    return grid;

}

function CreateUrl(url) {
    window.open(url, 'file_download');
}
function ReSetData(studentId) {
    DB.StudentServer.ResetStudentData(studentId, function() {
        LoadData();
    });
  //  alert(studentId);
    //LoadData();
}
function BackFile(id) {
    id = parseInt(id);
    
    //打回材料
    if (confirm("是否打回材料")) {
        DB.ParperSendServer.BackPaper(id, function () {
            LoadData();
            Ext.Msg.alert("消息提示","成功打回");
        });
    }
}

function LoadData() {
    var index = ExtFn.pageIndex;
    var collegeId = CollegeCom.getValue();
    var studentId = Text_StudentId.getValue();
    collegeId = collegeId == null ? 0 : collegeId;
    var apply = Text_CP.getValue();
    DB.StudentServer.SelectPaperList(apply, collegeId, studentId, index, function (rul) {
        var list = rul.ReturnValue;
        Grid_Store.loadData(list);
    });
    DB.StudentServer.SelectPaperCount(apply, collegeId, studentId, function (rul) {
        var count = rul.ReturnValue;
        ExtFn.pageMaxIndex = count;
        ExtFn.setTxt_Number(index + '/' + count);
    });
}
