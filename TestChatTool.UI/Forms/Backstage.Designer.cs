
namespace TestChatTool.UI.Forms
{
    partial class Backstage
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
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnRoomMaintain = new System.Windows.Forms.Button();
            this.btnUnlock = new System.Windows.Forms.Button();
            this.cbbRoom = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtTalk = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.txtRoomUsers = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCreate.Location = new System.Drawing.Point(540, 390);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(160, 40);
            this.btnCreate.TabIndex = 10;
            this.btnCreate.Text = "新增管理員";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btnVerify
            // 
            this.btnVerify.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnVerify.Location = new System.Drawing.Point(10, 10);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(160, 40);
            this.btnVerify.TabIndex = 11;
            this.btnVerify.Text = "帳號審核";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btnRoomMaintain
            // 
            this.btnRoomMaintain.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRoomMaintain.Location = new System.Drawing.Point(340, 10);
            this.btnRoomMaintain.Name = "btnRoomMaintain";
            this.btnRoomMaintain.Size = new System.Drawing.Size(160, 40);
            this.btnRoomMaintain.TabIndex = 12;
            this.btnRoomMaintain.Text = "聊天室管理";
            this.btnRoomMaintain.UseVisualStyleBackColor = true;
            this.btnRoomMaintain.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUnlock.Location = new System.Drawing.Point(175, 10);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(160, 40);
            this.btnUnlock.TabIndex = 13;
            this.btnUnlock.Text = "帳號解鎖";
            this.btnUnlock.UseVisualStyleBackColor = true;
            this.btnUnlock.Click += new System.EventHandler(this.ButtonClick);
            // 
            // cbbRoom
            // 
            this.cbbRoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbRoom.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbbRoom.FormattingEnabled = true;
            this.cbbRoom.Location = new System.Drawing.Point(510, 10);
            this.cbbRoom.Name = "cbbRoom";
            this.cbbRoom.Size = new System.Drawing.Size(190, 35);
            this.cbbRoom.TabIndex = 14;
            this.cbbRoom.SelectedValueChanged += new System.EventHandler(this.SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(10, 390);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 33);
            this.label3.TabIndex = 25;
            this.label3.Text = "發言";
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSend.Location = new System.Drawing.Point(455, 390);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(80, 40);
            this.btnSend.TabIndex = 24;
            this.btnSend.Text = "發送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.ButtonClick);
            // 
            // txtTalk
            // 
            this.txtTalk.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTalk.Location = new System.Drawing.Point(95, 390);
            this.txtTalk.Name = "txtTalk";
            this.txtTalk.Size = new System.Drawing.Size(353, 38);
            this.txtTalk.TabIndex = 23;
            // 
            // txtMessage
            // 
            this.txtMessage.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMessage.Location = new System.Drawing.Point(10, 60);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessage.Size = new System.Drawing.Size(490, 320);
            this.txtMessage.TabIndex = 22;
            // 
            // txtRoomUsers
            // 
            this.txtRoomUsers.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRoomUsers.Location = new System.Drawing.Point(510, 96);
            this.txtRoomUsers.Multiline = true;
            this.txtRoomUsers.Name = "txtRoomUsers";
            this.txtRoomUsers.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRoomUsers.Size = new System.Drawing.Size(190, 284);
            this.txtRoomUsers.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(506, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 33);
            this.label1.TabIndex = 27;
            this.label1.Text = "聊天室會員";
            // 
            // Backstage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 441);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRoomUsers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtTalk);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.cbbRoom);
            this.Controls.Add(this.btnUnlock);
            this.Controls.Add(this.btnRoomMaintain);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.btnCreate);
            this.Name = "Backstage";
            this.Text = "管理後台";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnRoomMaintain;
        private System.Windows.Forms.Button btnUnlock;
        private System.Windows.Forms.ComboBox cbbRoom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtTalk;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox txtRoomUsers;
        private System.Windows.Forms.Label label1;
    }
}