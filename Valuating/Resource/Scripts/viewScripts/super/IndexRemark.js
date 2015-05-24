
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
/// <reference path="../../dbserver/SysMenuServer.js" />
/// <reference path="../../dbserver/IndexSortServer.js" />
/// <reference path="../../model/IndexSortType.js" />


Ext.onReady(function () {
    CreateView();
});

function CreateView() {
    Ext.create('Ext.container.Viewport', {
        layout: 'border',

        items: [CreateGrid()]
    });
}

var DMODEL;
var GridStore;
var GridWinShowSort;
function WinShowSort() {
    GridWinShowSort =new Ext.window.Window({
        title: '设计评价指标套',
        width: 630,
        height: 300,
        items: [
            CreateSortGrid(), CreateSortForm()
        ]
    });
    GridWinShowSort.show();
}



var SortStore;
var SortName_txt;
function CreateSortForm() {
    SortName_txt = new Ext.form.TextArea({
     
        width: 280,
        style: 'margin-left:20px;margin-top:20px;',
        height:150

    });
    var from = new Ext.form.Panel({
        region: 'center',
        width: 320,
        height: 270,
        x: 300,
        y:-270,
        items: [
            new Ext.form.Label({
                width: 320,
                height: 50,
                text: "编辑指标套名称",
                style: 'margin-left:85px;margin-top:20px;font-size:22px;text-align: center;',
                align: 'center'

            }),
           SortName_txt
        ],
        buttons: [

            new Ext.Button({
                text: '新增',
                listeners: {
                    click: function () {
                        var model = WM.IndexSortType();
                        model.State = false;
                        SetSortForm(model);
                    }
                }
            }),
            new Ext.Button({
                text: '删除',
                listeners: {
                    click: function() {
                        if (GSModel != null) {
                            DB.IndexSortServer.DeleteSort(GSModel.Id, function() {
                                LoadSortStore();
                            });
                        } else {
                            Ext.Msg.alert("请选择需要删除的指标套");
                        }
                    }
                }
            }),
            new Ext.Button({
                text: "保存",
                listeners: {
                    click: function () {
                        if (GSModel == null) {
                            GSModel = new WM.IndexSortType();
                            GSModel.State = 0;
                        }
                        GSModel.Name = SortName_txt.getValue();
                        DB.IndexSortServer.SaveSort(GSModel, function(rul) {
                            var model = rul.ReturnValue;
                            SetSortForm(model);
                            LoadSortStore();
                        });
                    }
                }

            }),
            new Ext.Button({
                text: '启用',
                listeners: {
                    click: function() {
                        if (GSModel != null) {
                           
                            DB.IndexSortServer.Start(GSModel.Id, function() {
                                LoadSortStore();
                            });
                        } else {
                            Ext.Msg.alert("提示信息", "请选择需要启用的指标套");
                        }
                    }
                }
            })
        ]
    });
    return from;
}

function SaveSort() {
    
}

function LoadSortStore() {
    if (SortStore != null) {
        DB.IndexSortServer.SortList(function(rul) {
            var list = rul.ReturnValue;
            SortStore.loadData(list);
            if (list.length > 0) {
                SortComBox.setValue(list[0].Id);
            }
        });
    }
}

function CreateSortStore() {
    if (SortStore == null) {
        SortStore = new Ext.data.Store({
            fields: [
                'Id',
                'Name',
                'State'
            ]
        });
    }
}

function CreateSortGrid() {
    CreateSortStore();
    var grid = new Ext.grid.Panel({
        title: '',
        columns: [
            { header: '指标套名称', width: 200, dataIndex: 'Name' },
            {
                header: '状态', width: 80, dataIndex: 'State',
                renderer: function(value) {
                    return value ? "启用" : "禁用";
                }
            }
        ],
        width: 300,
        height: 270,
        store:SortStore,
        listeners: {
            selectionchange: function (view, models, c, d) {
                if (models.length == 0) {
                    return;
                }
                var model = models[0].data;
                SetSortForm(model);
                //DB.IndexSortServer.ReaderSort(model.Id, function(rul) {
                //    var 
                //});

            }
        },
        region:'west'

    });
    LoadSortStore();
    return grid;
}

var GSModel;
function SetSortForm(model) {
    if (model == null) {
        model = WM.IndexSortType();
        model.State = false;
    }
    GSModel = model;
    SortName_txt.setValue(model.Name);
}

var SortComBox;
function CreateGrid() {
    GridStore = new Ext.data.Store({
        fields: ["Name", "TypeName", "Content", "Ratio", "Id"]
    });
    var sortBtn = new Ext.Button({
        text: "设计评价指标套",
        width: 180,
        height: 25,
        listeners: {
            click: function() {
                WinShowSort();
            }
        }
    });
    CreateSortStore();
    LoadSortStore();
    SortComBox = new Ext.form.ComboBox({
        store: SortStore,
        valueField: 'Id',
        displayField: 'Name',
       queryMode:'local'
    });
    var tbar = new Ext.toolbar.Toolbar({
        items: [
            new Ext.form.Label({
                text:"指标套设计："
            }),
            SortComBox,
            new Ext.Button({
                text: '筛选数据',
                listeners: {
                    click: function() {
                        LoadDataGrid();
                    }
                }

            }),
            '->',
            sortBtn
        ]
    });
    var grid = new Ext.grid.Panel({
        store: GridStore,
        tbar:tbar,
        columns: [
             { header: '类别', dataIndex: 'TypeName', width: 60 },
            { header: '名称', dataIndex: 'Name', width: 60 },
            { header: '权重', dataIndex: 'Ratio', width: 40 },
            { header: '指标说明', dataIndex: 'Content', width: 540 },
            {
                header: '设置评语', dataIndex: 'Id', width: 60,
                renderer: function (value, b, c) {
                    return '<a href="javascript:CreateWin(20)">设置评语</a>';
                }
            }, {
                header: '编辑指标',
                dataIndex: 'Id',
                width: 60,
                renderer: function (value) {
                    return '<a href="javascript:EditIndex(' + value + ')">编辑</a>';
                }
            }, {
                header: '删除指标', dataIndex: 'Id', width: 60,
                renderer: function (value) {
                    return '<a href="javascript:DeleteIndex(' + value + ')">删除</a>';
                }

            }
        ],
        region: 'center',
        height: 300,
        forceFit: true,
        width: 700,
        listeners: {
            selectionchange: function (view, models) {
                if (models.length > 0) {
                    DMODEL = models[0].data;
                }

                //  CreateWin();
            }
        },
        buttons: [
            new Ext.Button({
                text: '新增指标',
                width: 120,
                height: 25,
                listeners: {
                    click: function () {
                        EditIndex(0);
                    }
                }
            })
        ]
    });

    LoadDataGrid();
    return grid;
}
//加载数据
function LoadDataGrid() {
    var typeId = 0;
    if (SortComBox != null) {
        typeId = SortComBox.getValue();
    }
    typeId = isNaN(typeId)||typeId==null ? 1 : typeId;
 
    DB.PresentServer.PresentList(typeId,function (rul) {
        var list = rul.ReturnValue;
        GridStore.loadData(list);
    });
}

function DeleteIndex(id) {
    Ext.Msg.confirm("提示信息", "指标删除后不可恢复", function (rul) {
        if (rul == "yes") {
            DB.PresentServer.DeleteId(id, function (rul) {
                LoadDataGrid();
            });
        }
    });

}

var EditIndexWin;
var IndexTypeStore;
function EditIndex(id) {
    var model;
     IndexTypeStore = new Ext.data.Store({
        fields: [
            'Id', 'Name'
        ]
    });
    var indexType = new Ext.form.ComboBox({
        fieldLabel: '指标类别',
        width: 400,
        displayField: 'Name',
        valueField: 'Id',
        queryMode: 'local',
        store: IndexTypeStore
    });
    var indexName = new Ext.form.Text({
        fieldLabel: '指标名称',
        width: 400,
        style: 'mangin-top:10px;'
    });
    var indexNumber = new Ext.form.Number({
        fieldLabel: '指标权重',
        width: 400,
        style: 'mangin-top:10px;'
    });
    var indexContent = new Ext.form.TextArea({
        width: 400,
        height: 80,
        fieldLabel: '指标说明',
        style: 'mangin-top:10px;'
    });
    var indexOrder = new Ext.form.Number({
        fieldLabel: '指标顺序',
        width: 400,
        style: 'mangin-top:10px;'
    });
    IndexTypeStore.loadData([
        { Id: 1, Name: '教案' },
        { Id: 2, Name: '课件' },
        { Id: 3, Name: '视频' },
        { Id: 4, Name: '反思' },
        { Id: 5, Name: '说课' }
    ]);
    EditIndexWin = new Ext.window.Window({
        title: '编辑指标',
        width: 410,
        height: 330,
        items: [
        indexType,
        indexName,
        indexNumber,
        indexOrder,
        indexContent
        ],
        buttons: [
            new Ext.Button({
                text: '保存指标',
                listeners: {
                    click: function () {
                        model.TypeId = indexType.getValue();
                        model.Name = indexName.getValue();
                        model.Content = indexContent.getValue();
                        model.Ratio = indexNumber.getValue();
                        model.OrderId = indexOrder.getValue();
                        model.Version = SortComBox.getValue();
                        DB.PresentServer.SaveType(model, function (rul) {
                            EditIndexWin.close();
                            LoadDataGrid();
                        });
                    }
                }
            })
        ]
    });

    DB.PresentServer.SelectId(id, function (rul) {
        model = rul.ReturnValue;
        model = model == null ? WM.PresentType() : model;
        indexType.setValue(model.TypeId);
        indexName.setValue(model.Name);
        indexContent.setValue(model.Content);
        indexNumber.setValue(model.Ratio);
        indexOrder.setValue(model.OrderId);

    });
    EditIndexWin.show();
}


var PYWIN;
function CreateWin(data) {

    PYWIN = new Ext.Window({
        title: '' + DMODEL.Name + '(' + DMODEL.Content + ")",
        width: 500,
        height: 410,
        items: [
             CreateForm()
        ]
    });
    PYWIN.show();
    LoadForm();
}

var LoadForm = function () {
    if (DMODEL == null) {
        alert("请选择正确的指标值再操作");
    }
    DB.IndexRemarkServer.RenderRemark(DMODEL.Id, function (rul) {
        var model = rul.ReturnValue;
        Text_A.setValue(model.A);
        Text_B.setValue(model.B);
        Text_C.setValue(model.C);
        Text_D.setValue(model.D);
        Text_E.setValue(model.E);
        IndexModel = model;
    });
}
var IndexModel;
var Text_A;
var Text_B;
var Text_C;
var Text_D;
var Text_E;
function CreateForm() {
    Text_A = new Ext.form.TextArea({
        width: 490,
        height: 60,
        fieldLabel: "优秀评价"
    });
    Text_B = new Ext.form.TextArea({
        width: 490,
        height: 60,
        fieldLabel: "良好评价"
    });
    Text_C = new Ext.form.TextArea({
        width: 490,
        height: 60,
        fieldLabel: "中等评价"
    });
    Text_D = new Ext.form.TextArea({
        width: 490,
        height: 60,
        fieldLabel: "合格评价"
    });
    Text_E = new Ext.form.TextArea({
        width: 490,
        height: 60,
        fieldLabel: "不合格评价"
    });
    var saveButton = new Ext.Button({
        text: "保存指标评语",
        listeners: {
            click: function () {
                IndexModel.A = Text_A.getValue();
                IndexModel.B = Text_B.getValue();
                IndexModel.C = Text_C.getValue();
                IndexModel.D = Text_D.getValue();
                IndexModel.E = Text_E.getValue();
                DB.IndexRemarkServer.SaveRemark(IndexModel, function () {
                    Ext.Msg.alert("消息提示", "保存成功");
                    if (PYWIN != null) {
                        PYWIN.close();
                    }
                });
            }
        },
        height: 30,
        width: 120
    });

    var from = new Ext.form.Panel({
        region: 'center',
        items: [
            Text_A,
            Text_B,
            Text_C,
            Text_D,
            Text_E,

        ],
        buttons: [
            saveButton
        ]
    });
    return from;
}