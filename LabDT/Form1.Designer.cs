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
            SuspendLayout();
            // 
            // lstClients
            // 
            lstClients.FormattingEnabled = true;
            lstClients.ItemHeight = 15;
            lstClients.Location = new Point(592, 59);
            lstClients.Margin = new Padding(3, 2, 3, 2);
            lstClients.Name = "lstClients";
            lstClients.Size = new Size(313, 364);
            lstClients.TabIndex = 0;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(464, 456);
            btnStop.Margin = new Padding(3, 2, 3, 2);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(82, 22);
            btnStop.TabIndex = 1;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click_1;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(464, 430);
            btnStart.Margin = new Padding(3, 2, 3, 2);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(82, 22);
            btnStart.TabIndex = 2;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click_1;
            // 
            // txtServerAddress
            // 
            txtServerAddress.Location = new Point(108, 59);
            txtServerAddress.Margin = new Padding(3, 2, 3, 2);
            txtServerAddress.Name = "txtServerAddress";
            txtServerAddress.Size = new Size(439, 23);
            txtServerAddress.TabIndex = 3;
            // 
            // txtInfo
            // 
            txtInfo.Location = new Point(108, 92);
            txtInfo.Margin = new Padding(3, 2, 3, 2);
            txtInfo.Multiline = true;
            txtInfo.Name = "txtInfo";
            txtInfo.ReadOnly = true;
            txtInfo.ScrollBars = ScrollBars.Vertical;
            txtInfo.Size = new Size(439, 331);
            txtInfo.TabIndex = 4;
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(43, 64);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(42, 15);
            lblServer.TabIndex = 5;
            lblServer.Text = "Server:";
            lblServer.Click += label1_Click;
            // 
            // lblClients
            // 
            lblClients.AutoSize = true;
            lblClients.Location = new Point(592, 42);
            lblClients.Name = "lblClients";
            lblClients.Size = new Size(46, 15);
            lblClients.TabIndex = 6;
            lblClients.Text = "Clients:";
            // 
            // LabDT
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(928, 482);
            Controls.Add(lblClients);
            Controls.Add(lblServer);
            Controls.Add(txtInfo);
            Controls.Add(txtServerAddress);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(lstClients);
            Margin = new Padding(3, 2, 3, 2);
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
    }
}