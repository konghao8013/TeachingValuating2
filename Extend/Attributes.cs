using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




public class TableName : Attribute
{
    /// <summary>
    /// 表名称
    /// </summary>
    public string Name { set; get; }
    /// <summary>
    /// 表说明
    /// </summary>
    public string Explain { set; get; }

    /// <summary>
    /// 条件修改列
    /// </summary>
    public string ConditionsChangeColumn { set; get; }
    /// <summary>
    /// 表的标识列
    /// </summary>
    /// <param name="name">表名</param>
    /// <param name="explain">表说明</param>
    /// <param name="isKey">表是否有主键</param>
    /// <param name="isIdentity">表是否有标识列</param>
    /// <param name="keyName">表的主键列名</param>
    /// <param name="IdentityName">表的标识列 列名</param>
    /// <param name="conditionsChangeColumn">默认条件修改列</param>
    public TableName(string name, string explain = null, string conditionsChangeColumn = null)
    {
        Name = name;
        Explain = explain;

        ConditionsChangeColumn = conditionsChangeColumn;
    }
}

public class MapName : Attribute
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { set; get; }
    public bool IsKey { set; get; }
    public bool IsIdentity { set; get; }
    public MapName(string name, bool isKey = false, bool isIdentity = false, bool isSQL = true)
    {
        this.Name = name;
        this.IsKey = isKey;
        this.IsIdentity = isIdentity;
        IsSQL = isSQL;
    }

    public bool IsSQL { set; get; }
}

