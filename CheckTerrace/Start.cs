using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Install
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BtnCheck.Click += BtnCheck_Click;

            ListViewArray.GridLines = true; //显示表格线
            ListViewArray.View = View.Details;//显示表格细节
            ListViewArray.LabelEdit = true; //是否可编辑,ListView只可编辑第一列。
            ListViewArray.Scrollable = true;//有滚动条
            ListViewArray.HeaderStyle = ColumnHeaderStyle.Clickable;//对表头进行设置
            ListViewArray.FullRowSelect = true;//是否可以选择行


            ListViewArray.Columns.Add("平台名称", 200);
            ListViewArray.Columns.Add("版本号码", 160);
            ListViewArray.Columns.Add("是否通过", 80);
        }

        void BtnCheck_Click(object sender, EventArgs e)
        {

            ListViewArray.Items.Clear();
            var isFw=CheckFramework();
            var isMSSQL=CheckMSSQL();
            var isIIS=CheckIIS();
            if (isFw && isMSSQL && isIIS)
            {
                var path = System.AppDomain.CurrentDomain.BaseDirectory + "Install.exe";
                if (File.Exists(path))
                {
                    Process.Start(path);
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show("未找到对应的安装文件");
                }
                  
            }
            else
            {
                MessageBox.Show("环境检查不通过，请部署安装环境");
            }
        }

        private bool CheckIIS()
        {

            RegistryKey key = Registry.LocalMachine;
            RegistryKey software = key.OpenSubKey("SOFTWARE\\Microsoft\\InetStp\\");
            string str="";
            if (software != null)
            {
                 str = software.GetValue("SetupString")+"";
            }
            
            var item = new ListViewItem(new[] { "Internet 信息服务(IIS)管理器", str + "", software == null ? "No" : "Yes" });
            ListViewArray.Items.Add(item);
            return software != null;
            //IIS 7.5
        }

        private bool CheckFramework()
        {
            var strs = GetDotNetVersions();
            var frameworkStr = "4.0";
            var isOk = false;
            foreach (var str in strs)
            {

                if (str.IndexOf("4.0", System.StringComparison.Ordinal) == 0)
                {
                    frameworkStr = str;
                    isOk = true;
                    break;

                }


            }
            var item = new ListViewItem(new[] { ".Net Framework", frameworkStr, isOk ? "Yes" : "No" });
            ListViewArray.Items.Add(item);
            return isOk;
        }

        private bool CheckMSSQL()
        {
            RegistryKey key = Registry.LocalMachine;
            RegistryKey software = key.OpenSubKey("SOFTWARE\\Microsoft\\Microsoft SQL Server 2008 Redist");
            var item = new ListViewItem(new[] { "Microsoft SQL Server", "2008", software == null ? "No" : "Yes" });
            ListViewArray.Items.Add(item);
            return software != null;
        }



        public string[] GetDotNetVersions()
        {
            DirectoryInfo[] directories = new DirectoryInfo(Environment.SystemDirectory + @"\..\Microsoft.NET\Framework").GetDirectories("v?.?.*");
            ArrayList list = new ArrayList();
            foreach (DirectoryInfo info2 in directories)
            {
                list.Add(info2.Name.Substring(1));
            }
            return (list.ToArray(typeof(string)) as string[]);
        }
    }
}
