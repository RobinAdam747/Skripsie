namespace TCPServer
{
    partial class Form1
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
            lblServer = new Label();
            lstClientIP = new ListBox();
            btnStart = new Button();
            txtIP = new TextBox();
            txtInfo = new TextBox();
            txtMessage = new TextBox();
            label2 = new Label();
            btnSend = new Button();
            lblClientIP = new Label();
            btnStop = new Button();
            SuspendLayout();
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(25, 45);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(53, 20);
            lblServer.TabIndex = 0;
            lblServer.Text = "Server:";
            // 
            // lstClientIP
            // 
            lstClientIP.FormattingEnabled = true;
            lstClientIP.ItemHeight = 20;
            lstClientIP.Location = new Point(673, 81);
            lstClientIP.Name = "lstClientIP";
            lstClientIP.Size = new Size(382, 484);
            lstClientIP.TabIndex = 1;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(573, 536);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 2;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // txtIP
            // 
            txtIP.Location = new Point(99, 42);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(568, 27);
            txtIP.TabIndex = 3;
            // 
            // txtInfo
            // 
            txtInfo.Location = new Point(99, 80);
            txtInfo.Multiline = true;
            txtInfo.Name = "txtInfo";
            txtInfo.ReadOnly = true;
            txtInfo.ScrollBars = ScrollBars.Both;
            txtInfo.Size = new Size(568, 417);
            txtInfo.TabIndex = 4;
            // 
            // txtMessage
            // 
            txtMessage.Location = new Point(111, 503);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(556, 27);
            txtMessage.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(37, 506);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 5;
            label2.Text = "Message:";
            // 
            // btnSend
            // 
            btnSend.Location = new Point(473, 536);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(94, 29);
            btnSend.TabIndex = 7;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            // 
            // lblClientIP
            // 
            lblClientIP.AutoSize = true;
            lblClientIP.Location = new Point(673, 57);
            lblClientIP.Name = "lblClientIP";
            lblClientIP.Size = new Size(66, 20);
            lblClientIP.TabIndex = 8;
            lblClientIP.Text = "Client IP:";
            // 
            // btnStop
            // 
            btnStop.Location = new Point(573, 571);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(94, 29);
            btnStop.TabIndex = 9;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(1101, 620);
            Controls.Add(btnStop);
            Controls.Add(lblClientIP);
            Controls.Add(btnSend);
            Controls.Add(txtMessage);
            Controls.Add(label2);
            Controls.Add(txtInfo);
            Controls.Add(txtIP);
            Controls.Add(btnStart);
            Controls.Add(lstClientIP);
            Controls.Add(lblServer);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TCP/IP Server";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblServer;
        private Button btnStart;
        private TextBox txtIP;
        private TextBox txtInfo;
        private TextBox txtMessage;
        private Label label2;
        private Button btnSend;
        private Label lblClientIP;
        private ListBox lstClientIP;
        private Button btnStop;
    }
}