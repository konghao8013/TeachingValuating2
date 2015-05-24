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
/// <reference path="~/Resource/Scripts/dbserver/DBSource.js"></script>
//初始化系统顶部菜单
function getTopBar() {
    var top_label = new Ext.form.Label({
        text:"登录信息"
    });
    $.Json("/login", "GetUser", null, function (user) {
        top_label.setText("登录信息: ID:"+user.LoginId+"\t姓名："+user.Name);
    }, function(error) {
        alert("错误");
    });
    var exitButton = new Ext.Button({
        text: "退出系统",
        width: 120,
       
        listeners: {
            click: function() {
                $.Json("/login", "Logout", null, function() {
                    location.href = "/login";
                });
            }
        }
    });
    return Ext.create("Ext.toolbar.Toolbar", {
        height: 30,
        region: 'north',
       
        items: [top_label,
            '->',
            exitButton
        ]
      

    });
    

}

function GetContentTable() {
    return Ext.create('Ext.tab.Panel', {
        resizeTabs: true,
        enableTabScroll: true,
        collapsible: false,
        id: 'tabCenterPanel',
        region: 'center',
        defaults: {
            autoScroll: true,
            bodyPadding: 10
        },
        plugins: Ext.create('Ext.ux.TabCloseMenu', {
            closeTabText: '关闭当前',
            closeOthersTabsText: '关闭其他',
            closeAllTabsText: '关闭所有',
        })


    });

}


//初始化系统底部菜单
function getBottomBar() {
    var toolbar = Ext.create('Ext.toolbar.Toolbar', {
        height: 30,
        region: 'south',
        items: [
            '->',
            {
                xtype: 'label',
                hideLabel: true,
                name: 'field1',
                text: DB.VT.SystemName,
                color: 'red',
            }
        ]
    });
    return toolbar;
}


//创建系统布局视图
function CreateViewport() {
    Ext.create("Ext.container.Viewport", {
        title: 'BorderLayout Panel',
        layout: {
            type: 'border',
            padding: 0
        },
        collapsible: true,

        defaults: {
            collapsible: true,
            split: true
        },
        items: [
            getTopBar(),
            getBottomBar(),
            {
                header: true,
                region: 'west',
                //items: GetMenus(),
                id: 'leftMenu',
                width: 200,
                title:'系统菜单'
            }, GetContentTable()
        ]
    });

}

Ext.onReady(function () {

    CreateViewport();
    LoadData();

});

//添加选项卡
function AddTabPanel(menu) {
    var tab = Ext.getCmp('tabCenterPanel');
    var tempId = 'id_menuitem_' + menu.Id;
    var item = Ext.getCmp(tempId);
    var iframe = $.Iframe(menu.Url);
    if (item == null) {

        item = {
            id: tempId,
            title: menu.Name,
            closable: true,
            html: iframe,
            bodyPadding: 0,


        };
        item = tab.add(item);
    }
    tab.setActiveTab(item);

}

//数据加载
function LoadData() {
    DB.SysMenuServer.List(3, function (log) {
        var list = log.ReturnValue;
        var leftPanel = Ext.getCmp("leftMenu");
        _.chain(list).where({ 'ObjId': 0 }).forEach(function (obj) {
          
            var a = Ext.create("Ext.panel.Panel", {
                title: obj.Name,
                collapsible: true,
               
                titleAlign: 'center',
                items: Ext.create("Ext.menu.Menu", {
                    floating: false,
                    titleAlign: 'center',
                    items: _.chain(list).where({ 'ObjId': obj.Id }).map(function (menu) {
                        var menuitem = Ext.create("Ext.menu.Item", {
                            text: menu.Name,
                            handler: function () {
                                AddTabPanel(menu);
                            }
                        });
                        return menuitem;
                    }).value()
                })
            });
            leftPanel.add(a);
        });
    });
}