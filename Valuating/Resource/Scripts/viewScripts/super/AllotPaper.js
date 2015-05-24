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

Ext.onReady(function () {
    CreateView();
});
function CreateView() {
    Ext.create('Ext.container.Viewport', {
        layout: 'border',

        items: [CreateGrid()]
    });
}

var Store_Grid;
var Text_StudentLoginId;
var Text_TeacherLoginId;
var Text_Com_OK;
var Text_TeacherType;
function CreateGrid() {
    Store_Grid = new Ext.data.Store({
        fields: [
            'Id',
            'UserId',
            'PaperId',
            'StudentId',
            'TeacherLoginId',
            'TeacherName',
            'LoginId',
            'StudentName',
            'College',
            'School'

        ]
    });

    Text_StudentLoginId = new Ext.form.Text();
    Text_TeacherLoginId = new Ext.form.Text();
    var oKStore = new Ext.data.Store({
        fields: ['Id', "Name"]
    });
    Text_Com_OK = new Ext.form.ComboBox({
        queryMode: 'local',
        store: oKStore,
        displayField: 'Name',
        valueField: 'Id'
    });

    oKStore.loadData([{ Id: 0, Name: '所有数据' }, { Id: 1, Name: '未分配教师' }]);
    Text_Com_OK.setValue(1);
    var teacherTypeStore = new Ext.data.Store({
        fields:['Id','Name']
    });
    teacherTypeStore.loadData([
        { Id: 0, Name: '所有类别' },
        { Id: 1, Name: '一评校内专家' },
        { Id: 2, Name: '一评校外专家' },
        { Id: 3, Name: '二评专家' },
        { Id: 4, Name: '仲裁专家' }
    ]);
    Text_TeacherType = new Ext.form.ComboBox({
        queryMode: 'local',
        displayField: 'Name',
        valueField: 'Id',
        store:teacherTypeStore
    });
    var plBtn = new Ext.Button({
        text: '按学院重新分配',
        width: 120,
        listeners: {
            click: function() {
                WindwoSchoolTeacher();
            }
        }
    });
    var plTeacherBtn = new Ext.Button({
        text: '按专家指派试卷',
        width: 120,
        listeners: {
            click: function () {
                WindwoTeacherTeacher();
            }
        }
    });
    Text_TeacherType.setValue(0);
    var btn = new Ext.Button({
        text: '筛选数据',
        listeners: {
            click: function () {
                LoadData(1);
            }
        }
    });
    var ttool = new Ext.toolbar.Toolbar({
        items: [
            new Ext.form.Label({ text: '学生帐号：' }),
            Text_StudentLoginId,
            new Ext.form.Label({ text: '教师帐号：' }),
            Text_TeacherLoginId,
            new Ext.form.Label({ text: '状态：' }),
            Text_Com_OK,
            Text_TeacherType
            ,
            btn
            //plTeacherBtn

        ]
    });
    GridExtPageFn = NewExtPageBar(function (index) {
        LoadData();
    });
    var grid = new Ext.grid.Panel({
        region: 'center',
        store: Store_Grid,
        columns: [
           
            { header: '试卷编号', dataIndex: 'PaperId', align: 'center' },
            { header: '学号', dataIndex: 'LoginId', width: 200, align: 'center' },
            { header: '学生名称', dataIndex: 'StudentName', align: 'center' },
             { header: '学校', dataIndex: 'School', align: 'center' },
            { header: '学院', dataIndex: 'College', align: 'center',width:130 },

            { header: '教师编号', dataIndex: 'TeacherLoginId', align: 'center' },

            { header: '教师名称', dataIndex: 'TeacherName', align: 'center' },
            {
                header: '教师类别', dataIndex: 'TeacherTypeId',
                renderer: function(value) {
                    return value == 1 ? "一评第校内专家" : value == 2 ? "一评第校外专家" : value == 3 ? "二评专家" : value == 4 ? "仲裁专家" : "未知";
                }, align: 'center'
            },
            {
                header: '操作', dataIndex: 'Id', width: 200,
                renderer: function (a, b, c) {
                    return '<a href="javascript:ShowWin(\'' + a + '\')">分配教师</a>';
                }, align: 'center'
            }
        ],
        tbar: ttool,
        bbar: GridExtPageFn.CreatePageBBar()
    });
    LoadData();
    return grid;
}



var GridExtPageFn;
function LoadData(dindex) {
    var stuId = "";
    var teacherId = "";
    var teacherTypeId = Text_TeacherType.getValue();
    var isOk = false;
    if (Text_StudentLoginId != null)
        stuId = Text_StudentLoginId.getValue();
    if (Text_TeacherLoginId != null)
        teacherId = Text_TeacherLoginId.getValue();
    if (Text_Com_OK != null)
        isOk = Text_Com_OK.getValue() == 1 ? false : true;

    var index = GridExtPageFn.pageIndex;

    index = dindex != null ? dindex : isNaN(index) ? 1 : index;
    
    //alert(index);
    DB.EvaluatingPaperServer.EvaluatingAllotList(stuId, teacherId, isOk,teacherTypeId, index, function (rul) {
        var list = rul.ReturnValue;
        Store_Grid.loadData(list);

    });
    DB.EvaluatingPaperServer.EvaluatingAllotCount(stuId, teacherId, isOk,teacherTypeId, function (rul) {
        var count = rul.ReturnValue;

        GridExtPageFn.pageMaxIndex = count;
        GridExtPageFn.setTxt_Number(index + "/" + count);

    });
}

var TeacherWin;
function ShowWin(id) {
    TeacherWin = new Ext.window.Window({
        title: '教师选择',
        width: 800,
        height: 410,
        items: [CreateTeacherGird(id)]
    });
    TeacherWin.show();
    // return win;
}

var Store_Teacher;
var School_Com;
var College_Com;
var Store_School;
var College_Store;
function LoadCollegeName() {

    var schoolId = School_Com.getValue();
    DB.SchoolServer.SelectTypeSchool(schoolId, function (rul) {

        var list = rul.ReturnValue;
        list.splice(0,0, {Id:0,Name:'所有学院'});
        College_Store.loadData(list);
        College_Com.setValue(0);
    });
}

function CreateTeacherGird(did) {


    Store_School = new Ext.data.Store({
        fields: ['Id', 'Name'],
        id: 'store_school_Id'
    });
    School_Com = new Ext.form.ComboBox({
        queryMode: 'local',
        store: Store_School,
        displayField: 'Name',
        valueField: 'Id',
        listeners: {
            change: function () {

                LoadCollegeName();
            }
        }

    });
    College_Store = new Ext.data.Store({
        fields: ['Id', 'Name']
    });
    College_Com = new Ext.form.ComboBox({
        queryMode: 'local',
        displayField: 'Name',
        valueField: 'Id',
        listeners: {
            change: function () {
                //alert("bb");
                // LoadCollegeName();
            }
        },
        store: College_Store
    });

    var btn = new Ext.Button({
        text: "筛选数据",
        listeners: {
            click: function () {
                LoadTeacherData(1);
            }
        }
    });
    
    DB.SchoolServer.SelectTypeSchool(0, function (rul) {
        var list = rul.ReturnValue;
        // alert(list[0].Id);

        Store_School.loadData(list);

        if (list.length > 0) {
            School_Com.setValue(list[0].Id);
        }
    });
   

    Store_Teacher = new Ext.data.Store({
        fields: [
            'Id',
            'Rid',
            'Name',
            'Email',
            'Phone',
            'School',
            'SchoolId',
            'CollegeId',
            'TypeId',
            'State',
            'CollegeName',
            'TypeId'
        ],
        id: 'store_teacher_Id'
    });
   
    var ttool = new Ext.toolbar.Toolbar({
        items: [
            new Ext.form.Label({ text: '选择学校:' }),
            School_Com,
            new Ext.form.Label({ text: '选择学院' }),
            College_Com,
            btn
          
        ]
    });
    
    TeacherPage = NewExtPageBar(function (index) {
        LoadTeacherData();
    });
   
    var grid = new Ext.grid.Panel({
        store: Store_Teacher,
        columns: [
           { header: '教师名称', dataIndex: 'Name', align: 'center' },
            {
                header: '教师类别', dataIndex: 'TypeId',
                renderer: function(value) {
                    return value == 1 ? "一评校内专家" : value == 2 ? "一评校外专家" : value == 3 ? "二评专家" : value == 4 ? "仲裁专家" : "未知";
                }, align: 'center'
            },
            { header: '手机', dataIndex: 'Phone', align: 'center' },
            { header: '所属学校', dataIndex: 'School', align: 'center' },
            {
                header: '所属学院', dataIndex: 'CollegeName',width:120, renderer: function (value, b, c) {
                 
                    return value;
                }, align: 'center'
            },
            {
                header: '账户状态', dataIndex: 'State',
                renderer: function(value) {
                    return value ? "已启用" : "已禁用";
                }, align: 'center'
            },
            {
                header: '操作', dataIndex: 'Id',
                renderer: function(value,b,c) {
                    return "<a href='javascript:UpdateXG(\""+did+"\",\""+(c.data.Rid)+"\")'>指派该教师阅卷</a>";
                }, align: 'center'
            }
        ],
        tbar: ttool,
        width: 790,
        height:380,
        bbar: TeacherPage.CreatePageBBar(),
        id: 'grid_teacher_Id'
    });

    
    
    return grid;
}

function UpdateXG(did, value) {
    did = parseInt(did);
    value = parseInt(value);
    DB.EvaluatingPaperServer.UpdateTeacher(did, value, function () {
        Ext.Msg.alert("消息提示","设置成功");
       
        TeacherWin.close();
        LoadData();
    });
}

var TeacherPage;
function LoadTeacherData(dindex) {
    var schoolId = 0;
    var collegeId = 0;

    if (College_Com != null) {
        collegeId = College_Com.getValue();
    }
    if (School_Com != null) {
        schoolId = School_Com.getValue();
    }
    var index =dindex!=null?dindex:TeacherPage.pageIndex;
    schoolId = schoolId == null ? 0 : schoolId;
    DB.TeacherServer.SelectSchoolTeacherList(schoolId, collegeId, index, function (rul) {
        var list = rul.ReturnValue;
        Store_Teacher.loadData(list);
        //   cc
       
    });
    DB.TeacherServer.SelectSchoolTeacherCount(schoolId, collegeId, function (rul) {
        var count = rul.ReturnValue;
        TeacherPage.pageMaxIndex = count;
        TeacherPage.setTxt_Number(index + "/" + count);
    });


}

function WindwoTeacherTeacher() {
    var win = new Ext.window.Window({
        title: '给专家指派试卷',
        width: 900,
        height: 500,
        items: [CreateTeacherZPGrid()]
    });
    win.show();
}

var Teacher_Grid_Store;
var Teacher_Grid_School;
var Teacher_Grid_CollegeId;
var Teacher_Grid_SchoolBox;
var Teacher_Grid_CollegeBox;
var Teacher_ExtFnPage;
function CreateTeacherZPGrid() {
     Teacher_Grid_Store = new Ext.data.Store({
        fields: [
              'Id',
            'Rid',
            'Name',
            'Email',
            'Phone',
            'School',
            'SchoolId',
            'CollegeId',
            'TypeId',
            'State',
            'CollegeName',
            'TypeId'
        ]
    });
    Teacher_Grid_School = new Ext.data.Store({
        fields:["Id","Name"]
    });
    Teacher_Grid_CollegeId = new Ext.data.Store({
        fields:["Id","Name"]
    });
    Teacher_Grid_SchoolBox = new Ext.form.ComboBox({
        store: Teacher_Grid_School,
        queryMode: 'local',
        valueField: 'Id',
        displayField: 'Name',
        listeners: {
            change: function () {
                var schoolId = Teacher_Grid_SchoolBox.getValue();
                DB.SchoolServer.SelectTypeSchool(schoolId, function(rul) {
                    var list = rul.ReturnValue;
                    list.splice(0, 0, { Id: 0, Name: '所有学院' });
                    Teacher_Grid_CollegeId.loadData(list);
                    Teacher_Grid_CollegeBox.setValue(0);
                });
            }
        }
    });
    Teacher_Grid_CollegeBox = new Ext.form.ComboBox({
        store: Teacher_Grid_CollegeId,
        queryMode: 'local',
        valueField: 'Id',
        displayField: 'Name',
    });
    DB.SchoolServer.SelectTypeSchool(0, function(rul) {
        var list = rul.ReturnValue;
       
        Teacher_Grid_School.loadData(list);
        if (list.length > 0) {
            Teacher_Grid_SchoolBox.setValue(list[0].Id);
        }
    });
    Teacher_ExtFnPage = NewExtPageBar(function(index) {
        Load_Teacher_Data();
    });
         //GridExtPageFn = NewExtPageBar(function (index) {
         //    LoadData();
         //});
    var teacher_Button = new Ext.Button({
        text: "筛选数据",
        listeners: {
            click: function() {
                Load_Teacher_Data();

            }
        }
    });
    var tool = new Ext.toolbar.Toolbar({
        items: [
            new Ext.form.Label({
                text: "选择学校："
            }),
            Teacher_Grid_SchoolBox,
            new Ext.form.Label({
                text: "选择学院："
            }),
            Teacher_Grid_CollegeBox,
            teacher_Button
        ]
    });
    var grid = new Ext.grid.Panel({
        store: Teacher_Grid_Store,
        width: 880,
        height: 470,
        tbar: tool,
        bbar:Teacher_ExtFnPage.CreatePageBBar(),
        columns: [
           { header: '教师名称', dataIndex: 'Name', align: 'center' },
            {
                header: '教师类别', dataIndex: 'TypeId',
                renderer: function (value) {
                    return value == 1 ? "一评校内专家" : value == 2 ? "一评校外专家" : value == 3 ? "二评专家" : value == 4 ? "仲裁专家" : "未知";
                }
            },
            { header: '手机', dataIndex: 'Phone', align: 'center' },
            { header: '所属学校', dataIndex: 'School', align: 'center' },
            {
                header: '所属学院', dataIndex: 'CollegeName', width: 120, renderer: function (value, b, c) {

                    return value;
                }
            },
            {
                header: '账户状态', dataIndex: 'State',
                renderer: function (value) {
                    return value ? "已启用" : "已禁用";
                }, align: 'center'
            },
            {
                header: '操作', dataIndex: 'Id',
                renderer: function (value, b, c) {
                    return "<a href='javascript:SettingPepar(\"" + value + "\",\"" + (c.data.Rid) + "\")'>选择试卷</a>";
                }, align: 'center'
            }
        ],
    });

    return grid;
}

var WinTeacherPaper;
function SettingPepar(id, teacherId) {
    WinTeacherPaper = new Ext.window.Window({
        title: '选择试卷',
        items: [
            CreatePaperGrid(teacherId)
        ],
        width: 800,
        height:450,
    });
    WinTeacherPaper.show();
    // alert(teacherId);
}

var PaperTeacher_Grid_Store;
var PaperTeacher_COMMBOX;
function CreatePaperGrid(teacherId) {
    PaperTeacher_Grid_Store = new Ext.data.Store({
        fields: [
            'PaperId',
            'StudentName',
            'School',
            'College',
            'TeacherTypeId',
            'Id'
        ]
    });
    var store = new Ext.data.Store({
        fields:['Id','Name']
    });
    store.loadData([
        { Id: -1, Name: '所有数据' },
        { Id: 0, Name: '未分配专家' },
        { Id: 1, Name: '已分配专家' }
    ]);
    PaperTeacher_COMMBOX = new Ext.form.ComboBox({
        queryMode: 'local',
        valueField: 'Id',
        displayField: 'Name',
        store:store
    });
    PaperTeacher_COMMBOX.setValue(-1);
    var btn = new Ext.Button({
        text: "筛选数据",
        listeners: {
            click: function() {
                LoadPaperTeacher_Grid_Store(teacherId);
            }
        }
    });
    var tool = new Ext.toolbar.Toolbar({
        items: [
            new Ext.form.Label({
                text:"试卷状态："
            }),
            PaperTeacher_COMMBOX,
            btn
        ]
    });
    var grid = new Ext.grid.Panel({
        tbar:tool,
        columns: [
            { header: '档案编号', dataIndex: 'Id', align: 'center' },
            { header: '试卷编号', dataIndex: 'PaperId', align: 'center' },
            { header: '学生名称', dataIndex: 'StudentName', align: 'center' },
             { header: '学校', dataIndex: 'School', align: 'center' },
            { header: '学院', dataIndex: 'College', align: 'center',width:130 },
            {
                header: '教师类别', dataIndex: 'TeacherTypeId',
                renderer: function (value) {
                    return value == 1 ? "一评第一专家" : value == 2 ? "一评第二专家" : value == 3 ? "二评专家" : value == 4 ? "仲裁专家" : "未知";
                }, align: 'center'
            },
            {
                header: '操作', dataIndex: 'Id', width: 120,
                renderer: function (a, b, c) {
                    return '<a href="javascript:SavePaperTeacher(\'' + a + '\',\'' + teacherId + '\')">选择试卷</a>';
                }, align: 'center'
            }
        ],
        region: 'center',
        store: PaperTeacher_Grid_Store,
        width: 780,
        height:420
    });
    LoadPaperTeacher_Grid_Store(teacherId);
    return grid;
}

function LoadPaperTeacher_Grid_Store(teacherId) {
    var status = PaperTeacher_COMMBOX.getValue();
    teacherId = parseInt(teacherId);
   // alert(teacherId);
    DB.EvaluatingPaperServer.SelectTeacherIdList(teacherId,status, function(rul) {
        var list = rul.ReturnValue;
        PaperTeacher_Grid_Store.loadData(list);
    });
}

function SavePaperTeacher(did, teacherId) {
   // alert(did);
    // return;
   
    did = parseInt(did);
    teacherId = parseInt(teacherId);
    DB.EvaluatingPaperServer.UpdateTeacher(did, teacherId, function() {
        LoadPaperTeacher_Grid_Store(teacherId);
    });
    
}

function Load_Teacher_Data(dindex) {
    //var Teacher_Grid_Store;
    //var Teacher_Grid_School;
    //var Teacher_Grid_CollegeId;
    //var Teacher_Grid_SchoolBox;
    //var Teacher_Grid_CollegeBox;
    //var Teacher_ExtFnPage;
  //  var index = Teacher_ExtFnPage.pageIndex;
    var schoolId = 0;
    var collegeId = 0;

    if (Teacher_Grid_CollegeBox != null) {
        collegeId = Teacher_Grid_CollegeBox.getValue();
    }
    if (Teacher_Grid_SchoolBox != null) {
        schoolId = Teacher_Grid_SchoolBox.getValue();
    }
    var index = dindex != null ? dindex : Teacher_ExtFnPage.pageIndex;
    schoolId = schoolId == null ? 0 : schoolId;
    DB.TeacherServer.SelectSchoolTeacherList(schoolId, collegeId, index, function (rul) {
        var list = rul.ReturnValue;
        Teacher_Grid_Store.loadData(list);
        //   cc

    });
    DB.TeacherServer.SelectSchoolTeacherCount(schoolId, collegeId, function (rul) {
        var count = rul.ReturnValue;
        Teacher_ExtFnPage.pageMaxIndex = count;
        Teacher_ExtFnPage.setTxt_Number(index + "/" + count);
    });
   // GridExtPageFn.setTxt_Number(index + "/" + count);
}