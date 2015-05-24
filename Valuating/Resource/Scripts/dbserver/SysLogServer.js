//系统日志操作类Syslog
DB.SysLogServer = function() {
};
//插入系统消息 日志内容,日志类别,来源		参数(1) System.String参数(2) System.String参数(3) System.String参数(4) System.String参数(5) System.Int64
DB.SysLogServer.InsertMessage=function(message,logType,source,loginid,time,success){
var array=new Array();
array[0]=message;
array[1]=logType;
array[2]=source;
array[3]=loginid;
array[4]=time;

DB.CreateWebLog("SysLogServer", "InsertMessage", array,success);
};
