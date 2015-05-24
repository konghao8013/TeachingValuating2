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
/// <reference path="~/Resource/Scripts/dbserver/DBSource.js"></script>GetScoreNumber
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
$(function() {
    $.Json("/teacher/index", "GetUser", null, function(model) {
        $("#span_teacher_name").html(model.Name);
    });
    $.Json("/teacher/index", "GetScoreNumber", { state: false }, function (number) {
        if (number == 0) {
            $("#exit_button").show();
            $("#start_button").hide();
        }
        $("#span_number").html(number);
    });
});
//
function StartAppraisal() {
    location.href = "/teacher/appraisal";
}
function ExitAppraisal() {
    $.Json("/Login", "Logout", null, function() {
        location.href = "/index";
    });
    //window.close();
}