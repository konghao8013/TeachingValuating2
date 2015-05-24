using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Text;
 public static class JsCompres
    {
        /// <summary>
        /// 压缩 JS代码
        /// </summary>
        /// <param name="strjs"></param>
        /// <returns></returns>
        public static string  JSCompres(this string strjs) {
            strjs = Regex.Replace(strjs, "/\\*[\\s\\S]*?\\*/", "");
            var lines = strjs.Split((char)13);
            strjs = RemoveTheline(lines);
            return strjs;
        }
        /// <summary>
        /// 去掉 新行 开始为注释的函数
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        static string RemoveTheline(string[] lines) {
           
            StringBuilder sb = new StringBuilder();
            foreach (var str in lines)
            {
              //  OkayCode(str);
               sb.Append(OkayCode(str)+(char)32);
                
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获得正确的注释下标
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        static int AnnotationIndex(string code,int value) {
            var index = code.IndexOf("//",value+1);
            if (index < 1)
            {
                return index;
            }
            else if (code[index - 1] == '\\')
            {
                return AnnotationIndex(code, index);
            }
            else
            {
                return index;
            }
        }
        /// <summary>
        /// 获取一行中有用的代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        static string OkayCode(string code) {
            var rul = "";
            code = code.Trim();

            var index = AnnotationIndex(code, -1);
            if (index < 0)
            {
                rul = code;
            }
            else if (index == 0)
            {
                rul = "";
            }
            else {
                var subbegin = code.Substring(0, index);
                var subend = code.Substring(index + 2, code.Length - 2 - index);
                var beg=Inquire(subbegin);
                var end=Inquire(subend);
             
                if (beg)
                {
                    rul = subbegin;
                }
                else if ((!beg) && (end))
                {
                    rul = subbegin;
                }
                else
                {
                    rul = code;
                }
            }
            return rul;
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chars"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        static int NotTransferredIndexOf(string value, string chars, int index)
        {
            var ix = value.IndexOf(chars, index);
            if (ix < 1)
            {
                return ix;
            }
            else 
            {
                var temp = value[ix - 1];
                if (temp+"" == "\\")
                {
                    return NotTransferredIndexOf(value, chars, ix+1);
                }
                else {
                    return ix;
                }
                
               
            }
           
        }
        static bool Inquire(string subbegin) {
            var singleIndex = NotTransferredIndexOf(subbegin, "'", 0); //subbegin.IndexOf("'");
            var index = NotTransferredIndexOf(subbegin, "\"", 0);
            if (singleIndex> index||singleIndex<0&&index>0)
            {
                var singleIndexEnd = NotTransferredIndexOf(subbegin, "\"", index + 1);
                if (singleIndexEnd > -1)
                {
                    var tempvalue = subbegin.Substring(index, singleIndexEnd - index);
                    return Inquire(subbegin.Substring(singleIndexEnd + 1, subbegin.Length - singleIndexEnd - 1));
                }
                else
                {
                    return false;
                }
            }
            else if (index == singleIndex)
            {
                return true;
            }
            else
            {
                var singleIndexEnd = NotTransferredIndexOf(subbegin,"'",singleIndex+1); 
                if (singleIndexEnd > -1)
                {
                    var tempvalue = subbegin.Substring(index, singleIndexEnd - index);
               
                    return Inquire(subbegin.Substring(singleIndexEnd + 1, subbegin.Length - singleIndexEnd - 1));
                }
                else
                {
                    return false;
                }
            }
        }
    }
