
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
var Did;
$(function() {
     Did = parseInt(GetParams("did"));
    DB.PresentServer.GetPY(Did, function(rul) {
        var list = rul.ReturnValue;
        var html = "";
        var length = list.length;
        for (var i = 0; i < length; i++) {
            html += "【"+(list[i].TypeId == 1 ? "教案" : list[i].TypeId == 2 ? "课件" : list[i].TypeId == 3 ? "视频" : list[i].TypeId == 4 ? "反思" : list[i].TypeId == 5 ? "说课" : "")+"-"+list[i].Name+"】"+list[i].Content+" ";
        }
       
        $("#text_py_teacher_auto").html(html);
     
    });
    //div_checkboxs
    DB.SysScoreServer.ReaderScoreType(function(rul) {
        var model = rul.ReturnValue;
        var createScore = model.Originality;
        var html = "";
        for (var i = 0; i <= createScore; i++) {
            html += ' <div class="div_radio_create" style="left:'+(i*80)+'px;"> <label for="createScore'+(i)+'">'+i+'分</label> <input type="radio" name="createScore" '+(i==0?'checked':'')+' id="createScore'+(i)+'"  value="'+(i)+'"/></div>';
        }
        $("#div_checkboxs").html(html);
    });
});

GetParams = function (name) {
    var urm = location.search;
    urm = urm.substring(1);
    var urls = urm.split('&');
    var length = urls.length;
    for (var i = 0; i < length; i++) {
        var us = urls[i].split('=');
        if (us.length == 2 && us[0].toLowerCase() == name.toLowerCase()) {
            return us[1];
        }

    }
    return null;
};
function ReScore() {
    top.ShowFrame(1);
}



function ScoreSave() {
    var value = parseInt($("#div_checkscore input:checked").attr("value"));
   
    if (value == 0) {
        //  alert("成绩无效待处理");
        $.Json("/teacher/accomplish", "VoteVown", {
            did: Did
        }, function () {
            Ext.Msg.alert("消息提示", "提交成功");
            top.GetPaperServer(0);
        });
        return;
    }
    var createScore = $("#div_checkboxs input:checked").attr("value");
    var remark = $("#text_py_teacher_auto").html();
   
    //int createScore,string remark
    $.Json("/teacher/accomplish", "SaveTeacherScore", {
        did: Did,
        createScore: createScore,
        remark:remark
    },function() {
        Ext.Msg.alert("消息提示", "提交成功");
        top.GetPaperServer(0);
    });

}