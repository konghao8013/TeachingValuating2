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
/// <reference path="../../model/ParperSendType.js" />

Ext.onReady(function () {
    CreateView();
});

function CreateView() {
    Ext.create('Ext.container.Viewport', {
        layout: 'border',

        items: [CreateGrid()]
    });
}

var COMM;
var GRIDSTORE;
function CreateGrid() {
    var comStore = new Ext.data.Store({
        fields:["key","text"]
    });
    var array = [
        { key: 0, text: "未处理" },
        { key: 1, text: "已处理" }
    ];
    comStore.loadData(array);
     COMM = new Ext.form.ComboBox({
        fieldLabel: "操作状态",
        store: comStore,
        displayField: 'text',
        valueField: 'key',
        queryMode: 'local',
        listeners: {
            select: function() {
                LoadGridData();
            }
        },
       
    });
    COMM.setValue(0);
    var tool = new Ext.toolbar.Toolbar({
        borderPadding: 0,
        border: 0,
        items: [COMM]
    });

    GRIDSTORE = new Ext.data.Store({
        fields: ["Id",
            "TeacherId",
            "PaperId",
            "CreateTime",
            "Content",
            "TeacherName",
            "StudentName",
            "TeachingPlan",
            "Courseware",
            "Video",
            "Reflection",
            "TeachingName",
            "CoursewareName",
            "VideoName",
            "ReflectionName",
            "State",
            "IsOK"
        ]
    });
    var grid = new Ext.grid.Panel({
        store: GRIDSTORE,
        columns: [
             { header: '教师名称', dataIndex: 'TeacherName', width: 100, align: 'center', sortable: true, xtype: 'gridcolumn', editable: false },
            { header: '学生名称', dataIndex: 'StudentName', width: 100, align: 'center', sortable: true, xtype: 'gridcolumn', editable: false },
            {
                header: '教案文件', dataIndex: 'TeachingName', width: 200,
                renderer: function (a, b, c, d) {
                    return '<a href="javascript:WDownload(\'' + c.data.TeachingPlan + '\')">' + a + '</a>';
                }, align: 'center', sortable: true, xtype: 'gridcolumn', editable: false
            },
            {
                header: '课件文件', dataIndex: 'CoursewareName', width: 200,
                renderer: function (a, b, c, d) {
                    return '<a href="javascript:WDownload(\'' + c.data.Courseware + '\')">' + a + '</a>';
                }, align: 'center', sortable: true, xtype: 'gridcolumn', editable: false
            },
            {
                header: '微格视频', dataIndex: 'VideoName', width: 200,
                renderer: function (a, b, c, d) {
                    return '<a href="javascript:WDownload(\'' + c.data.Video + '\')">' + a + '</a>';
                }, align: 'center', sortable: true, xtype: 'gridcolumn', editable: false
            },
            {
                header: '反思文件', dataIndex: 'ReflectionName', width: 200,
                renderer: function (a, b, c, d) {
                    return '<a href="javascript:WDownload(\''+c.data.Reflection+'\')">'+a+'</a>';
                }, align: 'center', sortable: true, xtype: 'gridcolumn', editable: false
            },
            {
                header: '退回原因', dataIndex: 'Content', width: 200 ,
                renderer: function (value, a,b,c,d) {
                  

                    return '<a href="javascript:ExtShow(\''+value+'\')">' + value + '</a>';
                }, align: 'center', sortable: true, xtype: 'gridcolumn', editable: false
                
            },
            {
                header:'状态',dataIndex:'Id',width:180,
                renderer: function (a, b, c) {
                    var html = "<div>";
                  
                    html += '<input type="button" class="bt_Button"  onclick="OK(\'' + c.data.Id + '\')" value="同意"/>';
                    html += '<input type="button" class="bt_Button"  onclick="NoOK(\'' + c.data.Id + '\')" value="不同意"/>';
                    html += "</div>";
                    if (c.data.State != 0) {
                        return c.data.IsOK == 1 ? "同意" : "不同意";
                    }
                    return html;
                }, align: 'center', sortable: true, xtype: 'gridcolumn', editable: false
            }
        ],
        region: 'center',
        height: 300,
        forceFit: true,
        width: 700,
        listeners: {

        },
        tbar: tool
    });
    //actioncolumn
    //加载数据
    LoadGridData();

    return grid;
}

function WDownload(url) {
    window.open(url,'download');
};
function LoadGridData() {
    var cs = COMM.getValue() == 1;
    DB.ParperSendServer.ListPaper(cs, function (rul) {
        var list = rul.ReturnValue;
        GRIDSTORE.loadData(list);
    });
}

function ExtShow(value) {
    Ext.Msg.alert("消息提示",value);
}

function OK(id) {
    $.Json("/super/BackStudent", "ISOK", { id: id }, function () {
        LoadGridData();
        
    });
}
function NoOK(id) {
    $.Json("/super/BackStudent", "NOOK", { id: id }, function () {
        LoadGridData();
    });
}

function OPWindow(url) {
    window.open(url);
}