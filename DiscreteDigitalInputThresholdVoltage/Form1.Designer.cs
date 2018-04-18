namespace DiscreteDigitalInputThresholdVoltage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BusOffOn = new System.Windows.Forms.Button();
            this.comboBoxChannel = new System.Windows.Forms.ComboBox();
            this.labelBusLoad = new System.Windows.Forms.Label();
            this.comboBoxBaudrate = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxAccess = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxSession = new System.Windows.Forms.ComboBox();
            this.buttonAccess = new System.Windows.Forms.Button();
            this.buttonSession = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.coverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonCMD = new System.Windows.Forms.Button();
            this.richTextBoxDisplay = new System.Windows.Forms.RichTextBox();
            this.buttonDebug = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBoxIO = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelChanged = new System.Windows.Forms.Label();
            this.checkBoxChanged = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BusOffOn);
            this.groupBox1.Controls.Add(this.comboBoxChannel);
            this.groupBox1.Controls.Add(this.labelBusLoad);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 77);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CAN Seeting";
            // 
            // BusOffOn
            // 
            this.BusOffOn.Location = new System.Drawing.Point(222, 45);
            this.BusOffOn.Name = "BusOffOn";
            this.BusOffOn.Size = new System.Drawing.Size(75, 23);
            this.BusOffOn.TabIndex = 0;
            this.BusOffOn.Text = "Bus On";
            this.BusOffOn.UseVisualStyleBackColor = true;
            this.BusOffOn.Click += new System.EventHandler(this.BusOffOn_Click);
            // 
            // comboBoxChannel
            // 
            this.comboBoxChannel.FormattingEnabled = true;
            this.comboBoxChannel.Location = new System.Drawing.Point(10, 20);
            this.comboBoxChannel.Name = "comboBoxChannel";
            this.comboBoxChannel.Size = new System.Drawing.Size(287, 20);
            this.comboBoxChannel.TabIndex = 2;
            // 
            // labelBusLoad
            // 
            this.labelBusLoad.Location = new System.Drawing.Point(107, 47);
            this.labelBusLoad.Name = "labelBusLoad";
            this.labelBusLoad.Size = new System.Drawing.Size(111, 19);
            this.labelBusLoad.TabIndex = 5;
            this.labelBusLoad.Text = "Bus Load:";
            this.labelBusLoad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxBaudrate
            // 
            this.comboBoxBaudrate.FormattingEnabled = true;
            this.comboBoxBaudrate.Items.AddRange(new object[] {
            "50",
            "100",
            "125",
            "250",
            "500"});
            this.comboBoxBaudrate.Location = new System.Drawing.Point(22, 58);
            this.comboBoxBaudrate.Name = "comboBoxBaudrate";
            this.comboBoxBaudrate.Size = new System.Drawing.Size(80, 20);
            this.comboBoxBaudrate.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxAccess);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboBoxSession);
            this.groupBox2.Controls.Add(this.buttonAccess);
            this.groupBox2.Controls.Add(this.buttonSession);
            this.groupBox2.Location = new System.Drawing.Point(12, 95);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(184, 79);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Session";
            // 
            // comboBoxAccess
            // 
            this.comboBoxAccess.FormattingEnabled = true;
            this.comboBoxAccess.Items.AddRange(new object[] {
            "01",
            "03",
            "05",
            "07",
            "09"});
            this.comboBoxAccess.Location = new System.Drawing.Point(41, 47);
            this.comboBoxAccess.Name = "comboBoxAccess";
            this.comboBoxAccess.Size = new System.Drawing.Size(35, 20);
            this.comboBoxAccess.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(7, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 14);
            this.label3.TabIndex = 17;
            this.label3.Text = "$27";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(7, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 14);
            this.label2.TabIndex = 16;
            this.label2.Text = "$10";
            // 
            // comboBoxSession
            // 
            this.comboBoxSession.FormattingEnabled = true;
            this.comboBoxSession.Items.AddRange(new object[] {
            "01",
            "02",
            "03"});
            this.comboBoxSession.Location = new System.Drawing.Point(41, 20);
            this.comboBoxSession.Name = "comboBoxSession";
            this.comboBoxSession.Size = new System.Drawing.Size(35, 20);
            this.comboBoxSession.TabIndex = 14;
            // 
            // buttonAccess
            // 
            this.buttonAccess.Location = new System.Drawing.Point(100, 47);
            this.buttonAccess.Name = "buttonAccess";
            this.buttonAccess.Size = new System.Drawing.Size(75, 23);
            this.buttonAccess.TabIndex = 13;
            this.buttonAccess.Text = "Access";
            this.buttonAccess.UseVisualStyleBackColor = true;
            this.buttonAccess.Click += new System.EventHandler(this.buttonAccess_Click);
            // 
            // buttonSession
            // 
            this.buttonSession.Location = new System.Drawing.Point(100, 18);
            this.buttonSession.Name = "buttonSession";
            this.buttonSession.Size = new System.Drawing.Size(75, 23);
            this.buttonSession.TabIndex = 7;
            this.buttonSession.Text = "Session";
            this.buttonSession.UseVisualStyleBackColor = true;
            this.buttonSession.Click += new System.EventHandler(this.buttonSessoin_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.richTextBox1);
            this.groupBox5.Location = new System.Drawing.Point(12, 232);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(306, 221);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Message";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.richTextBox1.Location = new System.Drawing.Point(10, 20);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(287, 195);
            this.richTextBox1.TabIndex = 16;
            this.richTextBox1.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.coverToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // coverToolStripMenuItem
            // 
            this.coverToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coverToolStripMenuItem.Name = "coverToolStripMenuItem";
            this.coverToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.coverToolStripMenuItem.Text = "Copy";
            this.coverToolStripMenuItem.Click += new System.EventHandler(this.coverToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // buttonCMD
            // 
            this.buttonCMD.Location = new System.Drawing.Point(202, 141);
            this.buttonCMD.Name = "buttonCMD";
            this.buttonCMD.Size = new System.Drawing.Size(107, 24);
            this.buttonCMD.TabIndex = 23;
            this.buttonCMD.Text = "Monitor";
            this.buttonCMD.UseVisualStyleBackColor = true;
            this.buttonCMD.Click += new System.EventHandler(this.buttonCMD_Click);
            // 
            // richTextBoxDisplay
            // 
            this.richTextBoxDisplay.ContextMenuStrip = this.contextMenuStrip1;
            this.richTextBoxDisplay.Location = new System.Drawing.Point(11, 19);
            this.richTextBoxDisplay.Name = "richTextBoxDisplay";
            this.richTextBoxDisplay.Size = new System.Drawing.Size(327, 416);
            this.richTextBoxDisplay.TabIndex = 22;
            this.richTextBoxDisplay.Text = "";
            // 
            // buttonDebug
            // 
            this.buttonDebug.Location = new System.Drawing.Point(202, 113);
            this.buttonDebug.Name = "buttonDebug";
            this.buttonDebug.Size = new System.Drawing.Size(107, 23);
            this.buttonDebug.TabIndex = 24;
            this.buttonDebug.Text = "Debug";
            this.buttonDebug.UseVisualStyleBackColor = true;
            this.buttonDebug.Click += new System.EventHandler(this.buttonDebug_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.richTextBoxDisplay);
            this.groupBox3.Location = new System.Drawing.Point(322, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(347, 441);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Display";
            // 
            // comboBoxIO
            // 
            this.comboBoxIO.FormattingEnabled = true;
            this.comboBoxIO.Items.AddRange(new object[] {
            "KEY",
            "ACC",
            "IGN",
            "CRANK",
            "刹车踏板",
            "后雨刮停靠",
            "RESERVED_1_IN",
            "RESERVED_2_IN",
            "RESERVED_3_IN",
            "倒车灯",
            "离合踏板",
            "危险灯报警",
            "P档",
            "空档",
            "后除霜",
            "后备箱",
            "左转向诊断",
            "右转向诊断",
            "后视镜加热",
            "后视镜折叠",
            "前雨刮1",
            "前雨刮2",
            "前雨刮停靠",
            "TRUNKAJA",
            "HORN",
            "冬季模式",
            "后窗锁止",
            "司机侧门锁反馈信号",
            "乘客侧门锁反馈",
            "左后侧门锁反馈",
            "右后侧门锁反馈",
            "FLAJA",
            "FRAJA",
            "RLAJA",
            "RRAJA",
            "RESERVED_4_IN",
            "RESERVED_5_IN"});
            this.comboBoxIO.Location = new System.Drawing.Point(88, 180);
            this.comboBoxIO.Name = "comboBoxIO";
            this.comboBoxIO.Size = new System.Drawing.Size(174, 20);
            this.comboBoxIO.TabIndex = 6;
            this.comboBoxIO.SelectedIndexChanged += new System.EventHandler(this.comboBoxIO_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(19, 182);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 19;
            this.label1.Text = "Input IO";
            // 
            // labelChanged
            // 
            this.labelChanged.AutoSize = true;
            this.labelChanged.Location = new System.Drawing.Point(110, 211);
            this.labelChanged.Name = "labelChanged";
            this.labelChanged.Size = new System.Drawing.Size(131, 12);
            this.labelChanged.TabIndex = 25;
            this.labelChanged.Text = "Num of State Changes:";
            // 
            // checkBoxChanged
            // 
            this.checkBoxChanged.AutoSize = true;
            this.checkBoxChanged.Location = new System.Drawing.Point(24, 210);
            this.checkBoxChanged.Name = "checkBoxChanged";
            this.checkBoxChanged.Size = new System.Drawing.Size(60, 16);
            this.checkBoxChanged.TabIndex = 26;
            this.checkBoxChanged.Text = "Enable";
            this.checkBoxChanged.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 461);
            this.Controls.Add(this.checkBoxChanged);
            this.Controls.Add(this.labelChanged);
            this.Controls.Add(this.buttonDebug);
            this.Controls.Add(this.buttonCMD);
            this.Controls.Add(this.comboBoxIO);
            this.Controls.Add(this.comboBoxBaudrate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "E10";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BusOffOn;
        private System.Windows.Forms.ComboBox comboBoxChannel;
        private System.Windows.Forms.ComboBox comboBoxBaudrate;
        private System.Windows.Forms.Label labelBusLoad;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxAccess;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxSession;
        private System.Windows.Forms.Button buttonAccess;
        private System.Windows.Forms.Button buttonSession;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button buttonCMD;
        private System.Windows.Forms.RichTextBox richTextBoxDisplay;
        private System.Windows.Forms.Button buttonDebug;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBoxIO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem coverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Label labelChanged;
        private System.Windows.Forms.CheckBox checkBoxChanged;
    }
}

