using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using ConfigurationTool;
using Microsoft.Win32;

namespace Install
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
            this.Load += Start_Load;

        }

        private String Path = "";
        void Start_Load(object sender, EventArgs e)
        {
            String[] CmdArgs = System.Environment.GetCommandLineArgs();
            if (CmdArgs.Length > 1)
            {
                //参数0是它本身的路径
                String arg0 = CmdArgs[0];
                Path = CmdArgs[1];
                Path = Path.Replace("\\\\", "\\");

            }
            else
            {
                Path = System.AppDomain.CurrentDomain.BaseDirectory;
            }

        }

        private string sql = "Data Source=.;Initial Catalog={0};Integrated Security=True;Connect Timeout=500";
        private string databasePath = "";
        private string databaseLogPath = "";
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BtnNext.Click += BtnNext_Click;
            txtDBUrl.Click += txtDBUrl_Click;

        }
        void txtDBUrl_Click(object sender, EventArgs e)
        {
            if (txtDatabase.Text.Trim().Length == 0)
            {
                MessageBox.Show("请写上正确的数据库名称如AT3");
                return;
            }
            txtDBUrl.Text = SelectFile();
        }

        string SelectFile()
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.SelectedPath;

            }
            else
            {
                return "";
            }
        }
        private bool CheckIIS()
        {

            RegistryKey key = Registry.LocalMachine;
            RegistryKey software = key.OpenSubKey("SOFTWARE\\Microsoft\\InetStp\\");
            string str = "";
            if (software != null)
            {
                str = software.GetValue("SetupString") + "";
                MessageBox.Show(str);
            }
            return software != null;
            //IIS 7.5
        }

        public void DosString(string dos)
        {
            string strOutput = "";
            Process p = null;
            p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            strOutput = dos;
            p.StandardInput.WriteLine(strOutput);
            p.StandardInput.WriteLine("exit");
            while (p.StandardOutput.EndOfStream)
            {
                strOutput = p.StandardOutput.ReadLine();
                Console.WriteLine(strOutput);
            }
            p.WaitForExit();
            p.Close();
        }

        void BtnNext_Click(object sender, EventArgs e)
        {
            var conn = CreateConn();

            //Process.Start(Path + "iis7x_setup.bat").WaitForExit();
         
            
            prog.Value = 20;

            //string str = "start /w pkgmgr /iu:IIS-WebServerRole;IIS-WebServer;IIS-CommonHttpFeatures;IIS-StaticContent;IIS-DefaultDocument;IIS-DirectoryBrowsing;IIS-HttpErrors;IIS-HttpRedirect;IIS-ApplicationDevelopment;IIS-ASPNET;IIS-NetFxExtensibility;IIS-ASP;IIS-ISAPIExtensions;IIS-ISAPIFilter;IIS-ServerSideIncludes;IIS-HealthAndDiagnostics;IIS-HttpLogging;IIS-LoggingLibraries;IIS-RequestMonitor;IIS-HttpTracing;IIS-CustomLogging;IIS-ODBCLogging;IIS-Security;IIS-BasicAuthentication;IIS-WindowsAuthentication;IIS-DigestAuthentication;IIS-ClientCertificateMappingAuthentication;IIS-IISCertificateMappingAuthentication;IIS-URLAuthorization;IIS-RequestFiltering;IIS-IPSecurity;IIS-Performance;IIS-WebServerManagementTools;IIS-ManagementConsole;IIS-ManagementScriptingTools;IIS-ManagementService;IIS-IIS6ManagementCompatibility;IIS-Metabase;IIS-WMICompatibility;IIS-LegacyScripts;IIS-LegacySnapIn;WAS-WindowsActivationService;WAS-ProcessModel;WAS-NetFxEnvironment;WAS-ConfigurationAPI";
            string str = @"start C:\Windows\System32\PkgMgr.exe /iu:IIS-WebServerRole;IIS-WebServer;IIS-CommonHttpFeatures;IIS-StaticContent;IIS-DefaultDocument;IIS-DirectoryBrowsing;IIS-HttpErrors;IIS-HttpRedirect;IIS-ApplicationDevelopment;IIS-ASPNET;IIS-NetFxExtensibility;IIS-ASP;IIS-ISAPIExtensions;IIS-ISAPIFilter;IIS-ServerSideIncludes;IIS-HealthAndDiagnostics;IIS-HttpLogging;IIS-LoggingLibraries;IIS-RequestMonitor;IIS-HttpTracing;IIS-CustomLogging;IIS-ODBCLogging;IIS-Security;IIS-BasicAuthentication;IIS-WindowsAuthentication;IIS-DigestAuthentication;IIS-ClientCertificateMappingAuthentication;IIS-IISCertificateMappingAuthentication;IIS-URLAuthorization;IIS-RequestFiltering;IIS-IPSecurity;IIS-Performance;IIS-WebServerManagementTools;IIS-ManagementConsole;IIS-ManagementScriptingTools;IIS-ManagementService;IIS-IIS6ManagementCompatibility;IIS-Metabase;IIS-WMICompatibility;IIS-LegacyScripts;IIS-LegacySnapIn;WAS-WindowsActivationService;WAS-ProcessModel;WAS-NetFxEnvironment;WAS-ConfigurationAPI";
           // DosString(str);
         
            try
            {
                ServiceController iis = new ServiceController("iisadmin");
                iis.Start();
            }
            catch (Exception ee)
            {
                var path = @"C:\Windows\System32\PkgMgr.exe";
                Process.Start(path,
                    "/iu:IIS-WebServerRole;IIS-WebServer;IIS-CommonHttpFeatures;IIS-StaticContent;IIS-DefaultDocument;IIS-DirectoryBrowsing;IIS-HttpErrors;IIS-HttpRedirect;IIS-ApplicationDevelopment;IIS-ASPNET;IIS-NetFxExtensibility;IIS-ASP;IIS-ISAPIExtensions;IIS-ISAPIFilter;IIS-ServerSideIncludes;IIS-HealthAndDiagnostics;IIS-HttpLogging;IIS-LoggingLibraries;IIS-RequestMonitor;IIS-HttpTracing;IIS-CustomLogging;IIS-ODBCLogging;IIS-Security;IIS-BasicAuthentication;IIS-WindowsAuthentication;IIS-DigestAuthentication;IIS-ClientCertificateMappingAuthentication;IIS-IISCertificateMappingAuthentication;IIS-URLAuthorization;IIS-RequestFiltering;IIS-IPSecurity;IIS-Performance;IIS-WebServerManagementTools;IIS-ManagementConsole;IIS-ManagementScriptingTools;IIS-ManagementService;IIS-IIS6ManagementCompatibility;IIS-Metabase;IIS-WMICompatibility;IIS-LegacyScripts;IIS-LegacySnapIn;WAS-WindowsActivationService;WAS-ProcessModel;WAS-NetFxEnvironment;WAS-ConfigurationAPI");
            }
            try
            {
                conn.Open();

            }
            catch (Exception erro)
            {

                MessageBox.Show("数据库链接错误" + erro.Message);
            }
            finally
            {
                conn.Close();
            }
            if (!CheckDatabase())
            {
                MessageBox.Show("数据库" + txtDatabase.Text + "已存在请重新设置数据库名称");
                return;
            }

            if (txtDBUrl.Text.Length == 0)
            {
                MessageBox.Show("请选择数据库文件存放位置");
                return;
            }

            databasePath = txtDBUrl.Text + "\\" + txtDatabase.Text + ".mdf";
            databaseLogPath = txtDBUrl.Text + "\\" + txtDatabase.Text + "_log.ldf";
            prog.Value = 40;
            BtnNext.Visible = false;
            txtDBUrl.Visible = false;
            InitDatabase();

            ReSetLabel();




        }

        private SqlConnection CreateConn()
        {
            var conn =
                new SqlConnection(string.Format(sql, "master"));
            return conn;
        }

        private void ReSetLabel()
        {
            var labDb = new Label();
            labDb.Text = txtDBUrl.Text;
            labDb.Dock = DockStyle.Fill;
            labDb.TextAlign = ContentAlignment.MiddleLeft;
            tableLayoutPanel1.Controls.Add(labDb, 2, 5);

            var labDBlOG = new Label();

            labDBlOG.Dock = DockStyle.Fill;
            labDBlOG.TextAlign = ContentAlignment.MiddleLeft;
            tableLayoutPanel1.Controls.Add(labDBlOG, 2, 6);
        }

        private void InitDatabase()
        {

            Tool.SQLNewDatabaseName = txtDatabase.Text;
            Tool.InitSQLHelp("master");
            CreateDataBase();
        }

        private void CreateDataBase()
        {

            string createDabaseSql = string.Format(@"create database {0} 
                  on  primary  -- 默认就属于primary文件组,可省略
                  (
                     name='{0}',  -- 主数据文件的逻辑名称
                      filename='{1}', -- 主数据文件的物理名称
                     size=5mb, --主数据文件的初始大小
                    maxsize=100mb, -- 主数据文件增长的最大值
                     filegrowth=15%--主数据文件的增长率
                )
                 log on
                 (
                     name='{0}_log',
                     filename='{2}',
                    size=2mb,
                     filegrowth=1mb
                 )", txtDatabase.Text, databasePath, databaseLogPath);
            var conn = CreateConn();
            var cmd = new SqlCommand(createDabaseSql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            prog.Value = 60;
            conn.Close();
            ReSetDataBase();
        }

        public bool CheckDatabase()
        {
            string sql = string.Format("select count(1) from master..sysdatabases where name='{0}'", txtDatabase.Text);
            var conn = CreateConn();
            var cmd = new SqlCommand(sql, conn);
            conn.Open();
            var count = (int)cmd.ExecuteScalar();
            conn.Close();
            return count == 0;
        }



        public void ReSetDataBase()
        {
            var conn = CreateConn();
            conn.Open();

            //KILL DataBase Process
            SqlCommand cmd = new SqlCommand(string.Format("SELECT spid FROM sysprocesses ,sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='{0}'", txtDatabase.Text), conn);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            ArrayList list = new ArrayList();
            while (dr.Read())
            {
                list.Add(dr.GetInt16(0));
            }
            dr.Close();
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    cmd = new SqlCommand(string.Format("KILL {0}", list[i]), conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {

                }

            }

            SqlCommand cmdRT = new SqlCommand();
            cmdRT.CommandType = CommandType.Text;
            cmdRT.Connection = conn;

            var path = Path + "database\\Valuating.bak";

            string sql = string.Format(@"USE master
restore database {0} 
from disk='{1}' with replace,
move 'Valuating' to '{2}',
move 'Valuating_log' to '{3}'", txtDatabase.Text, path, databasePath, databaseLogPath);
            cmdRT.CommandText = sql;

            cmdRT.ExecuteNonQuery();


            prog.Value = 80;
            conn.Close();
            InitWeb();
            Tool.SQLNewDatabaseName = txtDatabase.Text;



        }

        public void InitWeb()
        {

            var path = Path + "Valuating";

            var help = new IISHelp();

            help.CreateWEB("WEB" + Tool.SQLNewDatabaseName, 80, path);
            prog.Value = 90;
            InitASPNET();
            //C:\Windows\Microsoft.NET\Framework64\v4.0.30319\aspnet_regiis.exe -i
            SetServer();
        }

        private void InitASPNET()
        {
            var aspnet_regiisPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\aspnet_regiis.exe";
            if (!System.IO.File.Exists(aspnet_regiisPath))
            {
                aspnet_regiisPath = aspnet_regiisPath.Replace("C:", "D:");
                if (!System.IO.File.Exists(aspnet_regiisPath))
                {
                    aspnet_regiisPath = aspnet_regiisPath.Replace("D:", "E:");
                    if (!System.IO.File.Exists(aspnet_regiisPath))
                    {
                        MessageBox.Show("找不到Aspnet_regiis.exe的文件路径!");
                        return;
                    }
                }
            }
            Process.Start(aspnet_regiisPath, "-i").WaitForExit();
        }

        public void SetServer()
        {
            var configHelp = new ConfigHelp(Path + "Valuating/Web.config");
            configHelp.SetConnectionStrings("ValuationDB", string.Format(sql, Tool.SQLNewDatabaseName));
            prog.Value = 100;
            Process.Start("http://127.0.0.1");
            MessageBox.Show("程序部署完成，访问http://127.0.0.1");
            Application.Exit();
        }

    }

}
