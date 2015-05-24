/// <reference path="jquery-1.8.3.js" />


(function ($) {
   
    jQuery.ToLowerCase = function(value) {
        if (value == null) {
            return "";
        }

        return value.toLowerCase();
    };
    //返回iframe标签
    jQuery.Iframe = function (src) {
        var height = $(window).height() - 102;
        var iframe = "<iframe class=\"main_iframe\" src=\"" + src + "\" style=\"padding:0px;margin:0px;width:100%;height:"+height+"px\" frameborder=\"0\" scrolling=\"auto\"></iframe>";
       
        return iframe;
    };
    jQuery.Json = function (url, funcName, data, success, errorFunc) {
     
        data = data == null ? data : JSON.stringify(data);
       // alert(data);
        //    data = JSON.stringify(data);
        $.ajax({
            type: "POST",
            url: url + "/" + funcName,
            dataType: "json",
            async: true,
            data: data,
            contentType: "application/json;charset=utf-8",
            success: function (rul) {
                if (success != null) {
                    success(rul);
                   
                }
            },
            error: function (error) {
                if (errorFunc != null) {
                    errorFunc(error);
                   // alert("尼玛错误信息在那里" + error.responseText);
                } else {
                    alert(error.responseText+"错误提示");
                }
            }
        });
    };

    //设置Cookie 默认保存两个月 cookie名称 cookie值 cookie保存时间
    jQuery.Cookie = function(name, value,saveDate) {
        if (value != null) {
           
            var Days = 60; //cookie 将被保存两个月  
           
            if (saveDate != null&&IsInt(saveDate)) {
                Days = saveDate;
            }
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
        return null;
    };
    //Unicode转文本
    jQuery.HexToDec = function (str) {
        if (str != null && str.length > 0) {
            str = str.replace(/\\/g, "%");
        }
        
        return unescape(str);
    };
    //文本转Unicode
    jQuery.DecToHex = function (str) {
        var res = [];
        for (var i = 0; i < str.length; i++)
            res[i] = ("00" + str.charCodeAt(i).toString(16)).slice(-4);
        return "\\u" + res.join("\\u");
    }
   
})(jQuery);
window.Serialize = function (obj) {
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
window.PageName = function() {
    return location.href.split('/').pop().split('.').shift().toLowerCase();
};
window.ToUpperCase = function(value) {
    if (value == null) {
        return "";
    }
    return value.toUpperCase();
};
window.IsMoney = function(value) {
    var Regx = /^\d+(\.\d+)?$/;
    return Regx.test(value);
};
window.IsInt = function(value) {
    if (value == null) {
        return false;
    }
    var Regx = /^\d+$/
    return Regx.test(value);
};
window.Params = function(name) {
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
String.prototype.ToDate = function() {
    /// <summary>字符串转日期 2012-12-12 12:12:12</summary>

    var array = this.split(' ');
    var date = array[0].split('-');
    var time = array[1].split(':');
    return new Date(date[0], parseInt(date[1]) - 1, date[2], time[0], time[1], time[2]);


};
Date.prototype.IsSize = function(date2) {
    /// <summary>比较时间大小 如果 this 大于date2 择返回true</summary>

    /// <param name="date2" type="date">时间2</param>
    if (this.getTime() > date2.getTime()) {
        return true;
    }
    return false;
};
Date.prototype.Days = function() {
    /// <summary>获得 当前时间 月里有多少天</summary>

    var date = this;
    return new Date(date.getYear(), date.getMonth() + 1, 0).getDate();
};
Date.prototype.AddDay = function(number) {
    /// <summary>在当前日期 上添加 天数</summary>
    /// <param name="number" type="number">天数</param>

    return new Date(this.getTime() + number * 24 * 60 * 60 * 1000);
};
String.prototype.format = function(format, arg) {
    ///Date(1382765400000)/

    var date = new Date(parseInt(this.replace("/Date(", "").replace(")/", "")));
    return date.format(format, arg);
};
Date.prototype.format = function(format, arg) {
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

};
Date.prototype.BegEnd = function() {
    var theDate = this;
    var begdate = theDate.AddDay(-theDate.getDay());
    var endDate = theDate.AddDay(7 - theDate.getDay());
    return [begdate, endDate];
};
String.prototype.Week = function() {
    var date = new Date(parseInt(this.replace("/Date(", "").replace(")/", "")));
    var index = date.getDay();
    var array = ["日", "一", "二", "三", "四", "五", "六"];
    return "星期" + array[index];
};
Date.prototype.Week = function() {
    var date = this;
    var index = date.getDay();
    var array = ["日", "一", "二", "三", "四", "五", "六"];
    return "星期" + array[index];
};

//表示全局唯一标识符 (GUID)。
function Guid(g) {
    var arr = new Array(); //存放32位数值的数组

    if (typeof (g) == "string") { //如果构造函数的参数为字符串
        InitByString(arr, g);
    }
    else {
        InitByOther(arr);
    }
    //返回一个值，该值指示 Guid 的两个实例是否表示同一个值。
    this.Equals = function (o) {
        if (o && o.IsGuid) {
            return this.ToString() == o.ToString();
        }
        else {
            return false;
        }
    }
    //Guid对象的标记
    this.IsGuid = function () { }
    //返回 Guid 类的此实例值的 String 表示形式。
    this.ToString = function (format) {
        if (typeof (format) == "string") {
            if (format == "N" || format == "D" || format == "B" || format == "P") {
                return ToStringWithFormat(arr, format);
            }
            else {
                return ToStringWithFormat(arr, "D");
            }
        }
        else {
            return ToStringWithFormat(arr, "D");
        }
    }
    //由字符串加载
    function InitByString(arr, g) {
        g = g.replace(/\{|\(|\)|\}|-/g, "");
        g = g.toLowerCase();
        if (g.length != 32 || g.search(/[^0-9,a-f]/i) != -1) {
            InitByOther(arr);
        }
        else {
            for (var i = 0; i < g.length; i++) {
                arr.push(g[i]);
            }
        }
    }
    //由其他类型加载
    function InitByOther(arr) {
        var i = 32;
        while (i--) {
            arr.push("0");
        }
    }
    /*
    根据所提供的格式说明符，返回此 Guid 实例值的 String 表示形式。
    N  32 位： xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    D  由连字符分隔的 32 位数字 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx 
    B  括在大括号中、由连字符分隔的 32 位数字：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx} 
    P  括在圆括号中、由连字符分隔的 32 位数字：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx) 
    */
    function ToStringWithFormat(arr, format) {
        switch (format) {
            case "N":
                return arr.toString().replace(/,/g, "");
            case "D":
                var str = arr.slice(0, 8) + "-" + arr.slice(8, 12) + "-" + arr.slice(12, 16) + "-" + arr.slice(16, 20) + "-" + arr.slice(20, 32);
                str = str.replace(/,/g, "");
                return str;
            case "B":
                var str = ToStringWithFormat(arr, "D");
                str = "{" + str + "}";
                return str;
            case "P":
                var str = ToStringWithFormat(arr, "D");
                str = "(" + str + ")";
                return str;
            default:
                return new Guid();
        }
    }
}
//Guid 类的默认实例，其值保证均为零。
Guid.Empty = new Guid();
//初始化 Guid 类的一个新实例。
Guid.NewGuid = function () {
    var g = "";
    var i = 32;
    while (i--) {
        g += Math.floor(Math.random() * 16.0).toString(16);
    }
    return new Guid(g);
}
//a)         “N”： xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

//b)         “D”  由连字符分隔的 32 位数字 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx

//c)         “B”  括在大括号中、由连字符分隔的 32 位数字：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}

//d)         “P”  括在圆括号中、由连字符分隔的 32 位数字：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)

