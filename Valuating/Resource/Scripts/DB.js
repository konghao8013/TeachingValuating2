(function () {
    DB = this;
})(window);
DB.CreateWebLog = function (className, methodName, parameters,success) {
    var log = WebServerLog();
    log.ClassName = className;
    log.Name = methodName;
    log.Parameter = parameters;
    $.Json("/api/WebDB", "Invoking", log, function (data) {
      
        data = JSON.parse(data);
        if (success != null && data.Message=="OK") {
            success(data);
        } else if (data != null && data.Message != "OK") {
            alert(data.Message+"服务端错误");
        }


    }, function (error) {
        alert(error.responseText+ "方法调用错误请检查");
    });
};
DB.VT = function () {
    
};
//获得系统名称
DB.VT.SystemName = "教师专业能力评估系统   Powered By 重庆雅乐视科技有限公司   All Rights Reserved.";

