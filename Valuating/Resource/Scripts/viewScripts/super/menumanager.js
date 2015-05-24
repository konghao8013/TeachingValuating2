
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
/// <reference path="../../dbserver/SysMenuServer.js" />
Ext.onReady(function () {
    CreateView();
});

function CreateView() {
    Ext.create('Ext.container.Viewport', {
        layout: 'border',

        items: [CreateTreeMenu(), CreateForm()]
    });
}

var MenuStore;
var treePanel;
function CreateTreeMenu() {
    MenuStore = Ext.create('Ext.data.TreeStore', {
        id: 'UserStore',
        root: {
            expanded: true,
            children: [],
            id: "root",
            text: "根菜单",
            
        },
    });
    treePanel = Ext.create('Ext.tree.Panel', {
        title: '菜单管理',
        width: 250,
        height: 150,
        store: MenuStore,

        region: 'west',
        id: 'treePanel',
        listeners: {
             beforeitemmousedown: Treebeforeitemmousedown
        },
        rootVisible: false
    });
    
     InitTreePanel();
    return treePanel;
}



///点击树形节点发生
function Treebeforeitemmousedown(view, model, item, index, o) {
    var menuId = parseInt(model.data.id.replace("root_", ""));
    // treePanel.expandNode
    DB.SysMenuServer.Reader(menuId, function (rul) {
        var m = rul.ReturnValue;
        InitFrom(m);
    });
}
//设置表单值
function InitFrom(model) {
    GModel = model;
    menuName.setValue(model.Name);
    menuUrl.setValue(model.Url);
    Com_Menu.setValue(parseInt(model.ObjId));
}
//获取表单里面的值
function GetFrom() {
    if (GModel == null) {
        GModel = WM.SysMenuType();
    }
    GModel.Name = menuName.getValue();
    GModel.Url = menuUrl.getValue();
    GModel.ObjId = Com_Menu.getValue();
    GModel.Type = 3;
    return GModel;
}

var GModel;
function InitTreePanel() {
    DB.SysMenuServer.Select(function (rul) {
        var list = rul.ReturnValue;
        MenuStore.getRoot().removeAll();
        _.chain(list).where({ "ObjId": 0 }).forEach(function(model) {
            var node = Ext.create("Ext.data.TreeModel", { text: model.Name, id: 'root_' + model.Id,expanded:true });
            _.chain(list).where({ "ObjId": model.Id }).forEach(function(m) {
                var n = Ext.create("Ext.data.TreeModel", { text: m.Name, id: 'root_' + m.Id,leaf:true });
                node.appendChild(n);
            });
            MenuStore.getRoot().appendChild(node);
        });
    });

}
function CreateCenter() {
    var panel = Ext.create("Ext.panel.Panel", {
        region: 'center',
        title: '菜单设置',
        items: [CreateForm]
    });
    return panel;
}
//菜单名称
var menuName;
//菜单地址
var menuUrl;
//菜单类别
var Com_Menu;
function CreateForm() {
    menuName = new Ext.form.Text({
        style: 'margin-top:30px;',
        width:400,
    });
    menuName.fieldLabel = "菜单名称";
  //  menuName.setWidth(300);
    menuUrl = new Ext.form.Text({
        style: 'margin-top:30px;',
        width: 400,
    });
    menuUrl.fieldLabel = "菜单地址";
  //  menuUrl.setWeight(300);

    var MenuStore = Ext.create('Ext.data.Store', {
        id: 'menuStore',
        fields: ['Id', 'Name'],
        proxy: {
            type: 'memory',
            reader: {
                type: 'json',
                root: 'items'
            }
        }
    });
    Com_Menu = Ext.create('Ext.form.ComboBox', {
        fieldLabel: '父级菜单',
        store: MenuStore,
        queryMode: 'local',
        displayField: 'Name',
        valueField: 'Id',
        id: 'userTypeId',
        style: 'margin-top:30px;',
        width:400
    });

    DB.SysMenuServer.SelectObjIdList(0, function (rul) {
        var list = rul.ReturnValue;
        list[list.length] = { Id: 0, Name: '根菜单' };
        MenuStore.loadData(list);
    });
    var addBtn = Ext.create("Ext.Button", {
        text: '添加菜单',
        style: 'margin-left:10px;',
        width: 80,
        height: 30,
        listeners: {
            click: function () {
                InitFrom(WM.SysMenuType());
            }
        }
    });
    var deleteBtn = Ext.create("Ext.Button", {
        text: "删除菜单",
        style: 'margin-left:10px;',
        width: 80,
        height: 30,
        listeners: {
            click: function () {
                var model = GetFrom();
                if (model != null && model.Id > 0) {

                    DB.SysMenuServer.DeleteModel(model.Id, function () {
                        InitTreePanel();
                    });
                }
            }
        }
    });
    var saveBtn = Ext.create("Ext.Button", {
        text: '保存菜单',
        style: 'margin-left:10px;',
        width: 80,
        height:30,
        listeners: {
            click: function () {
                var model = GetFrom();
                DB.SysMenuServer.SaveModel(model, function () {
                    InitTreePanel();
                });
            }
        }
    });

    var form = new Ext.form.Panel({
        region: 'center',
        title: '系统菜单设置',
        defaultType: 'textfield',
        bodyPadding: 10,
        width: 400,

        border: 0,
        defaults: {
            labelWidth: 100
        },
        items: [
            menuName,
            menuUrl,
            Com_Menu
            ,
            new Ext.panel.Panel({
                style: 'margin-top:30px;',
                width: 400,
                border:0,
                buttons: [
                    addBtn,
                    deleteBtn,
                    saveBtn
                ]
            })
        ],
       
    });
    return form;
}