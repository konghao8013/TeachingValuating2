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
var ExtFn;
var Store_College;
var Store_School;
var Text_CollegeId;
var Text_SchoolId;
var Text_TeacherId;
function CreateGrid() {
    Grid_Store = new Ext.data.Store({
        fields: [
            'Id',
            'teacherLoginId',
            'School',
            'CollegeName',
            'QNumber',
            'WNumber',
            'WNumber',
            'NWNumber',
            'Phone'
        ]
    });
    ExtFn = NewExtPageBar(function (index) {
        LoadData();
    });
    Store_College = new Ext.data.Store({
        fields: ["Id", "Name"]
    });
    Store_School = new Ext.data.Store({
        fields: ["Id", "Name"]
    });
    Text_CollegeId = new Ext.form.ComboBox({
        queryMode: 'local',
        valueField: 'Id',
        displayField: 'Name',
        store: Store_College
    });
    Text_SchoolId = new Ext.form.ComboBox({
        queryMode: 'local',
        valueField: 'Id',
        displayField: 'Name',
        store: Store_School,
        listeners: {
            change: function () {
                var schoolId = Text_SchoolId.getValue();
                DB.SchoolServer.SelectTypeSchool(schoolId, function (rul) {
                    var list = rul.ReturnValue;
                    list.splice(0, 0, { Id: 0, Name: '所有学院' });
                    Store_College.loadData(list);
                    Text_CollegeId.setValue(0);
                });

            }
        }
    });
   
    Text_TeacherId = new Ext.form.Text();
    DB.SchoolServer.SelectTypeSchool(0, function (rul) {
        var list = rul.ReturnValue;
        Store_School.loadData(list);
        if (list.length > 0) {
            Text_SchoolId.setValue(list[0].Id);
        }

    });

    var tool = new Ext.toolbar.Toolbar({
        items: [
            Text_SchoolId,
            Text_CollegeId,
             new Ext.form.Label({ text: '教师帐号' }),
            Text_TeacherId,
            new Ext.Button({
                text: '筛选',
                listeners: {
                    click: function () {
                        LoadData();
                    }
                }
            })

        ]
    });


    var grid = new Ext.grid.Panel({
        columns: [
            { header: '教师名称', dataIndex: 'Name', width: 120 },
            { header: '教师帐号', dataIndex: 'teacherLoginId', width: 120 },
            { header: '联系手机', dataIndex: 'Phone', width: 120 },
            { header: '学校名称', dataIndex: 'School', width: 120 },
            { header: '学院名称', dataIndex: 'CollegeName', width: 120 },
            { header: '工作总量', dataIndex: 'QNumber', width: 120 },
            { header: '已完成数量', dataIndex: 'WNumber', width: 120 },
            { header: '未完成数量', dataIndex: 'NWNumber', width: 120 }
        ],
        region: 'center',
        tbar: tool,
        store:Grid_Store,
        bbar: ExtFn.CreatePageBBar()
    });
    LoadData();
    return grid;
}

function LoadData() {
    var index = ExtFn.pageIndex;
    
    var teacherId = Text_TeacherId.getValue();
    var collegeId = Text_CollegeId.getValue();
    collegeId = collegeId == null ? 0 : collegeId;
   
    DB.TeacherServer.SelectWorkList(teacherId, collegeId, index, function (rul) {
        var list = rul.ReturnValue;
        Grid_Store.loadData(list);
    });
    DB.TeacherServer.SelectWorkCount(teacherId, collegeId, function (rul) {
        var count = rul.ReturnValue;
        ExtFn.pageMaxIndex = count;
       
        ExtFn.setTxt_Number(index + '/' + count);
    });
}