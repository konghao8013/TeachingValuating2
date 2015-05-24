
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
/// <reference path="../../dbserver/PresentServer.js" />
/// <reference path="../../model/PresentType.js" />
/// <reference path="../../dbserver/IndexRemarkServer.js" />
/// <reference path="../../dbserver/PresentServer.js" />
/// <reference path="../../dbserver/SysScoreServer.js" />
/// <reference path="../../dbserver/SysMenuServer.js" />
$(function () {
    $.Json("/login", "GetUser", null, function (model) {
        $("#span_userdata").html(model.LoginId + "  " + model.Name);
    });
    $.Json("/student/upuserdata", "StudentUser", null, function (model) {
        $("#span_number").html(model.AppraisalNumber);
        if (model.AppraisalNumber > 1) {
            $("#div_reset_btn").hide();
        }
    });
    LoadData();
    $("#div_reset_btn").click(function () {
        $.Json("/student/score", "ResetPaper", null, function () {
            alert("重置成功");
            location.href = "/student/index";
        });
    });
});
//学生成绩结果数据
var STUDENTSCOREMODEL;
function LoadData() {
    InitSchedule();

    SetStudentData();
}


function InitTable() {
    $.Json("/student/score", "GetStudentScore", null, function (list) {
        var html = "";
        html += GetTabHtml(list, 1);
        html += GetTabHtml(list, 2);
        html += GetTabHtml(list, 5);
        html += GetTabHtml(list, 3);
        html += GetTabHtml(list, 4);
        html += GetCreateScoreHtml();
        var length = list.length;
        $("#div_tabs_score_list").height(length * 35 + (4 * 40));
        $("#div_reset_btn").css("top", $("#div_tabs_score_list").height() + 600);
        $("#bottom_html").css("top", $("#div_tabs_score_list").height() + 750);

        // alert((list.length * 25) + (5 * 40));
        // $("#div_tabs_score_list").height((list.length * 25)+(5*40));
        $("#div_tabs_score_list").html(html);
    });
}

function GetCreateScoreHtml() {
    var html = "";
    html += '<table class="tab_score_tb">';
    html += "<tr><th colspan='5'>教学个性、特色与创新" + " <span class='table_th_string'>" + (STUDENTSCOREMODEL.Innovate) + "分 总分（" + SYSMODEL.Originality + " 分）</span> </th></tr>";
    html += "<tr><td style='height:124px'><div class='div_text_py'>评语：" + STUDENTSCOREMODEL.Remark + "</div></td></tr>";
    html += "</table>";
    return html;
}

function GetTabHtml(list, typeId) {
    var html = "";

    var titleString = typeId == 1 ? "教案与撰写能力：" + " <span class='table_th_string'>" + (parseInt(STUDENTSCOREMODEL.TeachingPlanScore * 100) / 100) + "分 总分（" + SYSMODEL.TeachingPlan + " 分）</span>" : typeId == 2 ? "教育技术应用能力：" + " <span class='table_th_string'>" + (parseInt(STUDENTSCOREMODEL.CoursewareScore * 100) / 100) + "分 总分（" + SYSMODEL.Courseware + "分）</span>" : typeId == 5 ? "教学评价能力I：" + " <span class='table_th_string'>" + (parseInt(STUDENTSCOREMODEL.PresentScore * 100) / 100) + "分 总分（" + SYSMODEL.Present + " 分）</span>" : typeId == 3 ? "教学实施能力：" + " <span class='table_th_string'>" + (parseInt(STUDENTSCOREMODEL.VideoScore * 100) / 100) + "分 总分（" + SYSMODEL.Video + " 分）</span>" : typeId == 4 ? "教学评价能力II：" + " <span class='table_th_string'>" + (parseInt(STUDENTSCOREMODEL.ReflectionScore * 100) / 100) + "分 总分（" + SYSMODEL.Reflection + " 分）</span>" : "";
    var tlist = _.where(list, {
        "TypeId": typeId
    });
    var length = tlist.length;
    html += '<table class="tab_score_tb">';
    html += "<tr><th colspan='5'>" + titleString + "</th></tr>";
    for (var i = 0; i < length; i++) {
        var model = tlist[i];
        html += "<tr " + ((i % 2 == 1 ? "style='background:#efefef;'" : "")) + ">";
        html += "<td style='width:95px;  text-align:center;'>" + (i + 1) + "</td>";
        html += "<td style='width:120px;  text-align:center;'>" + (model.Name) + "</td>";
        html += "<td style='text-indent: 2em;'>" + (model.Content) + "</td>";
        html += "<td style='width:200px;'>" + GetRankHtml(model.Cgrade) + "</td>";
        html += "<td style='width:120px; text-align:center;'>" + GetStringHtml(model.Cgrade) + "</td>";
        html += "</tr>";
    }
    html += "</table>";
    return html;
}

function GetRankHtml(value) {
    var html = "";
    switch (value) {
        case 0.95:
            break;
        case 0.85:
            html = GetRedX(4);
            break;
        case 0.75:
            break;
        case 0.65:
            break;
        case 0.55:
            break;
    }
    html = value >= 0.9 ? GetRedX(5) : value >= 0.8 ? GetRedX(4) : value >= 0.7 ? GetRedX(3) : value >= 0.6 ? GetRedX(2) : GetRedX(1);
    return html;
}

function GetRedX(number) {
    //position: relative;
    //position: absolute;
    var html = "<div style='width:120px; position: relative;height:30px;'>";
    for (var i = 1; i <= 5; i++) {
        if (number >= i) {
            html += "<div class='div_xx1' style='left:" + (i * 20 + 30) + "px'></div>";
        } else {
            html += "<div class='div_xx2' style='left:" + (i * 20 + 30) + "px'></div>";
        }

    }
    html += "</div>";
    return html;
}

function GetStringHtml(value) {
    var html = "";

    if (value >= 0.9)
        html = "<span style='color:#009900'>优</span>";
    else if (value >= 0.8)
        html = "<span style='color:#009900'>良</span>";
    else if (value >= 0.7)
        html = "<span style='color:red'>中</span>";
    else if (value >= 0.6)
        html = "<span style='color:red'>合格</span>";
    else
        html = "<span style='color:red'>不合格</span>";

    return html;
}
function DownLoadScore() {
    $.Json("/student/score", "DowloadFile", {
        paperId: STUDENTSCOREMODEL.PaperId
    }, function (url) {
        location.href = url;
    });
}
function InitSchedule() {
    $.Json("/student/score", "GetUserScore", null, function (model) {
        if (model == null) {
            alert("无当前学生成绩结果");
            location.href = "/login";
        }
        STUDENTSCOREMODEL = model;
        var number = parseInt(model.SumScore * 100) / 100;
        var html = "测评结果：" + number + " 分，";
        html += number >= 90 ? "优" : number >= 80 ? "良" : number >= 70 ? "中" : number >= 60 ? "合格" : "不合格";

        $("#div_student_score_number").html(html);
        $("#div_score_schedule").width(320 * STUDENTSCOREMODEL.SumScore / 100);

        $("#div_tab_j").html(parseInt(STUDENTSCOREMODEL.TeachingPlanScore * 100) / 100);
        $("#div_tab_k").html(parseInt(STUDENTSCOREMODEL.CoursewareScore * 100) / 100);
        $("#div_tab_s").html(parseInt(STUDENTSCOREMODEL.PresentScore * 100) / 100);
        $("#div_tab_v").html(parseInt(STUDENTSCOREMODEL.VideoScore * 100) / 100);
        $("#div_tab_f").html(parseInt(STUDENTSCOREMODEL.ReflectionScore * 100) / 100);
        $("#div_tab_c").html(STUDENTSCOREMODEL.Innovate);
        $("#div_tab_sum").html(parseInt(STUDENTSCOREMODEL.SumScore * 100) / 100);
        //InitTable();
        DB.SysScoreServer.ReaderScoreType(function (rul) {
            var sysModel = rul.ReturnValue;
            SYSMODEL = sysModel;
            InitTable();
        });
    });
}

var SYSMODEL;
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