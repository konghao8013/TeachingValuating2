using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConfigurationTool;
using Microsoft.Web.Administration;
using Microsoft.Win32;

namespace Install
{
    public partial class InitWeb : Form
    {
        public InitWeb()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BtnNew.Click += BtnNew_Click;
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            Process.Start("http://127.0.0.1");

        }

        
      
    }
}
