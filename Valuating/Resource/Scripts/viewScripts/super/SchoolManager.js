
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
/// <reference path="../../dbserver/SchoolServer.js" />
/// <reference path="../../dbserver/SysMenuServer.js" />
Ext.onReady(function () {
    CreateView();
});

function CreateView() {

    Ext.create('Ext.container.Viewport', {
        layout: 'border',

        items: [CreateTree(), CreateForm()]
    });
}

var Schoolstore;

function CreateTree() {
    Schoolstore = Ext.create('Ext.data.TreeStore', {
        id: 'TreeschoolStore',
        root: {
            expanded: true,
            children: [],
            id: "root",
            text: "根菜单",

        },
    });
    var tree = new Ext.tree.Panel({
        title: '基础信息',
        store: Schoolstore,
        region: 'west',
        id: 'treePanel',
      
        width: 300,
        listeners: {
            beforeitemmousedown: TreeSelect
        },
        rootVisible: false
    });
    InitTree();
    return tree;
}


function TreeSelect(a, b, c, d) {
    var id = parseInt(b.data.id.replace("root_", ""));

    DB.SchoolServer.RenderSchool(id, function (rul) {
        var model = rul.ReturnValue;
        SetSchoolForm(model);
    });
}

function InitTree() {

    DB.SchoolServer.SelectList(function (rul) {
        var list = rul.ReturnValue;
        Schoolstore.getRoot().removeAll();

        _.chain(list).where({ "ObjId": 0 }).forEach(function (model) {
            var node = Ext.create("Ext.data.TreeModel", { text: model.Name, id: "root_" + model.Id, expanded: true });
            _.chain(list).where({ "ObjId": model.Id }).forEach(function (m) {
                var n = Ext.create("Ext.data.TreeModel", { text: m.Name, id: 'root_' + m.Id, leaf: true });
                node.appendChild(n);
            });
            Schoolstore.getRoot().appendChild(node);
        });
    });
}

function SetSchoolForm(model) {
    GModel = model;

    TextName.setValue(model.Name);
    Com_Type.setValue(parseInt(model.ObjId));

}

function GetSchoolForm() {
    if (GModel == null) {
        GModel = WM.SchoolType();
    }
    GModel.Name = TextName.getValue();
    GModel.ObjId = Com_Type.getValue();
    return GModel;
}

var GModel;
var TextName;
var Com_Type;
//绑定下拉框学校对象
function LoadTypeSchool() {
    DB.SchoolServer.SelectTypeSchool(0, function (rul) {

        var list = rul.ReturnValue;
        list[list.length] = { Id: 0, Name: "学校类别" };
        comStore.loadData(list);
    });
}

var comStore;
function CreateForm() {
    TextName = new Ext.form.Text({
        width: 300,
        style: 'margin-top:50px;margin-left:20px;',
        fieldLabel: '学校(院)名称',
        width:400,
    });
    comStore = Ext.create('Ext.data.Store', {
        id: 'schoolStore',
        fields: ['Id', 'Name'],
        proxy: {
            type: 'memory',
            reader: {
                type: 'json',
                root: 'items'
            }
        }
    });
    LoadTypeSchool();
    Com_Type = new Ext.form.ComboBox({
        queryMode: "local",
        store: comStore,
        valueField: "Id",
        displayField: "Name",
        width: 400,
        style: 'margin-top:60px;margin-left:20px;',
        fieldLabel:'所属类别'
    });


    var addButton = Ext.create("Ext.Button", {
        text: "添加学校(院)",
        width:80,
        height:30,
        listeners: {
            click: function () {
                SetSchoolForm(WM.SchoolType());
            }
        }

    });
    var deleteButton = Ext.create("Ext.Button", {
        text: "删除学校(院)",
        width: 80,
        height: 30,
        listeners: {
            click: function () {
                if (GModel != null && GModel.Id > 0) {
                    DB.SchoolServer.DeleteSchool(GModel.Id, function () {
                        InitTree();
                        if (GModel.ObjId == 0) {
                            LoadTypeSchool();
                        }
                    });
                } else {
                    Ext.Msg.alert("错误提示", "请选择正确的树形节点");
                }
            }
        }
    });
    var saveButton = Ext.create("Ext.Button", {
        text: "保存学校(院)",
        width: 80,
        height: 30,
        listeners: {
            click: function () {
                var model = GetSchoolForm();

                DB.SchoolServer.SaveSchool(model, function (rul) {
                    InitTree();
                    if (rul.ReturnValue.ObjId == 0) {
                        LoadTypeSchool();
                    }
                });
            }
        }
    });
  
    var form = new Ext.form.Panel({
        title: "学校信息",
        region: "center",
        items: [TextName, Com_Type,
            new Ext.panel.Panel({
                width: 400,
                style: 'margin-left:20px;margin-top:100px;',
                border: 0,
                buttons: [
                    addButton,
                    deleteButton,
                    saveButton
                ]
            })
        ]
    });
    return form;
}