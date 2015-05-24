

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
/// <reference path="../../ExtjsFn.js" />
/// <reference path="../../extjs/js/ext-all.js" />
Ext.onReady(function () {
    CreateGridView();
});
var PaperStore;
var TeacherTypeComBox;
var SchoolComBox;
var CollegeComBox;
var CollegeStore;
var ExtPageIndex;
var PageSizeComBox;
var PaperSelModel;
var PaperGrid;
function CreateGridView() {
    //b.TeachingPlan,b.TeachingName,b.Courseware,b.CoursewareName,b.Video,b.VideoName,b.Reflection,b.ReflectionName
    PaperStore = new Ext.data.Store({
        fields: ['Id',
            'UserId',
            'PaperId',
            'StudentId',
            'TeacherName',
            'StudentName',
            'TeachingPlan',
            'TeachingName',
            'Courseware',
            'CoursewareName',
            'Video',
            'VideoName',
            'Reflection',
            'ReflectionName',
            'TeacherTypeId',
            'College',
        'LoginId']
    });
    var teacherTypeStore = new Ext.data.Store({
        fields: ['Id', 'Name']
    });
    var teacherTypeList = DB.EnumTeacherTypeJson();
    //  teacherTypeList.splice(0, 0, { Id: 0, Name: '所有类别' });
    teacherTypeStore.loadData(teacherTypeList);
    TeacherTypeComBox = new Ext.form.ComboBox({
        queryMode: 'local',
        displayField: 'Name',
        valueField: 'Id',
        store: teacherTypeStore
    });
    TeacherTypeComBox.setValue(teacherTypeList[0].Id);
    var pageSizeStore = new Ext.data.Store({
        fields: ['Id', 'Name']
    });
    pageSizeStore.loadData([{ Id: 20, Name: '20条每页' }, { Id: 50, Name: '50条每页' }, { Id: 100, Name: '100条每页' }]);
    PageSizeComBox = new Ext.form.ComboBox({
        queryMode: 'local',
        displayField: 'Name',
        valueField: 'Id',
        store: pageSizeStore
    });
    PageSizeComBox.setValue(20);
    var schoolStore = new Ext.data.Store({
        fields: ['Id', 'Name']
    });

    CollegeStore = new Ext.data.Store({
        fields: ['Id', 'Name']
    });
    CollegeComBox = new Ext.form.ComboBox({
        queryMode: 'local',
        displayField: 'Name',
        valueField: 'Id',
        store: CollegeStore,
        listeners: {
            change: function() {
                LoadPaperStoreView();
            }
        }
    });
    SchoolComBox = new Ext.form.ComboBox({
        queryMode: 'local',
        displayField: 'Name',
        valueField: 'Id',
        store: schoolStore,
        listeners: {
            change: function () {
                var typeId = SchoolComBox.getValue();
                DB.SchoolServer.SelectTypeSchool(typeId, function (rul) {
                    var list = rul.ReturnValue;
                    list.splice(0, 0, { Id: 0, Name: '所有学院' });
                    CollegeStore.loadData(list);
                });
            }
        }
    });
    DB.SchoolServer.SelectTypeSchool(0, function (rul) {
        var list = rul.ReturnValue;
        list.splice(0, 0, { Id: -1, Name: '所有学校' });
        schoolStore.loadData(list);
    });
    PaperSelModel = Ext.create('Ext.selection.CheckboxModel');
    var ttoolBar = new Ext.toolbar.Toolbar({
        items: [
            new Ext.form.Label({ text: '教师类别' }),
            TeacherTypeComBox,
            new Ext.form.Label({ text: '学校：' }),
            SchoolComBox,
            new Ext.form.Label({ text: '学院：' }),
            CollegeComBox,
            new Ext.form.Label({ text: '数量：' }),
            PageSizeComBox,
            new Ext.Button({
                text: '筛选',
                listeners: {
                    click: function () {
                        LoadPaperStoreView();
                    }
                }
            }),
            new Ext.Button({
                text: '修改选中试卷的专家',
                listeners: {
                    click: function () {
                        CreateWin();
                       
                    }
                }
            }),
            new Ext.Button({
                text: '按照学院重新分配',
                listeners: {
                    click: function() {
                    var college = CollegeComBox.getValue();
                    if (college == null || college == 0) {
                        Ext.Msg.alert('错误提示', "请选择需要重新分配的学院");
                        return;
                    }
                    var text = CollegeComBox.getRawValue();
                   

                  
                    Ext.Msg.confirm("系统提示", "是否重新分配" + text + "的试卷", function (value) {
                        if (value === "yes") {
                            var rate = NewExtRate("正在重新分配试卷请等待");
                            rate.StartExtFnBox();
                            DB.EvaluatingPaperServer.UpdateCollege(college, function () {
                                Ext.Msg.alert("系统提示", text + "的试卷重新分配成功");
                                rate.EndExtFnBox();
                                LoadPaperStoreView();
                            });
                        } else {
                            Ext.Msg.alert("系统提示","取消重新分配"+text+"的试卷");
                        }
                        
                    });


                    }
                }
            })
        ]
    });
    ExtPageIndex = NewExtPageBar(function (index) {
        LoadPaperStoreView();
    });

    PaperGrid = new Ext.grid.Panel({
        renderTo: Ext.getBody(),
        selModel: PaperSelModel,
        region: 'Center',
        columns: [
             { header: '档案编号', dataIndex: 'Id', width: 80 },
            { header: '学生名称', dataIndex: 'StudentName', width: 100 },
            { header: '学生帐号', dataIndex: 'LoginId', width: 120 },
            { header: '学院信息', dataIndex: 'College', width: 120 },
            { header: '抽取专家', dataIndex: 'TeacherName', width: 100 },
            { header: '专家帐号', dataIndex: 'TeacherLoginId', width: 100 },
            {
                header: '试卷状态', dataIndex: 'TeacherTypeId', width: 120,
                renderer: function (value) {
                    return value == 1 ? "一评校内专家" : value == 2 ? "一评校外专家" : value == 3 ? "二评专家" : value == 4 ? "仲裁专家" : "未知";
                }
            },
            {
                header: '教案材料', dataIndex: "TeachingName", width: 120,
                renderer: function (value, b, c) {
                    return '<a href="javascript:CreateUrl(\'' + c.data.TeachingPlan + '\')">' + value + '</a>';
                }, align: 'center'
            },
            {
                header: '课件材料', dataIndex: "CoursewareName", width: 120,
                renderer: function (value, b, c) {
                    return '<a href="javascript:CreateUrl(\'' + c.data.Courseware + '\')">' + value + '</a>';
                }, align: 'center'
            },
            {
                header: '微格视频', dataIndex: "VideoName", width: 120,
                renderer: function (value, b, c) {
                    return '<a href="javascript:CreateUrl(\'' + c.data.Video + '\')">' + value + '</a>';
                }, align: 'center'
            },
            {
                header: '反思材料', dataIndex: "ReflectionName", width: 120,
                renderer: function (value, b, c) {
                    return '<a href="javascript:CreateUrl(\'' + c.data.Reflection + '\')">' + value + '</a>';
                }, align: 'center'
            }


        ],
        tbar: ttoolBar,
        bbar: ExtPageIndex.CreatePageBBar(),
        height: $(window).height(),
        store: PaperStore
    });
    LoadPaperStoreView();

}

var LoginText;
var TeacherStore;
var TeacherWin;
function CreateWin() {
    var schoolId = SchoolComBox.getValue();
    var collegeId = CollegeComBox.getValue();
    var teacherTypeId = TeacherTypeComBox.getValue();
    if (teacherTypeId == null || teacherTypeId==0) {
        Ext.Msg.alert('错误提示', "请选择需要分配正确的测评专家类别");
        return;
    }
    if (schoolId == null || schoolId == -1) {
        Ext.Msg.alert('错误提示', "请选择正确的学校");
        return;
    }

    if ((collegeId == null || collegeId == 0)&&teacherTypeId<3) {
        Ext.Msg.alert('错误提示', "请选择正确的学院");
        return;
    }
    //if ((schoolId == null || collegeId == null || schoolId == 0 || collegeId == 0)&&teacherTypeId<3) {
    //    Ext.Msg.alert('错误提示', "请选择正确的学院和学校");
    //    return;
    //}
    TeacherStore = new Ext.data.Store({
        fields: [
            'Id',
            'Rid',
            'Name',
            'Email',
            'Phone',
            'School',
            'CollegeName',
            'LoginId',
            'TypeId'

        ]
    });
     LoginText = new Ext.form.Text();
    var ttoolBar = new Ext.toolbar.Toolbar({
        items: [
            new Ext.form.Label({text:"教师帐号："}),
            LoginText,
            new Ext.Button({
                text: '筛选数据',
                listeners: {
                    click: function () {
                        LoadWinGridTeacher(schoolId, collegeId, teacherTypeId, LoginText.getValue());
                    }
                }

            })
        ]
    });
    var grid = new Ext.grid.Panel({
        store: TeacherStore,
        columns: [
            { header: '教师名称', dataIndex: 'Name', width: 120 },
             { header: '登录帐号', dataIndex: 'LoginId', width: 120 },
            { header: '联系邮箱', dataIndex: 'Email', width: 120 },
            { header: '手机号码', dataIndex: 'Phone', width: 120 },
            { header: '所在学校', dataIndex: 'School', width: 120 },
            { header: '所在学院', dataIndex: 'CollegeName', width: 120 },
            {
                header: '教师类别', dataIndex: 'TypeId', width: 120,
                renderer: function (value) {
                return value == 1 ? "一评校内专家" : value == 2 ? "一评校外专家" : value == 3 ? "二评专家" : value == 4 ? "仲裁专家" : "未知";
            } },
            {
                header: '操作', dataIndex: 'Rid', width: 120,
                renderer: function (value) {
                    return "<a href='javascript:InitPaper(\""+value+"\")'>指派该教师阅卷</a>";
                }
            }
        ],
        width: 980,
        height: 400,
        tbar: ttoolBar
    });
    TeacherWin = new Ext.window.Window({
        title: '给选中试卷指派专家',
        width: 1000,
        height: 450,

        items: [
            grid
        ]
    });

    TeacherWin.show();
    LoadWinGridTeacher(schoolId, collegeId,teacherTypeId,LoginText.getValue());
}

function InitPaper(teacherId) {
    var rows = PaperGrid.getSelectionModel().getSelection();
    var paperIds = new Array();
    if (rows.length == 0) {
        Ext.Msg.alert("错误提示","请选择需要分配的试卷");
        return;
    }
   for (var i = 0; i < rows.length; i++) {
       paperIds[paperIds.length] = rows[i].data.Id;
   }
   DB.EvaluatingPaperServer.UpdatePapers(JSON.stringify(paperIds), parseInt(teacherId), function () {
       Ext.Msg.alert("提示信息","修改成功！");
       TeacherWin.close();
       LoadPaperStoreView();
   });
  
}

function LoadWinGridTeacher(schoolId,collegeId,teacherTypeId,loginId) {
    if (teacherTypeId == 3 || teacherTypeId == 4) {
        collegeId = 0;
      //  schoolId = 0;
    }
    DB.TeacherServer.SelectTeacherList(schoolId, collegeId, teacherTypeId, loginId, function(rul) {
        var list = rul.ReturnValue;
        TeacherStore.loadData(list);
    });
}

function LoadPaperStoreView() {
    // ExtPageIndex = NewExtPageBar();
    var index = ExtPageIndex.pageIndex;
    var collegeId = CollegeComBox.getValue();
    var size = PageSizeComBox.getValue();
    var teacherTypeId = TeacherTypeComBox.getValue();
    var schoolId = SchoolComBox.getValue();
    collegeId = collegeId == null ? 0 : collegeId;
    teacherTypeId = teacherTypeId == null ? 0 : teacherTypeId;
    schoolId = schoolId == null ? 0 : schoolId;
    DB.EvaluatingPaperServer.SelectPaper(size, index, collegeId, teacherTypeId, schoolId, function (rul) {
        var list = rul.ReturnValue;
        PaperStore.loadData(list);
    });


    DB.EvaluatingPaperServer.PaperCount(size, collegeId, teacherTypeId, schoolId, function (rul) {
        var count = rul.ReturnValue;
        ExtPageIndex.pageMaxIndex = count;
        ExtPageIndex.setTxt_Number(index + '/' + count);
    });

}
function CreateUrl(url) {
    window.open(url, 'file_download');
}