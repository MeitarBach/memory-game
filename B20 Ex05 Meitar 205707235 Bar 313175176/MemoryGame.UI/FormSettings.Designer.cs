namespace MemoryGame.UI
{
    partial class FormSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.oponnentButton = new System.Windows.Forms.Button();
            this.boardSizeButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.firstPlayerTextBox = new System.Windows.Forms.TextBox();
            this.secondPlayerTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "First Player Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 105);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(290, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "Second Player Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 226);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 32);
            this.label3.TabIndex = 2;
            this.label3.Text = "Board Size:";
            // 
            // oponnentButton
            // 
            this.oponnentButton.Location = new System.Drawing.Point(722, 88);
            this.oponnentButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.oponnentButton.Name = "oponnentButton";
            this.oponnentButton.Size = new System.Drawing.Size(297, 73);
            this.oponnentButton.TabIndex = 3;
            this.oponnentButton.Text = "Against a Friend";
            this.oponnentButton.UseVisualStyleBackColor = true;
            this.oponnentButton.Click += new System.EventHandler(this.opponentButton_Click);
            // 
            // boardSizeButton
            // 
            this.boardSizeButton.BackColor = System.Drawing.Color.LightSkyBlue;
            this.boardSizeButton.Location = new System.Drawing.Point(57, 298);
            this.boardSizeButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.boardSizeButton.Name = "boardSizeButton";
            this.boardSizeButton.Size = new System.Drawing.Size(244, 155);
            this.boardSizeButton.TabIndex = 4;
            this.boardSizeButton.Text = "4x4";
            this.boardSizeButton.UseVisualStyleBackColor = false;
            this.boardSizeButton.Click += new System.EventHandler(this.boardSizeButton_Click);
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.Color.Lime;
            this.startButton.Location = new System.Drawing.Point(841, 384);
            this.startButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(178, 68);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "Start!";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // firstPlayerTextBox
            // 
            this.firstPlayerTextBox.Location = new System.Drawing.Point(370, 46);
            this.firstPlayerTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.firstPlayerTextBox.Name = "firstPlayerTextBox";
            this.firstPlayerTextBox.Size = new System.Drawing.Size(313, 38);
            this.firstPlayerTextBox.TabIndex = 1;
            // 
            // secondPlayerTextBox
            // 
            this.secondPlayerTextBox.Enabled = false;
            this.secondPlayerTextBox.Location = new System.Drawing.Point(370, 105);
            this.secondPlayerTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.secondPlayerTextBox.Name = "secondPlayerTextBox";
            this.secondPlayerTextBox.Size = new System.Drawing.Size(313, 38);
            this.secondPlayerTextBox.TabIndex = 2;
            this.secondPlayerTextBox.Text = "- computer -";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 465);
            this.Controls.Add(this.secondPlayerTextBox);
            this.Controls.Add(this.firstPlayerTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.boardSizeButton);
            this.Controls.Add(this.oponnentButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Memory Game - Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button oponnentButton;
        private System.Windows.Forms.Button boardSizeButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox firstPlayerTextBox;
        private System.Windows.Forms.TextBox secondPlayerTextBox;
    }
}