namespace InfernoEmu
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.maintainanceCheckBox = new System.Windows.Forms.CheckBox();
            this.headLabel = new System.Windows.Forms.Label();
            this.serverIpLabel = new System.Windows.Forms.Label();
            this.serverIpShow = new System.Windows.Forms.Label();
            this.loginServerPortShow = new System.Windows.Forms.Label();
            this.loginServerPortLabel = new System.Windows.Forms.Label();
            this.uiUpdater = new System.Windows.Forms.Timer(this.components);
            this.playersShow = new System.Windows.Forms.Label();
            this.playersLabel = new System.Windows.Forms.Label();
            this.gameServerPortShow = new System.Windows.Forms.Label();
            this.gameServerPortLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // maintainanceCheckBox
            // 
            this.maintainanceCheckBox.AutoSize = true;
            this.maintainanceCheckBox.Font = new System.Drawing.Font("Segoe Script", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maintainanceCheckBox.Location = new System.Drawing.Point(48, 186);
            this.maintainanceCheckBox.Name = "maintainanceCheckBox";
            this.maintainanceCheckBox.Size = new System.Drawing.Size(237, 35);
            this.maintainanceCheckBox.TabIndex = 38;
            this.maintainanceCheckBox.Text = "Server Maintenance";
            this.maintainanceCheckBox.UseVisualStyleBackColor = true;
            this.maintainanceCheckBox.CheckedChanged += new System.EventHandler(this.maintainanceCheckBox_CheckedChanged);
            // 
            // headLabel
            // 
            this.headLabel.AutoSize = true;
            this.headLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.headLabel.Font = new System.Drawing.Font("Segoe Script", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headLabel.ForeColor = System.Drawing.Color.Olive;
            this.headLabel.Location = new System.Drawing.Point(48, 9);
            this.headLabel.Name = "headLabel";
            this.headLabel.Size = new System.Drawing.Size(251, 33);
            this.headLabel.TabIndex = 37;
            this.headLabel.Text = "Inferno A3 Emulator";
            // 
            // serverIpLabel
            // 
            this.serverIpLabel.AutoSize = true;
            this.serverIpLabel.Font = new System.Drawing.Font("Segoe Script", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverIpLabel.ForeColor = System.Drawing.Color.Fuchsia;
            this.serverIpLabel.Location = new System.Drawing.Point(12, 51);
            this.serverIpLabel.Name = "serverIpLabel";
            this.serverIpLabel.Size = new System.Drawing.Size(130, 33);
            this.serverIpLabel.TabIndex = 39;
            this.serverIpLabel.Text = "Server IP :";
            // 
            // serverIpShow
            // 
            this.serverIpShow.AutoSize = true;
            this.serverIpShow.Font = new System.Drawing.Font("Segoe Script", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverIpShow.ForeColor = System.Drawing.Color.Black;
            this.serverIpShow.Location = new System.Drawing.Point(146, 51);
            this.serverIpShow.Name = "serverIpShow";
            this.serverIpShow.Size = new System.Drawing.Size(90, 33);
            this.serverIpShow.TabIndex = 40;
            this.serverIpShow.Text = "0.0.0.0";
            // 
            // loginServerPortShow
            // 
            this.loginServerPortShow.AutoSize = true;
            this.loginServerPortShow.Font = new System.Drawing.Font("Segoe Script", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginServerPortShow.ForeColor = System.Drawing.Color.Black;
            this.loginServerPortShow.Location = new System.Drawing.Point(243, 84);
            this.loginServerPortShow.Name = "loginServerPortShow";
            this.loginServerPortShow.Size = new System.Drawing.Size(30, 33);
            this.loginServerPortShow.TabIndex = 43;
            this.loginServerPortShow.Text = "0";
            // 
            // loginServerPortLabel
            // 
            this.loginServerPortLabel.AutoSize = true;
            this.loginServerPortLabel.Font = new System.Drawing.Font("Segoe Script", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginServerPortLabel.ForeColor = System.Drawing.Color.Fuchsia;
            this.loginServerPortLabel.Location = new System.Drawing.Point(12, 84);
            this.loginServerPortLabel.Name = "loginServerPortLabel";
            this.loginServerPortLabel.Size = new System.Drawing.Size(223, 33);
            this.loginServerPortLabel.TabIndex = 42;
            this.loginServerPortLabel.Text = "Login Server Port :";
            // 
            // uiUpdater
            // 
            this.uiUpdater.Enabled = true;
            this.uiUpdater.Interval = 500;
            this.uiUpdater.Tick += new System.EventHandler(this.uiUpdater_Tick);
            // 
            // playersShow
            // 
            this.playersShow.AutoSize = true;
            this.playersShow.Font = new System.Drawing.Font("Segoe Script", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playersShow.ForeColor = System.Drawing.Color.Black;
            this.playersShow.Location = new System.Drawing.Point(172, 150);
            this.playersShow.Name = "playersShow";
            this.playersShow.Size = new System.Drawing.Size(30, 33);
            this.playersShow.TabIndex = 45;
            this.playersShow.Text = "0";
            // 
            // playersLabel
            // 
            this.playersLabel.AutoSize = true;
            this.playersLabel.Font = new System.Drawing.Font("Segoe Script", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playersLabel.ForeColor = System.Drawing.Color.Fuchsia;
            this.playersLabel.Location = new System.Drawing.Point(55, 150);
            this.playersLabel.Name = "playersLabel";
            this.playersLabel.Size = new System.Drawing.Size(111, 33);
            this.playersLabel.TabIndex = 44;
            this.playersLabel.Text = "Players :";
            // 
            // gameServerPortShow
            // 
            this.gameServerPortShow.AutoSize = true;
            this.gameServerPortShow.Font = new System.Drawing.Font("Segoe Script", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameServerPortShow.ForeColor = System.Drawing.Color.Black;
            this.gameServerPortShow.Location = new System.Drawing.Point(243, 117);
            this.gameServerPortShow.Name = "gameServerPortShow";
            this.gameServerPortShow.Size = new System.Drawing.Size(30, 33);
            this.gameServerPortShow.TabIndex = 47;
            this.gameServerPortShow.Text = "0";
            // 
            // gameServerPortLabel
            // 
            this.gameServerPortLabel.AutoSize = true;
            this.gameServerPortLabel.Font = new System.Drawing.Font("Segoe Script", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameServerPortLabel.ForeColor = System.Drawing.Color.Fuchsia;
            this.gameServerPortLabel.Location = new System.Drawing.Point(12, 117);
            this.gameServerPortLabel.Name = "gameServerPortLabel";
            this.gameServerPortLabel.Size = new System.Drawing.Size(225, 33);
            this.gameServerPortLabel.TabIndex = 46;
            this.gameServerPortLabel.Text = "Game Server Port :";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 227);
            this.Controls.Add(this.gameServerPortShow);
            this.Controls.Add(this.gameServerPortLabel);
            this.Controls.Add(this.playersShow);
            this.Controls.Add(this.playersLabel);
            this.Controls.Add(this.loginServerPortShow);
            this.Controls.Add(this.loginServerPortLabel);
            this.Controls.Add(this.serverIpShow);
            this.Controls.Add(this.serverIpLabel);
            this.Controls.Add(this.maintainanceCheckBox);
            this.Controls.Add(this.headLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(358, 255);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InfernoEmu";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox maintainanceCheckBox;
        private System.Windows.Forms.Label headLabel;
        private System.Windows.Forms.Label serverIpLabel;
        private System.Windows.Forms.Label serverIpShow;
        private System.Windows.Forms.Label loginServerPortShow;
        private System.Windows.Forms.Label loginServerPortLabel;
        private System.Windows.Forms.Timer uiUpdater;
        private System.Windows.Forms.Label playersShow;
        private System.Windows.Forms.Label playersLabel;
        private System.Windows.Forms.Label gameServerPortShow;
        private System.Windows.Forms.Label gameServerPortLabel;
    }
}

