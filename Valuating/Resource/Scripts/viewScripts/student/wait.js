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
/// <reference path="../../ajaxfileupload.js" />
/// <reference path="../../dbserver/SysMenuServer.js" />
$(function() {
    LoadData();
});
function LoadData() {
    $.Json("/student/upuserdata", "StudentUser", null, function (model) {
        $("#span_number").html(model.AppraisalNumber + 1);
    });
    $.Json("/login", "GetUser", null, function (model) {
        $("#span_userdata").html(model.LoginId + "  " + model.Name);
    });
    SetStudentData();

}

function SetStudentData() {
    $.Json("/student/wait", "GetData", null, function (model) {
        
        if (model.TeachingSize > 0) {
            $("#div_j_name").html(model.TeachingName);
            $("#div_j_data").html("大小:" + parseInt(model.TeachingSize / 1024) + "kb 时间:" + model.TeachingPlanTime.format("yyyy年MM月dd日 hh:ss"));
            $("#div_j_download").html("<a class='download_a' target='download' href='" + model.TeachingPlan + "'>下载</a><a class='download_a' href=\"javascript:Preview(1,'" + model.TeachingPlan + "')\">预览</a>");
        }
        if (model.CoursewareSize > 0) {
            $("#div_k_name").html(model.CoursewareName);
            $("#div_k_data").html("大小:" + parseInt(model.CoursewareSize / 1024) + "kb 时间:" + model.CoursewareTime.format("yyyy年MM月dd日 hh:ss"));
            $("#div_k_download").html("<a class='download_a' target='download' href='" + model.Courseware + "'>下载</a><a class='download_a' href=\"javascript:Preview(2,'" + model.Courseware + "')\">预览</a>");
        }
        if (model.VideoSize > 0) {
            $("#div_s_name").html(model.VideoName);
            $("#div_s_data").html("大小:" + parseInt((model.VideoSize / 1024) / 1024) + "mb 时间:" + model.VideoTime.format("yyyy年MM月dd日 hh:ss"));
            $("#div_s_download").html("<a class='download_a' target='download' href='" + model.Video + "'>下载</a><a class='download_a' href=\"javascript:Preview(3,'" + model.Video + "')\">预览</a>");
        }
        if (model.RefiectionSize > 0) {

            $("#div_f_name").html(model.ReflectionName);
            $("#div_f_data").html("大小:" + parseInt(model.RefiectionSize / 1024) + "kb 时间:" + model.ReflectionTime.format("yyyy年MM月dd日 hh:ss"));
            $("#div_f_download").html("<a class='download_a' target='download' href='" + model.Reflection + "'>下载</a><a class='download_a' href=\"javascript:Preview(4,'" + model.Reflection + "')\">预览</a>");
        }

       
    });
}
function Preview(typeId, url) {
    var lists = url.split('.');
    var typeName = lists[lists.length - 1];
    url = url.replace("." + typeName, "");
    var wpath = "/resource/plug/";
    if (typeId != 3) {
        wpath += "ShowOffice.html?typeName=" + typeName + "&path=" + url + "&format=" + (typeId == 1 ? "doc" : typeId == 2 ? "ppt" : typeId == 4 ? "doc" : "");
    } else {
        wpath += "ShowVideo.html?typeName=" + typeName + "&path=" + url + "";
    }
    window.open(wpath, "wpathfile");
}