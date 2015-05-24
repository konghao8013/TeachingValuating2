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
/// <reference path="../../ajaxfileupload.js" />
/// <reference path="../../ExtjsFn.js" />
/// <reference path="../../dbserver/SysMenuServer.js" />

function CheckFile(lasts, last) {
  
    var arrays = lasts.split(',');
    var length = arrays.length;
    last = last.toLowerCase();
    var isOk = false;
    for (var i = 0; i < length; i++) {
        if (last === arrays[i]) {
            isOk = true;
            break;
        }
    }
    return isOk;
}

function FileUpload(btn) {
    var value = $("#file_select option:selected").val();
    value = parseInt(value);
    var path = $("#input_file").val();
    var arrs = path.split('.');
    var last = arrs[arrs.length-1];
    var $this = $(btn);
  
    var arrays = ['','doc,docx', 'ppt,pptx', 'wmv,mp4', 'doc,docx'];
    if (!CheckFile(arrays[value], last)) {
        Ext.Msg.alert("错误提示", "请上传以下数据格式文件" + arrays[value]);
      
        return;
    }
    $this.hide();
    var rate = NewExtRate();
    
    rate.StartExtFnBox();
    $.ajaxFileUpload
    (
        {
            url: '/admin/upload.ashx', //用于文件上传的服务器端请求地址
            secureuri: false, //是否需要安全协议，一般设置为false
            fileElementId: 'input_file', //文件上传域的ID
            dataType: 'json', //返回值类型 一般设置为json
            success: function (data, status) //服务器成功响应处理函数
            {
               
                $.Json("/student/index", "SaveData", {
                    value: value, data:data[0]
                }, function (model) {
                    SetStudentData();
                    $this.show();
                    //box.hide();
                    rate.EndExtFnBox();
                });

            },
            error: function (data, status, e) //服务器响应失败处理函数
            {
                $this.show();
                alert(e);
            }
        }
    );
    return false;

}



$(function () {
    LoadData();
});

function SetStudentData() {
    $.Json("/student/index", "GetDataState", null, function (state) {
        switch (state) {
            case 1:
                $("#file_select option[value=1]").attr("selected", true);
                break;
            case 2:
                $("#file_select option[value=2]").attr("selected", true);
                break;

            case 3:
                $("#file_select option[value=3]").attr("selected", true);
                break;
            case 4:
                $("#file_select option[value=4]").attr("selected", true);
                break;
        }
        FileSelected();
    });
    $.Json("/student/index", "GetData", null, function(model) {
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
            $("#div_s_data").html("大小:" + parseInt((model.VideoSize / 1024)/1024) + "mb 时间:" + model.VideoTime.format("yyyy年MM月dd日 hh:ss"));
            $("#div_s_download").html("<a class='download_a' target='download' href='" + model.Video + "'>下载</a><a class='download_a' href=\"javascript:Preview(3,'" + model.Video + "')\">预览</a>");
        }
        if (model.RefiectionSize > 0) {
          
            $("#div_f_name").html(model.ReflectionName);
            $("#div_f_data").html("大小:" + parseInt(model.RefiectionSize / 1024) + "kb 时间:" + model.ReflectionTime.format("yyyy年MM月dd日 hh:ss"));
            $("#div_f_download").html("<a class='download_a' target='download' href='" + model.Reflection + "'>下载</a><a class='download_a' href=\"javascript:Preview(4,'" + model.Reflection + "')\">预览</a>");
        }

        if (model.TeachingSize > 0 && model.CoursewareSize > 0 && model.VideoSize > 0 && model.RefiectionSize > 0) {
            $("#btn_apply").show();
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
    window.open(wpath,"wpathfile");
}
function ApplyFunc() {
    $.Json("/student/index", "Apply", null, function (msg) {
        if (parseInt(msg)==1) {
            location.reload();
        } else {
            Ext.Msg.alert("消息提示", msg);
        }
    });

}

function FileSelected() {

    var value = $("#file_select option:selected").val();
    switch (value) {
        case "1":
            $("#td_title_file_content").html(" 文件上传区（支持Word上传）");
            break;
        case "2":
            $("#td_title_file_content").html(" 文件上传区（支持PPT上传）");
            break;
        case "3":
            $("#td_title_file_content").html(" 文件上传区（支持mp4 wmv上传）");
            break;
        case "4":
            $("#td_title_file_content").html(" 文件上传区（支持word上传）");
            break;
    }

}

function LoadData() {
    $.Json("/student/upuserdata", "StudentUser", null, function (model) {
        $("#span_number").html(model.AppraisalNumber + 1);
     
    });
    $.Json("/login", "GetUser", null, function (model) {
        $("#span_userdata").html(model.LoginId + "  " + model.Name);
    });
    SetStudentData();
    
}