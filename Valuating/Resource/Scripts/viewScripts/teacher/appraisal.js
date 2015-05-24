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
/// <reference path="../../model/PerformanceType.js" />
/// <reference path="../../dbserver/SysMenuServer.js" />
/// <reference path="../../dbserver/EvaluatingPaperServer.js" />
/// <reference path="../../model/EvaluatingPaperType.js" />

$(function () {
    ReSize();
    $(window).resize(function () {
        ReSize();
    });
    LoadData();
    $("#div_TeachingPlan").click(function () {
        ShowFrame(1);
    });
    $("#div_Courseware").click(function () {
        ShowFrame(2);
    });
    $("#div_Video").click(function () {
        ShowFrame(3);
    });
    $("#div_Reflection").click(function () {
        ShowFrame(4);
    });
    $("#div_header_sjxx,#div_header_cp_menu").click(function () {
        var id = $(this).attr("data-Id");
        var $this=$("#"+id);
        var height = $this.attr("data-Height");
        var isOk = $this.attr("data-Ok");
        isOk = isOk == "true";
        if (isOk) {
          
            $this.attr("data-Ok", false);
            $this.height(height);
        } else {
            $this.attr("data-Ok", true);
            $this.height("27px");
           
        }
        ReSetPFSize();

    });
});
var ReSetPFSize = function () {
    var setHeight = 30;
  
    $("#div_pf_menu").height(setHeight);
    $("#div_cp_menu_pf").height(setHeight - 27);
    var height = $(document).height();
     setHeight = height - 40-110 - $("#div_cp_menu").height() - $("#div_sjxx").height();
   // alert(setHeight + "," + height + "," + ($("#div_cp_menu").height() + $("#div_sjxx").height()));
    $("#div_pf_menu").height(setHeight);
    $("#div_cp_menu_pf").height(setHeight - 27);
};
var TempStartDate = new Date();
//1教案 2课件 3视频 4反思 5课前说课
function ShowFrame(typeId) {
    var state = true;
    $("#table_pf_header,#div_table_checkbox").show();
    if (DMODEL == null) {
        Ext.Msg.alert("消息提示", "未抽取到正确的试卷请重新抽取");
        return;
    }
    var url = "";
    var text = "";
    if (DAMODEL.VideoScore + DAMODEL.RresentScore > 0 && (DAMODEL.VideoScore == 0 || DAMODEL.RresentScore == 0) && $("#iframe_content").attr("src").indexOf("ShowVideo") > 0) {
        state = false;

    }
    if (state) {
        switch (typeId) {
            case 1:
                url = "/teacher/showoffice?typeid=" + typeId + "&pid=" + DMODEL.Id + "&type=doc";
                text = "教学设计-评分面板";
                break;
            case 2:
                url = "/teacher/showoffice?typeid=" + typeId + "&pid=" + DMODEL.Id + "&type=ppt";
                text = "教育技术应用能力";
                break;
            case 3:
                url = "/teacher/ShowVideo?typeid=" + typeId + "&pid=" + DMODEL.Id + "&type=ppt";
                text = "教学实施能力";
                break;
            case 4:
                url = "/teacher/showoffice?typeid=" + typeId + "&pid=" + DMODEL.Id + "&type=doc";
                text = "教学评价能力";
                break;
        }
        $("#iframe_content").attr("src", url);
    }

  
    $("#div_pfmianban").html(text);
    TempStartDate = new Date();
    $.Json("/teacher/appraisal", "GetPresents", {
        typeId: typeId,
        did: DMODEL.Did
    }, function (list) {
        if ($("#div_insertHtml").length > 0) {
            $("#div_insertHtml").remove();
        }
        var length = list.length;
        var html = "";
        var height = 0;
        if (typeId == 3) {
            var insertHtml = "<tr id='div_insertHtml' class=\"table_video_tr\"><td colspan=\"6\"><div class='div_tr_video_css'><div id='table_trs1_td' onclick='ShowXS(\"table_trs1\")' >课前说课</div><div id='table_trs2_td' onclick='ShowXS(\"table_trs2\")' >上课部分</div><div></td></tr>";
            if ($("#table_tr_pf_header").length > 0) {
                var headerHtml = $("#table_tr_pf_header").prop("outerHTML");
              
                $("#table_pf_header").html(insertHtml + headerHtml);
            }

            height = length * 35 + 70;
            var tlist = _.where(list, { "TypeId": 5 });
            html += "<tr class='video_tr_table' ><td colspan=\"6\"><div id=\"div_trs_xy\">";
            var ts = GetHtml(tlist);
            var trs1 = "<table id='table_trs1'>" + ts + "";
           // trs1 += " <tr style=\"height:50px; widht:180px;\"><td colspan=\"6\"><input onclick='BTN_ZB_SAVE(\"table_trs1\",this,\"5\")' value='保存' type=\"button\" id=\"btn_zb_Save\"></td></tr>";
            trs1 += "</table>";
            html += trs1;
            tlist = _.where(list, { "TypeId": 3 });
            var trs2 = "<table id='table_trs2'>" + GetHtml(tlist) + "";
            //trs2 += " <tr style=\"height:50px; widht:180px;\"><td colspan=\"6\"><input onclick='BTN_ZB_SAVE(\"table_trs2\",this,\"3\")' value='保存' type=\"button\" id=\"btn_zb_Save\"></td></tr>";
            trs2 += " <tr style=\"height:50px; widht:180px;\"><td colspan=\"6\"><input onclick='SaveViewo(this)' value='保存' type=\"button\" id=\"btn_zb_Save\"></td></tr>";
            trs2 += "</table>";
            html += trs2;
            html += "</div></td></tr>";

            $("#div_table_checkbox").height(height).html(html);
            if ($(ts).find("input:checked").length > 0) {

                ShowXS("table_trs2");
            } else {
                ShowXS("table_trs1");
            }


            return;


        } else {
            height = length * 35;
            html = GetHtml(list);
            html += "<tr style=\"height:50px; widht:180px;\"><td colspan=\"6\"><input onclick='BTN_ZB_SAVE(\"div_table_checkbox\",this,\"" + typeId + "\")' value='保存' type=\"button\" id=\"btn_zb_Save\"></td></tr>";
        }


        height += 50;
        $("#div_table_checkbox").height(height).html(html);
        //  ShowXS("table_trs1");

    });
}

function SaveViewo(btn) {
   
        var trs1 = $("#table_trs1 tr[name=\"radio_tr\"]");
        var trs2 = $("#table_trs2 tr[name=\"radio_tr\"]");
        if (ISCheckBox(trs1, btn) && ISCheckBox(trs2, btn)) {

        } else {

            alert("有未评分项，请检查，如果想退出，直接点击关闭");
            return;
        }

    BTN_ZB_SAVE("table_trs2", btn, "3", function() {
        videoIndex++;
        NextVideo();
    });
    BTN_ZB_SAVE("table_trs1", btn, "5", function() {
        videoIndex++;
        NextVideo();
    });
}

var videoIndex = 0;
function NextVideo() {
    if (videoIndex == 2) {
        alert("保存成功");
        LoadScore(DMODEL.Did);
        videoIndex = 0;
    }
}

function ShowXS(id) {
    $("#table_trs2").hide();
    $("#table_trs1").hide();
    var table = $("#" + id);

    $("#table_trs2_td").css({
        'background': "url('/resource/images/btn_bg1.gif')",
        'color': '#000',
        'background-repeat': 'repeat-x',
        'background-position-y': '-25px'
    });
    $("#table_trs1_td").css({
        'background': "url('/resource/images/btn_bg1.gif')",
        'color': '#000',
        'background-repeat': 'repeat-x',
        'background-position-y': '-25px'
    });

    $("#" + id + "_td").css({
        'background': "url('/resource/images/btn_bg1.gif')",
        'color': '#fff',
        'width': '80px',
        'height': '25px'
    });

    table.show();
    var length = table.find("tr").length;
    var height = length * 35 + 35;
    $("#div_table_checkbox").height(height);

}

function ISCheckBox(trs,btn) {
    var trs1length = trs.length;
  
    for (var i = 0; i < trs1length; i++) {
        var checked = $(trs[i]).find("td>input:checked");
        if (checked.length == 0) {
            
            $(btn).show();
            return false;
        }
    }
    return true;

}

function BTN_ZB_SAVE(tabId, btn, typeId,sucess) {
  
    $(btn).hide();
    var trs = $("#" + tabId + " tr[name=\"radio_tr\"]");
    var rlength = trs.length;
    var list = new Array();
  
  
    for (var i = 0; i < rlength; i++) {
        var checked = $(trs[i]).find("td>input:checked");
        
        if (checked.length == 0) {
            alert("有未评分项，请检查，如果想退出，直接点击关闭");
            $(btn).show();
            return;
        }
        var perform = WM.PerformanceType();
        perform.Id = $(trs[i]).attr("id").replace("table_tr_id_", "");
        perform.ZId = checked.attr("name").replace("radio_", "");
        perform.DId = DMODEL.Did;
        perform.Grade = parseFloat(checked.attr("value"));
        perform.GradeScore = parseFloat($(trs[i]).attr("value")) * parseFloat(checked.attr("value"));
        list[list.length] = perform;

    }

    $.Json("/teacher/appraisal", "PerformanceSave", {
        list: list, date: TempStartDate, typeId: typeId
    }, function () {


        //   $({ 1: "#div_TeachingPlan_OK", 2: "#div_Courseware_OK", 3: "#div_Video_OK", 4: "#div_Reflection_OK" }[typeId]).css("display", "block");
        if (sucess == null) {
            alert("保存成功");
            LoadScore(DMODEL.Did);
        } else {
            sucess();
        }
       
        //$(btn).show();
        // NextDocument();
      

    });


}

var DAMODEL;
function LoadScore(did) {
    $.Json("/teacher/appraisal", "GetScore", {
        id: did
    }, function (model) {
        if (model.TeachingPlanScore > 0) {
            $("#div_TeachingPlan").html("教案: " + model.TeachingPlanScore);
            $("#div_TeachingPlan_OK").css("display", "block");
        } else {
            $("#div_TeachingPlan").html("教案");
            $("#div_TeachingPlan_OK").hide();
            $("#div_score_sum").html("");
        }
        if (model.CoursewareScore > 0) {
            $("#div_Courseware").html("课件： " + model.CoursewareScore);
            $("#div_Courseware_OK").show();

        } else {
            $("#div_Courseware").html("课件");
            $("#div_Courseware_OK").hide();
            $("#div_score_sum").html("");
        }
        if (model.VideoScore > 0 && model.RresentScore > 0) {
            $("#div_Video").html("说课：" + model.RresentScore + " 上课：" + model.VideoScore);
            $("#div_Video_OK").show();
        }
        else {
            $("#div_Video").html("微格视频");
            $("#div_Video_OK").hide();
            $("#div_score_sum").html("");
        }
        if (model.ReflectionScore > 0) {
            $("#div_Reflection").html("反思:" + model.ReflectionScore);
            $("#div_Reflection_OK").show();
        } else {
            $("#div_Reflection").html("反思");
            $("#div_Reflection_OK").hide();
            $("#div_score_sum").html("");
        }
        if (model.TeachingPlanScore > 0 && model.CoursewareScore > 0 && model.VideoScore > 0 && model.ReflectionScore > 0 && model.RresentScore > 0) {
            $("#div_score_sum").html("总分：" + parseInt((model.TeachingPlanScore + model.CoursewareScore + model.VideoScore + model.ReflectionScore + model.RresentScore) * 100) / 100);
        }
        DAMODEL = model;
        NextDocument();
    });
}

function GetHtml(list) {
    var html = "";
    var length = list.length;
    for (var i = 0; i < length; i++) {

        html += "<tr title=\"" + list[i].Content + "\" name=\"radio_tr\" id=\"table_tr_id_" + list[i].Cid + "\" value=\"" + (list[i].Ratio) + "\">";

        html += "<td class='table_class_td_bj' style='width:40px; text-align: center;' id=\"radio_td_" + list[i].Id + "\">" + list[i].Name + "</td>";
        html += "<td class='table_class_td_bj' style='width:30px; text-align: center;'> <input type=\"radio\"  " + ((list[i].Cgrade) == 0.55 ? "checked='checked'" : "") + " value=\"" + (0.55) + "\" name=\"radio_" + list[i].Id + "\" /></td>";
        html += "<td class='table_class_td_bj' style='width:30px; text-align: center;'> <input type=\"radio\"  " + ((list[i].Cgrade) == 0.65 ? "checked='checked'" : "") + "  value=\"" + (0.65) + "\" name=\"radio_" + list[i].Id + "\" /></td>";
        html += "<td class='table_class_td_bj' style='width:30px; text-align: center;'> <input type=\"radio\"  " + (list[i].Cgrade == 0.75 ? "checked" : "") + "  value=\"" + (0.75) + "\" name=\"radio_" + list[i].Id + "\" /></td>";
        html += "<td class='table_class_td_bj' style='width:30px; text-align: center;'> <input type=\"radio\"  " + ((list[i].Cgrade) == 0.85 ? "checked='checked'" : "") + "  value=\"" + (0.85) + "\" name=\"radio_" + list[i].Id + "\" /></td>";
        html += "<td class='table_class_td_bj' style='width:30px; text-align: center;'> <input type=\"radio\"  " + ((list[i].Cgrade) == 0.95 ? "checked='checked'" : "") + "  value=\"" + (0.95) + "\" name=\"radio_" + list[i].Id + "\" /></td>";
        html += "</tr>";

    }
    return html;
}

function LoadData() {
    GetPaperServer(0);


}
//v00009对象 Did为档案对象
var DMODEL;
function GetPaperServer(id) {

   
    $.Json("/teacher/index", "GetScoreNumber", { state: true }, function (number) {
        $("#span_y_number").html(number);
    });
    $.Json("/teacher/index", "GetScoreNumber", { state: false }, function (number) {
        $("#span_d_number").html(number);
    });

    $.Json("/teacher/appraisal", "GetPaperServer", { id: id }, function (model) {
        if (model == null && id == 0) {
            alert("已经没有学生资料需要测评");
            location.href = "/teacher/index";
            return;
        }
        if (model == null && id != 0) {
            alert("已经没有学生资料需要测评");

            return;
        }
        DMODEL = model;
        LoadScore(model.Did);

        DB.UserServer.Reader(model.Rid, function (rul) {
            var user = rul.ReturnValue;
            var loginId = user.LoginId.substring(0, 4) + "********";
            $("#span_student_number").html(loginId);
        });

        if (model.TeacherTypeId == 4) {
            DB.EvaluatingPaperServer.EvaluatingList(DMODEL.Id, function (rul) {
                var list = rul.ReturnValue;

                var html = '<table class="tab_teacher_score_header">';
                //  alert("4");
                $("#div_student_Number").hide();
                $("#div_teacher_score").show();
                var length = list.length;

                for (var i = 0; i < length; i++) {
                    var model = list[i];
                    html += '<tr>';
                    html += '<td class="td_teacher_name">' + (i + 1) + '</td>';
                    html += '<td class="td_score">' + model.TeachingPlanScore + '</td>';
                    html += '<td class="td_score">' + model.CoursewareScore + '</td>';
                    html += '<td class="td_score">' + model.VideoScore + '</td>';
                    html += '<td class="td_score">' + model.ReflectionScore + '</td>';
                    html += '<td class="td_score">' + model.RresentScore + '</td>';
                    html += '<td class="td_score">' + model.SumScore + '</td>';

                    html += '</tr>';
                }
                html += '</table>';
                $("#div_teacher_score_number").html(html);
            });
        }

    });

}
function NextDocument() {

    var typeId = 0;
    if ($("#div_TeachingPlan_OK").css("display") == "none") {
        typeId = 1;
    } else if ($("#div_Courseware_OK").css("display") == "none") {
        typeId = 2;
    } else if ($("#div_Video_OK").css("display") == "none") {
        typeId = 3;
       
    } else if ($("#div_Reflection_OK").css("display") == "none") {
        typeId = 4;
    }
    if (typeId == 0) {
        $("#iframe_content").attr("src", "/teacher/Accomplish?Did=" + DMODEL.Did);
        $("#table_pf_header,#div_table_checkbox").hide();
        $("#div_pfmianban").html("评分完成");
        return;
    }

    ShowFrame(typeId);
}

function ReSize() {
    var height = $(document).height();
    var width = $(document).width();
    ReSetPFSize();
    $("#iframe_content").width(width - 240).height(height - 82 - 40);

}

Switchover = function () {
    GetPaperServer(DMODEL.Id);
};
StudentRetreat = function () {
    $("#iframe_content").attr("src", "/teacher/StudentRetreat?Id=" + DMODEL.Id);
};