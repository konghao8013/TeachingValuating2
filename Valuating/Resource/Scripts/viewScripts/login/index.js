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
LoadPage = function () {
    InitData();
    InitEvent();
    InitAhelp();

};

function InitAhelp() {
    
    $.Json("login", "Reader",null, function(model) {
      
        $("#downloadrj").attr("href", model.DownLoadFile);
        $("#help_a").attr("href", model.HelpVideo);
    });
}

InitSize = function () {
    $("#imgBackground").width(Width).height(Height);
};
var InitData = function () {
    SetTextName();
};
var renamepwd = function () {
    $("#name_html").val("");
    $("#pwd_html").val("");
};
//获取Cookie设置 登录帐号默认值和复选框状态
var SetTextName = function () {
    var userNameCookie = $.Cookie("userName");
    userNameCookie = $.HexToDec(userNameCookie);
    if (userNameCookie != null && userNameCookie != "null" && userNameCookie != "请输入账户") {

        $("#name_html").val(userNameCookie);
        $("#remember").attr("checked", "checked");
    } else {
        $("#name_html").val("请输入账户");

    }
};
//初始化页面事件操作
var InitEvent = function () {
    BearDivEvent();
    TextFocusEvent();
    $("#Save_button").click(function () {
        UserLogin();
    });
    $(document).keydown(function (key) {
        if (key.keyCode == 13) {
            UserLogin();
        }
    });
};
//设置文本框获得焦点事件
var TextFocusEvent = function () {
    $("#name_html").focus(function () {
        $(this).val("");
    });
    $("#pwd_html").focus(function () {
        $(this).val("");
    });
};
//切换复选框状态以及保存取消 Cookie
var BearDivEvent = function () {
    //alert("");
    $("#remember").click(function () {

        var div = $(this);

        if (div.attr("checked") == "checked") {

            var userName = $("#name_html").val();
            if (userName.length > 0) {
                div.attr("selected", true);
                $.Cookie("userName", userName);
            }


        } else {
            div.attr("checked", null);
            $.Cookie("userName", "请输入账户");
        }


    });
};
//用户点击登录按钮时发生
var UserLogin = function () {

    $("#messageDiv").css("display", "none").html("");
    var loginid = $("#name_html").val();
    var password = $("#pwd_html").val();
    $.Json("login", "LoginUser", {
        loginId: loginid, password: password
    }, function (data) {
        if (data == null) {
            Ext.Msg.alert("错误提示", "登录帐号或者密码错误请重新输入");
        } else {
            if (data.State) {
                if ($("#remember").attr("checked") == "checked") {
                    $.Cookie("userName", data.LoginId);
                }
                location.href = data.Reurl;
            } else {
                Ext.Msg.alert("错误提示", "管理员禁止该账户登录");
            }

        }
    });
};