using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using ICSharpCode.SharpZipLib.Zip;


public static class StringExpand
{
    static JavaScriptSerializer _json = new JavaScriptSerializer{MaxJsonLength = int.MaxValue};
    public static T Deserialize<T>(this string value)
    {
       
        return _json.Deserialize<T>(value);
    }
    public static string Serialize(this Object o)
    {
        
        return _json.Serialize(o);
    }
    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="value"></param>
    public static void JsonDeserialize(this string value)
    {

    }
    /// <summary>
    /// 序列化对象
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string SeralizeJson(this Object o)
    {
        if (o.GetType().IsPrimitive || o is string)
        {
            return o.ToString();
        }
        var sb = new StringBuilder();

        SeralizeJsonHelp(o, sb);
        return "";
    }
    /// <summary>
    /// 文件压缩
    /// </summary>
    /// <param name="zipFilePath"></param>
    /// <param name="filePaths"></param>
    public static void CreateZip(this String zipFilePath, params String[] filePaths)
    {
        var zips = new ZipOutputStream(File.Create(zipFilePath));
        zips.SetLevel(9);
        //zips.Password = "123456";
        var buffer = new byte[4096];
        foreach (var file in filePaths)
        {
            if (!File.Exists(file))
            {
                continue;
            }
            var entry = new ZipEntry(Path.GetFileName(file));
            entry.DateTime = DateTime.Now;
            zips.PutNextEntry(entry);
            var fs = File.OpenRead(file);
            int sourceBytes;
            while ((sourceBytes = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                zips.Write(buffer, 0, sourceBytes);
            }
            fs.Close();
            fs.Dispose();

        }
        zips.Finish();
        zips.Close();

    }
    private static void SeralizeJsonHelp(object o, StringBuilder sb)
    {

        var properties = o.GetType().GetProperties();
        sb.AppendLine("{");
        foreach (var propertie in properties)
        {
            //if (IsBasicType(propertie))
            //{
            sb.AppendLine(propertie.Name + ":" + propertie.GetValue(o,null) + ",");
            //}
            //else
            //{
            //    SeralizeJsonHelp(propertie.GetValue(o, null), sb);
            //}

        }
        sb.AppendLine("}");


    }
    /// <summary>
    /// 判断属性是否为基本类型
    /// </summary>
    /// <param name="info"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    static bool IsBasicType(PropertyInfo info)
    {
        bool ischeck = false;
      
        switch (info.PropertyType.FullName)
        {
            case "System.String":
                ischeck = true;
                break;
            case "System.Int32":
                ischeck = true;
                break;
            case "System.Byte":
                ischeck = true;
                break;
            case "System.SByte":
                ischeck = true;
                break;
            case "System.Char":
                ischeck = true;
                break;
            case "System.Decimal":
                ischeck = true;
                break;
            case "System.Double":
                ischeck = true;
                break;
            case "System.Single":
                ischeck = true;
                break;
            case "System.UInt32":
                ischeck = true;
                break;
            case "System.Guid":
                ischeck = true;
                break;
            case "System.DateTime":
                ischeck = true;
                break;
            default:
                ischeck = false;
                break;
        }
        return ischeck;
    }


    /// <summary>
    /// 将String调整为正确的类型
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Object ConvertCorrect(this string value, string fullName)
    {
        object result = value;

        switch (fullName)
        {
            case "System.String":
                result = value;
                break;
            case "System.Int32":
                if (value.Length == 0)
                {
                    result = 0;
                    break;

                }
                result = int.Parse(value);
                break;
            case "System.Int64":
                result = Int64.Parse(value);
                break;
            case "System.Byte":
                result = Byte.Parse(value);
                break;
            case "System.SByte":
                result = SByte.Parse(value);
                break;
            case "System.Char":
                result = Char.Parse(value);
                break;
            case "System.Decimal":
                result = Decimal.Parse(value);
                break;
            case "System.Double":
                result = Double.Parse(value);
                break;
            case "System.Single":
                result = Double.Parse(value);
                break;
            case "System.UInt32":
                result = UInt32.Parse(value);
                break;
            case "System.Guid":
                result = Guid.Parse(value);
                break;
            case "System.DateTime":
                result = DateTime.Parse(value);
                break;
            case "System.Boolean":

                result =value=="True" || value != "False" && value.ToInt32() == 1;
                break;
                
            default:
                result = value;
                break;
        }
        return result;
    }
    /// <summary>
    /// 判断字符串长度是否为0
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsLength(this string value)
    {
        return value.Length > 0;
    }

    /// <summary>
    /// SHA512加密
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string SHA512_Encrypt(this string plainText)
    {
        if (plainText == null) {
            plainText = "";
        }
        SHA512 sha512 = new SHA512Managed();
        byte[] tmpByte = Encoding.UTF8.GetBytes(plainText);
        tmpByte = sha512.ComputeHash(tmpByte);
        sha512.Clear();
        string rv = BitConverter.ToString(tmpByte).Replace("-", "");
        return rv;

    }
    /// <summary>
    /// 判断对象是否为空 如果对象为空返回str字符 如果不为空返回""空白字符
    /// </summary>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string IsNull(this object value, string str)
    {
        return value == null ? str : "";
    }

    public static int Divide(this int value, int value2)
    {

        return (int)Math.Ceiling(value / (double)value2);
    }
    /// <summary>
    /// 每个字符传入值
    /// </summary>
    /// <param name="value"></param>
    /// <param name="chare"></param>
    /// <returns></returns>
    public static string InsertChar(this string value, string chare)
    {
        var cs = value.ToArray();
        var str = "";
        for (int i = 0; i < cs.Length; i++)
        {
            str += cs[i] + chare;
        }
        return str;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="span"></param>
    /// <param name="ager">HH:mm:ss:xxxx</param>
    /// <returns></returns>
    public static string ToStringR(this TimeSpan span)
    {

        var hours = span.Hours;
        var minutes = span.Minutes;
        minutes += (hours * 60);
        return minutes + ":" + span.Seconds;
    }
    public static string IsThe(this Int32 value)
    {
        if (value == 0)
        {
            return "零";
        }
        if (value > 99999)
        {
            return "未知";
        }
        string reValue = "";

        var strs = new[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        var g = value % 10;
        reValue = strs[g];
        reValue = (value / 10 > 0 ? strs[value % 100 / 10] + (value % 100 / 10 > 0 ? "十" : "") : "") + reValue;
        reValue = (value / 100 > 0 ? strs[value % 1000 / 100] + (value % 1000 / 100 > 0 ? "百" : "") : "") + reValue;
        reValue = (value / 1000 > 0 ? strs[value % 10000 / 1000] + (value % 10000 / 1000 > 0 ? "千" : "") : "") + reValue;
        reValue = (value / 10000 > 0 ? strs[value % 100000 / 10000] + (value % 100000 / 10000 > 0 ? "万" : "") : "") + reValue;

        reValue = Regex.Replace(reValue, "零{2}", "零");
        reValue = reValue[reValue.Length - 1] == '零' ? reValue.Remove(reValue.Length - 1, 1) : reValue;
        return reValue;

    }

    /// <summary>
    ///  判断是否为INT类型 是 返回True 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsInt(this string value)
    {

        var reg = "^[0-9]{1,11}$";

        return value != null && Regex.IsMatch(value, reg);

    }
    /// <summary>
    /// 判断时间格式
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsDate24(this string value)
    {
        string reg = "^(([1-2][0-3]|[0-1][0-9]|[0-9]):[0-5][0-9])$";
        return Regex.IsMatch(value, reg);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsDate(this string value)
    {
        string reg = @"^(\d{4}-)*([01]*\d-[0-3]*\d\/)*[0-2]*\d:[0-5]*\d$";
        return Regex.IsMatch(value, reg);
    }

    /// <summary>
    /// 判断STR是否为IP值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsIp(this string value)
    {

        const string ip = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])(:[0-9]{1,11}|0{0})$";

        return Regex.IsMatch(value, ip);
    }


    public static bool IsUrl(this string value)
    {
        value = value ?? "";
        if (value.IndexOf("http://localhost", System.StringComparison.Ordinal) > -1)
        {
            return true;
        }
        //http://localhost
        string url = @"^(https?|ftp|file)://[-a-zA-Z0-9+&@#/%?=~_|!:,.;]*[-a-zA-Z0-9+&@#/%=~_|]";

        return Regex.IsMatch(value, url);
    }

    /// <summary>
    /// 判断Value是否为数字 不为数字调用方法
    /// </summary>
    /// <param name="value"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Int32 ToInt32(this string value, Action action)
    {

        if (!value.IsInt())
        {

            action();
            return 0;
        }

        return Int32.Parse(value);
    }
    public static string RemoveHtml(this string value)
    {
        string regexstr = @"<[^>]*>";    //去除所有的标签

        //@"<script[^>]*?>.*?</script>" //去除所有脚本，中间部分也删除

        // string regexstr = @"<img[^>]*>";   //去除图片的正则

        // string regexstr = @"<(?!br).*?>";   //去除所有标签，只剩br

        // string regexstr = @"<table[^>]*?>.*?</table>";   //去除table里面的所有内容

        //string regexstr = @"<(?!img|br|p|/p).*?>";   //去除所有标签，只剩img,br,p
        value = Regex.Replace(value, regexstr, string.Empty, RegexOptions.IgnoreCase);
        return value;


    }

    public static Int32 ToInt32(this string value)
    {
        if (!value.IsInt())
        {
            return 0;
        }
        return Int32.Parse(value);
    }
    /// <summary>
    /// 判断String是否为空 为空返回True
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNull(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// 判断String是否为空 为空返回False
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool NotIsNull(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }
    /// <summary>
    /// 验证字符复杂度 0 长度小于6位 1长度大于6位 2长度大于六位并且有两种字符组成 3长度大于六位并且由三种字符组成 4长度大于六由四种字符组成 5长度大于留位并且有四种以上字符组成
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int CheckStringComplexity(this string value)
    {
        int complexity = 0;
        if (value.Length < 6)
        {

            complexity = 0;
        }
        else
        {
            complexity = IndexComplexity(value);
        }
        return complexity;
    }
    /// <summary>
    /// 查询字符串中有几种字符
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static int IndexComplexity(string value)
    {
        var complexity = 0;
        var number = new Regex("[0-9]");
        var a = new Regex("[a-z]");
        var A = new Regex("[A-Z]");
        var t = new Regex("[~!@#$%^&*()_+{}|:\"?></\\.,\\\\]");
        var n = new Regex("[0-9a-zA-Z~!@#$%^&*()_+{}|:\"?></\\.,\\\\]");
        if (number.IsMatch(value))
        {
            complexity += 1;
        }

        if (a.IsMatch(value))
        {
            complexity += 1;
        }

        if (A.IsMatch(value))
        {
            complexity += 1;
        }

        if (t.IsMatch(value))
        {
            complexity += 1;
        }
        if (!n.IsMatch(value))
        {
            complexity += 1;
        }
        return complexity;
    }

}
