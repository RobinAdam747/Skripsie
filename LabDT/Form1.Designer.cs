namespace LabDT
{
    partial class LabDT
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lstClients = new ListBox();
            btnStop = new Button();
            btnStart = new Button();
            txtServerAddress = new TextBox();
            txtInfo = new TextBox();
            lblServer = new Label();
            lblClients = new Label();
            btnUnitTest = new Button();
            SuspendLayout();
            // 
            // lstClients
            // 
            lstClients.FormattingEnabled = true;
            lstClients.ItemHeight = 20;
            lstClients.Location = new Point(676, 79);
            lstClients.Name = "lstClients";
            lstClients.Size = new Size(357, 484);
            lstClients.TabIndex = 0;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(530, 608);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(94, 29);
            btnStop.TabIndex = 1;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click_1;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(530, 573);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 2;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click_1;
            // 
            // txtServerAddress
            // 
            txtServerAddress.Location = new Point(123, 79);
            txtServerAddress.Name = "txtServerAddress";
            txtServerAddress.Size = new Size(501, 27);
            txtServerAddress.TabIndex = 3;
            // 
            // txtInfo
            // 
            txtInfo.Location = new Point(123, 123);
            txtInfo.Multiline = true;
            txtInfo.Name = "txtInfo";
            txtInfo.ReadOnly = true;
            txtInfo.ScrollBars = ScrollBars.Vertical;
            txtInfo.Size = new Size(501, 440);
            txtInfo.TabIndex = 4;
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(49, 86);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(53, 20);
            lblServer.TabIndex = 5;
            lblServer.Text = "Server:";
            lblServer.Click += label1_Click;
            // 
            // lblClients
            // 
            lblClients.AutoSize = true;
            lblClients.Location = new Point(676, 56);
            lblClients.Name = "lblClients";
            lblClients.Size = new Size(56, 20);
            lblClients.TabIndex = 6;
            lblClients.Text = "Clients:";
            // 
            // btnUnitTest
            // 
            btnUnitTest.Location = new Point(123, 573);
            btnUnitTest.Name = "btnUnitTest";
            btnUnitTest.Size = new Size(94, 29);
            btnUnitTest.TabIndex = 7;
            btnUnitTest.Text = "Unit Test";
            btnUnitTest.UseVisualStyleBackColor = true;
            btnUnitTest.Click += btnUnitTest_Click;
            // 
            // LabDT
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1060, 642);
            Controls.Add(btnUnitTest);
            Controls.Add(lblClients);
            Controls.Add(lblServer);
            Controls.Add(txtInfo);
            Controls.Add(txtServerAddress);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(lstClients);
            Name = "LabDT";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lab Digital Twin";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lstClients;
        private Button btnStop;
        private Button btnStart;
        private TextBox txtServerAddress;
        private TextBox txtInfo;
        private Label lblServer;
        private Label lblClients;
        private Button btnUnitTest;
    }
}