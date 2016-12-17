namespace wfaAD
{
    partial class frmCheckAD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCheckAD));
            this.txtEmails = new System.Windows.Forms.TextBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtEmails
            // 
            this.txtEmails.Location = new System.Drawing.Point(12, 41);
            this.txtEmails.Multiline = true;
            this.txtEmails.Name = "txtEmails";
            this.txtEmails.Size = new System.Drawing.Size(188, 381);
            this.txtEmails.TabIndex = 0;
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(12, 12);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(185, 23);
            this.btnCheck.TabIndex = 1;
            this.btnCheck.Text = "&Check for users / subscriptions";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // txtResults
            // 
            this.txtResults.Location = new System.Drawing.Point(206, 41);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResults.Size = new System.Drawing.Size(707, 381);
            this.txtResults.TabIndex = 3;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(403, 17);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(16, 13);
            this.lblUser.TabIndex = 4;
            this.lblUser.Text = "...";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(333, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Next";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(206, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(121, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Check same user";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmCheckAD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 437);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.txtEmails);
			//this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCheckAD";
            this.Text = "Check for AD users";
            this.Resize += new System.EventHandler(this.frmCheckAD_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEmails;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

