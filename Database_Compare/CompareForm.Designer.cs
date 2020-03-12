namespace Database_Compare
{
    partial class CompareForm
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
            this.richTextBoxKaynak = new System.Windows.Forms.RichTextBox();
            this.richTextBoxHedef = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBoxKaynak
            // 
            this.richTextBoxKaynak.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxKaynak.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxKaynak.Name = "richTextBoxKaynak";
            this.richTextBoxKaynak.Size = new System.Drawing.Size(700, 825);
            this.richTextBoxKaynak.TabIndex = 0;
            this.richTextBoxKaynak.Text = "";
            this.richTextBoxKaynak.WordWrap = false;
            this.richTextBoxKaynak.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.CompareForm_MouseWheel);
            // 
            // richTextBoxHedef
            // 
            this.richTextBoxHedef.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxHedef.Location = new System.Drawing.Point(718, 12);
            this.richTextBoxHedef.Name = "richTextBoxHedef";
            this.richTextBoxHedef.Size = new System.Drawing.Size(700, 825);
            this.richTextBoxHedef.TabIndex = 1;
            this.richTextBoxHedef.Text = "";
            this.richTextBoxHedef.WordWrap = false;
            this.richTextBoxHedef.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.CompareForm_MouseWheel);
            // 
            // CompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1427, 849);
            this.Controls.Add(this.richTextBoxHedef);
            this.Controls.Add(this.richTextBoxKaynak);
            this.Name = "CompareForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CompareForm";
            this.Load += new System.EventHandler(this.CompareForm_Load);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.CompareForm_MouseWheel);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.RichTextBox richTextBoxKaynak;
        public System.Windows.Forms.RichTextBox richTextBoxHedef;
    }
}