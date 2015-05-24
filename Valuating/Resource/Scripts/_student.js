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
var UserDataWin;
$(function () {
    ReWidth();
    $(window).resize(function () {
        ReWidth();
    });
    LoadSysName();

    $("#div_setpwd").click(function () {
        var win = new Ext.window.Window({
            title: '修改账户信息',
            height: 300,
            width: 520,
            layout: 'fit',
            items: {
                title: '修改账户信息',
                header: false,
                html: '<iframe style="border-top-width: 0px; border-left-width: 0px; border-bottom-width: 0px; width: 728px; height: 455px; border-right-width: 0px" src="/student/upuserData" frameborder="0" width="100%" scrolling="no" height="100%"></iframe>',
                border: false
            }
        }).show();
        UserDataWin = function () {
            win.close();
        }
    });
    $("#out_sys").click(function () {
        //location.href = "/Login";
     //   location.href = "/Login";
        $.Json("/Login", "Logout", null, function () {
            location.href = "/Login";
        });
    });
    $("#help_sys").click(function () {

        window.open("/resource/help/helpindex.htm", "help_sys");
    });
});

var ReWidth = function () {
    var width = $(document).width();
    var height = $(document).height();
    //  $("#header_title").width(width);
    //alert(height);
    $("#bottom_html").css("top", (height - 40) + "px");
    $("#right_buttoms").css("left", width - 280);

};
var LoadSysName = function () {
    $.Json("/student/index", "SysName", null, function (sysName) {

        $("#bottom_html").html(sysName);
    });
};