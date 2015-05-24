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
var Text_CollegeId;
var Text_SchoolId;
var Text_TypeName;
var Text_StudentId;
var Store_School;
var Store_College;
function CreateGrid() {
    Grid_Store = new Ext.data.Store({
        fields: [
            'Id',
            'Name',
            'StudentName',
            'StudentLoginId',
            'School',
            'College',
            'TeachingPlanScore',
            'CoursewareScore',
            'PresentScore',
            'PresentScore',
            'VideoScore',
            'Innovate',
            'Remark',
            'SumScore',
             'SumScore',
             'AppraisalNumber'
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
    Text_TypeName = new Ext.form.Text();
    Text_StudentId = new Ext.form.Text();
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
            new Ext.form.Label({ text: '测评批次' }),
            Text_TypeName,
             new Ext.form.Label({ text: '学生帐号' }),
            Text_StudentId,
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
        store: Grid_Store,
        columns: [
            { header: '测评批次', dataIndex: 'Name', width: 120, align: 'center' },
            { header: '学生姓名', dataIndex: 'StudentName', width: 60, align: 'center' },
            { header: '学生帐号', dataIndex: 'StudentLoginId', width: 120, align: 'center' },
         { header: '测评次数', dataIndex: 'AppraisalNumber', width: 60, align: 'center' },
            { header: '学校名称', dataIndex: 'School', width: 120, align: 'center' },
            { header: '学院名称', dataIndex: 'College', width: 120, align: 'center' },
            { header: '教案', dataIndex: 'TeachingPlanScore', width: 40, align: 'center' },
            { header: '课件', dataIndex: 'CoursewareScore', width: 40, align: 'center' },
            { header: '说课', dataIndex: 'PresentScore', width: 40, align: 'center' },
            { header: '视频', dataIndex: 'VideoScore', width: 40, align: 'center' },
            { header: '创新', dataIndex: 'Innovate', width: 40, align: 'center' },
            {
                header: '评语', dataIndex: 'Remark', width: 60,
                renderer: function (value) {
                    return '<a href="javascript:winShow(\'' + value + '\')">查看</a>';
                }, align: 'center'
            },
            { header: '总分', dataIndex: 'SumScore', width: 40, align: 'center' },
            {
                header: '完成时间', dataIndex: 'CreateTime', width: 140,
                renderer: function (value) {
                    return value.format('yy年MM月dd日 hh:mm');
                }, align: 'center'
            }
        ],
        tbar: tool,
        bbar: ExtFn.CreatePageBBar(),
        region: 'center'
    });
    LoadData();
    return grid;
}

function winShow(value) {
    Ext.Msg.alert("查看评语", value);
}

function LoadData() {
    //ExtFn = NewExtPageBar(function (index) {

    //});
    var index = ExtFn.pageIndex;
    var schoolId = Text_SchoolId.getValue();
    var collegeId = Text_CollegeId.getValue();
    var typeName = Text_TypeName.getValue();
    collegeId = collegeId == null ? 0 : collegeId;
    var studentId = Text_StudentId.getValue();
    DB.StudentScoreServer.SelectStudentScore(typeName, studentId, collegeId, index, function (rul) {
        var list = rul.ReturnValue;
        Grid_Store.loadData(list);
    });
    DB.StudentScoreServer.SelectStudentScoreCount(typeName, studentId, collegeId, function (rul) {
        var count = rul.ReturnValue;
        ExtFn.pageMaxIndex = count;
        ExtFn.setTxt_Number(index + '/' + count);
    });

}