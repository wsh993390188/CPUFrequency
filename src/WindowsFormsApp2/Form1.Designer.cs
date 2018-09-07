namespace WindowsFormsApp2
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Aga.Controls.Tree.TreeColumn treeColumn1 = new Aga.Controls.Tree.TreeColumn();
            Aga.Controls.Tree.TreeColumn treeColumn2 = new Aga.Controls.Tree.TreeColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.treeViewAdv = new Aga.Controls.Tree.TreeViewAdv();
            this.nodeNameIcon = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.nodeTextBoxName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBoxValue = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // treeViewAdv
            // 
            this.treeViewAdv.BackColor = System.Drawing.SystemColors.Window;
            treeColumn1.Header = "Name";
            treeColumn1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            treeColumn1.Width = 250;
            treeColumn2.Header = "Value";
            treeColumn2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            treeColumn2.Width = 100;
            this.treeViewAdv.Columns.Add(treeColumn1);
            this.treeViewAdv.Columns.Add(treeColumn2);
            this.treeViewAdv.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeViewAdv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewAdv.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewAdv.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewAdv.Location = new System.Drawing.Point(0, 0);
            this.treeViewAdv.Model = null;
            this.treeViewAdv.Name = "treeViewAdv";
            this.treeViewAdv.NodeControls.Add(this.nodeNameIcon);
            this.treeViewAdv.NodeControls.Add(this.nodeTextBoxName);
            this.treeViewAdv.NodeControls.Add(this.nodeTextBoxValue);
            this.treeViewAdv.SelectedNode = null;
            this.treeViewAdv.Size = new System.Drawing.Size(800, 450);
            this.treeViewAdv.TabIndex = 0;
            this.treeViewAdv.Text = "treeViewAdv";
            this.treeViewAdv.UseColumns = true;
            // 
            // nodeNameIcon
            // 
            this.nodeNameIcon.DataPropertyName = "Image";
            // 
            // nodeTextBoxName
            // 
            this.nodeTextBoxName.DataPropertyName = "Text";
            // 
            // nodeTextBoxValue
            // 
            this.nodeTextBoxValue.Column = 1;
            this.nodeTextBoxValue.DataPropertyName = "Data";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.treeViewAdv);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "SV Test";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv treeViewAdv;
        private Aga.Controls.Tree.NodeControls.NodeIcon nodeNameIcon;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxName;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxValue;
        private System.Windows.Forms.Timer timer1;
    }
}

