/// <reference path="ajaxfileupload.js" />


window.Serialize = function(obj) {
    switch (obj.constructor) {
    case Object:
        var str = "{";
        for (var o in obj) {
            str += o + ":" + Serialize(obj[o]) + ",";
        }
        if (str.substr(str.length - 1) == ",") str = str.substr(0, str.length - 1);
        return str + "}";
        break;
    case Array:
        var str = "[";
        for (var o in obj) {
            str += Serialize(obj[o]) + ",";
        }
        if (str.substr(str.length - 1) == ",") str = str.substr(0, str.length - 1);
        return str + "]";
        break;
    case Boolean:
        return "\"" + obj.toString() + "\"";
        break;
    case Date:
        return "\"" + obj.toString() + "\"";
        break;
    case Function:
        break;
    case Number:
        return "\"" + obj.toString() + "\"";
        break;
    case String:
        return "\"" + obj.toString() + "\"";
        break;
    }
};
window.ToLowerCase = function (value) {
    if (value == null) {
        return "";
    }

    return value.toLowerCase();
}
window.PageName = function () {
    return location.href.split('/').pop().split('.').shift().toLowerCase();
}
window.ToUpperCase = function (value) {
    if (value == null) {
        return "";
    }
    return value.toUpperCase();
}
window.IsMoney = function (value) {
    var Regx = /^\d+(\.\d+)?$/;
    return Regx.test(value);
}
window.IsInt = function (value) {
    if (value == null) {
        return false;
    }
    var Regx = /^\d+$/
    return Regx.test(value);
}
window.Params = function (name) {
    var urm = location.search;
    urm = urm.substring(1);
    var urls = urm.split('&');
    var length = urls.length;
    for (var i = 0; i < length; i++) {
        var us = urls[i].split('=');
        if (us.length == 2 && ToLowerCase(us[0]) == ToLowerCase(name)) {
            return us[1];
        }
    }
    return null;
}
web = function () { }
web.Ajax2 = function (url, funcName, data, success) {
    $.ajax({
        type: "POST",
        url: url + "/" + funcName,
        dataType: "json",
        async: true,
        data: data,
        contentType: "application/json;charset=utf-8",
        success: success,
        error: function (error) {
            alert(error.responseText);
        }
    });
};
web.AjaxUpdate = function (url, fileId, success) {
    $.ajaxFileUpload(
            {
                url: url,
                fileElementId: fileId,
                dataType: 'text',
                success: success
            }
    );
}
String.prototype.ToDate = function () {
    /// <summary>字符串转日期 2012-12-12 12:12:12</summary>

    var array = this.split(' ');
    var date = array[0].split('-');
    var time = array[1].split(':');
    return new Date(date[0], parseInt(date[1]) - 1, date[2], time[0], time[1], time[2]);



}
Date.prototype.IsSize = function (date2) {
    /// <summary>比较时间大小 如果 this 大于date2 择返回true</summary>

    /// <param name="date2" type="date">时间2</param>
    if (this.getTime() > date2.getTime()) {
        return true;
    }
    return false;
    //var array = this.format("yyyy-MM-dd hh:mm:ss").split(' ');
    //var date = array[0].split('-');
    //var time = array[1].split(':');

    //var array1 = date2.format("yyyy-MM-dd hh:mm:ss").split(' ');
    //var date1 = array1[0].split('-');
    //var time1 = array1[1].split(':');

    //if (parseInt(date[0] + date[1] + date[2] + time[0] + time[1] + time[2]) > parseInt(date1[0] + date1[1] + date1[2] + time1[0] + time1[1] + time1[2])) {
    //    return true;
    //}
    //alert(this.format("yyyy-MM-dd hh:mm:ss")+"+"+date2.format("yyyy-MM-dd hh:mm:ss"))
    //return false;



}
Date.prototype.Days = function () {
    /// <summary>获得 当前时间 月里有多少天</summary>

    var date = this;
    return new Date(date.getYear(), date.getMonth() + 1, 0).getDate();
}
Date.prototype.AddDay = function (number) {
    /// <summary>在当前日期 上添加 天数</summary>
    /// <param name="number" type="number">天数</param>

    return new Date(this.getTime() + number * 24 * 60 * 60 * 1000);
}
String.prototype.format = function (format, arg) {
    ///Date(1382765400000)/

    var date = new Date(parseInt(this.replace("/Date(", "").replace(")/", "")));
    return date.format(format, arg)
}
Date.prototype.format = function (format, arg) {
    /// <summary>格式化日期</summary>
    /// <param name="format" type="String">格式化方式（如:yyyy-MM-dd）</param>
    /// <param name="arg" type="Boolean">不满足2位是否用0填充（只有传入false的时候不填充，其他时候均会被填充）</param>
    var fullYear = this.getFullYear();
    var year = this.getYear();
    var month = this.getMonth() + 1;
    var dayOfMonth = this.getDate();
    var hour = this.getHours();
    var minutes = this.getMinutes();
    var second = this.getSeconds();
    var dayOfWeek = this.getDay();
    function getFull(time) {
        return time >= 10 ? time : ('0' + time);
    }
    if (arg !== false) {
        month = getFull(month);
        dayOfMonth = getFull(dayOfMonth);
        hour = getFull(hour);
        minutes = getFull(minutes);
        second = getFull(second);
        dayOfWeek = getFull(dayOfWeek);
        year = getFull(year);
    }

    var reg_FullYear = /yyyy/g;
    var reg_year = /yy/g;
    var reg_month = /MM/g;
    var reg_dayOfMonth = /dd/g;
    var reg_hour = /hh/g;
    var reg_minutes = /mm/g;
    var reg_second = /ss/g;
    var reg_dayOfWeek = /DD/g;

    return format.replace(reg_FullYear, fullYear).replace(reg_year, year).replace(reg_month, month).replace(reg_dayOfMonth, dayOfMonth).replace(reg_hour, hour).replace(reg_minutes, minutes).replace(reg_second, second).replace(reg_dayOfWeek, dayOfWeek);

}
Date.prototype.BegEnd = function () {
    var theDate = this;
    var begdate = theDate.AddDay(-theDate.getDay());
    var endDate = theDate.AddDay(7 - theDate.getDay());
    return [begdate, endDate]
}
String.prototype.Week = function () {
    var date = new Date(parseInt(this.replace("/Date(", "").replace(")/", "")));
    var index = date.getDay();
    var array = ["日", "一", "二", "三", "四", "五", "六"];
    return "星期" + array[index];
}
Date.prototype.Week = function () {
    var date = this;
    var index = date.getDay();
    var array = ["日", "一", "二", "三", "四", "五", "六"];
    return "星期" + array[index];
}
web.Date = function (value) {
    var _date = new Date();
    var _ = (/\d+/).exec(value);
    _date.setTime(_);
    return _date;
}
function favSite() {
    //网站网址
    url = location.href;
    //网站名称
    title = window.title;
    if (window.sidebar) {
        window.sidebar.addPanel(title, url, "");
    }
    else if (document.all) {
        window.external.AddFavorite(url, title);
    }
    else {
        return true;
    }
}
window.Cookie = function (name, value) {
    if (value != null) {
        var Days = 60; //cookie 将被保存两个月    
        var exp = new Date(); //获得当前时间  
        exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000); //换成毫秒   
        document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
    }
    var arry = document.cookie.split("; ");
    var length = arry.length;
    for (var i = 0; i < length; i++) {
        var ary = arry[i].split("=");

        if (ary[0] == name) {

            return ary[1];
        }
    }
    return false;
}
