
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
Ext.onReady(function () {

    CreateView();
});
//当前选择的用户
var TempUserType = WM.UserType();
var userStore;
var User_S_Text;
var user_ATPage;
var UserTypeBox;
var SchoolComBox;
var CollegeComBox;
var CollegeStore;
function CreateUserPanel() {
    userStore = Ext.create('Ext.data.Store', {
        id: 'simpsonsStore',
        fields: ['Name', 'LoginId', 'School', 'College', 'Email', 'Phone'],
        pageSize: 10,
        proxy: {
            type: 'memory',
            reader: {
                type: 'json',
                root: 'items'
            }
        }
    });
    
    //DB.UserServer.List(function (data) {
    //    var list = data.ReturnValue;
    //    userStore.loadData(list);

    //});
    LoadUserGrid(1);
    var pagingToolbar = new Ext.PagingToolbar
({
    emptyMsg: "没有数据",
    displayInfo: true,
    displayMsg: "显示从{0}条数据到{1}条数据，共{2}条数据",
    store: userStore,
    pageSize: 10
});
    var user_upPage = new Ext.Button({
        text: "上一页",
        border: 0,
        bodyPadding: 0,
        listeners: {
            click: function() {
                
                LoadUserGrid(index-1);
            }
        }

    });
    var user_nextPage = new Ext.Button({
        text: "下一页",
        border: 0,
        bodyPadding: 0,
        listeners: {
            click: function() {
                LoadUserGrid(index+1);
            }
        }
    });
     user_ATPage = new Ext.form.Text({
        border: 0,
        bodyPadding: 0,
        width:60
    });
    var user_SelectPage = new Ext.Button({
        text: "跳页",
        border: 0,
        bodyPadding: 0,
        listeners: {
            click: function () {
                var number =parseInt(user_ATPage.getValue());
                LoadUserGrid(number);
            }
        }
    });
    
    User_S_Text = new Ext.form.Text({
        width: 120,
        border: 0,
        bodyPadding: 0,
    });

    var tool = new Ext.toolbar.Toolbar({
        border: 0,
        bodyPadding: 0,
        items: [
            user_upPage,
            user_ATPage,
            user_nextPage,
            user_SelectPage
            
        ]
    });
    var userTypeStore = new Ext.data.Store({
        fields:['Id','Name']
    });
    //School,College,Email,Phone,
     UserTypeBox = new Ext.form.ComboBox({
        queryMode: 'local',
        displayField: 'Name',
        valueField: 'Id',
        store: userTypeStore
     });
     var schoolStore = new Ext.data.Store({
         fields:['Id','Name']
     });
    DB.SchoolServer.SelectTypeSchool(0, function(rul) {
        var list = rul.ReturnValue;
        list.splice(0, 0, {Id:0,Name:'所有学校'});
        schoolStore.loadData(list);
    });
    CollegeStore = new Ext.data.Store({
        fields:['Id','Name']
    });
    CollegeComBox = new Ext.form.ComboBox({
        queryMode: 'local',
        displayField: 'Name',
        valueField: 'Id',
        store: CollegeStore
    });
     SchoolComBox = new Ext.form.ComboBox({
         queryMode: 'local',
         displayField: 'Name',
         valueField: 'Id',
         store: schoolStore,
         listeners: {
             change: function() {
                 var typeId = SchoolComBox.getValue();
                 typeId = typeId == 0 ? -1 : typeId;
                 DB.SchoolServer.SelectTypeSchool(typeId, function (rul) {
                     var list = rul.ReturnValue;
                     list.splice(0, 0, { Id: 0, Name: '所有学院' });
                     CollegeStore.loadData(list);
                 });
             }
         }
     });
   

    userTypeStore.loadData(DB.EnumUserTeacherJson());
    var gridPanel = Ext.create('Ext.grid.Panel', {

        store: Ext.data.StoreManager.lookup('simpsonsStore'),
        columns: [
            { header: '帐号', dataIndex: 'LoginId', width: 160 },
            { header: '名称', dataIndex: 'Name', width: 120 },
            { header: '学校', dataIndex: 'School', width: 80 },
            { header: '学院', dataIndex: 'College', width: 160 },
            { header: '邮箱', dataIndex: 'Email', width: 100 },
            { header: '手机', dataIndex: 'Phone', width: 120 }

        ],

        region: 'west',
        height: 300,
        forceFit: true,
        width: 700,
        tbar: [
            new Ext.form.Label({text:'账户类别'}),
            UserTypeBox,
            SchoolComBox,
            CollegeComBox,
             new Ext.form.Label({ text: '帐号' }),
            User_S_Text,
            new Ext.Button({
                text: "筛选",
                listeners: {
                    click: function() {
                        LoadUserGrid(1);
                    }
                }
            })
        ],
       bbar:tool,
        listeners: {
            selectionchange: function (view, models, c, d) {
                if (models.length == 0) {
                    return;
                }
                var model = models[0].data;
                DB.UserServer.Reader(model.Id, function (log) {
                    var user = log.ReturnValue;
                    SetUserForm(user);
                });

            },
        }

    });

    return gridPanel;

}


function SetUserForm(model) {
    TempUserType = model;
  

    if (model.Id == null || model.Id == 0) {
        userLoginId.setReadOnly(false);
    } else {
        userLoginId.setReadOnly(true);
    }
    if (model.TypeId == 1 && model.Id != 0) {
        DB.StudentServer.ReaderStudent(model.Id, function (rul) {
            var student = rul.ReturnValue;
            SetStudentForm(student);
        });
    }
    if (model.TypeId == 2 && model.Id != 0) {
        DB.TeacherServer.SelectTeacherRid(model.Id, function (rul) {
            Teach_Model = rul.ReturnValue;
            SetTeacherUserForm(Teach_Model);
        });
    }


    userLoginId.setValue(model.LoginId);
    userName.setValue(model.Name);
    userPassword.setValue("111111");
    userType.setValue(model.TypeId == "0" ? 0 : model.TypeId);

    var tab;
    switch (model.TypeId) {
        case 1:
            tab = UserTable.getComponent("user_studentManager");
            break;
        case 2:
            tab = UserTable.getComponent("user_TeacherManager");
            break;
        case 3:
            tab = UserTable.getComponent("user_AdminManager");
            break;
    }
    if (tab != null)
        UserTable.setActiveTab(tab);
}



//用户帐号文本框
var userLoginId;
//用户名称文本框
var userName;
//用户密码文本框
var userPassword;
var userType;
function UserFormPanel() {

    userType = Ext.create('Ext.form.ComboBox', {
        fieldLabel: '用户类别',
        store: DB.EnumUserTypeStore(),
        queryMode: 'local',
        displayField: 'name',
        valueField: 'typeId',
        id: 'userTypeId',
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,

    });
    userLoginId = Ext.create("Ext.form.Text", {
        fieldLabel: '用户帐号',
        id: 'userLoginId',
        style: 'margin-left:20px;margin-top:10px;',
        width:360,
    });

    userName = Ext.create('Ext.form.Text', {
        fieldLabel: '用户名称',
        id: 'userName',
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
    });
    userPassword = Ext.create("Ext.form.Text", {
        fieldLabel: '用户密码',
        id: 'userPassword',
        inputType: 'password',
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
    });
    var deleteUser = Ext.create("Ext.Button", {
        text: '删除账户',
        width: 120,
        height:30,
        listeners: {
            click: function () {
                if (TempUserType == null || TempUserType.Id == 0) {
                    Ext.Msg.alert("错误提示", "当前用户不存在");
                    return;
                }

                DB.UserServer.DeleteUser(TempUserType.Id, function (log) {
                    Ext.Msg.alert("操作成功", log.ReturnValue);
                    var array = Ext.pluck(userStore.data.items, 'data');
                    array = _.where(array, function (m) {
                        return m.Id != TempUserType.Id;
                    });
                    userStore.loadData(array);
                    // userStore.remove(TempUserType);
                    SetUserForm(WM.UserType());
                });
            }
        }
    });
    var addUser = Ext.create("Ext.Button", {
        text: '添加账户',
        width: 120,
        height:30,
        listeners: {
            click: function () {
                var model = WM.UserType();
                model.Id = 0;
                SetUserForm(model);
            }
        }
    });

    var saveUser = Ext.create('Ext.Button', {
        text: '保存账户',
        width: 120,
        height:30,
        listeners: {
            click: function () {
                if (TempUserType == null) {
                    Ext.Msg.alert("错误提示", "未选择任何账户操作");
                    return;
                }
                TempUserType.Name = userName.getValue();
                TempUserType.LoginId = userLoginId.getValue();
                TempUserType.TypeId = userType.getValue();
                if (userPassword.getValue() != null && userPassword.getValue() != "111111") {
                    TempUserType.Password = userPassword.getValue();
                }
                DB.UserServer.SaveUser(TempUserType, function (log) {
                    if (log.ReturnValue.Id > 0) {
                   
                        LoadUserGrid(1);

                    } else {
                        Ext.Msg.alert("错误提示", "设置账户信息失败：请保证登录帐号的唯一性");
                    }

                });
            }
        }
    });
  
    var form = new Ext.form.Panel({
        width:400,
        region: 'center',
        title: '',
        defaultType: 'textfield',
        bodyPadding: 10,
        border: 0,
        defaults: {
            labelWidth: 100
        },
        items: [
            userLoginId,
            userName,
            userPassword,
            userType,
        ],
        buttons: [
             addUser,
            deleteUser,
            saveUser
        ]

    });

    return form;
}

var index = 1;
function LoadUserGrid(tempIndex) {
    var value = "";
    var userTypeId = 0;
    if (UserTypeBox != null) {
        userTypeId = UserTypeBox.getValue();
    }

    var schoolId = 0;
    if (SchoolComBox != null) {
        schoolId = SchoolComBox.getValue();
    }

    var collegeId = 0;
    if (CollegeComBox != null) {
        collegeId= CollegeComBox.getValue();
    }
    
    if (User_S_Text != null) {
        value = User_S_Text.getValue();
    }
  
    if (tempIndex != null) {
        index = tempIndex;
    }
    if (index < 1) {
        Ext.Msg.alert("错误提示","当前已是第一页");
        return;
    }
    var typeId = Params("typeId");
    typeId = typeId == null ? 0 : parseInt(typeId);
    if (typeId == 2) {
        //if (UserTable != null) {
        //    UserTable.getComponent("user_studentManager").close();
        //    UserTable.getComponent("user_AdminManager").close();
        //}

    }
    if (typeId != 0) {
        userTypeId = typeId;
    }
    schoolId = schoolId == null ? 0 : schoolId;
    collegeId = collegeId == null ? 0 : collegeId;
    userTypeId = userTypeId==null?0:userTypeId;
    DB.UserServer.ListPageUser(value, index, userTypeId, schoolId, collegeId, function (rul) {
        var list = rul.ReturnValue;
        userStore.loadData(list);
       
    });
 
    DB.UserServer.Count(value, userTypeId, schoolId, collegeId, function (rul) {
        var count = rul.ReturnValue;
        user_ATPage.setValue(tempIndex+"/"+count);
    });

}

var Stu_LoginId;
var Stu_Name;
var Stu_InitPwd;
var Stu_School;
var Stu_College;
var Stu_CollegeStore;
var Stu_Email;
var Stu_Phone;
var Stu_Status;
//设置学生表单数据
function SetStudentForm(stu) {
    if (stu != null && stu.Rid != 0) {
        Stu_LoginId.setValue(TempUserType.LoginId);
        Stu_LoginId.setReadOnly(true);
        Stu_School.setValue(stu.SchoolId);
        StuSelectSchool(stu.CollegeId);
    } else if (stu == null || stu.Rid == 0) {
        Stu_LoginId.setReadOnly(false);
        var user = WM.UserType();
        user.TypeId = 1;
        SetUserForm(user);
        //TempUserType
        Stu_LoginId.setValue("");
    }

    Stu_Name.setValue(stu.Name);
    Stu_InitPwd.setValue("111111");
   
    Stu_Email.setValue(stu.Email);
    Stu_Phone.setValue(stu.Phone);
    Stu_Status.setValue(stu.Status ? 1 : 0);
    Stu_Grade.setValue(stu.Grade);
    StuModel = stu;
}
//选择学校
function StuSelectSchool(cid) {

    var id = Stu_School.getValue();
    DB.SchoolServer.SelectTypeSchool(id, function (rul) {
        var list = rul.ReturnValue;

        Stu_CollegeStore.loadData(list);
        if (list.length > 0) {
            if (cid != null) {
                Stu_College.setValue(cid);
            } else {
                Stu_College.setValue(list[0].Id);
            }

        }
    });
}

var StuModel;
function SaveStudent() {
    if (TempUserType != null && TempUserType.TypeId != 1) {
        Ext.Msg.alert("错误提示", "修改的账户不是学生帐号");
        return;
    } else if (TempUserType == null) {
        TempUserType = WM.UserType();
    }
    TempUserType.LoginId = Stu_LoginId.getValue();
    TempUserType.Name = Stu_Name.getValue();
    TempUserType.TypeId = 1;
    if (Stu_InitPwd.getValue() != "111111")
        TempUserType.Password = Stu_InitPwd.getValue();
    DB.UserServer.SaveUser(TempUserType, function (rul) {
        var user = rul.ReturnValue;

        if (StuModel == null) {
            StuModel = WM.StudentType();
        }
        StuModel.Rid = user.Id;
        StuModel.Name = Stu_Name.getValue();
        StuModel.School = Stu_School.getRawValue();
        StuModel.SchoolId = Stu_School.getValue();
        StuModel.College = Stu_College.getRawValue();
        StuModel.CollegeId = Stu_College.getValue();
        StuModel.Email = Stu_Email.getValue();
        StuModel.Phone = Stu_Phone.getValue();
        StuModel.Status = parseInt(Stu_Status.getValue());
        StuModel.Grade = Stu_Grade.getValue();
        DB.StudentServer.SaveStudent(StuModel, function () {
            Ext.Msg.alert("提示", "保存成功");
            LoadUserGrid(1);
        });
        //  StuModel

    });


}

var Teach_LoginId;
var Teach_Name;
var Teach_Email;
var Teach_Phone;
var Teach_School;
var Teach_College;
var Teach_TypeId;
var Teach_State;
var Teach_CollegeStore;
var Teach_SchoolStore;
var Teach_Pwd;
var Teach_Model;

function SetTeacherUserForm(model) {
    Teach_Model = model;
    if (model == null) {
        return;
    }
    if (model.Id > 0) {
        Teach_LoginId.setReadOnly(true);
    } else {
        Teach_LoginId.setReadOnly(false);
    }

    if (model.Rid == 0) {
         model = WM.UserType();
        model.Id = 0;
        TempUserType =model ;
    }
  
    Teach_LoginId.setValue(TempUserType.LoginId);
    Teach_Name.setValue(model.Name);
    Teach_Email.setValue(model.Email);
    Teach_Phone.setValue(model.Phone);
    //Teach_College.setValue(model.CollegeId);
    //Teach_State.setValue(model.State ? 1 : 0);
    Teach_School.setValue(model.SchoolId);
    Teach_College.setValue(model.CollegeId);
    Teach_TypeId.setValue(model.TypeId);
    Teach_State.setValue(model.State?1:0);
    Teach_Pwd.setValue("111111");
   
}

function TeachUserSave() {
    if (TempUserType == null) {
        TempUserType = WM.UserType();
    }
    TempUserType.LoginId = Teach_LoginId.getValue();
    TempUserType.Name = Teach_Name.getValue();
    if (Teach_Pwd.getValue() != "111111")
        TempUserType.Password = Teach_Pwd.getValue();
    TempUserType.TypeId = 2;
    DB.UserServer.SaveUser(TempUserType, function (rul) {
        TempUserType = rul.ReturnValue;
        if (Teach_Model == null) {
            Teach_Model = WM.TeacherType();

        }

        Teach_Model.Rid = TempUserType.Id;
        Teach_Model.Name = Teach_Name.getValue();
        Teach_Model.Email = Teach_Email.getValue();
        Teach_Model.Phone = Teach_Phone.getValue();
        Teach_Model.School = Teach_School.getRawValue();
        Teach_Model.SchoolId = Teach_School.getValue();
        Teach_Model.CollegeName = Teach_College.getRawValue();
        Teach_Model.TypeId = Teach_TypeId.getValue();
        Teach_Model.State = Teach_State.getValue();
        Teach_Model.CollegeId = Teach_College.getValue();
        DB.TeacherServer.SaveTeacher(Teach_Model, function (rul) {
            LoadUserGrid(1);
            Ext.Msg.alert("提示信息", "保存专家账户成功");
        });

    });

}
var Teach_School_Change = function () {
    var id = Teach_School.getValue();
   if (id == null) {
       return;
   }
    DB.SchoolServer.SelectTypeSchool(id, function (rul) {
        var list = rul.ReturnValue;
        Teach_CollegeStore.loadData(list);
        if (list.length > 0) {
            Teach_College.setValue(list[0].Id);
        }
    });
}




function TeacherFormPanel() {
    Teach_LoginId = new Ext.form.Text({
        fieldLabel: "专家帐号",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
    });
    Teach_Name = new Ext.form.Text({
        fieldLabel: "专家姓名",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
    });
    Teach_Pwd = new Ext.form.Text({
        fieldLabel: "帐号密码",
        inputType: "password",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
    });
    Teach_Email = new Ext.form.Text({
        fieldLabel: "用户邮箱",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
    });
    Teach_Phone = new Ext.form.Text({
        fieldLabel: "用户手机",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
    });
    Teach_SchoolStore = new Ext.data.Store({
        id: "Teach_SchoolStore",
        fields: ["id", "Name"]
    });

    Teach_School = new Ext.form.ComboBox({
        fieldLabel: "所属学校",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
        store: Teach_SchoolStore,
        queryMode: 'local',
        displayField: "Name",
        valueField: "Id",
        listeners: {
            change: Teach_School_Change
        }
    });
    DB.SchoolServer.SelectTypeSchool(0, function (rul) {
        var list = rul.ReturnValue;
        Teach_SchoolStore.loadData(list);
        if (list.length > 0) {
            Teach_School.setValue(list[0].Id);
        }
    });
    Teach_CollegeStore = new Ext.data.Store({
        fields: ["Id", "Name"],
        id: "Teach_CollegeStore"
    });
    Teach_College = new Ext.form.ComboBox({
        fieldLabel: "所属学院",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
        store: Teach_CollegeStore,
        queryMode: "local",
        displayField: "Name",
        valueField: "Id",
    });
    var teach_TypeIdStore = new Ext.data.Store({
        id: "teach_TypeIdStore",
        fields: ["Id", "Name"]
    });
    teach_TypeIdStore.loadData(DB.EnumTeacherTypeJson());
    Teach_TypeId = new Ext.form.ComboBox({
        fieldLabel: "专家类别",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
        store: teach_TypeIdStore,
        displayField: "Name",
        valueField: "Id",
        queryMode: "local"
    });
    Teach_TypeId.setValue(1);
    var teach_StateStore = new Ext.data.Store({
        id: "teach_StateStore",
        fields: ["Id", "Name"]
    });
    teach_StateStore.loadData([
        { Id: 0, Name: "禁用账户" },
        { Id: 1, Name: "启用账户" }]);
    Teach_State = new Ext.form.ComboBox({
        fieldLabel: "账户状态",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
        store: teach_StateStore,
        displayField: "Name",
        valueField: "Id",
        queryMode: "local"
    });
    Teach_State.setValue(0);
    var addTeacher = new Ext.Button({
        text: "添加专家",
        width: 120,
        height: 30,
        style: 'margin-left: 0px;',
        listeners: {
            click: function () {
                var model = WM.TeacherType();
                model.Id = 0;
                SetTeacherUserForm(model);
            }
        }
    });
    var deleteTeacher = new Ext.Button({
        text: "删除专家",
        cls: 'testbutton',
        width: 120,
        height: 30,
        style: 'margin-left: 10px;',
        listeners: {
            click: function () {
                if (Teach_Model == null || Teach_Model.Id == 0) {
                    Ext.Msg.alert("错误提示", "请选择需要删除的专家");
                } else {
                    DB.TeacherServer.DeleteTeacher(Teach_Model, function () {
                        Ext.Msg.alert("提示消息", "删除成功");
                        LoadUserGrid(1);
                    });
                }
            }
        }
    });
    var stuDownload = new Ext.Button({
        text: "下载专家模版",
        width: 100,
        height: 30,
        //style: 'margin-left:80px;',
        listeners: {
            click: function() {
                location.href = "/resource/file/专家模版.xlsx";
            }
        }
    });
    var upTool = new Ext.form.FileUploadField({
        xtype: 'filefield',
        name: 'photo',
        fieldLabel: '',
        labelWidth: 50,
        msgTarget: 'side',
        allowBlank: false,
        anchor: '100%',
        buttonText: '浏览文件',
       
        style:'margin-top:20px;'
    });
    var stuUpLoad =new Ext.Button({
        text: '导入专家账户',
        width: 100,
        height: 30,
        style:'margin-left:20px;',
        handler: function () {
            var form = this.up('form').getForm();
            if (form.isValid()) {
                form.submit({
                    url: '/admin/InputUser.ashx?typeId=2',
                    waitMsg: '文件上传中',
                    success: function (fp, o) {
                        Ext.Msg.alert('Success', 'Your photo  has been uploaded.');
                    }, failure: function (a, b, c, d) {
                        if (b.response.responseText.length == 0) {
                            Ext.Msg.alert("消息提示", "账户导入成功");
                        } else {
                            Ext.Msg.alert("错误提示", b.response.responseText);
                        }
                        LoadUserGrid(1);
                    }
                });
            }
        }
    });

   
    var saveTeacher = new Ext.Button({
        text: "保存专家",
        listeners: {
            click: TeachUserSave
        },
        width: 120,
        height: 30,
        style:'margin-left:10px;'
    });
   
    var teacher = new Ext.form.Panel({
        title: "专家账户设置",
        id: "user_TeacherManager",
        items: [
            Teach_LoginId,
            Teach_Name,
            Teach_Pwd,
            Teach_Email,
            Teach_Phone,
            Teach_School,
            Teach_College,
            Teach_TypeId,
            Teach_State,
            addTeacher,
            deleteTeacher,
            saveTeacher,
            new Ext.form.Panel({
                width:400,
                height:120,
                border:0,
                style: 'margin-top:20px',
                title: '导入专家数据',
                items: [
                    upTool
                ],
                buttons: [
                      stuDownload,
                      stuUpLoad
                ]
            })
         
          
            

        ]
    });

    return teacher;
}

var stu_schoolStore;
var Stu_Grade;
//初始化学生账户面板
function StudentFormPanel() {
    Stu_LoginId = new Ext.form.Text({
        fieldLabel: "登录帐号",
       
        id: "stu_LoginId",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
    });
    Stu_Name = new Ext.form.Text({
        fieldLabel: "学生姓名",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
        id: "stu_Name"
    });
    Stu_Grade = new Ext.form.Text({
        fieldLabel: "学生年级",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
        id:"stu_grade"
    });
    Stu_InitPwd = new Ext.form.Text({
        fieldLabel: "初始密码",
        inputType: "password",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
        id: "stu_initpwd"

    });
    stu_schoolStore = new Ext.data.Store({
        id: "stu_schoolStore",
        fields: ['Id', 'Name'],

    });


    Stu_School = new Ext.form.ComboBox({
        queryMode: 'local',
        valueField: "Id",
        displayField: "Name",
        store: stu_schoolStore,
        fieldLabel: "所属学校",
        listeners: {
            select: StuSelectSchool
        },
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
    });
    DB.SchoolServer.SelectTypeSchool(0, function (rul) {
        var list = rul.ReturnValue;

        stu_schoolStore.loadData(list);
        if (list.length > 0) {
            Stu_School.setValue(list[0].Id);
            StuSelectSchool();
        }


    });
    Stu_CollegeStore = new Ext.data.Store({
        fields: ["Id", "Name"],
        id: "stu_CollegeStore"
    });
    Stu_College = new Ext.form.ComboBox({
        queryMode: "local",
        valueField: "Id",
        displayField: "Name",
        store: Stu_CollegeStore,
        fieldLabel: "所属学院",
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
    });
    Stu_Email = new Ext.form.Text({
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
        fieldLabel: "联系邮箱",

    });
    Stu_Phone = new Ext.form.Text({
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
        fieldLabel: "联系手机"
    });
    var Stu_StatusStore = new Ext.data.Store({
        fields: ["Id", "Name"]
    });
    Stu_StatusStore.loadData([{ Id: 0, Name: "禁止登录" }, { Id: 1, Name: "激活账户" }]);
    Stu_Status = new Ext.form.ComboBox({
        style: 'margin-left:20px;margin-top:10px;',
        width: 360,
        fieldLabel: "账户状态",
        store: Stu_StatusStore,
        queryMode: "local",
        valueField: "Id",
        displayField: "Name"

    });
    Stu_Status.setValue(0);
    var addButton = new Ext.Button({
        text: "添加账户",
        width: 120,
        height: 30,
        style:'margin-left:0px',
        listeners: {
            click: function () {
                var model = WM.StudentType();
                model.Id = 0;
                SetStudentForm(model);
            }
        }
    });
    var deleteButton = new Ext.Button({
        text: "删除账户",
        width: 120,
        height: 30,
        style: 'margin-left:10px',
        listeners: {
            click: function () {
                if (StuModel != null && StuModel.Id != 0) {
                    DB.StudentServer.DeleteStudent(StuModel, function () {
                        LoadUserGrid(1);
                        SetStudentForm(WM.StudentType());
                        Ext.Msg.alert("提示信息", "删除账户信息成功");
                    });
                }
            }
        }
    });
    var saveButton = new Ext.Button({
        text: "保存账户",
        width: 120,
        height: 30,
        style: 'margin-left:20px',
        listeners: {
            click: SaveStudent
        }
    });

 
    var stuDownload = new Ext.Button({
        text: "下载学生模版",
        width:120,
        height: 30,
        style:'margin-left:0px;',
        listeners: {
            click: function() {
                location.href = "/resource/file/学生模版.xlsx";
            }
        }
    });
    var upTool = new Ext.form.FileUploadField({
        xtype: 'filefield',
        name: 'photo',
        fieldLabel: '',
        style: 'margin-top:20px',
        labelWidth: 50,
        msgTarget: 'side',
        allowBlank: false,
        anchor: '100%',
        buttonText: '浏览文件'
    });
    var stuUpLoad = {
        text: '导入学生账户',
        height:30,
        width:120,
        handler: function() {
            var form = this.up('form').getForm();
            if (form.isValid()) {
                form.submit({
                    url: '/admin/InputUser.ashx?typeId=1',
                    waitMsg: '文件上传中',
                    success: function(fp, o) {
                        Ext.Msg.alert('Success', 'Your photo  has been uploaded.');
                    }, failure: function (a, b, c, d) {
                        if (b.response.responseText.length==0) {
                            Ext.Msg.alert("消息提示", "账户导入成功");
                        } else {
                            Ext.Msg.alert("错误提示", b.response.responseText);
                        }
                        LoadUserGrid(1);
                    }
                });
            }
        }
    };
   

    var student = new Ext.form.Panel({
        id: "user_studentManager",
        title: "学生账户设置",
        items: [
            Stu_LoginId,
            Stu_Name,
            Stu_InitPwd,
            Stu_School,
            Stu_College,
            Stu_Grade,
            Stu_Email,
            Stu_Phone,
            Stu_Status,
            addButton, deleteButton, saveButton,
            new Ext.form.Panel({
                style:'margin-top:20px;',
                title: '导入学生数据',
                border:0,
                width: 400,
                height: 120,

                items: [
                     upTool
                ],
                buttons: [
                    stuDownload,
                    stuUpLoad
                ]
            })

        ]

    });
    return student;
}

var UserTable;
function CreateCenter() {
    var userMessage = Ext.create('Ext.panel.Panel', {
        title: '管理员账户设置',
        items: [UserFormPanel()],
        bodyPadding: 0,
        id: "user_AdminManager"
    });
    var typeId = parseInt(Params("typeId"));
    UserTable = Ext.create("Ext.tab.Panel", {
        region: 'center',
        collapsible: false,
        enableTabScroll: true,
        id: 'tabCenterPanel',

        defaults: {
            autoScroll: true,
            bodyPadding: 10
        },
        items: (typeId == 2 ? [TeacherFormPanel()]:typeId==1?[StudentFormPanel()] : [userMessage, StudentFormPanel(), TeacherFormPanel()])
    });

    return UserTable;
}

function CreateView() {

    Ext.create('Ext.container.Viewport', {
        layout: 'border',

        items: [CreateUserPanel(), CreateCenter()]
    });
}