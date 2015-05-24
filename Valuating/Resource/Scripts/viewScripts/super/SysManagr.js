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
/// <reference path="../../dbserver/SysScoreServer.js" />
/// <reference path="../../dbserver/SysSettingServer.js" />
/// <reference path="../../dbserver/TeacherServer.js" />
/// <reference path="../../model/SysSettingType.js" />
/// <reference path="../../model/SysScoreType.js" />
/// <reference path="../../dbserver/SysMenuServer.js" />
Ext.onReady(function () {

    CreateView();

});
var Txt_TeachingPalan;
var Txt_Courseware;
var Txt_Video;
var Txt_Reflection;
var Txt_VideoPass;
var Txt_Pass;
var Txt_DifferenceValue;
var Txt_BatchName;
var Txt_BatchStateDate;
var Txt_BatchEndDate;
var Txt_Originality;
var TxtPresent;

var Sys_Name;
var Sys_LoginHint;
var Sys_Phone;
var Sys_TestGrade;
var Sys_IsDebug;
var Sys_SchoolIco;
var Sys_HelpVideo;
var SysScoreModel;
var SysSettingModel;
var Sys_DownLoad;
var Sys_TeacherLogin;
var Sys_StudentLogin;
function LoadData() {
    DB.SysScoreServer.ReaderScoreType(function (rul) {
        var model = rul.ReturnValue;
        SetTxtForm(model);

    });
    DB.SysSettingServer.ReaderSettingType(function (rul) {
        SetSysForm(rul.ReturnValue);
    });
}

function SetSysForm(model) {
    Sys_Name.setValue(model.Name);
    Sys_LoginHint.setValue(model.LoginHint);
    Sys_Phone.setValue(model.Phone);
    Sys_TestGrade.setValue(model.TestGrade);

    Sys_DownLoad.setRawValue(model.DownLoadFile);
    //  model.IsDebug
    if (model.IsDebug) {
        Ext.getCmp("IsDebug_True").setValue(true);
        //  alert(Ext.getCmp("IsDebug_True"));
    } else {
        Ext.getCmp("IsDebug_False").setValue(true);
    }
    if (model.TeacherLogin) {

        Ext.getCmp("TeacherLogin_True").setValue(true);
    } else {
        Ext.getCmp("TeacherLogin_False").setValue(true);
    }
    if (model.StudentLogin) {
        Ext.getCmp("StudentLogin_True").setValue(true);
    } else {
        Ext.getCmp("StudentLogin_False").setValue(true);
    }
    Sys_SchoolIco.setRawValue(model.SchoolIco);
    Sys_HelpVideo.setRawValue(model.HelpVideo);

    SysSettingModel = model;
}
//保存Syssetting值
function SaveSysSetting() {

    SysSettingModel.Name = Sys_Name.getValue();

    SysSettingModel.LoginHint = Sys_LoginHint.getValue();
    SysSettingModel.Phone = Sys_Phone.getValue();
    SysSettingModel.TestGrade = Sys_TestGrade.getValue();
    if (Sys_DownLoad.getRawValue().length > 0)
        SysSettingModel.DownLoadFile = Sys_DownLoad.getRawValue();
    //  alert(Sys_DownLoad.getRawValue());
    if (Ext.getCmp("IsDebug_False").checked) {
        SysSettingModel.IsDebug = false;

    } else {
        SysSettingModel.IsDebug = true;

    }
    if (Ext.getCmp("StudentLogin_False").getValue()) {
        SysSettingModel.StudentLogin = false;
    } else {
        SysSettingModel.StudentLogin = true;
    }
    if (Ext.getCmp("TeacherLogin_False").getValue()) {
        SysSettingModel.TeacherLogin = false;
    } else {
        SysSettingModel.TeacherLogin = true;
    }
    //   SysSettingModel.IsDebug = Sys_IsDebug.getValue();
    if (Sys_SchoolIco.getRawValue().length > 0)
        SysSettingModel.SchoolIco = Sys_SchoolIco.getRawValue();
    if (Sys_HelpVideo.getRawValue().length > 0)
        SysSettingModel.HelpVideo = Sys_HelpVideo.getRawValue();
    // alert(SysSettingModel.IsDebug);
    DB.SysSettingServer.Save(SysSettingModel, function (rul) {
        SetSysForm(rul.ReturnValue);
        Ext.Msg.alert("消息提示", "保存成功");
    });
}

//设置面板值
function SetTxtForm(model) {
    Txt_TeachingPalan.setValue(model.TeachingPlan);
    Txt_Courseware.setValue(model.Courseware);
    Txt_Video.setValue(model.Video);
    Txt_VideoPass.setValue(model.VideoPass);
    Txt_Reflection.setValue(model.Reflection);
    Txt_Pass.setValue(model.Pass);
    Txt_DifferenceValue.setValue(model.DifferenceValue);
    Txt_BatchName.setValue(model.BatchName);
    Txt_Originality.setValue(model.Originality);
    TxtPresent.setValue(model.Present);
    Txt_BatchStateDate.setValue(model.BatchStateDate.format("yyyy-MM-dd"));
    Txt_BatchEndDate.setValue(model.BatchEndDate.format("yyyy-MM-dd"));
    SysScoreModel = model;
}

function CreateView() {
    var width = $(document).width();
    var height = $(document).height();
    Txt_TeachingPalan = new Ext.form.Number({
        fieldLabel: "教案分数",
        width: (width / 2-50),
        style: 'margin-top:0px;margin-left:20px;'

    });
    Txt_Courseware = new Ext.form.Number({
        fieldLabel: "课件分数",
        width: (width/2)-50,
        style: 'margin-top:10px;margin-left:20px;'
    });
    Txt_Video = new Ext.form.Number({
        fieldLabel: "视频分数",
        width: (width/2)-50,
        style: 'margin-top:10px;margin-left:20px;'
    });
    Txt_Reflection = new Ext.form.Number({
        fieldLabel: "反思分数",
        width: (width/2)-50,
        style: 'margin-top:10px;margin-left:20px;'
    });
    Txt_VideoPass = new Ext.form.Number({
        fieldLabel: "视频及格分数线",
        width: (width/2)-50,
        style: 'margin-top:10px;margin-left:20px;'
    });
    Txt_DifferenceValue = new Ext.form.Number({
        fieldLabel: "三评差值",
        width: (width/2)-50,
        style: 'margin-top:10px;margin-left:20px;'
    });
    Txt_Pass = new Ext.form.Number({
        fieldLabel: "及格分数",
        width: (width/2)-50,
        style: 'margin-top:10px;margin-left:20px;'
    });
    Txt_BatchName = new Ext.form.Text({
        fieldLabel: "测评批次",
        width: (width/2)-50,
        style: 'margin-top:10px;margin-left:20px;',
        format: 'Y/m/d',
    });
    Txt_BatchStateDate = new Ext.form.Date({
        fieldLabel: "开始时间",
        width: (width/2)-50,
        style: 'margin-top:10px;margin-left:20px;',
        emptyText: '请选择',
        format: 'Y/m/d',

    });
    Txt_BatchEndDate = new Ext.form.Date({
        fieldLabel: "结束时间",
        width: (width/2)-50,
        style: 'margin-top:10px;margin-left:20px;',
        emptyText: '请选择',
        format: 'Y/m/d',
    });
    Txt_Originality = new Ext.form.Number({
        fieldLabel: "创意得分",
        width: (width/2)-50,
        style: 'margin-top:10px;margin-left:20px;'
    });
    TxtPresent = new Ext.form.Number({
        fieldLabel: "课前说课",
        width: (width/2)-50,
        style: 'margin-top:10px;margin-left:20px;'
    });
    var saveButton = new Ext.Button({
        text: "保存测评分值",
        width: 120,
        height: 30,
        listeners: {
            click: function () {
                SysScoreModel.TeachingPlan = Txt_TeachingPalan.getValue();

                SysScoreModel.Courseware = Txt_Courseware.getValue();
                SysScoreModel.Video = Txt_Video.getValue();
                SysScoreModel.Reflection = Txt_Reflection.getValue();
                SysScoreModel.VideoPass = Txt_VideoPass.getValue();
                SysScoreModel.Pass = Txt_Pass.getValue();
                SysScoreModel.DifferenceValue = Txt_DifferenceValue.getValue();
                SysScoreModel.BatchName = Txt_BatchName.getValue();
                SysScoreModel.BatchStateDate = Txt_BatchStateDate.getValue();
                SysScoreModel.BatchEndDate = Txt_BatchEndDate.getValue();
                SysScoreModel.Originality = Txt_Originality.getValue();
                SysScoreModel.Present = TxtPresent.getValue();
                DB.SysScoreServer.Save(SysScoreModel, function (rul) {
                    SetTxtForm(rul.ReturnValue);
                    Ext.Msg.alert("消息提示", "保存成功");
                });
            }
        }

    });


    var numberPanel = new Ext.form.Panel({
        title: "测评分值设定",
        width: width/2,
        height: height,
        border: 0,
        x: 0,
        y: 0,
        items: [
            Txt_BatchName,
            Txt_BatchStateDate,
            Txt_BatchEndDate,
            Txt_TeachingPalan,
            Txt_Courseware,
            TxtPresent,
            Txt_Video,
            Txt_Reflection,
            Txt_VideoPass,
            Txt_Pass,
            Txt_DifferenceValue,
            Txt_Originality,


        ],
        buttons: [
            saveButton
        ]
    });

    Sys_Name = new Ext.form.Text({
        fieldLabel: "系统名称",
        width: (width/2)-100,
        style: 'margin-top:10px;margin-left:20px;'
    });
    Sys_LoginHint = new Ext.form.TextArea({
        fieldLabel: "登录提示",
        width: (width/2)-100,
        height: 150,
        style: 'margin-top:10px;margin-left:0px;'
    });
    Sys_Phone = new Ext.form.Number({
        fieldLabel: "监控手机",
        width: (width/2)-100,
        style: 'margin-top:10px;margin-left:20px;'
    });
    Sys_TestGrade = new Ext.form.Number({
        fieldLabel: "测评年级",
        width: (width/2)-100,
        style: 'margin-top:10px;margin-left:20px;'
    });
    Sys_IsDebug = new Ext.form.RadioGroup(
    {

        items: [
            { boxLabel: '是', name: 'rb', id: 'IsDebug_True', inputValue: '1' },

            {
                boxLabel: '否', name: 'rb', id: 'IsDebug_False', inputValue: '0', checked: true
            }
        ],
        width: 166,

    });

    Sys_TeacherLogin = new Ext.form.RadioGroup(
    {

        items: [
            { boxLabel: '是', name: 'teacherLogin', id: 'TeacherLogin_True', inputValue: '1' },

            {
                boxLabel: '否', name: 'teacherLogin', id: 'TeacherLogin_False', inputValue: '0', checked: true
            }
        ],
        width: 166,

    });

    Sys_StudentLogin = new Ext.form.RadioGroup(
    {

        items: [
            { boxLabel: '是', name: 'studentLogin', id: 'StudentLogin_True', inputValue: '1' },

            {
                boxLabel: '否', name: 'studentLogin', id: 'StudentLogin_False', inputValue: '0', checked: true
            }
        ],
        width: 166,

    });

    Sys_SchoolIco = new Ext.form.FileUploadField({
        buttonText: "浏览文件",
        fieldLabel: "选择校徽",
        allowBlank: false,
        msgTarget: 'side',
        name: "school_ico_html",
        width: (width/2)-100,
        style: 'top:-130px;',


    });
    Sys_HelpVideo = new Ext.form.FileUploadField({
        buttonText: "浏览文件",
        fieldLabel: "帮助视频",
        allowBlank: false,
        msgTarget: 'side',

        name: "help_video_html",
        width: (width/2)-100,
        style: 'margin-top:10px;'
    });
    Sys_DownLoad = new Ext.form.FileUploadField({
        buttonText: "浏览文件",
        fieldLabel: "下载软件",
        allowBlank: false,
        msgTarget: 'side',

        name: "downLoad_html",
        width: (width/2)-100,
        style: 'margin-top:10px;'
    });
    var sys_Save = new Ext.Button({
        text: "保存系统信息",
        width: 120,
        height: 30,
        listeners: {
            click: function () {
                var form = this.up('form').getForm();
                if (form.isValid()) {
                    form.submit({
                        url: '/admin/upload.ashx',
                        waitMsg: '文件上传中',
                        success: function (fp, o) {
                            Ext.Msg.alert('Success', 'Your photo  has been uploaded.');
                        }, failure: function (a, b, c, d) {
                            if (b.response.responseText.length > 0) {
                                var array = JSON.parse(b.response.responseText);

                                var schoolIco = _.find(array, { "Key": "school_ico_html" });
                                var schoolHelp = _.find(array, { "Key": "help_video_html" });
                                var download = _.find(array, { "Key": "downLoad_html" });

                                if (download.Url != null && download.Url.length > 0) {
                                    Sys_DownLoad.setRawValue(download.Url);
                                }
                                if (schoolIco.Url != null && schoolIco.Url.length > 0) {
                                    Sys_SchoolIco.setRawValue(schoolIco.Url);

                                }
                                if (schoolHelp.Url != null && schoolHelp.Url.length) {
                                    Sys_HelpVideo.setRawValue(schoolHelp.Url);
                                }
                            }
                            SaveSysSetting();
                        }
                    });
                }
            }
        }
    });

    var basicPanel = new Ext.form.Panel({
        title: "基本设置",
        width: width / 2,
        
        border: 1,
        x: width/2,
        y: -height,
        height: height,
        items: [
            Sys_Name,
            Sys_Phone,
            Sys_TestGrade,
          
          new Ext.Panel({
              x: 20,
              y: -0,
              width: (width/2)-100,
              border:0,
              items: [
                   Sys_SchoolIco,
                    Sys_HelpVideo,
                    Sys_DownLoad,
                    Sys_LoginHint
              ]
          })
          , new Ext.Panel({
              title: '收发短信',
              width: 166,
              border: 1,
              height: 70,
              x: 20,
              items: [
                  Sys_IsDebug
              ]
          }),
            new Ext.Panel({
                title: '学生登录',
                width: 166,
                border: 1,
                height: 70,
                x: 186,
                y: -70,
                items: [
                    Sys_StudentLogin
                ]
            }),
            new Ext.Panel({
                title: '专家登录',
                width: 170,
                height: 70,
                x: 350,
                y: -140,
                border: 1,
                items: [
                    Sys_TeacherLogin
                ]
            }),

        ],
        buttons: [
            sys_Save
        ]

    });

    new Ext.Panel({
        title: "",
        renderTo: Ext.getBody(),

        items: [
            numberPanel,
            basicPanel
        ]
    });
    LoadData();
}