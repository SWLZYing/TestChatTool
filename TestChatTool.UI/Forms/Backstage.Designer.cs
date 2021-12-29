
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
            this.btnRoomCreate = new System.Windows.Forms.Button();
            this.btnUnlock = new System.Windows.Forms.Button();
            this.cbbRoom = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtTalk = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.txtRoomUsers = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRoomUpdate = new System.Windows.Forms.Button();
            this.btnRoomDelete = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
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
            this.btnVerify.Size = new System.Drawing.Size(150, 40);
            this.btnVerify.TabIndex = 11;
            this.btnVerify.Text = "帳號審核";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btnRoomCreate
            // 
            this.btnRoomCreate.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRoomCreate.Location = new System.Drawing.Point(450, 10);
            this.btnRoomCreate.Name = "btnRoomCreate";
            this.btnRoomCreate.Size = new System.Drawing.Size(80, 40);
            this.btnRoomCreate.TabIndex = 12;
            this.btnRoomCreate.Text = "新增";
            this.btnRoomCreate.UseVisualStyleBackColor = true;
            this.btnRoomCreate.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUnlock.Location = new System.Drawing.Point(165, 10);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(150, 40);
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
            this.cbbRoom.Location = new System.Drawing.Point(510, 60);
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
            this.txtRoomUsers.Location = new System.Drawing.Point(510, 145);
            this.txtRoomUsers.Multiline = true;
            this.txtRoomUsers.Name = "txtRoomUsers";
            this.txtRoomUsers.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRoomUsers.Size = new System.Drawing.Size(190, 235);
            this.txtRoomUsers.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(515, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 33);
            this.label1.TabIndex = 27;
            this.label1.Text = "聊天室會員";
            // 
            // btnRoomUpdate
            // 
            this.btnRoomUpdate.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRoomUpdate.Location = new System.Drawing.Point(540, 10);
            this.btnRoomUpdate.Name = "btnRoomUpdate";
            this.btnRoomUpdate.Size = new System.Drawing.Size(80, 40);
            this.btnRoomUpdate.TabIndex = 28;
            this.btnRoomUpdate.Text = "修改";
            this.btnRoomUpdate.UseVisualStyleBackColor = true;
            this.btnRoomUpdate.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btnRoomDelete
            // 
            this.btnRoomDelete.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRoomDelete.Location = new System.Drawing.Point(630, 10);
            this.btnRoomDelete.Name = "btnRoomDelete";
            this.btnRoomDelete.Size = new System.Drawing.Size(80, 40);
            this.btnRoomDelete.TabIndex = 29;
            this.btnRoomDelete.Text = "刪除";
            this.btnRoomDelete.UseVisualStyleBackColor = true;
            this.btnRoomDelete.Click += new System.EventHandler(this.ButtonClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(330, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 33);
            this.label2.TabIndex = 30;
            this.label2.Text = "聊天室";
            // 
            // Backstage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 441);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRoomDelete);
            this.Controls.Add(this.btnRoomUpdate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRoomUsers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtTalk);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.cbbRoom);
            this.Controls.Add(this.btnUnlock);
            this.Controls.Add(this.btnRoomCreate);
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
        private System.Windows.Forms.Button btnRoomCreate;
        private System.Windows.Forms.Button btnUnlock;
        private System.Windows.Forms.ComboBox cbbRoom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtTalk;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox txtRoomUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRoomUpdate;
        private System.Windows.Forms.Button btnRoomDelete;
        private System.Windows.Forms.Label label2;
    }
}