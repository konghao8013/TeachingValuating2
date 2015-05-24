var NewExtPageBar = function(pageClick) {
    var bar = new Object();
    bar.pageIndex = 1;
    bar.pageMaxIndex = 0;
    bar.Txt_PageNumber = null;
    bar.PageClick = function(index) {
        bar.pageIndex = index;
       
        pageClick(index);
    };
    bar.setPageMaxIndex= function(count) {
        bar.pageMaxIndex = count;
    }
    bar.CreatePageBBar = function() {
        var upPage = new Ext.Button({
            text: '上一页',
            listeners: {
                click: function () {
                    if (bar.pageIndex - 1 > 0) {
                        bar.pageIndex -= 1;
                    } else {
                        Ext.Msg.alert("消息提示", "当前为第一页");
                    }
                    bar.PageClick(bar.pageIndex);
                }
            }
        });
        var nextPage = new Ext.Button({
            text: '下一页',
            listeners: {
                click: function () {
                    if (bar.pageIndex + 1<= bar.pageMaxIndex) {
                        bar.pageIndex += 1;
                    } else {
                      
                        Ext.Msg.alert("消息提示", "当前为最后页");
                    }
                    bar.PageClick(bar.pageIndex);
                }
            }
        });
        bar.Txt_PageNumber = new Ext.form.Text({ width: 80,  });
       
      
        var firstPage = new Ext.Button({
            text: '首页',
            listeners: {
                click: function () {
                    bar.PageClick(1);
                }
            }
        });
        var lastPage = new Ext.Button({
            text: '尾页',
            listeners: {
                click: function () {
                    bar.PageClick(bar.pageMaxIndex);
                }
            }
        });
        var skipPage = new Ext.Button({
            text: '跳页',
            listeners: {
                click: function () {
                    var i = parseInt(bar.Txt_PageNumber.getValue());
                    i = isNaN(i) ? 1 : i;
                    bar.PageClick(i);
                }
            }
        });

        return new Ext.toolbar.Toolbar({
            items: [
                firstPage,
                upPage,
                bar.Txt_PageNumber,
                nextPage,
                lastPage,
                skipPage

            ]
        });

    }
    bar.getNumber= function() {
        return bar.Txt_PageNumber.getValue();
    }
    bar.setTxt_Number=function(value) {
        bar.Txt_PageNumber.setValue(value);
    }
    return bar;
};

//var PageIndex = 1;
//var PageMaxIndex = 0;
var NewExtRate = function (text) {
    var msg = text == null ? "数据上传中请等待" : text;
    var rate = new Object();
    rate.Box = Ext.MessageBox.show({
        title: '请等待',
        msg: msg,
        width: 240,
        progress: true,
        closable: false,
        value: 9
    });
    rate.StartExtFnBox=function () {
        rate.IsUpDate = true;
        rate.UpdateNumber();
    }
    rate.EndExtFnBox=function () {
        rate.IsUpDate = false;
        rate.Box.close();
    }
    rate.RateNumber = 1;
    rate.IsUpDate = false;
    rate.UpdateNumber=function () {
        if (rate.IsUpDate) {
            rate.RateNumber++;
            if (rate.RateNumber == 100) {
                rate.RateNumber = 1;
            }
            Ext.MessageBox.updateProgress(rate.RateNumber / 100, "", msg);
            setTimeout(rate.UpdateNumber, 10);
        }
    }
    return rate;
}

