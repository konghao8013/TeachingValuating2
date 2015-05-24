namespace Install
{
    partial class Start
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnCheck = new System.Windows.Forms.Button();
            this.ListViewArray = new System.Windows.Forms.ListView();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.BtnCheck, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ListViewArray, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(465, 262);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // BtnCheck
            // 
            this.BtnCheck.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnCheck.Location = new System.Drawing.Point(352, 3);
            this.BtnCheck.Name = "BtnCheck";
            this.BtnCheck.Size = new System.Drawing.Size(110, 24);
            this.BtnCheck.TabIndex = 0;
            this.BtnCheck.Text = "检查安装环境";
            this.BtnCheck.UseVisualStyleBackColor = true;
            // 
            // ListViewArray
            // 
            this.ListViewArray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewArray.Location = new System.Drawing.Point(3, 33);
            this.ListViewArray.Name = "ListViewArray";
            this.ListViewArray.Size = new System.Drawing.Size(459, 196);
            this.ListViewArray.TabIndex = 1;
            this.ListViewArray.UseCompatibleStateImageBehavior = false;
            // 
            // Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 262);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Start";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "部署测评软件";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button BtnCheck;
        private System.Windows.Forms.ListView ListViewArray;
    }
}

