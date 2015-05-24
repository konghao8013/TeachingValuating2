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
/// <reference path="../../dbserver/SysMenuServer.js" />
/// <reference path="../../dbserver/ParperSendServer.js" />
/// <reference path="../../dbserver/StudentRecordServer.js" />
/// <reference path="../../model/ParperSendType.js" />

Ext.onReady(function () {
    CreateView();
});
function CreateView() {
    Ext.create('Ext.container.Viewport', {
        layout: 'border',

        items: [CreateGrid()]
    });
}

var Grid_Store;
function CreateGrid() {
    Grid_Store = new Ext.data.Store({
        fields: ['TypeName', 'School', 'College', 'Number']
    });
    var grid = new Ext.grid.Panel({
        store:Grid_Store,
        columns: [
        { header: '测评批次', dataIndex: 'TypeName', width: 120, align: 'center' },
            {
                header: '学校名称', dataIndex: 'School', width: 240, align: 'center'
                
            },
        { header: '学院名称', dataIndex: 'College', width: 240, align: 'center' },
        { header: '试卷数量', dataIndex: 'Number', width: 120, align: 'center' }

        ],
        region:'center'
    });
    DB.StudentRecordServer.GroupSchoolNumber(function(rul) {
        var list = rul.ReturnValue;
        Grid_Store.loadData(list);
    });
    return grid;
}