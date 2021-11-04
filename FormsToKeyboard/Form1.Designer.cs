
namespace FormsToKeyboard
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
            this.ClearBtn = new System.Windows.Forms.Button();
            this.InputText = new System.Windows.Forms.RichTextBox();
            this.ProcessList = new System.Windows.Forms.ComboBox();
            this.ClipboardBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(164, 182);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(75, 23);
            this.ClearBtn.TabIndex = 2;
            this.ClearBtn.Text = "Clear";
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // InputText
            // 
            this.InputText.Location = new System.Drawing.Point(12, 12);
            this.InputText.Name = "InputText";
            this.InputText.Size = new System.Drawing.Size(308, 163);
            this.InputText.TabIndex = 3;
            this.InputText.Text = "";
            // 
            // ProcessList
            // 
            this.ProcessList.FormattingEnabled = true;
            this.ProcessList.Location = new System.Drawing.Point(12, 183);
            this.ProcessList.Name = "ProcessList";
            this.ProcessList.Size = new System.Drawing.Size(146, 23);
            this.ProcessList.TabIndex = 4;
            // 
            // ClipboardBtn
            // 
            this.ClipboardBtn.Location = new System.Drawing.Point(245, 183);
            this.ClipboardBtn.Name = "ClipboardBtn";
            this.ClipboardBtn.Size = new System.Drawing.Size(75, 23);
            this.ClipboardBtn.TabIndex = 5;
            this.ClipboardBtn.Text = "Copy Clipboard";
            this.ClipboardBtn.UseVisualStyleBackColor = true;
            this.ClipboardBtn.Click += new System.EventHandler(this.ClipboardBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 244);
            this.Controls.Add(this.ClipboardBtn);
            this.Controls.Add(this.ProcessList);
            this.Controls.Add(this.InputText);
            this.Controls.Add(this.ClearBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ClearBtn;
        private System.Windows.Forms.RichTextBox InputText;
        private System.Windows.Forms.ComboBox ProcessList;
        private System.Windows.Forms.Button ClipboardBtn;
    }
}

