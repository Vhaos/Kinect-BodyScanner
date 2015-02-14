namespace QR_encoder
{
    partial class Form1
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
            this.qrimage = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.texttoencode = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.qrimage)).BeginInit();
            this.SuspendLayout();
            // 
            // qrimage
            // 
            this.qrimage.Location = new System.Drawing.Point(13, 13);
            this.qrimage.Name = "qrimage";
            this.qrimage.Size = new System.Drawing.Size(313, 292);
            this.qrimage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.qrimage.TabIndex = 0;
            this.qrimage.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 311);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter Text";
            // 
            // texttoencode
            // 
            this.texttoencode.Location = new System.Drawing.Point(73, 311);
            this.texttoencode.Name = "texttoencode";
            this.texttoencode.Size = new System.Drawing.Size(243, 20);
            this.texttoencode.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 337);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(312, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Create";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(14, 367);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(312, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Save QR Code";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(14, 400);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(148, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Load QR Code";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(168, 400);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(157, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "Extract QR Code Data";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 435);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.texttoencode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.qrimage);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "QR Code";
            ((System.ComponentModel.ISupportInitialize)(this.qrimage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox qrimage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox texttoencode;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

