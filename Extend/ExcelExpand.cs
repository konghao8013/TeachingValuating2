using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using OfficeOpenXml;

namespace ALOS.Expand
{
    public class ExcelExpand
    {
        /// <summary>
        /// 读取Excel为table
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataTable GetExcel(string url, string name)
        {

            var tab = new DataTable();

            FileStream fs = new FileStream(url, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            ExcelPackage excel = new ExcelPackage(fs);

            var sheet = excel.Workbook.Worksheets[name];
            if (sheet == null)
            {
                return null;
            }
            int columnlength = sheet.Dimension.End.Column;

            int rowlength = sheet.Dimension.End.Row;

            for (var i = 0; i < columnlength; i++)
            {

                var type = typeof(string);

                if (rowlength > 1)
                {

                    type = sheet.Cells[2, i + 1].Value.GetType();

                }
                var value = sheet.Cells[1, i + 1].Value;
                if (value == null)
                {
                    columnlength = i;
                    break;
                }
                tab.Columns.Add(value.ToString(), type);



            }

            for (var i = 1; i < rowlength; i++)
            {

                if (sheet.Cells[i + 1, 1].Value == null)
                {

                    break;

                }
                tab.Rows.Add(tab.NewRow());

                for (var j = 0; j < columnlength; j++)
                {

                    tab.Rows[i - 1][j] = sheet.Cells[i + 1, j + 1].Value;

                }

            }

            fs.Close();

            excel.Dispose();

            return tab;

        }
        /// <summary>
        /// DataTable导出为 execel
        /// </summary>
        /// <param name="table"></param>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ExcelUrl(DataTable table, string url, string name)
        {

            string saveUrl = url;



            ExcelPackage excel = new ExcelPackage();

            excel.Workbook.Worksheets.Add(name);

            var sheet = excel.Workbook.Worksheets[name];

            int columnlength = table.Columns.Count;

            int rowlength = table.Rows.Count;

            for (var i = 0; i < columnlength; i++)
            {

                sheet.Cells[1, i + 1].Value = table.Columns[i].ColumnName;



            }

            for (var i = 1; i <= rowlength; i++)
            {

                for (var j = 0; j < columnlength; j++)
                {

                    sheet.Cells[i + 1, j + 1].Value = table.Rows[i - 1][j].ToString();

                }

            }

            FileStream OutputStream = new FileStream(saveUrl, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);

            excel.SaveAs(OutputStream);

            OutputStream.Close();



            excel.Dispose();

            return saveUrl;



        }
    }
}
