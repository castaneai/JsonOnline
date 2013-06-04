namespace ChatSampleClient
{
	partial class Form1
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.usersListBox = new System.Windows.Forms.ListBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.messageInputTextBox = new System.Windows.Forms.TextBox();
			this.messageSendButton = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.chatLogListBox = new System.Windows.Forms.ListBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// usersListBox
			// 
			this.usersListBox.FormattingEnabled = true;
			this.usersListBox.ItemHeight = 12;
			this.usersListBox.Location = new System.Drawing.Point(6, 18);
			this.usersListBox.Name = "usersListBox";
			this.usersListBox.Size = new System.Drawing.Size(110, 184);
			this.usersListBox.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.usersListBox);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(123, 215);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Users";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.messageSendButton);
			this.groupBox2.Controls.Add(this.messageInputTextBox);
			this.groupBox2.Location = new System.Drawing.Point(12, 233);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(429, 47);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Message Input";
			// 
			// messageInputTextBox
			// 
			this.messageInputTextBox.Location = new System.Drawing.Point(6, 18);
			this.messageInputTextBox.Name = "messageInputTextBox";
			this.messageInputTextBox.Size = new System.Drawing.Size(336, 19);
			this.messageInputTextBox.TabIndex = 3;
			// 
			// messageSendButton
			// 
			this.messageSendButton.Location = new System.Drawing.Point(348, 16);
			this.messageSendButton.Name = "messageSendButton";
			this.messageSendButton.Size = new System.Drawing.Size(75, 23);
			this.messageSendButton.TabIndex = 3;
			this.messageSendButton.Text = "Send";
			this.messageSendButton.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.chatLogListBox);
			this.groupBox3.Location = new System.Drawing.Point(141, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(300, 215);
			this.groupBox3.TabIndex = 3;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Chat Log";
			// 
			// chatLogListBox
			// 
			this.chatLogListBox.FormattingEnabled = true;
			this.chatLogListBox.ItemHeight = 12;
			this.chatLogListBox.Location = new System.Drawing.Point(6, 18);
			this.chatLogListBox.Name = "chatLogListBox";
			this.chatLogListBox.Size = new System.Drawing.Size(288, 184);
			this.chatLogListBox.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(453, 289);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "Form1";
			this.Text = "ChatSampleClient";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox usersListBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button messageSendButton;
		private System.Windows.Forms.TextBox messageInputTextBox;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ListBox chatLogListBox;
	}
}

