using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EFCode
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        private static string Url { set; get; }
        private static string Namespace { set; get; }
        private static string Path { set; get; }
        private static string MapName { set; get; }
        private static string TabName { set; get; }
        /// <summary>
        /// 表示是否更改Model
        /// </summary>
        private static bool IsModel { set; get; }

        /// <summary>
        /// args 启动参数定义
        /// </summary>
        /// <param name="args">参数一输出路径,参数二 数据库链接,参数三命名空间名称,参数四类名顶部标志,参数五属性名称顶部标志,参数六表示Model初始化之后不再更改默认false</param>

        static void Main(string[] args)
        {

            if (args == null || args.Length < 6)
            {
                args = new string[10];
                args[0] = "../../../Model";
                args[1] = "server=aitaoqian.com,1444;user=sa;password=xiaozhang915;Database=Valuating";
                args[2] = "ALOS.Model";
                args[3] = "[TableName(\"{0}\",\"{1}\",\"{2}\")]";
                args[4] = "[MapName(\"{0}\"{1}{2})]";
                args[5] = "true";
            }
            Path = args[0];
            Url = args[1];
            Namespace = args[2];
            TabName = args[3];
            MapName = args[4];
            IsModel = bool.Parse(args[5]);
            _help = new SQLServerHelp(Url);
            CreateModel();
        }

        private static SQLServerHelp _help;
        private static void CreateModel()
        {

            var list = _help.TabList();
            var path = Path;
            foreach (var tab in list)
            {
                var ts = GetClassString(tab);
                Writer(path + "\\" + tab.Name.GetTabValue() + ".cs", ts);
            }
        }

        public static void Writer(string path, string value)
        {
            if (IsModel && File.Exists(path))
            {
                return;
            }
            if (File.Exists(path))
            {
                var sr = new StreamReader(path, Encoding.UTF8);
                var strs = sr.ReadToEnd();
                sr.Close();
                if (strs == value)
                {
                    return;
                }

            }
            var sw = new StreamWriter(path, false, Encoding.UTF8);
            sw.Write(value);
            sw.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static string GetClassString(Table table)
        {
            var sb = new StringBuilder();
            var list = _help.FieldList(table.Name);
            var key = _help.KeyName(table.Name);
            sb.AppendLine("using System;");
            sb.AppendLine("namespace " + Namespace);
            sb.AppendLine("{");
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine("\t/// " + table.Content);
            sb.AppendLine("\t/// </summary>");
            sb.AppendLine("\t" + String.Format(TabName, table.Name, table.Content, key));
            sb.AppendLine("\tpublic  class " + table.Name.GetTabValue());
            sb.AppendLine("\t{");

            var identitys = _help.IdentityList(table.Name);
            foreach (var field in list)
            {
                bool iskey = false;
                bool isIdentity = false;
                if (key != null && key == field.Name)
                {
                    iskey = true;

                }
                if (identitys.Contains(field.Name))
                {
                    isIdentity = true;

                }
                sb.AppendLine(Field(field, iskey, isIdentity));
            }
            sb.AppendLine("\t\tpublic string Model_Type {get { return this.GetType().FullName; } }");
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string Field(Field fid, bool isKey, bool isIdentity)
        {
            var sb = new StringBuilder();
            //isKey:{1},isIdentity:{2}
            sb.AppendLine("\t\t/// <summary>");
            sb.AppendLine("\t\t/// " + fid.Explin);
            sb.AppendLine("\t\t/// </summary>");
            sb.AppendLine("\t\t" + string.Format(MapName, fid.Name, (isKey ? ",isKey:true" : ""), (isIdentity ? ",isIdentity:true" : "")));
            sb.AppendLine("\t\tpublic " + _help.Dic[fid.TypeName] + " " + fid.Name);
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tget;");
            sb.AppendLine("\t\t\tset;");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }

    }


}
